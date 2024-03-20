using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Windows;



namespace MyDex
{
    [Serializable]
    public class WordsList
    {
        [XmlArray]
        public ObservableCollection<Word> Words { get; set; }

        [XmlIgnore]
        public ObservableCollection<Word> FilterWords { get; set; }

        [XmlIgnore]
        public ObservableCollection<string> m_Category { get; set; }

        [XmlIgnore]
        public Word SelectedWord { get; set; }
        public WordsList() 
        {
            Words = new ObservableCollection<Word>();
            FilterWords = new ObservableCollection<Word>();
            m_Category = new ObservableCollection<string>();
        }

        public void readElements()
        {
            string fileName = "saveWords.xml";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            if (File.Exists(filePath))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(WordsList));
                FileStream file = new FileStream("saveWords.xml", FileMode.Open);
                WordsList reader = (xmlser.Deserialize(file) as WordsList);
                foreach (var word in new ObservableCollection<Word>(reader.Words))
                {
                    Words.Add(word);
                }
                foreach (var word in new ObservableCollection<Word>(reader.Words))
                {
                    FilterWords.Add(word);
                }
                foreach (var word in Words)
                {
                    if (!m_Category.Contains(word.Category))
                    {
                        m_Category.Add(word.Category);
                    }
                }
                var categoryBuffer = new ObservableCollection<string>(m_Category.OrderBy(s => s));
                m_Category.Clear();
                foreach (var category in categoryBuffer)
                {
                    m_Category.Add(category);
                }
                m_Category.Insert(0, "None");
                file.Dispose();
            }
        }
        public void saveElementsInXML()
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(WordsList));
            FileStream file = new FileStream("saveWords.xml", FileMode.Create);
            xmlSer.Serialize(file, this);
            file.Dispose();
        }

        public void AddWord(Word word) 
        { 
            Words.Add(word);
            if (!m_Category.Contains(word.Category))
            {
                m_Category.Add(word.Category);
            }
            FilterWords.Clear();
            foreach (var w in new ObservableCollection<Word>(Words.OrderBy(cuv => cuv.Name)))
            {
               FilterWords.Add(w);
            }
            Words.Clear();
            foreach (var fWord in FilterWords)
            {
                Words.Add(fWord);
            }
            saveElementsInXML();
        }
        public bool IsInWords(string nume)
        {
            foreach (Word word in Words)
            {
                if (word.Name.Equals(nume))
                    return true;
            }
            return false;
        }

        public Word GetWord(string wordName)
        {
            foreach (var word in Words)
            {
                if (word.Name.ToLower() == wordName.ToLower())
                {
                    return word;
                }
            }
            return null;
        }

        public void DeleteWord(string nume)
        {
            Word deleteWord = new Word(); 
            foreach (Word word in Words)
            {
                if (word.Name.Equals(nume))
                    deleteWord = word;
            }
            Words.Remove(deleteWord);
            FilterWords.Remove(deleteWord);
            if (deleteWord.Image != "Load Image") 
            saveElementsInXML();
            
        }
        public void filter(string filterString, string Category = null)
        {
            IEnumerable<Word> filteredWords;

            if (Category == "None" || string.IsNullOrEmpty(Category))
            {
                filteredWords = Words.Where(w => w.Name.StartsWith(filterString, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                filteredWords = Words.Where(w => w.Name.StartsWith(filterString, StringComparison.CurrentCultureIgnoreCase) && (Category == null || Category == "" || w.Category == Category));
            }

            FilterWords.Clear();
            foreach (var word in filteredWords)
            {
                FilterWords.Add(word);
            }
        }


    }
}
