using System.Text.Json.Serialization;

namespace Firma_tootajate_registreerimissusteem.Models
{
    public class Worktime
    {
        [JsonIgnore]
        public int Id { get; set; }
        public DayOfWeek Nadalapaev { get; set; }
        public string? ToopaevaAlgus { get; set; } = "00:00:00";
        public string? ToopaevaLopp { get; set; } = "00:00:00";
        public int RegisterId { get; set; }
        [JsonIgnore]
        public Register? Register { get; set; }
    }
}
