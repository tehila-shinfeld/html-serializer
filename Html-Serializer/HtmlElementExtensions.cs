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

        //פונקציית הרחבה למציאת אלמנטים בעץ על פי סלקטור
        // פונקציית הרחבה למציאת אלמנטים בעץ על פי סלקטור
        public static HashSet<HtmlElement> FindBySelector(this HtmlElement root, Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            FindBySelectorRecursive(root, selector, results);
            return results;
        }

        private static void FindBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            // אם האלמנט הנוכחי עונה לקריטריונים של הסלקטור
            if (MatchesSelector(element, selector))
            {
                // הוספת האלמנט לתוצאות
                if (selector.Child == null)
                {
                    results.Add(element);
                }
                else
                {
                    // אם יש סלקטור בן, בודקים את הילדים של האלמנט הנוכחי
                    foreach (var child in element.Children)
                    {
                        FindBySelectorRecursive(child, selector.Child, results);
                    }
                }
            }

            // נמשיך לבדוק את הילדים של האלמנט הנוכחי גם אם הוא עצמו לא מתאים
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
            bool matchesClasses = !selector.Classes.Any() || selector.Classes.All(cls => element.Classes.Contains(cls));

            return matchesTag && matchesId && matchesClasses;
        }
    }



}
