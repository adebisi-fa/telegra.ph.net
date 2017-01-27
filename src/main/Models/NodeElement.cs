using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Telegraph.Net.Models
{
    public class NodeElement
    {
        public string Tag { get; set; }

        [JsonProperty("attrs")]
        public Dictionary<string, string> Attributes { get; set; }

        public List<NodeElement> Children { get; set; }

        public NodeElement()
        {
            Attributes = new Dictionary<string, string>();
        }

        public NodeElement (string tag, Dictionary<string, string> attributes, params NodeElement[] children)
        {
            Tag = tag;
            Attributes = attributes;
            Children = children.ToList();
        }

        public NodeElement(string text) : this()
        {
            Tag = "_text";
            Attributes.Add("value", text);
        }

        public static implicit operator string(NodeElement node) => node.Tag == "_text" ? node.Attributes["value"] : null;

        public static implicit operator NodeElement(string text) => string.IsNullOrEmpty(text) ? null : new NodeElement(text);

        public dynamic ToRequestObject()
        {
            if (Tag == "_text")
                return (string) this;
            else
            {
                return new
                {
                    tag = Tag,
                    attrs = Attributes,
                    children = Children?.Select(c => c?.ToRequestObject()).Where(c => c != null)
                };
            }
        }
    }

    public static class NodeElementExtension
    {
        public static IEnumerable<dynamic> ToRequestObjects(this IEnumerable<NodeElement> nodeElements)
        {
            return nodeElements?.Select(n => n?.ToRequestObject()).Where(n => n != null);
        }
    }
}
