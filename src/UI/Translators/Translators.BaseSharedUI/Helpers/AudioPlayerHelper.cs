using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Interfaces;

namespace Translators.UI.Helpers
{
    public class AudioPlayerHelper : AudioPlayerBaseHelper, IAudioPlayer, ISimpleAudioPlayer
    {
        static ISimpleAudioPlayer _Current;
        public static ISimpleAudioPlayer Current
        {
            get
            {
                return _Current;
            }
            set
            {
                _Current = value;
                CurrentBase = new AudioPlayerHelper();
            }
        }

        public double Duration => Current.Duration;

        public double CurrentPosition => Current.CurrentPosition;

        public double Volume { get => Current.Volume; set => Current.Volume = value; }
        public double Balance { get => Current.Balance; set => Current.Balance = value; }

        public bool IsPlaying => Current.IsPlaying;

        public bool Loop { get => Current.Loop; set => Current.Loop = value; }

        public bool CanSeek => Current.CanSeek;

        public Func<Task> PlaybackEnded { get => Current.PlaybackEnded; set => Current.PlaybackEnded = value; }

        public void Dispose()
        {
            Current.Dispose();
        }

        public bool Load(Stream audioStream)
        {
            return Current.Load(audioStream);
        }

        public bool Load(string fileName)
        {
            return Current.Load(fileName);
        }

        public void Pause()
        {
            Current.Pause();
        }

        public void Play()
        {
            Current.Play();
        }

        public void Seek(double position)
        {
            Current.Seek(position);
        }

        public void SetSpeed(double speed)
        {
            Current.SetSpeed(speed);
        }

        public void Stop()
        {
            Current.Stop();
        }
    }
}
