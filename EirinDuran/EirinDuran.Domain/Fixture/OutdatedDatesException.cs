using System;
using System.Runtime.Serialization;

namespace EirinDuran.Domain.Fixture
{
    public class OutdatedDatesException : InvalidDateException
    {
        public override string Message
        {
            get { return "The start date is greater than the end date."; }
        }
    }
}