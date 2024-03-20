using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyDex
{
    [Serializable]
    public class Word
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlAttribute]
        public string Category { get; set; }

        [XmlElement]
        public string Image { get; set; }
        public Word() { }

        public Word(string name, string description, string category, string image)
        {
            this.Name = name;
            this.Description = description;
            this.Category = category;
            this.Image = image;
        }
    }
}
