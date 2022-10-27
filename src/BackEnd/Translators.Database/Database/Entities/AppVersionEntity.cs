﻿using Translators.Contracts.Common.DataTypes;

namespace Translators.Database.Entities
{
    public class AppVersionEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// نوع نرم افزار
        /// </summary>
        public ApplicationType ApplicationType { get; set; }
        /// <summary>
        /// نسخه ی موجود اپ
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// حتما باید به نسخه ی جدید بروزرسانی شود
        /// </summary>
        public int ForceUpdateNumber { get; set; }
        /// <summary>
        /// قبل از این نسخه باید کش کلاینت باید پاک شود
        /// </summary>
        public int CleanCacheTempNumber { get; set; }
    }
}
