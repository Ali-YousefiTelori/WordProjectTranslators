using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Contracts.Common.Files;
using Translators.Contracts.Responses.Pages;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Storages;
using Translators.ServiceManagers;
using Translators.ViewModels;

namespace Translators.Logics
{
    public class AudioPlayerManager : BaseModel
    {
        public Func<Task> SwipeRight { get; set; }
        public Func<Task> SwipeLeft { get; set; }
        public BaseViewModel ViewModel { get; set; }

        protected bool IsDownloadedStreams { get; set; }
        /// <summary>
        /// try download again the audio from client
        /// </summary>
        public bool DoDownloadAgain { get; set; }

        private double _AudioDuration;
        private double _PlaybackCurrentPosition;
        public double PlaybackCurrentPosition
        {
            get
            {
                return _PlaybackCurrentPosition;
            }
            set
            {
                if (_PlaybackCurrentPosition == value)
                    return;
                _PlaybackCurrentPosition = value;
                OnPropertyChanged(nameof(PlaybackCurrentPosition));
                if (AudioPlayerBaseHelper.CurrentBase.CanSeek)
                {
                    Seek(value * _AudioDuration);
                }
            }
        }

        private int _ScrollToIndex;
        public int ScrollToIndex
        {
            get => _ScrollToIndex;
            set
            {
                _ScrollToIndex = value;
                OnPropertyChanged(nameof(ScrollToIndex));
            }
        }

        bool _isLoadingPlayStream = false;
        public bool IsLoadingPlayStream
        {
            get => _isLoadingPlayStream;
            set
            {
                _isLoadingPlayStream = value;
                OnPropertyChanged(nameof(IsLoadingPlayStream));
            }
        }

        bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
                ApplicationHelper.Current.KeepScreenOn(value);
            }
        }
        public PageResponseContract Page { get; set; }
        List<(SimpleFileContract Audio, Stream Stream)> Audios { get; set; }
        int _AudioIndex = 0;
        double _LastPlaybackSpeedRatoSet = 0;
        public void Initalize(BaseViewModel baseViewModel, Func<Task> swipeRight, Func<Task> swipeLeft)
        {
            ViewModel = baseViewModel;
            SwipeRight = swipeRight;
            SwipeLeft = swipeLeft;
        }

        public async Task PlayOrPause()
        {
            try
            {
                IsLoadingPlayStream = true;
                if (!IsDownloadedStreams)
                {
                    var audios = GetAudios();
                    if (audios.Count == 0)
                    {
                        await AlertHelper.Display("ناموجود", "باشه");
                        return;
                    }

                    Audios = await DownloadAudios(audios, DoDownloadAgain);
                    DoDownloadAgain = false;
                    var run = AudioPlayerBaseHelper.CurrentBase.Load(GetCopyStream(_AudioIndex));
                    AudioPlayerBaseHelper.CurrentBase.PlaybackEnded = Current_PlaybackEnded;
                    PlaybackCurrentPosition = 0;
                }

                if (IsPlaying)
                    AudioPlayerBaseHelper.CurrentBase.Pause();
                else
                    PlayBase();
                IsPlaying = !IsPlaying;
                IsDownloadedStreams = true;
            }
            finally
            {
                IsLoadingPlayStream = false;
            }
        }

        private async Task Current_PlaybackEnded()
        {
            if (!IsPlaying)
                return;
            if (_AudioIndex == Audios.Count - 1)
            {
                try
                {
                    await SwipeRight();
                    await PlayOrPause();
                }
                catch (Exception ex)
                {
                    await BaseViewModel.AlertExcepption(ex);
                }
            }
            else
            {
                PlayAudio(_AudioIndex + 1);
            }
        }

        void ScrollToTop()
        {
            ScrollToIndex = 1;
            ScrollToIndex = 0;
        }

        void PlayBase()
        {
            //In Android it will stop instead of pause
            if (_LastPlaybackSpeedRatoSet != BaseViewModel._PlaybackSpeedRato)
            {
                _LastPlaybackSpeedRatoSet = BaseViewModel._PlaybackSpeedRato;
                AudioPlayerBaseHelper.CurrentBase.SetSpeed(BaseViewModel._PlaybackSpeedRato);
            }
            AudioPlayerBaseHelper.CurrentBase.Play();
        }

        List<SimpleFileContract> GetAudios()
        {
            var result = Page.AudioFiles?.ToList();
            if (result == null || result.Count == 0)
                return Page.Paragraphs.Where(x => x.AudioFiles?.Count > 0).SelectMany(x => x.AudioFiles.OrderByDescending(x => x.IsMain)).ToList();
            return result;
        }

        async Task<List<(SimpleFileContract, Stream)>> DownloadAudios(List<SimpleFileContract> audioFiles, bool doReDownload)
        {
            List<(SimpleFileContract, Stream)> result = new List<(SimpleFileContract, Stream)>();
            double duration = 0;
            foreach (var audio in audioFiles)
            {
                string key = $"{audio.PageId.GetValueOrDefault()}_{audio.Id}";
                var saver = new ApplicationBookAudioData();
                saver.Initialize(key, ".mp3");
                var stream = await saver.DownloadFileStream($"{TranslatorService.ServiceAddress}/Storage/DownloadFile?fileId={audio.Id}&password={audio.Password}", doReDownload);
                string groupKey = GetAudioGroupKey(audio);
                result.Add((audio, stream));
                duration += new TimeSpan(audio.DurationTicks).TotalSeconds;
            }
            _AudioDuration = duration;
            return result;
        }

        string GetAudioGroupKey(SimpleFileContract audioFile)
        {
            return $"{audioFile.LanguageId}_{audioFile.TranslatorId}";
        }

        TimeSpan GetToCurrentAudioIndexDuration(int index)
        {
            return new TimeSpan(Audios.Take(index).Sum(x => GetAudioDuration(x.Audio).Ticks));
        }

        TimeSpan GetAudioDuration(SimpleFileContract audioFile)
        {
            return new TimeSpan(audioFile.DurationTicks);
        }


        void SetScrollByAudio()
        {
            if (ViewModel.HasAutoScrollInPlayback)
            {
                if (Audios.Count == 1)
                {
                    int index = 0;
                    var fullLength = Items.Sum(x => x.TranslatedValue.Length + x.MainDisplayValue.Length);
                    var itemsLengths = Items.Select(x => new { Length = x.TranslatedValue.Length + x.MainDisplayValue.Length }); ;
                    double from = 0;
                    List<(double from, double to, int index)> PositionItems = new List<(double from, double to, int index)>();
                    foreach (var item in Items)
                    {
                        double to = from + (item.TranslatedValue.Length + item.MainDisplayValue.Length) / (double)fullLength;
                        PositionItems.Add((from, to, index));
                        from = to;
                        index++;
                    }
                    ScrollToIndex = PositionItems.Where(x => _PlaybackCurrentPosition > x.from && _PlaybackCurrentPosition < x.to).Select(x => x.index).FirstOrDefault();
                }
                else if (Audios.Count == Page.Paragraphs.Count)
                    ScrollToIndex = _AudioIndex;
                else
                {
                    ScrollToIndex = (int)((_AudioIndex / (double)Audios.Count) * Page.Paragraphs.Count);
                }
            }
        }

        void Seek(double position)
        {
            try
            {
                if (Audios?.Count > 1)
                {
                    var selectedAudio = (int)(position / _AudioDuration * Audios.Count);
                    if (selectedAudio == Audios.Count)
                        selectedAudio--;
                    if (selectedAudio != _AudioIndex)
                    {
                        PlayAudio(selectedAudio);
                    }
                    var paragraphTotal = GetAudioDuration(Audios[selectedAudio].Audio).TotalSeconds;
                    var seek = paragraphTotal - (GetToCurrentAudioIndexDuration(selectedAudio).TotalSeconds + paragraphTotal - position);
                    _PlaybackCurrentPosition = 1 * (position / _AudioDuration);
                    AudioPlayerBaseHelper.CurrentBase.Seek(seek);
                }
                else
                    AudioPlayerBaseHelper.CurrentBase.Seek(position);
            }
            catch (Exception ex)
            {
                TranslatorService.LogException($"{Audios == null}-{AudioPlayerBaseHelper.CurrentBase == null}: {ex}");
            }
        }

        void PlayAudio(int index)
        {
            _AudioIndex = index;
            bool doPlay = IsPlaying;
            IsPlaying = false;
            AudioPlayerBaseHelper.CurrentBase.PlaybackEnded = null;
            AudioPlayerBaseHelper.CurrentBase.Stop();
            var run = AudioPlayerBaseHelper.CurrentBase.Load(GetCopyStream(index));
            AudioPlayerBaseHelper.CurrentBase.PlaybackEnded = Current_PlaybackEnded;
            if (doPlay)
            {
                _LastPlaybackSpeedRatoSet = 0;
                PlayBase();
                IsPlaying = !IsPlaying;
            }
        }

        MemoryStream GetCopyStream(int index)
        {
            var resultStream = new MemoryStream();
            Audios[index].Stream.Seek(0, SeekOrigin.Begin);
            Audios[index].Stream.CopyTo(resultStream);
            resultStream.Seek(0, SeekOrigin.Begin);
            return resultStream;
        }

        void ResetPlayBack()
        {
            _AudioIndex = 0;
            IsDownloadedStreams = false;
            IsPlaying = false;
            AudioPlayerBaseHelper.CurrentBase.Stop();
            _LastPlaybackSpeedRatoSet = 0;
        }

        public void GoToPosition()
        {
            if (IsPlaying && AudioPlayerBaseHelper.CurrentBase.CanSeek)
            {
                var currentPosition = GetToCurrentAudioIndexDuration(_AudioIndex).TotalSeconds;
                _PlaybackCurrentPosition = 1 * ((currentPosition + AudioPlayerBaseHelper.CurrentBase.CurrentPosition) / _AudioDuration);

                SetScrollByAudio();
                OnPropertyChanged(nameof(PlaybackCurrentPosition));
            }
        }

        public void Reset()
        {
            ResetPlayBack();
            ScrollToTop();
        }

        ObservableCollection<ParagraphModel> Items { get; set; }
        public void InitializeItems(ObservableCollection<ParagraphModel> items)
        {
            Items = items;
        }
    }
}
