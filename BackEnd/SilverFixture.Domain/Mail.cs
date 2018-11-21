using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SilverFixture.Domain
{
    public struct Mail
    {
        private readonly string pString;

        public Mail(string pString)
        {
            Regex validMailFormat = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!validMailFormat.IsMatch(pString))
            {
                throw new DomainException(pString, "is not a valid mail");
            }
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