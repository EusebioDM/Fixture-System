using System;

namespace SilverFixture.IServices.Exceptions
{
    public class EncounterWithOverlappingDatesException : ServicesException
    {
        public override string Message
        {
            get { return "There are two encounters for the same team on the same date."; }
        }
    }
}