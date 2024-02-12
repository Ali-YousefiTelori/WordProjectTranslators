using System;
using System.Collections.Generic;
using TranslatorApp.GeneratedServices;

namespace Translators.Models
{
    public enum ServiceType : byte
    {
        None = 0,
        Category = 1,
        Book = 2
    }

    public class CategoryModel : BaseModel
    {
        public long Id { get; set; }
        public List<ValueContract> Names { get; set; }
        public ServiceType Type { get; set; }

        public Action SelectionChanged { get; set; }
        bool _IsSelected = true;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                SelectionChanged?.Invoke();
            }
        }

        public static implicit operator CategoryModel(CategoryContract category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Names = category.Names?.ToList(),
                Type = ServiceType.Category
            };
        }

        public static implicit operator CategoryModel(BookContract book)
        {
            return new CategoryModel()
            {
                Id = book.Id,
                Names = book.Names?.ToList(),
                Type = ServiceType.Book
            };
        }
    }
}
