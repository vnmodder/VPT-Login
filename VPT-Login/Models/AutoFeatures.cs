using KAutoHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace VPT_Login.Models
{
    public class AutoFeatures
    {
        private DataModel mCharacter;

        // public AutoIT au3 = new AutoIT();
        public Random random = new Random();

        public AutoFeatures(DataModel mCharacter)
        {
            this.mCharacter = mCharacter;
        }


        public void AnNhanVat()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            CloseAllDialog();
            if (FindImageByGroup("global", "caidat"))
            {
                for (int i = 0; i < 10; i++)
                {
                    ClickImageByGroup("global", "caidat");
                    Thread.Sleep(Constant.TimeMediumShort);
                    if (FindImageByGroup("global", "caidat_check"))
                    {
                        ClickImageByGroup("global", "annhanvat", percent: .95);
                        Thread.Sleep(Constant.TimeMediumShort);
                        return;
                    }
                }
            }

        }

 
        public void CloseAllDialog()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            // Đóng tất cả hộp thoại đang có
            for (int i = 0; i <= 3; i++)
            {
                ClickHelper.ControlSendKey(mCharacter.HWnd.Value, Keys.Escape);
            }
        }


        public void SendKey(Keys key, int wait = 1000)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            ClickHelper.ControlSendKey(mCharacter.HWnd.Value, key);
            Thread.Sleep(wait);
        }

        public void ClickPoint(int x = 0, int y = 0, int numClick = 1, int wait = Constant.TimeShort, bool isRightClick = false)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            if (!Helper.IsWindow(mCharacter.HWnd.Value))
            {
                stopAuto();
            }
            float scale = Helper.GetCurrentScale();

            if (isRightClick)
            {
                ClickHelper.ClickRight(mCharacter.HWnd.Value, numClick, (int)(x * scale), (int)(y * scale));
            }
            else
            {
                ClickHelper.Click(mCharacter.HWnd.Value, numClick, (int)(x * scale), (int)(y * scale));
            }

            Thread.Sleep(wait);
        }

        public bool clickImage(Bitmap image, int xRange = 0, int yRange = -20, int numClick = 1, int wait = Constant.TimeShort)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, image, .8);
            if (pBtn != null)
            {
                ClickHelper.Click(mCharacter.HWnd.Value, numClick, pBtn.Value.X + xRange, pBtn.Value.Y + yRange);
                Thread.Sleep(wait);
                return true;
            }
            return false;
        }

        public bool ClickToImage(string imagePath, int xRange = 0, int yRange = -20, int numClick = 1, int wait = Constant.TimeShort, double percent = 0.8)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            if (!Helper.IsWindow(mCharacter.HWnd.Value))
            {
                stopAuto();
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            //imagePath = Path.Combine(Constant.rootPath, imagePath);
            imagePath = Constant.img_cn + imagePath;

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);
            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn, percent);

            if (pBtn != null)
            {
                float scale = Helper.GetCurrentScale();
                //float scale = 1;

                int clickX = (int)((pBtn.Value.X + xRange) * scale);
                int clickY = (int)((pBtn.Value.Y + yRange) * scale);

                ClickHelper.Click(mCharacter.HWnd.Value, numClick, clickX, clickY);
                Thread.Sleep(wait);
                return true;
            }
            return false;
        }

        public bool FindImage(string imagePath, double percent = 0.8)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            if (!Helper.IsWindow(mCharacter.HWnd.Value))
            {
                stopAuto();
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Constant.img_cn + imagePath;

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);


            if (!Directory.Exists(Constant.tracking))
            {
                Directory.CreateDirectory(Constant.tracking);
            }

            screen.Save(Constant.tracking + "/" + mCharacter.Name + ".png");
            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn, percent);
            if (pBtn != null)
            {
                return true;
            }

            return false;
        }

        public List<System.Drawing.Point> FindImages(string imagePath, double percent = 0.8)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return null;
            }

            if (!Helper.IsWindow(mCharacter.HWnd.Value))
            {
                stopAuto();
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Constant.img_cn + imagePath;


            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);

            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            return ImageScanOpenCV.FindOutPoints((Bitmap)screen, iBtn, percent);
        }

        public void CaptureImage()
        {
            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);
            screen.Save(Constant.tracking + "/" + mCharacter.Name + "_captured.png");
        }

        public void Bay()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            if (!Helper.IsWindow(mCharacter.HWnd.Value))
            {
                stopAuto();
            }

            while (FindImageByGroup("global", "bay"))
            {
                Thread.Sleep(Constant.TimeMediumShort);
                ClickToImage(Constant.ImagePathGlobalBay);
            }
        }

        public void BayXuong()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            if (!Helper.IsWindow(mCharacter.HWnd.Value))
            {
                stopAuto();
            }
            CloseAllDialog();
            ClickHelper.ControlSendKey(mCharacter.HWnd.Value, Keys.F);
            Thread.Sleep(Constant.TimeMediumShort);

            while (FindImageByGroup("global", "xuong", true, true))
            {
                ClickToImage(Constant.ImagePathGlobalXuong);
                Thread.Sleep(Constant.TimeMediumShort);
            }
        }

        public bool FindImageByGroup(string group, string name, bool active = false, bool hover = false, double percent = 0.8)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                //WriteStatus("FindImageByGroup chưa có nhân vật nào dang chạy");
                return false;
            }
            //WriteStatus("tìm" + name);
            string groupPath = Constant.ImagePathGlobalFolder;
            switch (group)
            {
                case "bat_pet":
                    groupPath = Constant.ImagePathBatPetFolder;
                    break;
                case "tlinh":
                    groupPath = Constant.ImagePathTinhLinhFolder;
                    break;
                case "thai_co":
                    groupPath = Constant.ImagePathThaiCoFolder;
                    break;
                case "xu_que":
                    groupPath = Constant.ImagePathXuQueFolder;
                    break;
                case "nguyen_lieu":
                    groupPath = Constant.ImagePathNLFolder;
                    break;
                case "maps":
                    groupPath = Constant.ImagePathMapsFolder;
                    break;
                case "rutbo":
                    groupPath = Constant.ImagePathRutBoFolder;
                    break;
                case "nvhn":
                    groupPath = Constant.ImagePathNVHNFolder;
                    break;
                case "stmt":
                    groupPath = Constant.ImagePathSTMTFolder;
                    break;
                case "phu_ban":
                    groupPath = Constant.ImagePathPhuBanFolder;
                    break;
                case "mat_bao":
                    groupPath = Constant.ImagePathMatBaoFolder;
                    break;
                case "char_name":
                    groupPath = Constant.ImagePathCharNameFolder;
                    break;
                case "tri_an":
                    groupPath = Constant.ImagePathTriAnFolder;
                    break;
                case "in_map":
                    groupPath = Constant.ImagePathInMapFolder;
                    break;
                case "global":
                default:
                    groupPath = Constant.ImagePathGlobalFolder;
                    break;
            }

            bool found = FindImage(groupPath + name + ".png", percent) ||
                (active && FindImage(groupPath + name + "_active.png", percent)) ||
                (hover && FindImage(groupPath + name + "_hover.png", percent));
            if (!found)
            {
                //WriteStatus("RindImageByGroup không tìm thấy " + groupPath + name + ".png");
            }

            return found;
        }

        public void ClickImageByGroup(string group, string name, bool active = false, bool hover = false, int numClick = 1, int x = 0, int y = -20, double percent = 0.8)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }
            // WriteStatus("tìm" + name);
            string groupPath = Constant.ImagePathGlobalFolder;
            switch (group)
            {
                case "bat_pet":
                    groupPath = Constant.ImagePathBatPetFolder;
                    break;
                case "tlinh":
                    groupPath = Constant.ImagePathTinhLinhFolder;
                    break;
                case "thai_co":
                    groupPath = Constant.ImagePathThaiCoFolder;
                    break;
                case "xu_que":
                    groupPath = Constant.ImagePathXuQueFolder;
                    break;
                case "nguyen_lieu":
                    groupPath = Constant.ImagePathNLFolder;
                    break;
                case "maps":
                    groupPath = Constant.ImagePathMapsFolder;
                    break;
                case "rutbo":
                    groupPath = Constant.ImagePathRutBoFolder;
                    break;
                case "nvhn":
                    groupPath = Constant.ImagePathNVHNFolder;
                    break;
                case "stmt":
                    groupPath = Constant.ImagePathSTMTFolder;
                    break;
                case "phu_ban":
                    groupPath = Constant.ImagePathPhuBanFolder;
                    break;
                case "mat_bao":
                    groupPath = Constant.ImagePathMatBaoFolder;
                    break;
                case "char_name":
                    groupPath = Constant.ImagePathCharNameFolder;
                    break;
                case "tri_an":
                    groupPath = Constant.ImagePathTriAnFolder;
                    break;
                case "in_map":
                    groupPath = Constant.ImagePathInMapFolder;
                    break;
                case "global":
                default:
                    groupPath = Constant.ImagePathGlobalFolder;
                    break;
            }
            if (!FindImageByGroup(group, name, active, hover, percent))
            {
                //WriteStatus("ClickImageByGroup không tìm thấy " + groupPath + name + ".png");
                return;
            }
            if (ClickToImage(groupPath + name + ".png", x, y, numClick, percent: percent))
            {
                return;
            }
            if (active && ClickToImage(groupPath + name + "_active.png", x, y, numClick, percent: percent))
            {
                return;
            }
            if (hover)
            {
                ClickToImage(groupPath + name + "_hover.png", x, y, numClick, percent: percent);
            }
        }
        public bool DangTrongTranDau()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }
            if (!Helper.IsWindow(mCharacter.HWnd.Value))
            {
                stopAuto();
            }

            if (!FindImage(Constant.ImagePathKhongTrongTranDau) && FindImageByGroup("global", "inbattlethongtin"))
            {
                //writeStatus("Nhân vật đang trong trận đấu ...");
                return true;
            }
            //writeStatus("Nhân vật đang không trong trận đấu ...");
            return false;
        }

        private void stopAuto()
        {
            mCharacter.HWnd.Value = IntPtr.Zero;
            foreach (var thread in Helper.ThreadList)
            {
                if (thread.Name.Contains($"{mCharacter.Name}"))
                {                  
                    mCharacter.Active.Value = false;
                    thread.Abort();
                }
            }

        }

    }
}
