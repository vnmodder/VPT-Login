using KAutoHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class AutoFeatures
    {
        private DataModel mCharacter;

        public AutoIT au3 = new AutoIT();
        public Random random = new Random();
        public string mWindowName;

        public void CloseFlash()
        {
            au3.winclose(mWindowName);
        }

        public void SendKey(string key, int wait = 1000)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }

            au3.controlsend(mWindowName, key);
            Thread.Sleep(wait);
        }

        public bool ClickRightToImage(string imagePath, int xRange = 0, int yRange = -20)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return false;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Path.Combine(Constant.rootPath, imagePath);

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);
           
            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn);
            if (pBtn != null)
            {
                au3.clickRight(mWindowName, 1, pBtn.Value.X + xRange, pBtn.Value.Y + yRange);
                Thread.Sleep(Constant.TimeShort);
                return true;
            }
            return false;
        }

        public void ClickToWindow(int xRange = 0, int yRange = -20, int numClick = 1, int wait = Constant.TimeShort)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }

            au3.click(mWindowName, numClick, xRange, yRange);
            Thread.Sleep(wait);
        }

        public void ClickPoint(int x = 0, int y = 0, int numClick = 1, int wait = Constant.TimeShort, bool isRightClick = false)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }

            if (isRightClick)
            {
                au3.clickRight(mWindowName, numClick, x, y);
            }
            else
            {
                au3.click(mWindowName, numClick, x, y);
            }

            Thread.Sleep(wait);
        }

        public bool clickImage(Bitmap image, int xRange = 0, int yRange = -20, int numClick = 1, int wait = Constant.TimeShort)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return false;
            }

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, image);
            if (pBtn != null)
            {
                au3.click(mWindowName, numClick, pBtn.Value.X + xRange, pBtn.Value.Y + yRange);
                Thread.Sleep(wait);
                return true;
            }
            return false;
        }

        public bool ClickToImage(string imagePath, int xRange = 0, int yRange = -20, int numClick = 1, int wait = Constant.TimeShort, double percent = 0.95 )
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return false;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Path.Combine(Constant.rootPath, imagePath);

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);
            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn, percent);
            if (pBtn != null)
            {
                au3.click(mWindowName, numClick, pBtn.Value.X + xRange, pBtn.Value.Y + yRange);
                Thread.Sleep(wait);
                return true;
            }
            return false;
        }

        public bool FindImage(string imagePath, double percent = 0.95)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return false;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Path.Combine(Constant.rootPath, imagePath);

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);

            screen.Save(Application.StartupPath + "\\tracking\\" + mCharacter.No + ".png");

            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn, percent);
            if (pBtn != null)
            {
                return true;
            }

            return false;
        }

        public List<Point> FindImages(string imagePath, double percent = 0.95)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return null;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Path.Combine(Constant.rootPath, imagePath);

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);


            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            return ImageScanOpenCV.FindOutPoints((Bitmap)screen, iBtn, percent);
        }

        public void CaptureImage()
        {
            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);
            screen.Save(Application.StartupPath + "\\tracking\\" + mCharacter.No + "_captured.png");
        }

        public void Bay()
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }

            ClickToImage(Constant.ImagePathGlobalBay);
        }

        public void BayXuong()
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }

            ClickToImage(Constant.ImagePathGlobalXuong);
        }
    }
}
