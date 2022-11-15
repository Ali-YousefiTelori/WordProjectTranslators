using SignalGo.Shared.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translators.Contracts.Common
{
    public class ValidationContract
    {
        public string Message { get; set; }
        public override string ToString()
        {
            return Message;
        }

        public static implicit operator ValidationContract(BaseValidationRuleInfoAttribute baseValidationRuleInfo)
        {
            return (ValidationContract)BaseValidationRuleInfoAttribute.GetErrorValue(baseValidationRuleInfo);
        }
    }
}
