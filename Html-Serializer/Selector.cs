using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Console.WriteLine("The query its not ok");
            }
            var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Selector root = null;
            Selector current = null;

            foreach (var level in levels)
            {
                var selector = new Selector();
                var arrParts = level.Split(new[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);
                var separators = level.Where(c => c == '#' || c == '.').ToArray();

                // עיבוד החלקים
                for (int i = 0; i < arrParts.Length; i++)
                {
                    // המקרה הראשון - הטאג הראשי
                    if (i == 0 && (separators.Length == 0 || separators[0] != '#' && separators[0] != '.'))
                    {
                        if (HtmlHelper.Instance.arrHtmlTags.Contains(arrParts[i]))
                            selector.TagName = arrParts[i];
                    }
                    else if (i > 0 && separators[i - 1] == '#') // בדוק ש-i גדול מ-0
                    {
                        selector.Id = arrParts[i];
                    }
                    else if (i > 0 && separators[i - 1] == '.') // בדוק ש-i גדול מ-0
                    {
                        selector.Classes.Add(arrParts[i]);
                    }
                }
                // חיבור לעץ
                if (root == null)
                {
                    root = selector;
                }
                else
                {
                    current.Child = selector;
                    selector.Parent = current;
                }

                // עדכון הסלקטור הנוכחי
                current = selector;
            }

            return root;
        }

        internal static object ParseQueryToSelectorObj()
        {
            throw new NotImplementedException();
        }
    }
}