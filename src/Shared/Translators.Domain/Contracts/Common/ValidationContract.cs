namespace Translators.Contracts.Common
{
    public class ValidationContract
    {
        public string Message { get; set; }
        public override string ToString()
        {
            return Message;
        }

        //public static implicit operator ValidationContract(BaseValidationRuleInfoAttribute baseValidationRuleInfo)
        //{
        //    return (ValidationContract)BaseValidationRuleInfoAttribute.GetErrorValue(baseValidationRuleInfo);
        //}
    }
}
