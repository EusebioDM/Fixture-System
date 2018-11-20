namespace EirinDuran.WebApi.Models
{
    public class TeamPosition
    {
        public string TeamId { get; set; }
        public int Position { get; set; }

        public TeamPosition()
        {
            
        }

        public TeamPosition(string teamId, int position)
        {
            TeamId = teamId;
            Position = position;
        }
    }
}