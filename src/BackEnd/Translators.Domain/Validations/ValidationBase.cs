using SignalGo.Shared.DataTypes;
using System.Reflection;

namespace Translators.Validations
{
    public abstract class ValidationBase : ValidationRuleInfoAttribute
    {
        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public override object GetChangedValue()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// property name check
        /// </summary>
        public string PropertyName
        {
            get
            {
                string name = PropertyInfo != null ? PropertyInfo.Name : ParameterInfo.Name;
                return MethodInfo == null ? name : name + $" of method {MethodInfo.Name}";
            }
        }

        /// <summary>
        /// type name
        /// </summary>
        public string TypeName
        {
            get
            {
                return PropertyInfo != null ? $" of type {PropertyInfo.DeclaringType.Name}" : "";
            }
        }
    }
}
