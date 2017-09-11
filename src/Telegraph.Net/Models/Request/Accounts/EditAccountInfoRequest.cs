using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Telegraph.Net.Models
{
    public class EditAccountInfoRequest
    {
        /// <summary>
        /// Required. Access token of the Telegraph account.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// New account name.
        /// </summary>
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// New default author name used when creating new articles.
        /// </summary>
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        /// <summary>
        /// New default profile link, opened when users click on the author's name below the title. Can be any link, not necessarily to a Telegram profile or channel.
        /// </summary>
        [JsonProperty("author_url")]
        public string AuthorUrl { get; set; }
    }
}
