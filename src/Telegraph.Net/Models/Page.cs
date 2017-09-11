using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Telegraph.Net.Models
{
    public class Page
    {
        public string Path { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_url")]
        public string AuthorUrl { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        public List<NodeElement> Content { get; set; }

        public int Views { get; set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }

    }

    public class PageList
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        public List<Page> Pages { get; set; }
    }

    public class PageViews
    {
        public int Views { get; set; }
    }
}
