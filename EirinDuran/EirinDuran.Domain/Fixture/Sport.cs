using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Sport
    {
        private string name;
        private StringValidator validator;
        public string Name { get => name; set => SetNameIfValid(value); }

        public Sport(string name)
        {
            validator = new StringValidator();
            Name = name;
        }

        private void SetNameIfValid(string value)
        {
            bool valid = validator.ValidateNotNullOrEmptyString(value);
            if (!valid)
            {
                throw new EmptyFieldException();
            }
            else
            {
                name = value;
            }
        }
    }
}
