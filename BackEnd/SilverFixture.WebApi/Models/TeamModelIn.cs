using SilverFixture.Domain.User;
using SilverFixture.IServices.DTOs;
using System.ComponentModel.DataAnnotations;

namespace SilverFixture.WebApi.Models
{
    public class TeamModelIn
    {
        [Required]
        public string Name { get; set; }
        
        public string Logo { get; set; }

        [Required]
        public string SportName { get; set; }

        public TeamModelIn()
        {
        }

        public TeamDTO Map( )
        {
            return new TeamDTO()
            {
                Name = Name,
                Logo = Logo,
                SportName = SportName
            };
        }
    }
}