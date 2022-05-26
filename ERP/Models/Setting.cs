using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Setting
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }

}