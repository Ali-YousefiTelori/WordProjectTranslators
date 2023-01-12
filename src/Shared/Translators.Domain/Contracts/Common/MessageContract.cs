﻿using System;
using System.Collections.Generic;

namespace Translators.Contracts.Common
{
    public enum FailedReasonType : byte
    {
        None = 0,
        Default = 1,
        All = 2,
        Other = 3,
        Unknown = 4,
        Nothing = 5,
        SessionAccessDenied = 6,
        AccessDenied = 7,
        InternalError = 8,
        Dupplicate = 9,
        Empty = 10,
        NotFound = 11,
        ValidationsError = 12,
        StreamError = 13
    }

    public class ErrorContract
    {
        public List<ValidationContract> Validations { get; set; } = new List<ValidationContract>();
        public FailedReasonType FailedReasonType { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
        {
            return $"{FailedReasonType}\r\n{Message}\r\n${Details}${StackTrace}$";
        }
    }

    public class MessageContract
    {
        public bool IsSuccess { get; set; }
        public ErrorContract Error { get; set; }

        public virtual object GetResult()
        {
            throw new NotImplementedException();
        }

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

        public static implicit operator bool(MessageContract contract)
        {
            return contract.IsSuccess;
        }

        public static implicit operator MessageContract((FailedReasonType FailedReasonType, string Message) result)
        {
            return new MessageContract()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    Message = result.Message,
                    FailedReasonType = result.FailedReasonType
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

        public override string ToString()
        {
            return $"{IsSuccess}\r\n{Error}";
        }
    }

    public class MessageContract<T> : MessageContract
    {
        public T Result { get; set; }

        public override object GetResult()
        {
            return Result;
        }

        public bool HasResult()
        {
            return IsSuccess && Result is not null;
        }

        public static implicit operator bool(MessageContract<T> contract)
        {
            return contract.IsSuccess;
        }

        public static implicit operator MessageContract<T>(T contract)
        {
            if (contract == null)
            {
                return new MessageContract<T>()
                {
                    IsSuccess = false,
                    Error = new ErrorContract()
                    {
                        FailedReasonType = FailedReasonType.NotFound,
                        StackTrace = Environment.StackTrace,
                        Message = "یافت نشد."
                    }
                };
            }
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

        public static implicit operator MessageContract<T>((FailedReasonType FailedReasonType, string Message) details)
        {
            return new MessageContract<T>()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    FailedReasonType = details.FailedReasonType,
                    StackTrace = Environment.StackTrace,
                    Message = details.Message
                }
            };
        }

        public static implicit operator MessageContract<T>(FailedReasonType failedReasonType)
        {
            return new MessageContract<T>()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    FailedReasonType = failedReasonType,
                    StackTrace = Environment.StackTrace,
                    Message = failedReasonType.ToString()
                }
            };
        }

        public static implicit operator MessageContract<T>(Exception exception)
        {
            return new MessageContract<T>()
            {
                IsSuccess = false,
                Error = new ErrorContract()
                {
                    FailedReasonType = FailedReasonType.InternalError,
                    StackTrace = Environment.StackTrace,
                    Message = exception.Message,
                    Details = exception.ToString()
                }
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

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
