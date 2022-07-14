﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Converters;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.DataTypes;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class ParagraphBaseViewModel<T> : BaseCollectionViewModel<T>
    {
        public async Task TouchBase(ParagraphBaseModel paragraphBaseModel, bool hasGoToPage, bool hasGoToLink = true)
        {
            List<string> menus = new List<string> { "کپی همه", "کپی آیه", "کپی ترجمه", hasGoToPage ? "رفتن به صفحه..." : "", TranslatorService.IsAdmin ? "کپی جهت لینک دادن" : "" };
            menus.Add((TranslatorService.ParagraphForLink != null && TranslatorService.IsAdmin) ? "آیه‌ی کپی شده را لینک کن" : "");
            menus.Add(hasGoToLink && paragraphBaseModel.HasLink ? "مشاهده‌ی لینک ها" : "");

            var selectedType = await AlertHelper.Display<VerseRightClickType>("عملیات", "انصراف", menus.ToArray());
            switch (selectedType)
            {
                case VerseRightClickType.CopyAll:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine(paragraphBaseModel.MainValue);
                        stringBuilder.Append(paragraphBaseModel.TranslatedValue);
                        if (!string.IsNullOrEmpty(paragraphBaseModel.DisplayName))
                            stringBuilder.Append(paragraphBaseModel.DisplayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyVerse:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(paragraphBaseModel.MainValue);
                        if (!string.IsNullOrEmpty(paragraphBaseModel.DisplayName))
                            stringBuilder.Append(paragraphBaseModel.DisplayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyTranslate:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(paragraphBaseModel.TranslatedValue);
                        if (!string.IsNullOrEmpty(paragraphBaseModel.DisplayName))
                            stringBuilder.Append(paragraphBaseModel.DisplayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.GoToPage:
                    {
                        await PageHelper.PushPage(paragraphBaseModel.PageNumber, paragraphBaseModel.BookId, paragraphBaseModel.CatalogId, PageType.PagesFastRead, this);
                        //await PageHelper.PushPage(0, 0, paragraphBaseModel, PageType.DoLinkPage, this is SearchResultPageViewModel);
                        //await AlertHelper.Alert("قابلیت", "این قابلیت هنوز اضافه نشده!");
                        //PageHelper.PushPage
                        break;
                    }
                case VerseRightClickType.CopyForLink:
                    {
                        TranslatorService.ParagraphForLink = paragraphBaseModel;
                        break;
                    }
                case VerseRightClickType.PasteForLink:
                    {
                        await PageHelper.PushPage(0, 0, paragraphBaseModel, PageType.DoLinkPage, this);

                        //StringBuilder builder = new StringBuilder();
                        //builder.AppendLine("آیا می‌خواهید");
                        //builder.AppendLine(TranslatorService.ParagraphForLink.TranslatedValue);
                        //if (!string.IsNullOrEmpty(TranslatorService.ParagraphForLink.DisplayName))
                        //    builder.AppendLine(TranslatorService.ParagraphForLink.DisplayName);
                        //builder.AppendLine("را به");
                        //builder.AppendLine(paragraphBaseModel.TranslatedValue);
                        //if (!string.IsNullOrEmpty(paragraphBaseModel.DisplayName))
                        //    builder.AppendLine(paragraphBaseModel.DisplayName);
                        //builder.AppendLine("لینک کنید؟");
                        //if (await AlertHelper.DisplayQuestion("لینک", builder.ToString()))
                        //{
                        //    var result = await TranslatorService.GetParagraphService(true).LinkParagraphAsync(TranslatorService.ParagraphForLink.Id, paragraphBaseModel.Id);
                        //    if (result.IsSuccess)
                        //    {
                        //        await AlertHelper.Alert("لینک", "آیه‌ی مورد نظر با موفقیت لینک شد.");
                        //    }
                        //    else
                        //    {
                        //        await AlertHelper.Alert("خطا در لینک", result.Error.Message);
                        //    }
                        //}
                        break;
                    }
                case VerseRightClickType.ShowLinks:
                    {
                        try
                        {
                            IsLoading = true;
                            var result = await TranslatorService.GetParagraphService(true).GetLinkedParagraphsAsync(paragraphBaseModel.Id);
                            if (result.IsSuccess)
                                await PageHelper.PushPage(0, 0, result.Result, PageType.ParagraphResult, this);
                            else
                                await AlertContract(result);
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
            var result = new SearchResultModel()
            {
                Id = value.ParagraphId,
                HasLink = value.HasLink,
                TranslatedValue = string.Join(" ", value.ParagraphWords.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => !x.IsMain && x.Language.Code == "fa-ir").Select(x => x.Value)) + displayName,
                MainValue = string.Join(" ", value.ParagraphWords.OrderBy(x => x.Index).SelectMany(x => x.Values).Where(x => x.IsMain).Select(x => x.Value)) + displayName,
                CatalogId = value.CatalogId,
                BookId = value.BookId,
                PageNumber = value.PageNumber
            };
            return result;
        }
    }
}