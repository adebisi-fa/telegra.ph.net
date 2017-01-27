using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Telegraph.Net.Models
{
    public class GetAccountInfoRequest
    {
        /// <summary>
        /// Required. Access token of the Telegraph account.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// List of account fields to return (Default: ["short_name","author_name","author_url"]). Available fields: short_name, author_name, author_url, auth_url, page_count.
        /// </summary>
        public string[] Fields { get; set; }
    }

    [Flags]
    public enum AccountFields
    {
        ShortName = 1,
        AuthorName = 2,
        AuthorUrl = 4,
        AuthorizationUrl = 8,
        PageCount = 16
    }
}
