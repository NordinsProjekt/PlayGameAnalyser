using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlayGameAnalyser.Service.Extensions
{
    public static class BitmapExtensions
    {
        public static byte[] GenerateBitmapDataArray(this Bitmap bitmap)
        {
            BitmapData refPicturePic;
            refPicturePic = bitmap.LockBits(new Rectangle(0, 0, 2560, 330), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr2 = refPicturePic.Scan0;
            int bytes2 = Math.Abs(refPicturePic.Stride) * refPicturePic.Height;
            var safePic = new byte[bytes2];
            Marshal.Copy(ptr2, safePic, 0, bytes2);
            return safePic;
        }
    }
}
