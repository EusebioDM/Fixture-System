using System;
using System.Collections.Generic;
using SilverFixture.Domain.Fixture;

namespace SilverFixture.Domain.User
{
    public class User
    {
        public Role Role { get; set; }
        private ICollection<Team> followedTeams;

        public User()
        {
            followedTeams = new List<Team>();
        }

        public User(string userName) : this()
        {
            UserName = userName;
        }

        public User(Role role, string userName, string name, string surname, string password, string mail) : this(userName)
        {
            Name = name;
            Surname = surname;
            Password = password;
            Mail = mail;
            Role = role;
        }

        public User(Role role, string userName, string name, string surname, string password, string mail, ICollection<Team> followedTeams) : this(role, userName, name, surname, password, mail)
        {
            this.followedTeams = followedTeams;
        }

        public NonEmptyString UserName { get; set; }
        
        public Name Name { get; set; }

        public Name Surname { get; set; }

        public NonEmptyString Password { get; set; }

        public Mail Mail { get; set; }

        public void AddFollowedTeam(Team team)
        {
            followedTeams.Add(team);
        }

        public void RemoveFollowedTeam(Team team)
        {
            followedTeams.Remove(team);

        }

        public IEnumerable<Team> FollowedTeams => followedTeams;

        public override bool Equals(object obj)
        {
            var user = obj as User;
            return user != null &&
                   UserName == user.UserName;
        }

        public override int GetHashCode()
        {
            return -1424944255 + EqualityComparer<string>.Default.GetHashCode(UserName);
        }
    }
}
