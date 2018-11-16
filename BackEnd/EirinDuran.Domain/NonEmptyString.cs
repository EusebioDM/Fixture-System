using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace EirinDuran.Domain
{
    public struct NonEmptyString : IEquatable<NonEmptyString>, IEquatable<string>
    {
        private readonly string pString;

        public NonEmptyString(string pString)
        {
            StringValidator validator = new StringValidator();
            validator.ValidateNotNullOrEmptyString(pString);
            this.pString = pString;
        }

        public static implicit operator string(NonEmptyString nonEmptyString)
        {
            return nonEmptyString.pString;
        }
        
        public override string ToString()
        {
            return pString;
        }

        public static implicit operator NonEmptyString(string pString)
        {
            return new NonEmptyString(pString);
        }

        public bool Equals(string other)
        {
            return pString == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is NonEmptyString other && Equals(other) ||
                   obj is string otherS && otherS == pString;
        }

        public bool Equals(NonEmptyString other)
        {
            return string.Equals(pString, other.pString);
        }

        public override int GetHashCode()
        {
            return (pString != null ? pString.GetHashCode() : 0);
        }

        public static bool operator ==(NonEmptyString left, NonEmptyString right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NonEmptyString left, NonEmptyString right)
        {
            return !left.Equals(right);
        }
    }
}