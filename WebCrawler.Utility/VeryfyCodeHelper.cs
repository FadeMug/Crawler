using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Utility
{
    public class VeryfyCodeHelper
    {
        public string GetVeryfyCode(string url)
        {
            //Bitmap bmp = new Bitmap(@"E:\1111.jpg");
            //tessnet2.Tesseract ocr = new tessnet2.Tesseract();
            //// ocr.SetVariable("tessedit_char_whitelist", "0123456789");
            //ocr.Init(null, "fra", false);
            //// List<tessnet2.Word> r1 = ocr.DoOCR(bmp, new Rectangle(792, 247, 130, 54));
            //List<tessnet2.Word> result = ocr.DoOCR(bmp, Rectangle.Empty);
            //string code = result[0].Text;
            //return code;
            return null;
        }

    }
   public class UnCodebase
    {
        public Bitmap bmpobj;
        public UnCodebase(Bitmap pic)
        {
            bmpobj = new Bitmap(pic);    //转换为Format32bppRgb  
        }
        /// <summary>  
        /// 根据RGB，计算灰度值  
        /// </summary>  
        /// <param name="posClr">Color值</param>  
        /// <returns>灰度值，整型</returns>  
        private int GetGrayNumColor(System.Drawing.Color posClr)
        {
            return (posClr.R * 19595 + posClr.G * 38469 + posClr.B * 7472) >> 16;
        }
        /// <summary>  
        /// 灰度转换,逐点方式  
        /// </summary>  
        public Bitmap GrayByPixels()
        {
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int tmpValue = GetGrayNumColor(bmpobj.GetPixel(j, i));
                    bmpobj.SetPixel(j, i, Color.FromArgb(tmpValue, tmpValue, tmpValue));
                }
            }
            return bmpobj;
        }
        /// <summary>
        ///  去掉杂点（适合杂点/杂线粗为1）
        /// </summary>
        /// <param name="dgGrayValue">背前景灰色界限</param>
        /// <returns></returns>
        public void ClearNoise(int dgGrayValue, int MaxNearPoints)
        {
            Color piexl;
            int nearDots = 0;
            //逐点判断
            for (int i = 0; i < bmpobj.Width; i++)
                for (int j = 0; j < bmpobj.Height; j++)
                {
                    piexl = bmpobj.GetPixel(i, j);
                    if (piexl.R < dgGrayValue)
                    {
                        nearDots = 0;
                        //判断周围8个点是否全为空
                        if (i == 0 || i == bmpobj.Width - 1 || j == 0 || j == bmpobj.Height - 1)  //边框全去掉
                        {
                            bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            if (bmpobj.GetPixel(i - 1, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i - 1, j).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i - 1, j + 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i, j + 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j + 1).R < dgGrayValue) nearDots++;
                        }

                        if (nearDots < MaxNearPoints)
                            bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));   //去掉单点 && 粗细小3邻边点
                    }
                    else  //背景
                        bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
        }
        /// <summary>  
        /// 去图形边框  
        /// </summary>  
        /// <param name="borderWidth"></param>  
        public void ClearPicBorder(int borderWidth)
        {
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    if (i < borderWidth || j < borderWidth || j > bmpobj.Width - 1 - borderWidth || i > bmpobj.Height - 1 - borderWidth)
                        bmpobj.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }
            }
        }
        /// <summary>  
        /// 灰度转换,逐行方式  
        /// </summary>  
        public void GrayByLine()
        {
            Rectangle rec = new Rectangle(0, 0, bmpobj.Width, bmpobj.Height);
            BitmapData bmpData = bmpobj.LockBits(rec, ImageLockMode.ReadWrite, bmpobj.PixelFormat);// PixelFormat.Format32bppPArgb);  
            //    bmpData.PixelFormat = PixelFormat.Format24bppRgb;  
            IntPtr scan0 = bmpData.Scan0;
            int len = bmpobj.Width * bmpobj.Height;
            int[] pixels = new int[len];
            Marshal.Copy(scan0, pixels, 0, len);
            //对图片进行处理  
            int GrayValue = 0;
            for (int i = 0; i < len; i++)
            {
                GrayValue = GetGrayNumColor(Color.FromArgb(pixels[i]));
                pixels[i] = (byte)(Color.FromArgb(GrayValue, GrayValue, GrayValue)).ToArgb();      //Color转byte  
            }
            bmpobj.UnlockBits(bmpData);
        }
        /// <summary>  
        /// 得到有效图形并调整为可平均分割的大小  
        /// </summary>  
        /// <param name="dgGrayValue">灰度背景分界值</param>  
        /// <param name="CharsCount">有效字符数</param>  
        /// <returns></returns>  
        public void GetPicValidByValue(int dgGrayValue, int CharsCount)
        {
            int posx1 = bmpobj.Width;
            int posy1 = bmpobj.Height;
            int posx2 = 0;
            int posy2 = 0;
            for (int i = 0; i < bmpobj.Height; i++)      //找有效区  
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int pixelValue = bmpobj.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //根据灰度值  
                    {
                        if (posx1 > j)
                            posx1 = j;
                        if (posy1 > i)
                            posy1 = i;
                        if (posx2 < j)
                            posx2 = j;
                        if (posy2 < i)
                            posy2 = i;
                    };
                };
            };
            // 确保能整除  
            int Span = CharsCount - (posx2 - posx1 + 1) % CharsCount;   //可整除的差额数  
            if (Span < CharsCount)
            {
                int leftSpan = Span / 2;    //分配到左边的空列 ，如span为单数,则右边比左边大1  
                if (posx1 > leftSpan)
                    posx1 = posx1 - leftSpan;
                if (posx2 + Span - leftSpan < bmpobj.Width)
                    posx2 = posx2 + Span - leftSpan;
            }
            //复制新图  
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            bmpobj = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);
        }
        /// <summary>  
        /// 得到有效图形,图形为类变量  
        /// </summary>  
        /// <param name="dgGrayValue">灰度背景分界值</param>  
        /// <param name="CharsCount">有效字符数</param>  
        /// <returns></returns>  
        public void GetPicValidByValue(int dgGrayValue)
        {
            int posx1 = bmpobj.Width;
            int posy1 = bmpobj.Height;
            int posx2 = 0;
            int posy2 = 0;
            for (int i = 0; i < bmpobj.Height; i++)      //找有效区  
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int pixelValue = bmpobj.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //根据灰度值  
                    {
                        if (posx1 > j)
                            posx1 = j;
                        if (posy1 > i)
                            posy1 = i;
                        if (posx2 < j)
                            posx2 = j;
                        if (posy2 < i)
                            posy2 = i;
                    };
                };
            };
            //复制新图  
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            bmpobj = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);
        }
        /// <summary>  
        /// 得到有效图形,图形由外面传入  
        /// </summary>  
        /// <param name="dgGrayValue">灰度背景分界值</param>  
        /// <param name="CharsCount">有效字符数</param>  
        /// <returns></returns>  
        public Bitmap GetPicValidByValue(Bitmap singlepic, int dgGrayValue)
        {
            int posx1 = singlepic.Width;
            int posy1 = singlepic.Height;
            int posx2 = 0;
            int posy2 = 0;
            for (int i = 0; i < singlepic.Height; i++)      //找有效区  
            {
                for (int j = 0; j < singlepic.Width; j++)
                {
                    int pixelValue = singlepic.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //根据灰度值  
                    {
                        if (posx1 > j)
                            posx1 = j;
                        if (posy1 > i)
                            posy1 = i;
                        if (posx2 < j)
                            posx2 = j;
                        if (posy2 < i)
                            posy2 = i;
                    };
                };
            };
            //复制新图  
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            return singlepic.Clone(cloneRect, singlepic.PixelFormat);
        }
        /// <summary>  
        /// 平均分割图片  
        /// </summary>  
        /// <param name="RowNum">水平上分割数</param>  
        /// <param name="ColNum">垂直上分割数</param>  
        /// <returns>分割好的图片数组</returns>  
        public Bitmap[] GetSplitPics(int RowNum, int ColNum)
        {
            if (RowNum == 0 || ColNum == 0)
                return null;
            int singW = bmpobj.Width / RowNum;
            int singH = bmpobj.Height / ColNum;
            Bitmap[] PicArray = new Bitmap[RowNum * ColNum];
            Rectangle cloneRect;
            for (int i = 0; i < ColNum; i++)      //找有效区  
            {
                for (int j = 0; j < RowNum; j++)
                {
                    cloneRect = new Rectangle(j * singW, i * singH, singW, singH);
                    PicArray[i * RowNum + j] = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);//复制小块图  
                }
            }
            return PicArray;
        }
        public int GetdgGrayValue(Bitmap singlepic)
        {
            Color piexl;
            int max, min;
            piexl = singlepic.GetPixel(0, 0);
            max = piexl.R;
            min = max;
            for (int posy = 0; posy < singlepic.Height; posy++)
                for (int posx = 0; posx < singlepic.Width; posx++)
                {
                    piexl = singlepic.GetPixel(posx, posy);
                    if (piexl.R > max)    // Color.Black )  
                        max = piexl.R;
                    else if (piexl.R < min)
                        min = piexl.R;
                }
            return (max + min) / 2;
        }
        /// <summary>  
        /// 返回灰度图片的点阵描述字串，1表示灰点，0表示背景  
        /// </summary>  
        /// <param name="singlepic">灰度图</param>  
        /// <param name="dgGrayValue">背前景灰色界限</param>  
        /// <returns></returns>  
        public string GetSingleBmpCode(Bitmap singlepic, int dgGrayValue)
        {
            Color piexl;
            string code = "";
            for (int posy = 0; posy < singlepic.Height; posy++)
                for (int posx = 0; posx < singlepic.Width; posx++)
                {
                    piexl = singlepic.GetPixel(posx, posy);
                    if (piexl.R < dgGrayValue)    // Color.Black )  
                    {
                        singlepic.SetPixel(posx, posy, Color.Black);
                        code = code + "1";
                    }
                    else
                    {
                        singlepic.SetPixel(posx, posy, Color.White);
                        code = code + "0";
                    }
                }
            return code;
        }
    }
}
