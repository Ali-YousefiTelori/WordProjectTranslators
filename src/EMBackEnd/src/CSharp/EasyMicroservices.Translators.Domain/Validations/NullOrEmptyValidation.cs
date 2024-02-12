using System;
using System.Collections;
using System.Reflection;
using Translators.Contracts.Common;

namespace Translators.Validations
{
    public class NullOrEmptyValidation : ValidationBase
    {
        ///// <summary>
        ///// validation check
        ///// check null/empty data & list count
        ///// check min datetime value
        ///// </summary>
        ///// <returns></returns>
        //public override bool CheckIsValidate()
        //{
        //    var type = PropertyInfo == null ? ParameterInfo.ParameterType : PropertyInfo.PropertyType;

        //    // check strign length
        //    if (CurrentValue == null || string.IsNullOrEmpty(CurrentValue.ToString()) || (CurrentValue is Enum && CurrentValue.ToString() == "0") || CurrentValue.Equals(GetDefault(type)))
        //        return false;
        //    // check list count
        //    else if (CurrentValue is ICollection list && list.Count == 0)
        //        return false;
        //    // check datetime
        //    else if (CurrentValue is DateTime date && date == DateTime.MinValue)
        //        return false;
        //    else if (CurrentValue is Guid guid && guid == Guid.Empty)
        //        return false;
        //    return true;
        //}

        ///// <summary>
        ///// when a validation error occured
        ///// </summary>
        ///// <returns></returns>
        //public override object GetErrorValue()
        //{
        //    return new ValidationContract()
        //    {
        //        Message = $"Please fill property {PropertyName} {TypeName}"
        //    };
        //}
    }
}