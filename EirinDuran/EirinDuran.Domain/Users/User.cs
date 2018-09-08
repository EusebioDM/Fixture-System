using System;

namespace EirinDuran.Domain.User
{
    public class User
    {
        private string userName;
        private string name;
        private string surname;
        private string password;
        private string mail;
        private StringValidator validator;

        public User()
        {
        }

        public User(Role role, string userName, string name, string surname, string password, string mail)
        {
            UserName = userName;
            Name = name;
            Surname = surname;
            Password = password;
            Mail = mail;
            Role = role;
        }

        private Role Role { get; set; }
        public string UserName {
            get {
                return userName;
            }

            protected set {
                if (validator.ValidateNotNullOrEmptyString(value))
                {
                    userName = value;
                }
                else
                {
                    throw new EmptyFieldException();
                }
            }
        }

        public string Name {
            get { return name; }

            set {
                if (validator.ValidateOnlyLetersString(value))
                {
                    name = value.ToUpper();
                }
                else
                {
                    throw new InvalidCharactersFieldExcepion();
                }
            }
        }

        public string Surname {
            get { return surname; }

            set {
                if (validator.ValidateOnlyLetersString(value))
                {
                    surname = value.ToUpper();
                }
                else
                {
                    throw new InvalidCharactersFieldExcepion();
                }
            }
        }

        public string Password {
            get { return password; }

            set {
                if (validator.ValidateNotNullOrEmptyString(value))
                {
                    password = value;
                }
                else
                {
                    throw new EmptyFieldException();
                }
            }
        }

        public string Mail {
            get { return mail; }
            set {
                if (validator.ValidateMailFormat(value))
                {
                    mail = value;
                }
                else
                {
                    throw new InvalidMailFormatException();
                }
            }
        }

        public bool HasRole(Role role)
        {
            return (Role == role);
        }
    }
}
