﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlTreeBuilder
    {
        private string GetFirstWord(string line)
        {
            var match = Regex.Match(line, @"^/?\w+");
            return match.Success ? match.Value : string.Empty;
        }

        private bool IsSelfClosingTag(string tagName, string line)
        {
            return HtmlHelper.Instance.arrHtmlVoidTags.Contains(tagName) || line.EndsWith("/>");
        }

        private HtmlElement CreateElement(string line, string tagName)
        {
            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
            List<string> attributeList = new List<string>();

            foreach (Match match in attributes)
            {
                string attributeString = $"{match.Groups[1].Value}=\"{match.Groups[2].Value}\"";
                attributeList.Add(attributeString);
            }

            var classes = attributeList
            .Where(attr => attr.StartsWith("class=\""))
            .Select(attr => attr.Substring(7, attr.Length - 8)) 
            .ToList(); 

            var id = attributeList
            .Where(attr => attr.StartsWith("id=\"")) 
            .Select(attr => attr.Substring(4, attr.Length - 5)) 
            .FirstOrDefault(); 

            return new HtmlElement
            {
                Name = tagName,
                Attributes = attributeList,
                Classes = classes,
                Id = id
            };
        }

        public  HtmlElement BuildTree(List<string> htmlLines)
        {

            var root = new HtmlElement { Name = "root" }; // אלמנט שורש מדומה
            var currentElement = root; // האובייקט הנוכחי בלולאה
            var stack = new Stack<HtmlElement>(); // מחסנית למעקב אחרי אלמנטים פתוחים
            stack.Push(root);

            foreach (var line in htmlLines)
            {
                // חילוץ שם התווית או קבלת מחרוזת ריקה
                var firstWord = GetFirstWord(line);

                if (firstWord == "html/")
                {
                    break; // סוף העץ
                }

                if (firstWord.StartsWith("/"))
                {
                    // תגית סוגרת - חוזרים לאבא
                    if (stack.Count > 1)
                    {
                        stack.Pop();
                        currentElement = stack.Peek();
                    }
                }

                else if (HtmlHelper.Instance.arrHtmlTags.Contains(firstWord))
                {
                    // תגית חדשה - יצירת אלמנט
                    var newElement = CreateElement(line, firstWord);
                    currentElement.Children.Add(newElement);
                    newElement.Parent = currentElement;

                    // אם זה לא תגית סוגרת עצמית, היא יכולה להכיל ילדים
                    if (!IsSelfClosingTag(firstWord, line))
                    {
                        stack.Push(newElement);
                        currentElement = newElement;
                    }
                }
                else
                {
                    // טקסט פנימי
                    currentElement.InnerHtml += line.Trim();
                }
            }
            return root;
        }
    }
}
