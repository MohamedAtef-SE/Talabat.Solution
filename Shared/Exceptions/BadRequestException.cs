﻿using Talabat.Shared.Errors;

namespace Talabat.Shared.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
