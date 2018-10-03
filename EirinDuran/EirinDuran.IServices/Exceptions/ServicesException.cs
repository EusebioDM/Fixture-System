﻿using System;

namespace EirinDuran.IServices.Exceptions
{
    public class ServicesException : Exception
    {
        public ServicesException()
        {
        }

        public ServicesException(string message) : base()
        {
        }

        public ServicesException(string message, Exception innerException) : base()
        {
        }
    }
}