using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Tool
{
    public class CombinImageHelper
    {
        /// <summary>
        /// 生成文字图片
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isBold"></param>
        /// <param name="fontSize"></param>
        public static Image CreateImage(string text, bool isBold, int fontSize)
        {
            int wid = 460;
            int high = 300;
            Font font;
            if (isBold)
            {
                font = new Font("微软雅黑", fontSize, FontStyle.Bold);

            }
            else
            {
                font = new Font("微软雅黑", fontSize, FontStyle.Regular);

            }
            //绘笔颜色
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            Bitmap image = new Bitmap(wid, high);
            Graphics g = Graphics.FromImage(image);
            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);//得到文本的宽高
            int width = (int)(sizef.Width + 1);
            int height = (int)(sizef.Height + 1);
            image.Dispose();
            image = new Bitmap(width, height);
            g = Graphics.FromImage(image);
            g.Clear(Color.White);//透明

            RectangleF rect = new RectangleF(0, 0, width, height);
            //绘制图片
            g.DrawString(text, font, brush, rect);
            //释放对象
            g.Dispose();
            return image;
        }
        /// <summary>  
        /// 合并图片  
        /// </summary>  
        /// <param name="imgBack"></param>  
        /// <param name="img"></param>  
        /// <returns></returns>  
        public static Bitmap CombinImage(Image imgBack, Image img, int xDeviation = 0, int yDeviation = 0)
        {

            Bitmap bmp = new Bitmap(imgBack.Width + xDeviation, imgBack.Height + img.Height + yDeviation);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2 + xDeviation, imgBack.Height + yDeviation, img.Width, img.Height);
            GC.Collect();
            return bmp;
        }
        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public static Image ResizeImage(Image bmp, int newW, int newH, int mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height),
                            GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
