using System;
using System.Collections.Generic;

namespace EirinDuran.Domain.User
{
    public class User
    {
        public Role Role { get; set; }
        public Guid Id { get; private set; }
        private string userName;
        private string name;
        private string surname;
        private string password;
        private string mail;
        private StringValidator validator;

        public User(string userName)
        {
            validator = new StringValidator();
            UserName = userName;
        }

        public User(Role role, string userName, string name, string surname, string password, string mail) : this(userName)
        {
            Name = name;
            Surname = surname;
            Password = password;
            Mail = mail;
            Role = role;
            Id = Guid.Empty;
        }

        public User(Role role, Guid id, string userName, string name, string surname, string password, string mail) : this(role,userName, name, surname, password,mail)
        {
            Id = id;
        }

        public string UserName {
            get {
                return userName;
            }

            set {
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

        public override bool Equals(object obj)
        {
            var user = obj as User;
            return user != null &&
                   userName == user.userName;
        }

        public override int GetHashCode()
        {
            return -1424944255 + EqualityComparer<string>.Default.GetHashCode(userName);
        }
    }
}
