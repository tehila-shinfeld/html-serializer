using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlElement
    {
//============================Properties==========================
        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> Attributes { get; set; } = new List<string>();

        public List<string> Classes { get; set; } = new List<string>();

        public string InnerHtml { get; set; }

        public HtmlElement Parent { get; set; }

        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

//===============================ctors=============================

        public HtmlElement()
        {
            
        }

        public HtmlElement(string id, string name, List<string> attributes, List<string> classes, string innerHtml, HtmlElement parent, List<HtmlElement> children)
        {
            Id = id;
            Name = name;
            Attributes = attributes;
            Classes = classes;
            InnerHtml = innerHtml;
            Parent = parent;
            Children = children;
        }

//==============================function===========================
        ///פונקציה שמחזירה את כל הצאצאים של אלמנט למטה בקודש
        public IEnumerable<HtmlElement> Descendants()
        {
            var descendantsHelemnts = new Queue<HtmlElement>();
            descendantsHelemnts.Enqueue(this);
            while (descendantsHelemnts.Count > 0)
            {
                var current = descendantsHelemnts.Dequeue();
                //מעביר את השליטה למי שקורא לפונקציה
                //רק מתי שבאמת יצטרכו להשתמש עם הנתונים של האלמנט הזה הוא יכנס לרשימה
                yield return current;

                foreach (var child in current.Children)
                {
                    descendantsHelemnts.Enqueue(child);
                }
            }
        }

        //פונקציה שמחזיר את כל אבות האלמנט למעלה בקודש
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this.Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }


    }
}
