using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Firma_tootajate_registreerimissusteem.Models
{
    public class Login
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Parool { get; set; }
        public DayOfWeek TananePaev { get; set; } = DayOfWeek.Monday;
        public TimeSpan PraeguneAeg { get; set; } = new TimeSpan(0, 0, 0);
        public List<Worktime> Worktime { get; set; } = new();
    }
}
