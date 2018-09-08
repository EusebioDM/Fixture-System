using System.Text.RegularExpressions;

namespace EirinDuran.Domain
{
    public static class Validations
    {

        public static bool ValidateNotNullOrEmptyString(string aString)
        {
            return !string.IsNullOrWhiteSpace(aString);
        }

        public static bool ValidateOnlyLetersString(string aString)
        {
            Regex stringLettersOnly = new Regex(@"^[a-zA-ZäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+$");
            bool isValid = false;

            if (stringLettersOnly.IsMatch(aString))
            {
                isValid = true;
            }

            return isValid;
        }

        public static bool ValidateMailFormat(string mail)
        {
            Regex validMailFormat = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            bool isValid = false;

            if (validMailFormat.IsMatch(mail))
            {
                isValid = true;
            }

            return isValid;
        }
    }
}