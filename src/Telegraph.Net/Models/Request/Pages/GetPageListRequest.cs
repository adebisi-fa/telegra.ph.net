using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Telegraph.Net.Models
{
    public class GetPageListRequest
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
