using System;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.IO;
using System.Text;

namespace RecifyLite
{
    public class RecifyCodeGenerator
    {
        private static readonly string DIGIT = "0123456789";
        private static readonly string LETTER = "abcdefghijklmnopqrstuvwyxz";
        private static readonly string DIGIT_OR_LETTER = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static string Generate(int len, string pattern)
        {
            if (len < 0)
            {
                throw new ArgumentException("invalid len");
            }
            
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                sb.Append(pattern[rand.Next(pattern.Length)]);
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// 生成指定长的随机数
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>指定长度的随机数</returns>
        public static String GenerateRandomNumberString(int len)
        {
            return Generate(len, DIGIT);
        }

        /// <summary>
        /// 产生指定长度的数字字母混合字符串
        /// </summary>
        /// <param name="size">字符串长度</param>
        /// <returns></returns>
        public static string GenerateRandomString(int len)
        {
            return Generate(len, DIGIT_OR_LETTER);
        }

        /// <summary>
        /// 生成指定长度的字母字符串
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>随机字符串</returns>
        public static string GenerateRandomLetterString(int len)
        {
            return Generate(len, LETTER);
        }

        /// <summary>
        /// 生成指定大小的验证码图片
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>验证码图片</returns>
        public static Bitmap GenerateRecifyImage(string verifyCode, int width = 150, int height = 50)
        {
            Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
            Brush brush;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            SizeF totalSizeF = g.MeasureString(verifyCode, font);
            SizeF curCharSizeF;

            PointF startPointF = new PointF(0, (height - totalSizeF.Height) / 2);
            Random random = new Random(); //随机数产生器
            g.Clear(Color.White); //清空图片背景色  

            for (int i = 0; i < verifyCode.Length; i++)
            {
                brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                g.DrawString(verifyCode[i].ToString(), font, brush, startPointF);
                curCharSizeF = g.MeasureString(verifyCode[i].ToString(), font);
                startPointF.X += curCharSizeF.Width;
            }

            //画图片的干扰线  
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(bitmap.Width);
                int x2 = random.Next(bitmap.Width);
                int y1 = random.Next(bitmap.Height);
                int y2 = random.Next(bitmap.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            //画图片的前景干扰点  
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            g.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1); //画图片的边框线  
            g.Dispose();

            return bitmap;
        }

        /// <summary>
        /// 获取图像流
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static Stream GenerateRecifyImageStream(string verifyCode, int width = 150, int height = 50)
        {
            Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
            Brush brush;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            SizeF totalSizeF = g.MeasureString(verifyCode, font);
            SizeF curCharSizeF;

            PointF startPointF = new PointF(0, (height - totalSizeF.Height) / 2);
            Random random = new Random(); //随机数产生器
            g.Clear(Color.White); //清空图片背景色  

            for (int i = 0; i < verifyCode.Length; i++)
            {
                brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                g.DrawString(verifyCode[i].ToString(), font, brush, startPointF);
                curCharSizeF = g.MeasureString(verifyCode[i].ToString(), font);
                startPointF.X += curCharSizeF.Width;
            }

            //画图片的干扰线  
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(bitmap.Width);
                int x2 = random.Next(bitmap.Width);
                int y1 = random.Next(bitmap.Height);
                int y2 = random.Next(bitmap.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            //画图片的前景干扰点  
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            g.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1); //画图片的边框线  

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            g.Dispose();
            bitmap.Dispose();
            return ms;
        }
    }
}
