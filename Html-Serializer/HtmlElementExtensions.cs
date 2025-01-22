using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public static class HtmlElementExtensions
    {
        // מי האלמטים שעונים לקריטריונים של הסלקטור כלומר תגיות שיש להם את הסלקטור המבוקש
        public static HashSet<HtmlElement> FindBySelector(this HtmlElement root, Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            FindBySelectorRecursive(root, selector, results);
            return results;
        }

        private static void FindBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            // קבלת כל הצאצאים של האלמנט הנוכחי
            var elementDescendants = element.Descendants() ?? Enumerable.Empty<HtmlElement>();
            //Console.WriteLine($"Element: {element.Name}, Descendants Count: {elementDescendants.Count()}, Descendants: {string.Join(", ", elementDescendants.Select(e => e.Name))}");

            //סינון הצאצאים לפי הקריטריונים של הסלקטור הנוכחי

            var filteredDescendants = elementDescendants.Where(e =>
            {
                bool matchesTag = selector.TagName == null || e.Name == selector.TagName;
                bool matchesId = selector.Id == null || e.Id == selector.Id;
                bool matchesClasses = !selector.Classes.Any() || selector.Classes.All(cls => e.Classes.Contains(cls));

                //Console.WriteLine($"Element: {e.Name}, Matches Tag: {matchesTag}, Matches ID: {matchesId}, Matches Classes: {matchesClasses}");

                return matchesTag && matchesId && matchesClasses;
            });
            //תנאי עצירה של העץ המנוון...
            if (selector.Child == null)
            {
                if ((selector.TagName == null || element.Name == selector.TagName) &&
                   (selector.Id == null || element.Id == selector.Id) &&
                   (!selector.Classes.Any() || selector.Classes.All(cls => element.Classes.Contains(cls))))
                {
                    results.Add(element);
                }
                else
                {
                    results.UnionWith(filteredDescendants); // הוספת כל האלמנטים המסוננים
                }
            }
            else
            {
                // המשך ריקורסיה: מעבר על האלמנטים המסוננים עם הסלקטור הבא
                foreach (var child in filteredDescendants)
                {
                    FindBySelectorRecursive(child, selector.Child, results);
                }
            }
        }
    }
}