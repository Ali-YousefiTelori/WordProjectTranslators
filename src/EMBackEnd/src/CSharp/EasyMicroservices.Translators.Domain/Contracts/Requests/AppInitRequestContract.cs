namespace Translators.Contracts.Requests
{
    public class AppInitRequestContract
    {
        /// <summary>
        /// کد یکتای کاربر
        /// </summary>
        public string IMEIId { get; set; }
        /// <summary>
        /// کد یک تای وای فای
        /// </summary>
        public string WifiUniqId { get; set; }
        /// <summary>
        /// نسخه ی ای پی آی سیستم عامل
        /// 26
        /// </summary>
        public int OperationSystemApiVersion { get; set; }
        /// <summary>
        /// نسخه ی متنی سیستم عامل
        /// 1.2.6255.558
        /// </summary>
        public string OperationSystemVersion { get; set; }
        /// <summary>
        /// اطلاعات و نام سیستم عامل
        /// Windows 7
        /// Android Lollipop
        /// </summary>
        public string OperationSystemInfo { get; set; }
        /// <summary>
        /// نسخه ی نرم افزار
        /// </summary>
        public int AppVersion { get; set; }
        /// <summary>
        /// نوع سیستم عامل
        /// </summary>
        public OperatingSystemType OperationSystemType { get; set; }
        /// <summary>
        /// پلفترم کلاینت
        /// </summary>
        public PlatformType PlatformType { get; set; }
        /// <summary>
        /// نوع اپلیکیشن
        /// </summary>
        public AppType AppType { get; set; }
        /// <summary>
        /// زبان پیشفرضی که کاربر انتخاب کرده است
        /// </summary>
        public string LanguageCode { get; set; } = "fa-IR";
    }

    /// <summary>
    /// نوع اپلیکیشن
    /// </summary>
    public enum AppType : byte
    {
        None = 0,
        /// <summary>
        /// دیگر
        /// </summary>
        Other = 1,
        /// <summary>
        /// مشتری تجاری
        /// </summary>
        BusinessCustomer = 2,
        /// <summary>
        /// اپلیکیشن کلاینت
        /// </summary>
        EndUser = 3,
        /// <summary>
        /// اپ ادمین
        /// </summary>
        AdminUser = 4,
        /// <summary>
        /// اپ پنل ادمین
        /// </summary>
        AdminPanelUser = 5,
        /// <summary>
        /// تلگرام
        /// </summary>
        BotUser = 6
    }

    /// <summary>
    /// بستر نرم افزاری کاربر
    /// </summary>
    public enum PlatformType : byte
    {
        None = 0,
        /// <summary>
        /// دیگر
        /// </summary>
        Other = 1,
        /// <summary>
        /// موبایل
        /// </summary>
        Mobile = 2,
        /// <summary>
        /// دسکتاپ
        /// </summary>
        Desktop = 3,
        /// <summary>
        /// وبسایت
        /// </summary>
        Web = 4,
    }

    /// <summary>
    /// سیستم عامل
    /// </summary>
    public enum OperatingSystemType : byte
    {
        None = 0,
        /// <summary>
        /// دیگر
        /// </summary>
        Other = 1,
        /// <summary>
        /// ویندوز
        /// </summary>
        Windows = 2,
        /// <summary>
        /// اندروید
        /// </summary>
        Android = 3,
        /// <summary>
        /// مک
        /// </summary>
        Mac = 4,
        /// <summary>
        /// آی او اس
        /// </summary>
        IOS = 5,
        /// <summary>
        /// لینوک
        /// </summary>
        Linux = 6
    }
}
