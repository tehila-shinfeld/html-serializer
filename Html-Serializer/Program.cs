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
static string CleanHtml(string input)
{
    if (string.IsNullOrWhiteSpace(input))
        return string.Empty;

    // 1. מסיר רווחים בתחילת וסוף כל שורה
    input = Regex.Replace(input, @"^\s+|\s+$", "", RegexOptions.Multiline);

    // 2. מחליף רצף של רווחים (כולל טאב) ברווח בודד
    input = Regex.Replace(input, @"[ \t]+", " ");

    // 3. מוחק שורות ריקות (כולל שורות שמכילות רק רווחים)
    input = Regex.Replace(input, @"^\s*\n", "", RegexOptions.Multiline);

    return input;
}
static List<string> arrangeHtml(string html)
{

    var cleanHtml = CleanHtml(html);

    var htmlLines = new Regex("<(.*?)>")
        .Split(cleanHtml)
        .Select(s => s.Trim()) // מסיר רווחים מכל פריט אחרי הפיצול
        .Where(s => !string.IsNullOrWhiteSpace(s)) // מסיר שורות ריקות
        .ToList();
    return htmlLines;
}
var html = await Load("https://hebrewbooks.org/");
var cleanHtml = CleanHtml(html);
var htmlLines =arrangeHtml(cleanHtml);
//===============================================
HtmlTreeBuilder htmlTreeBuilder = new HtmlTreeBuilder();
var root = htmlTreeBuilder.BuildTree(htmlLines);
//פרק את השאילתה לסלקטור
var selector = Selector.ParseQueryToSelectorObj(".keyboard");
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



