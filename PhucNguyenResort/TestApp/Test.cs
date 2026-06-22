using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string docxPath = @"e:\SWP391_PROJECT\su26-swp391-se2023-g1\Tailieu\giay-xac-nhan-luu-tru-2_2010172147.docx";
        string outputPath = @"e:\SWP391_PROJECT\su26-swp391-se2023-g1\PhucNguyenResort\docx_content.txt";

        try
        {
            using (ZipArchive archive = ZipFile.OpenRead(docxPath))
            {
                ZipArchiveEntry documentEntry = archive.GetEntry("word/document.xml");
                if (documentEntry == null)
                {
                    Console.WriteLine("Could not find word/document.xml in docx.");
                    return;
                }

                using (Stream stream = documentEntry.Open())
                {
                    XDocument xdoc = XDocument.Load(stream);
                    XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
                    
                    StringBuilder sb = new StringBuilder();
                    foreach (var paragraph in xdoc.Descendants(w + "p"))
                    {
                        StringBuilder paraText = new StringBuilder();
                        foreach (var text in paragraph.Descendants(w + "t"))
                        {
                            paraText.Append(text.Value);
                        }
                        if (paraText.Length > 0)
                        {
                            sb.AppendLine(paraText.ToString());
                        }
                    }
                    
                    File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
                    Console.WriteLine("Successfully written docx content to docx_content.txt");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
