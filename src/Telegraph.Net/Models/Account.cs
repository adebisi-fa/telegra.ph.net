using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Telegraph.Net.Models
{
    public class Account
    {
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_url")]
        public string AuthorUrl { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("auth_url")]
        public string AuthorizationUrl { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }
    }
}
