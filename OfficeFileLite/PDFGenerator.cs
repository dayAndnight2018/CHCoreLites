using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OfficeFileLite
{
    public class PDFGenerator
    {
        public static Stream GeneratePDF(Stream modelFile, Dictionary<string, string> contents)
        {
            BaseFont baseFont = BaseFont.CreateFont("font.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            PdfReader reader = new PdfReader(modelFile);

            MemoryStream stream = new MemoryStream();
            PdfStamper stamp = new PdfStamper(reader, stream);
            AcroFields form = stamp.AcroFields;
            stamp.FormFlattening = true;

            foreach (KeyValuePair<string, string> parameter in contents)
            {
                form.SetFieldProperty(parameter.Key, "textfont", baseFont, null);
                form.SetField(parameter.Key, parameter.Value);
            }

            stamp.Close();
            reader.Close();
            return stream;
        }


        public static void GeneratePDF(string modelFile, Dictionary<string, string> contents, string outputPath)
        {
            BaseFont baseFont = BaseFont.CreateFont("font.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            PdfReader reader = new PdfReader(modelFile);

            MemoryStream stream = new MemoryStream();
            PdfStamper stamp = new PdfStamper(reader, stream);
            AcroFields form = stamp.AcroFields;
            stamp.FormFlattening = true;

            foreach (KeyValuePair<string, string> parameter in contents)
            {
                form.SetFieldProperty(parameter.Key, "textfont", baseFont, null);
                form.SetField(parameter.Key, parameter.Value);
            }

            stamp.Close();
            reader.Close();

            FileStream fs = new FileStream(outputPath, FileMode.Create);
            var array = stream.ToArray();
            fs.Write(array, 0, array.Length);
            fs.Flush();
            stream.Close();
            fs.Close();
        }
    }
}
