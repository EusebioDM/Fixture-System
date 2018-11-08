using System.Text.RegularExpressions;
using EirinDuran.Domain.User;

namespace EirinDuran.Domain
{
    internal class StringValidator
    {
        public void ValidateNotNullOrEmptyString(string aString)
        {
            if (string.IsNullOrWhiteSpace(aString))
            {
                throw new DomainException($"Field was empty");
            }
        }

        public void ValidateOnlyLettersString(string aString)
        {
            Regex stringLettersOnly = new Regex(@"^[a-zA-ñZäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ ]+$");
            if (!stringLettersOnly.IsMatch(aString))
            {
                throw new DomainException($"String with value {aString} contained invalid elements");
            }
        }

        public void ValidateMailFormat(string mail)
        {
            Regex validMailFormat = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!validMailFormat.IsMatch(mail))
            {
                throw new DomainException($"{mail} is an invalid mail");
            }
        }
    }
}