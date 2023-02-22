using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Extensions
{
    public class VerificationCodeServices
    {
        /// <summary>  
     /// 生成指定长度的随机字符串 
     /// </summary>  
     /// <param name="codeLength">字符串的长度</param>  
     /// <returns>返回随机数字符串</returns>  
        private string RndomStr(int codeLength)
        {
            //组成字符串的字符集合  0-9数字、大小写字母
            string chars = "0,1,2,3,4,5,6,7,8,9";

            string[] charArray = chars.Split(new Char[] { ',' });
            string code = "";
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  
            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < codeLength + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(chars.Replace(",","").Length);
                if (temp == t)
                {
                    return RndomStr(codeLength);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += charArray[t];//随机数的位数加一  
            }
            return code;
        }

        /// <summary>  
        /// 将生成的字符串写入图像文件  
        /// </summary>  
        /// <param name="code">验证码字符串</param>
        /// <param name="length">生成位数（默认4位）</param>  

        public MemoryStream Create(out string code, int length = 4)
        {
            code = RndomStr(length);
            Bitmap Img = null;
            Graphics graphics = null;
            MemoryStream ms = null;
            Random random = new Random();
            //颜色集合  
            Color[] color = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            //定义图像的大小，生成图像的实例  
            Img = new Bitmap((int)code.Length * 18, 32);
            graphics = Graphics.FromImage(Img);//从Img对象生成新的Graphics对象    
            graphics.Clear(Color.White);//背景设为白色  

            //在随机位置画背景点  

            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(Img.Width);
                int y = random.Next(Img.Height);
                graphics.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }

            //验证码绘制在graphics中  

            for (int i = 0; i < code.Length; i++)
            {
                int colorIndex = random.Next(7);//随机颜色索引值  
                int fontIndex = random.Next(4);//随机字体索引值  
                Font font = new Font(fonts[fontIndex], 15, FontStyle.Bold);//字体  
                Brush brush = new SolidBrush(color[colorIndex]);//颜色  
                int y = 4;
                if ((i + 1) % 2 == 0)//控制验证码不在同一高度  
                {
                    y = 2;
                }
                graphics.DrawString(code.Substring(i, 1), font, brush, 3 + (i * 12), y);//绘制一个验证字符  
            }
            try
            {
                ms = new MemoryStream();//生成内存流对象  
                Img.Save(ms, ImageFormat.Jpeg);//将此图像以Png图像文件的格式保存到流中
                return ms;
            }
            catch {
                return null;
            }
            finally
            {
                graphics.Dispose();
                Img.Dispose();
            }
        }
    }
}
