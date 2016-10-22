using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace HtmlParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBoxHtml.Text.Trim(); //"http://trashripper.ru/publ/0-6";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\parsed.txt";
            StreamWriter sw = null;
            
            //проверка на введенный текст нужно для правильного SelectNodes
            if (textBoxElem.Text.Trim() != String.Empty)
                textBoxElem.Text = "/" + textBoxElem.Text;
            try
            {
                sw = new StreamWriter(@path, true, Encoding.Default);
                if(SetData(sw, url))
                    MessageBox.Show(String.Format("Запись завершена в {0}", path));
            }
            catch (Exception)
            {
                MessageBox.Show(String.Format("Неудача записи в {0}", path));
            }
            finally
            {
                if (sw != null) sw.Close();
            }           
        }

        /// <summary>
        /// Нахождение и запись данных
        /// </summary>
        /// <returns>true если удалось найти и записать</returns>
        bool SetData(StreamWriter sw, string url)
        {
            HtmlDocument HD = new HtmlDocument();
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            HD = web.Load(url);
            //выбирае деревья из класса написанного в textBox и элемента написанного
            HtmlNodeCollection NoAltElements = HD.DocumentNode.SelectNodes("//div[@class='" +
                                                                           textBoxClass.Text.Trim() +
                                                                           "']" + 
                                                                           textBoxElem.Text.Trim());

            if (NoAltElements != null)
            {
                foreach (HtmlNode hn in NoAltElements)
                {
                    string outputText = hn.InnerText.Trim();

                    sw.WriteLine(outputText);
                    sw.WriteLine(sw.NewLine);
                }
            }
            else
            {
                MessageBox.Show("Такого элемента нет!");
                return false;
            }
            return true;
        }
    }
}
