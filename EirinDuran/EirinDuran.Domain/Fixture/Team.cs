using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace EirinDuran.Domain.Fixture
{
    public class Team
    {
        public string Name { get => name; set => SetNameIfValid(value); }
        public Guid Id { get; private set; }
        private string name;
        public Image Logo { get; set; }
        private StringValidator validator;

        public Team(string name)
        {
            validator = new StringValidator();
            Name = name;
            Id = Guid.Empty;
            Logo = GetDefaultImage();
        }

        private Image GetDefaultImage()
        {
            Bitmap bitmap = new Bitmap(500, 500);
            return bitmap;
        }

        public Team(string name, Image logo) : this(name)
        {
            Logo = logo;
        }

        public Team(string name, Guid id, Image logo) : this(name,logo)
        {
            Id = id;
        }

        private void SetNameIfValid(string value)
        {
            bool valid = validator.ValidateNotNullOrEmptyString(value);
            if (!valid)
                throw new EmptyFieldException();
            else
                name = value;
        }

        public override bool Equals(object obj)
        {
            var team = obj as Team;
            return team != null &&
                   Name == team.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
