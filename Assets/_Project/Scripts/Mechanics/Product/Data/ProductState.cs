using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mechanics.Product
{
    [JsonConverter(typeof(ContractStateConverter))]
    public enum ContractState
    {
        None = 0,
        Executing = 1,
        Completed = 2,
        Available = 3,  // in market, not taken
    }

    /// <summary>Handles backward compatibility: "Developing"->Executing, "Released"->Completed</summary>
    public class ContractStateConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var s = reader.Value?.ToString();
                if (s == "Developing") return ContractState.Executing;
                if (s == "Released") return ContractState.Completed;
                if (s == "Available") return ContractState.Available;
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}