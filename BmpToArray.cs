using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace fasterimagefuncs
{
    public static class BmpToArray
    {
        public static Color[] ToArray(this Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb) throw new Exception("unsupported format");


            BitmapData data = bitmap.LockBits(new Rectangle(0,0,bitmap.Width,bitmap.Height),ImageLockMode.ReadOnly,bitmap.PixelFormat);

            int bytes = Math.Abs(data.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytes];
            //copy bitmapdata to a byte array
            Marshal.Copy(data.Scan0, rgbValues, 0, bytes);

            Color[] ret = new Color[bytes / 4];

            for (int i =0;i < ret.Length;i++)
            {
                Color ib = ReadColor(i, rgbValues);
                ret[i] = ib;
            }

            bitmap.UnlockBits(data);
            return ret;
        }

        static Color ReadColor(int i, byte[] rgbvalues)
        {
            i *= 4;
            return Color.FromArgb(rgbvalues[i + 3], rgbvalues[i + 2], rgbvalues[i + 1], rgbvalues[i]);
        }

    }
}
