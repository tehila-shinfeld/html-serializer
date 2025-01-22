using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] arrHtmlTags { get; set; }
        public string[] arrHtmlVoidTags { get; set; }

        private HtmlHelper()
        {
            var htmlTagsJson = File.ReadAllText("JSON Files\\HtmlTags.json");
            arrHtmlTags = JsonSerializer.Deserialize<string[]>(htmlTagsJson);

            var htmlVoidTagsJson = File.ReadAllText("JSON Files\\HtmlVoidTags.json");
            arrHtmlVoidTags = JsonSerializer.Deserialize<string[]>(htmlVoidTagsJson);
        }

    }
}
