namespace Translators.Contracts.Common
{
    public class ErrorContract
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public string StackTrace { get; set; }
    }

    public class MessageContract
    {
        public bool IsSuccess { get; set; }
        public ErrorContract Error { get; set; }

        public static implicit operator MessageContract(bool result)
        {
            return new MessageContract()
            {
                IsSuccess = result,
                Error = result ? null : new ErrorContract()
                {
                    Message = "No details!"
                }
            };
        }

        public static implicit operator MessageContract(string result)
        {
            return new MessageContract()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    Message = result
                }
            };
        }
    }

    public class MessageContract<T> : MessageContract
    {
        public T Result { get; set; }

        public static implicit operator MessageContract<T>(T contract)
        {
            return new MessageContract<T>()
            {
                IsSuccess = true,
                Result = contract
            };
        }

        public MessageContract<TContract> ToContract<TContract>()
        {
            return new MessageContract<TContract>()
            {
                IsSuccess = IsSuccess,
                Error = Error
            };
        }

        public static implicit operator MessageContract<T>((string Result, string Details) data)
        {
            return new MessageContract<T>()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    Message = data.Result,
                    Details = data.Details
                }
            };
        }
    }
}
