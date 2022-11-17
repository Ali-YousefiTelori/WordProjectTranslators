namespace Translators.Schemas
{
    public class FileSchema
    {
        public long Id { get; set; }
        /// <summary>
        /// رمز عبور یک فایل که در هنگام دانلود فایل نیاز است تا فایل باز شود
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// آدرس جهت دانلود فایل
        /// </summary>
        public string Url { get; set; }
    }
}
