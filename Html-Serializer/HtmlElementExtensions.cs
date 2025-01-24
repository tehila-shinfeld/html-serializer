using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public static class HtmlElementExtensions
    {
        // פונקציית הרחבה למציאת אלמנטים בעץ על פי סלקטור
        public static HashSet<HtmlElement> FindBySelector(this HtmlElement root, Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            FindBySelectorRecursive(root, selector, results);
            return results;
        }

        private static void FindBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            bool isMatch = MatchesSelector(element, selector);

            if (isMatch && selector.Child == null)
            {
                results.Add(element);
            }
            // אם האלמנט מתאים ויש סלקטור בן, המשך לחפש את הבן בתוך הילדים של האלמנט
            if (isMatch && selector.Child != null)
            {
                foreach (var child in element.Children)
                {
                    FindBySelectorRecursive(child, selector.Child, results);
                }
            }

            // המשך לבדוק את הילדים של האלמנט הנוכחי, ללא תלות אם הוא מתאים או לא
            foreach (var child in element.Children)
            {
                FindBySelectorRecursive(child, selector, results);
            }
        }
        // פונקציה לבדיקת התאמה בין אלמנט לבין סלקטור
        private static bool MatchesSelector(HtmlElement element, Selector selector)
        {
            bool matchesTag = selector.TagName == null || Regex.Replace(element.Name, @"class|id", "") == selector.TagName;
            bool matchesId = selector.Id == null || element.Id == selector.Id;
            bool matchesClasses = !selector.Classes.Any() || selector.Classes.All(cls =>
                element.Classes.Any(eClass => string.Equals(eClass, cls, StringComparison.OrdinalIgnoreCase)));

            return matchesTag && matchesId && matchesClasses;
        }

    }
}
