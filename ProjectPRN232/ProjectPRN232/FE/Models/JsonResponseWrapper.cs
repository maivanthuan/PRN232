using System.Text.Json.Serialization;

namespace FE.Models
{
    public class JsonResponseWrapper<T>
    {
        [JsonPropertyName("$values")]
        public List<T> Values { get; set; } = new List<T>();
    }
}
