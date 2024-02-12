using EasyMicroservices.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorApp.GeneratedServices;
using Translators.Converters;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.DataTypes;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class ParagraphBaseViewModel<T> : BaseCollectionViewModel<T>
        where T : ParagraphBaseModel
    {
        public bool IsOnSelectionMode(T paragraphBaseModel)
        {
            if (IsEnableMultipleSelection && paragraphBaseModel != null)
            {
                paragraphBaseModel.IsSelected = !paragraphBaseModel.IsSelected;
                return true;
            }
            return false;
        }

        protected override void OnIsEnableMultipleSelectionChanged()
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }
        }

        public async Task TouchBase(T paragraphBaseModel, bool hasGoToPage, bool hasGoToLink = true)
        {
            if (paragraphBaseModel == null)
            {
                await TouchMultipleBase();
                return;
            }
            await DoBusinessItems(hasGoToPage, hasGoToLink, paragraphBaseModel);
        }

        async Task TouchMultipleBase()
        {
            var selectedItems = Items.Where(x => x.IsSelected).ToArray();
            await DoBusinessItems(false, true, selectedItems);
        }

        async Task DoBusinessItems(bool hasGoToPage, bool hasGoToLink, params T[] items)
        {
            if (items == null || items.Length == 0)
            {
                //display alert
                return;
            }
            bool hasOne = items.Length == 1;
            bool hasMany = !hasOne;
            var first = items.First();
            CatalogContract catalogResult = default;
            try
            {
                IsLoading = true;
                var catalog = await TranslatorService.GetChapterService(false).GetChaptersAsync(first.CatalogId);
                if (catalog.IsSuccess)
                    catalogResult = catalog.Result;
            }
            finally
            {
                IsLoading = false;
            }
            if (hasOne)
            {
                string displayName = first.DisplayName;
                displayName = $"({LanguageValueBaseConverter.GetValue(catalogResult?.BookNames, false, "fa-ir")} {catalogResult?.Number}-{CleanArabicChars(LanguageValueBaseConverter.GetValue(catalogResult?.Names, false, "fa-ir"))} آیه‌ی {first.Number})";
                first.DisplayName = displayName;
            }
            else
            {
                string displayName = first.DisplayName;
                displayName = $"({LanguageValueBaseConverter.GetValue(catalogResult?.BookNames, false, "fa-ir")} {catalogResult?.Number}-{CleanArabicChars(LanguageValueBaseConverter.GetValue(catalogResult?.Names, false, "fa-ir"))} صفحه‌ی {first.PageNumber})";
                first.DisplayName = displayName;
            }

            List<string> menus = new List<string> { "کپی همه", "کپی آیه", "کپی ترجمه", hasGoToPage ? "رفتن به صفحه..." : "", TranslatorService.IsAdmin ? "کپی جهت لینک دادن" : "" };
            menus.Add((TranslatorService.ParagraphsForLink?.Length > 0 && TranslatorService.IsAdmin) ? "آیه‌ی کپی شده را لینک کن" : "");
            menus.Add(hasGoToLink && (hasOne && first.HasLink) ? "مشاهده‌ی لینک ها" : "");

            var selectedType = await AlertHelper.Display<VerseRightClickType>("عملیات", "انصراف", menus.ToArray());
            switch (selectedType)
            {
                case VerseRightClickType.CopyAll:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (var item in items)
                        {
                            stringBuilder.AppendLineBefore(item.MainValue);
                            if (hasMany)
                                stringBuilder.Append($"({item.Number})");
                            stringBuilder.AppendLineBefore(item.TranslatedValue);
                            if (hasMany)
                                stringBuilder.Append($"({item.Number})");
                        }

                        if (!string.IsNullOrEmpty(first.DisplayName))
                            stringBuilder.Append(first.DisplayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyVerse:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (var item in items)
                        {
                            stringBuilder.AppendLineBefore(item.MainValue);
                            if (hasMany)
                                stringBuilder.Append($"({item.Number})");
                        }
                        if (!string.IsNullOrEmpty(first.DisplayName))
                            stringBuilder.Append(first.DisplayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyTranslate:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (var item in items)
                        {
                            stringBuilder.AppendLineBefore(item.TranslatedValue);
                            if (hasMany)
                                stringBuilder.Append($"({item.Number})");
                        }
                        if (!string.IsNullOrEmpty(first.DisplayName))
                            stringBuilder.Append(first.DisplayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyForLink:
                    {
                        TranslatorService.ParagraphsForLink = items;
                        break;
                    }
                case VerseRightClickType.PasteForLink:
                    {
                        await PageHelper.PushPage(0, 0, items, PageType.DoLinkPage, this);
                        break;
                    }
                case VerseRightClickType.GoToPage:
                    {
                        await PageHelper.PushPage(first.PageNumber, first.BookId, first.CatalogId, PageType.PagesFastRead, this);
                        break;
                    }
                case VerseRightClickType.ShowLinks:
                    {
                        try
                        {
                            IsLoading = true;
                            var groupResult = await TranslatorService.GetParagraphService(true).GetLinkedParagraphsGroupsAsync(first.Id);
                            if (groupResult.IsSuccess)
                            {
                                long groupId = groupResult.Result.First().Id;
                                if (groupResult.Result.Count > 1)
                                {
                                    var groupTitle = await AlertHelper.Display("انتخاب گروه", "انصراف", groupResult.Result.Select(x => x.Title).ToArray());
                                    if (groupTitle == "انصراف" || string.IsNullOrEmpty(groupTitle))
                                        return;
                                    groupId = groupResult.Result.FirstOrDefault(x => x.Title == groupTitle).Id;
                                }
                                var result = await TranslatorService.GetParagraphService(true).GetLinkedParagraphsAsync(groupId);
                                if (result.IsSuccess)
                                    await PageHelper.PushPage(0, 0, result.Result, PageType.ParagraphResult, this);
                                else
                                    await AlertContract(result.ToContract());
                            }
                            else
                                await AlertContract(groupResult.ToContract());
                        }
                        finally
                        {
                            IsLoading = false;
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        public SearchResultModel Map(SearchValueContract value)
        {
            var displayName = " " + $"({LanguageValueBaseConverter.GetValue(value.BookNames, false, "fa-ir")} - {LanguageValueBaseConverter.GetValue(value.CatalogNames, false, "fa-ir")} - آیه‌ی {value.Number})";
            string mainTransliterationValue = string.Join(" ", value.ParagraphWords.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => !x.IsMain && x.IsTransliteration && x.Language.Code == "fa-ir").Select(x => x.Value));
            var result = new SearchResultModel()
            {
                Id = value.ParagraphId,
                HasLink = value.HasLink,
                TranslatedValue = string.Join(" ", value.ParagraphWords.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => !x.IsMain && !x.IsTransliteration && x.Language.Code == "fa-ir").Select(x => x.Value)) + displayName,
                MainValue = string.Join(" ", value.ParagraphWords.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => x.IsMain).Select(x => x.Value)) + displayName,
                MainTransliterationValue = string.IsNullOrEmpty(mainTransliterationValue) ? null : mainTransliterationValue + displayName,
                CatalogId = value.CatalogId,
                BookId = value.BookId,
                PageNumber = value.PageNumber,
                Number = value.Number,
            };
            return result;
        }
    }
}
