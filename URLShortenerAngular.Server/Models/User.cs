using System.Text.Json.Serialization;
using URLShortenerAngular.Server.Data.Enums;

namespace URLShortenerAngular.Server.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        public DateTime RegisterDate { get; set; }
        public AccessLevelEnum AccessLevel { get; set; }

    }
}
