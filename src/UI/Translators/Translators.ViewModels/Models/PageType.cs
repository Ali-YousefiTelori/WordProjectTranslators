namespace Translators.Models
{
    public enum PageType : byte
    {
        None = 0,
        Category = 1,
        Book = 2,
        /// <summary>
        /// sura
        /// </summary>
        Chapter = 3,
        /// <summary>
        /// Ayat
        /// </summary>
        Pages = 4
    }
}
