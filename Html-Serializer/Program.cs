using Html_Serializer;
using System.Text.RegularExpressions;
using System.Xml.Linq;
static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
var html = await Load("https://hebrewbooks.org/");
// המחרוזת המקורית
//var html = "<div id=\"root\">\r\n " +
//    "   <div class=\"container\">\r\n " +
//    "   <div class=\"item\"></div>\r\n     " +
//    "   <div class=\"item\"></div>\r\n    </div>\r\n  " +
//    "   <div class=\"container\">\r\n     " +
//"   <div class=\"item\"></div>\r\n  " +
//    "  </div>\r\n</div>";
//var htmlWithoutSpace = new Regex("\\s").Replace(html, "");
//var htmlWithoutSpace = new Regex("\\s").Replace(html, "");
//var htmlLines = new Regex("<(.*?)>").Split(htmlWithoutSpace).Where(s => s.Length > 0).ToList();
var htmlCleaned = string.Join("\n", html
    .Split('\n')
    .Select(line => line.Trim())
    .Where(line => !string.IsNullOrWhiteSpace(line)));
// שימוש ב-Regex לתיקון המחרוזת
var htmlLines = new Regex("<(\\w+)([a-zA-Z0-9=\"\\s.-]*)>")
    .Matches(htmlCleaned)
    .Cast<Match>()
    .Select(m => $"<{m.Groups[1].Value} {m.Groups[2].Value.Trim()}>")
    .ToList();
//===============================================
//בנית העץ
HtmlTreeBuilder htmlTreeBuilder = new HtmlTreeBuilder();
var root = htmlTreeBuilder.BuildTree(htmlLines);
//פרק את השאילתה לסלקטור
var selector = Selector.ParseQueryToSelectorObj("td");

// חפש את הסלקטור בעץ
var matchingElements = root.FindBySelector(selector);

// בדוק אם הסלקטור נמצא
if (matchingElements.Any())
{
    Console.WriteLine("Selector found in HTML!: " + matchingElements.Count);

    foreach (var element in matchingElements)
    {
        Console.WriteLine($"tag: {element.Name}, Id: {element.Id}, Classes: {string.Join(", ", element.Classes)}");
    }
}
else
{
    Console.WriteLine("Selector not found in HTML.");
}