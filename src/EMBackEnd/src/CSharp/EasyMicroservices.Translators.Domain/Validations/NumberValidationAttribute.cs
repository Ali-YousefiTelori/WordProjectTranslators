﻿using System;

namespace Translators.Validations
{
    public class NumberValidationAttribute : Attribute
    {
        //public override bool CheckIsValidate()
        //{
        //    if (CurrentValue == null || (CurrentValue is long l && l == 0) || (CurrentValue is int i && i == 0))
        //        return false;
        //    return true;
        //}

        //public override object GetChangedValue()
        //{
        //    throw new NotImplementedException();
        //}

        //public override object GetErrorValue()
        //{
        //    return new ValidationContract()
        //    {
        //        Message = $"Validation error on {CurrentValue}"
        //    };
        //}
    }
}
