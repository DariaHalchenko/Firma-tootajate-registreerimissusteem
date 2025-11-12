using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Firma_tootajate_registreerimissusteem.Models
{
    public class Register
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Nimi { get; set; }
        public string Email { get; set; }
        public string Parool { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false; // false - работник, true - администратор
    }
}