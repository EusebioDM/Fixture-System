using System;
using System.Runtime.Serialization;

namespace EirinDuran.Services
{
    public class EncounterWithOverlappingDatesException : Exception
    {
        public override string Message
        {
            get { return "There are two encounters for the same team on the same date."; }
        }
    }
}