using ExtendsLite;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OfficeFileLite
{
    /// <summary>
    /// PDF文档操作类
    /// </summary>
    //------------------------------------调用--------------------------------------------
    //PDFOperation pdf = new PDFOperation();
    //pdf.Open(new FileStream(path, FileMode.Create));
    //pdf.AddParagraph("测试文档（生成时间：" + DateTime.Now + "）", 15, 1, 20, 0, 0);
    //pdf.Close();
    //-------------------------------------------------------------------------------------
    public class PDFOperation
    {

        private Font font;
        private Rectangle rect;   //文档大小
        private Document document;//文档对象
        private BaseFont basefont;//字体

        /// <summary>
        /// 对齐方式
        /// </summary>
        public enum Alignment
        {
            Left = 0,
            Right = 2,
            Center = 1
        }

        public PDFOperation() : this(PageSize.A4, 60, 60, 40, 40) { }

        public PDFOperation(Rectangle pageSize) : this(pageSize, 60, 60, 40, 40) { }

        public PDFOperation(Rectangle pageSize, float marginTop, float marginBottom, float marginLeft, float marginRight)
        {
            rect = pageSize;
            SetBaseFont("font.ttf");
            document = new Document(rect, marginLeft, marginRight, marginTop, marginBottom);
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        public void SetBaseFont(string path)
        {
            basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="size">字体大小</param>
        public void SetFont(float size)
        {
            font = new Font(basefont, size);
        }

        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="os"></param>
        public void Open(Stream os)
        {
            PdfWriter.GetInstance(document, os);
            document.Open();
        }

        /// <summary>
        /// 关闭打开的文档
        /// </summary>
        public void Finish()
        {
            document.Close();
        }

        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="fontsize">字体大小</param>
        public void AddParagraph(string content, float fontsize)
        {
            AddParagraph(content, fontsize, Alignment.Left);
        }

        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="Alignment">对齐方式（1为居中，0为居左，2为居右）</param>
        /// <param name="SpacingAfter">段后空行数（0为默认值）</param>
        /// <param name="SpacingBefore">段前空行数（0为默认值）</param>
        /// <param name="MultipliedLeading">行间距（0为默认值）</param>
        public void AddParagraph(string content, float fontsize, Alignment alignment, float indentWords = -1, float SpacingAfter = 5f, float SpacingBefore = 5f, float MultipliedLeading = 1.5f)
        {
            SetFont(fontsize);
            Paragraph pra = new Paragraph(content, font);
            pra.Alignment = alignment.IntValue();

            pra.SpacingAfter = SpacingAfter;

            pra.SpacingBefore = SpacingBefore;

            pra.MultipliedLeading = MultipliedLeading;            

            if (indentWords == -1)
            {
                indentWords = 2; 
            }

            pra.FirstLineIndent = indentWords * fontsize;

            document.Add(pra);
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="alignment"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public void AddImage(string path, Alignment alignment, float newWidth = 320, float newHeight = 180)
        {
            Image img = Image.GetInstance(path);
            img.Alignment = alignment.IntValue();
            if (newWidth != 0)
            {
                img.ScaleAbsolute(newWidth, newHeight);
            }
            else
            {
                if (img.Width > PageSize.A4.Width)
                {
                    img.ScaleAbsolute(rect.Width, img.Width * img.Height / rect.Height);
                }
            }
            document.Add(img);
        }

        /// <summary>
        /// 添加链接
        /// </summary>
        /// <param name="Content">链接文字</param>
        /// <param name="FontSize">字体大小</param>
        /// <param name="address">链接地址</param>
        public void AddAnchorReference(string Content, string address, float FontSize)
        {
            SetFont(FontSize);
            Anchor auc = new Anchor(Content, font);
            auc.Reference = address;
            document.Add(auc);
        }

        /// <summary>
        /// 添加锚点
        /// </summary>
        /// <param name="Content">锚点文字</param>
        /// <param name="FontSize">字体大小</param>
        /// <param name="Name">锚点</param>
        public void AddAnchorName(string Content, string Name, float FontSize)
        {
            SetFont(FontSize);
            Anchor auc = new Anchor(Content, font);
            auc.Name = Name;
            document.Add(auc);
        }

    }
}