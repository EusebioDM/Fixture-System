using System.Collections.Generic;

namespace EirinDuran.Domain
{
    public struct Mail
    {
        private readonly string pString;

        public Mail(string pString)
        {
            StringValidator validator = new StringValidator();
            validator.ValidateNotNullOrEmptyString(pString);
            validator.ValidateMailFormat(pString);
            this.pString = pString;
        }

        public static implicit operator string(Mail mail)
        {
            return mail.pString;
        }

        public static implicit operator Mail(string pString)
        {
            return new Mail(pString);
        }
        
        public override string ToString()
        {
            return pString;
        }

        public override bool Equals(object obj)
        {
            return (obj is string other && pString == other ) || 
                (obj is Mail otherM && pString == otherM.pString);
        }

        public override int GetHashCode()
        {
            return pString.GetHashCode();
        }

        public static bool operator ==(Mail left, Mail right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Mail left, Mail right)
        {
            return !left.Equals(right);
        }
    }
}