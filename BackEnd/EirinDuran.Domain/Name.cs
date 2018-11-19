using System;
using System.Text.RegularExpressions;

namespace EirinDuran.Domain
{
    public struct Name : IEquatable<Name>, IEquatable<string>
    {
        private readonly string pString;

        public Name(string pString)
        {
            if (string.IsNullOrWhiteSpace(pString))
            {
                throw new DomainException( pString, "string was null or empty");
            }
            
            Regex stringLettersOnly = new Regex(@"^[a-zA-ñZäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ ]+$");
            if (!stringLettersOnly.IsMatch(pString))
            {
                throw new DomainException(pString, "string had non letter characters");
            }
            
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