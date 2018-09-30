using System;

namespace EirinDuran.IServices.Exceptions
{
    public class EncounterWithOverlappingDatesException : EncounterServicesException
    {
        public override string Message
        {
            get { return "There are two encounters for the same team on the same date."; }
        }
    }
}