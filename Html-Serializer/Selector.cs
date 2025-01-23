using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector
    {
        public string TagName { get; set; }

        public string Id { get; set; }

        public List<string> Classes { get; set; } = new List<string>();

        public Selector Parent { get; set; }

        public Selector Child { get; set; }

        //פונקציה סטטית לייצוג אררכית סלקטורים ע"י עץ מנוון של סלקטורים
        public static Selector ParseQueryToSelectorObj(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("The query is not valid.");
                return null;
            }

            // חלוקה לרמות לפי רווחים
            var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Selector root = null;
            Selector current = null;

            foreach (var level in levels)
            {
                var selector = new Selector();

                // חלוקה לפי מפרידים # ו-. (נקודה)
                var parts = Regex.Split(level, @"(?=[#\.])");

                foreach (var part in parts)
                {
                    if (string.IsNullOrWhiteSpace(part)) continue;

                    if (part.StartsWith("#"))
                    {
                        selector.Id = part.Substring(1); // החלק אחרי #
                    }
                    else if (part.StartsWith("."))
                    {
                        selector.Classes.Add(part.Substring(1)); // החלק אחרי .
                    }
                    else
                    {
                        // אם זה לא מתחיל ב-# או . זה כנראה שם תגית
                        if (HtmlHelper.Instance.arrHtmlTags.Contains(part))
                            selector.TagName = part;
                    }
                }

                // יצירת הקשר לעץ הסלקטורים
                if (root == null)
                {
                    root = selector; // השורש של העץ
                }
                else
                {
                    current.Child = selector; // חיבור הילד לסלקטור הנוכחי
                    selector.Parent = current; // חיבור ההורה
                }

                current = selector; // עדכון הסלקטור הנוכחי
            }

            return root;
        }
    }
}