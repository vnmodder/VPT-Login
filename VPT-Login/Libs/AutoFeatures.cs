
using KAutoHelper;
using Reactive.Bindings;
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
using ZedGraph;

namespace VPT_Login.Libs
{
    public class AutoFeatures
    {
        private DataModel mCharacter;
        private ReactiveProperty<string> mTextBoxStatus;

        public AutoIT au3 = new AutoIT();
        public Random random = new Random();
        public string mWindowName;

        public AutoFeatures(DataModel mCharacter, string mWindowName, ReactiveProperty<string> textBoxStatus)
        {
            this.mCharacter = mCharacter;
            this.mWindowName = mWindowName;
            this.mTextBoxStatus = textBoxStatus;
        }

        public void WriteStatus(string statusText)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }
            System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
            {
                mTextBoxStatus.Value += (mCharacter.Name + ": " + statusText + Environment.NewLine);
            });
        }
        public void CloseAllDialog()
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }

            // Đóng tất cả hộp thoại đang có
            for (int i = 0; i <= 3; i++)
            {
                au3.controlsend(mWindowName, "{ESC}");
            }
        }

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
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, image,.8);
            if (pBtn != null)
            {
                au3.click(mWindowName, numClick, pBtn.Value.X + xRange, pBtn.Value.Y + yRange);
                Thread.Sleep(wait);
                return true;
            }
            return false;
        }

        public bool ClickToImage(string imagePath, int xRange = 0, int yRange = -20, int numClick = 1, int wait = Constant.TimeShort, double percent = 0.8)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return false;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            //imagePath = Path.Combine(Constant.rootPath, imagePath);
            imagePath = Constant.img_cn + imagePath;

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);
            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn, percent);
            if (pBtn != null)
            {
                float scale = Helper.GetCurrentScale();
                //float scale = 1;

                int clickX = (int)((pBtn.Value.X + xRange) * scale);
                int clickY = (int)((pBtn.Value.Y + yRange) * scale);

                au3.click(mWindowName, numClick, clickX, clickY);
                Thread.Sleep(wait);
                return true;
            }
            return false;
        }

        public bool FindImage(string imagePath, double percent = 0.8)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return false;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Constant.img_cn + imagePath;

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);


            if (!Directory.Exists(Constant.tracking))
            {
                Directory.CreateDirectory(Constant.tracking);
            }
            screen.Save(Constant.tracking + "/" + mCharacter.No + ".png");

            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn, percent);
            if (pBtn != null)
            {
                return true;
            }

            return false;
        }

        public List<Point> FindImages(string imagePath, double percent = 0.8)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return null;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Constant.img_cn + imagePath;

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);


            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            return ImageScanOpenCV.FindOutPoints((Bitmap)screen, iBtn, percent);
        }

        public void CaptureImage()
        {
            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);
            screen.Save(Constant.tracking + "/" + mCharacter.No + "_captured.png");
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

        public bool FindImageByGroup(string group, string name, bool active = false, bool hover = false)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                WriteStatus("FindImageByGroup chưa có nhân vật nào dang chạy");
                return false;
            }

            string groupPath = Constant.ImagePathGlobalFolder;
            switch (group)
            {
                case "bat_pet":
                    groupPath = Constant.ImagePathBatPetFolder;
                    break;
                case "global":
                default:
                    groupPath = Constant.ImagePathGlobalFolder;
                    break;
            }

            bool found = FindImage(groupPath + name + ".png") || (active && FindImage(groupPath + name + "_active.png")) || (hover && FindImage(groupPath + name + "_hover.png"));
            if (!found)
            {
                WriteStatus("RindImageByGroup không tìm thấy " + groupPath + name + ".png");
            }

            return found;
        }

        public void ClickImageByGroup(string group, string name, bool active = false, bool hover = false, int numClick = 1, int x = 0, int y = -20)
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                return;
            }

            string groupPath = Constant.ImagePathGlobalFolder;
            switch (group)
            {
                case "bat_pet":
                    groupPath = Constant.ImagePathBatPetFolder;
                    break;               
                case "global":
                default:
                    groupPath = Constant.ImagePathGlobalFolder;
                    break;
            }
            if (!FindImageByGroup(group, name, active, hover))
            {
                WriteStatus("ClickImageByGroup không tìm thấy " + groupPath + name + ".png");
                return;
            }

            ClickToImage(groupPath + name + ".png", x, y, numClick);
            if (active)
            {
                ClickToImage(groupPath + name + "_active.png", x, y, numClick);
            }
            if (hover)
            {
                ClickToImage(groupPath + name + "_hover.png", x, y, numClick);
            }
        }
        public bool DangTrongTranDau()
        {
            if (mCharacter.HWnd == IntPtr.Zero)
            {
                     return false;
            }

            if (!FindImage(Constant.ImagePathKhongTrongTranDau) && FindImageByGroup("global", "inbattlethongtin"))
            {
                //writeStatus("Nhân vật đang trong trận đấu ...");
                return true;
            }
            //writeStatus("Nhân vật đang không trong trận đấu ...");
            return false;
        }
    }
}
