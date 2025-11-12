using System.Text.Json.Serialization;

namespace Firma_tootajate_registreerimissusteem.Models
{
    public class Worktime
    {
        public int Id { get; set; }
        public DayOfWeek Paev { get; set; }
        public string? AvatudAlates { get; set; } = "00:00:00";
        public string? AvatudKuni { get; set; } = "00:00:00";
        public int KasutajaId { get; set; }
        [JsonIgnore]
        public Login? Login { get; set; }
    }
}
