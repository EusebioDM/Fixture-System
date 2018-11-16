using System;

namespace EirinDuran.Domain
{
    public struct Name : IEquatable<Name>, IEquatable<string>
    {
        private readonly string pString;

        public Name(string pString)
        {
            StringValidator validator = new StringValidator();
            validator.ValidateNotNullOrEmptyString(pString);
            validator.ValidateOnlyLettersString(pString);
            this.pString = pString;
        }

        public static implicit operator string(Name name)
        {
            return name.pString;
        }

        public override string ToString()
        {
            return pString;
        }

        public static implicit operator Name(string pString)
        {
            return new Name(pString);
        }

        public bool Equals(string other)
        {
            return pString == other;
        }

        public bool Equals(Name other)
        {
            return pString == other.pString;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Name other && Equals(other);
        }
       

        public override int GetHashCode()
        {
            return (pString != null ? pString.GetHashCode() : 0);
        }

        public static bool operator ==(Name left, Name right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Name left, Name right)
        {
            return !left.Equals(right);
        }
    }
}