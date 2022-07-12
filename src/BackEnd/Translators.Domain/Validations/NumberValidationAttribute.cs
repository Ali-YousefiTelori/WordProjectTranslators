using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;

namespace Translators.Validations
{
    public class NumberValidationAttribute : ValidationRuleInfoAttribute
    {
        public override bool CheckIsValidate()
        {
            if (CurrentValue == null || (CurrentValue is long l && l == 0) || (CurrentValue is int i && i == 0))
                return false;
            return true;
        }

        public override object GetChangedValue()
        {
            throw new NotImplementedException();
        }

        public override object GetErrorValue()
        {
            return new MessageContract()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    Message = $"Validation error on {CurrentValue}"
                }
            };
        }
    }
}
