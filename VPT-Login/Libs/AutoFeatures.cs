
using Emgu.CV;
using KAutoHelper;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

        public bool MoveToMap(string mapName, int x = 0, int y = -20)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            int loop = 0;
            while (!FindImageByGroup("maps", mapName + "_check") && loop < Constant.MaxLoopShort)
            {
                CloseAllDialog();

                Thread.Sleep(Constant.TimeShort);
                // Mở bản đồ nhỏ
                SendKey("~");

                // Mở bản đồ thể giới
                ClickImageByGroup("maps", "world_map");

                int loopInMap = 0;
                while (!FindImageByGroup("maps", mapName, true) && loopInMap < Constant.MaxLoopShort)
                {
                    ClickImageByGroup("maps", "second_world_map", false, false, 1, x, y);
                    loopInMap++;
                }

                ClickImageByGroup("maps", mapName, true, false, 1, x, y);
                Thread.Sleep(Constant.VeryTimeShort);
                if(FindImageByGroup("global","xacnhanco", false, true))
                {
                    ClickImageByGroup("global", "xacnhanco", false, true);
                }


                loop++;
                Thread.Sleep(Constant.TimeShort);
            }

            if (loop >= Constant.MaxLoopShort)
            {
                WriteStatus("Không thể di chuyển đến " + mapName);
                return false;
            }

            return true;

        }

        public void OpenQuestByNVHN(string questName)
        {
            Bay();
            // Mở nhiệm vụ hàng ngày    
            while (!FindImageByGroup("nvhn", "bang_check"))
            {
                WriteStatus("Mở bảng nhiệm vụ hàng ngày");
                ClickImageByGroup("nvhn", "bang");
                Thread.Sleep(Constant.TimeShort);
            }

            string direction = "xuong";
            while (!FindImageByGroup("nvhn", questName, true, true))
            {
                if (FindImageByGroup("nvhn", "xuonghet", true))
                {
                    direction = "len";
                }

                if (FindImageByGroup("nvhn", "lenhet", true))
                {
                    direction = "xuong";
                }

                WriteStatus("Tìm nhiệm vụ " + questName);
                ClickImageByGroup("nvhn", direction, true, false, 7);
                Thread.Sleep(Constant.TimeShort);
            }

            ClickImageByGroup("nvhn", questName, true, true, 1, 370);
            Thread.Sleep(Constant.TimeMedium);
        }

        public bool MoveToNPC(string npc, string locationName)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            int loop = 1;
            do
            {
                CloseAllDialog();

                // Mở bản đồ nhỏ
                ClickImageByGroup("maps", "map");

                // Click vào vị trí cần đến
                ClickImageByGroup("in_map", locationName);

                // Check còn đang di chuyển không ?
                while (isMoving())
                {
                    Thread.Sleep(2000);
                }
                loop++;
            } while (!findNPC(npc) && loop <= Constant.MaxLoop);

            if (loop >= Constant.MaxLoop)
            {
                WriteStatus("Không thể di chuyển đến NPC " + npc);
                return false;
            }

            return true;
        }

        public bool TalkToNPC(string npc, int loopTime = 0, int x = 0, int y = -20, string mapName ="")
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            loopTime++;
            CloseAllDialog();

            string npcViTriTenImagePath1 = Constant.ImagePathViTriNPC + "ten" + npc + "1.png";
            string npcViTriTenImagePath2 = Constant.ImagePathViTriNPC + "ten" + npc + "2.png";
            string npcViTriImagePath1 = Constant.ImagePathViTriNPC + npc + "1.png";
            string npcViTriImagePath2 = Constant.ImagePathViTriNPC + npc + "2.png";

            // Click vào NPC
            WriteStatus("Click vào vị trí NPC ...");
            ClickToImage(npcViTriImagePath1, x, y);
            ClickToImage(npcViTriImagePath2, x, y);

            if (!IsTalkWithNPC(npc) && loopTime < Constant.MaxLoop )
            {
                // Click vào vị trí khác bên cạnh NPC
                WriteStatus("Click vào vị trí khác bên cạnh NPC ...");
                ClickToImage(npcViTriTenImagePath1, random.Next(-100, 100), random.Next(-100, 100));
                ClickToImage(npcViTriTenImagePath2, random.Next(-100, 100), random.Next(-100, 100));

                //if(!FindImageByGroup("maps", mapName + "_check"))
                //{
                //    MoveToMap()
                //}

                TalkToNPC(npc, loopTime, x, y, mapName: mapName);
            }

            if (loopTime >= Constant.MaxLoop)
            {
                WriteStatus("Không nói chuyện được với NPC ...");
                return false;
            }

            WriteStatus("Đang nói chuyện được với NPC ...");
            return true;
        }

        public bool IsTalkWithNPC(string npc)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            string npcDoiThoaiImagePath = Constant.ImagePathDoiThoai + npc + ".png";
            if (FindImage(npcDoiThoaiImagePath))
            {
                WriteStatus("Nhân vật đang đối thoại với " + npc + " ...");
                ClickToImage(npcDoiThoaiImagePath, 0, -20, 1, Constant.TimeShort);
                return true;
            }
            WriteStatus("Nhân vật đang không đối thoại với " + npc + " ...");
            return false;
        }

        public bool findNPC(string npc)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            string npcViTriTenImagePath1 = Constant.ImagePathViTriNPC + "ten" + npc + "1.png";
            string npcViTriTenImagePath2 = Constant.ImagePathViTriNPC + "ten" + npc + "2.png";
            string npcViTriImagePath1 = Constant.ImagePathViTriNPC + npc + "1.png";
            string npcViTriImagePath2 = Constant.ImagePathViTriNPC + npc + "2.png";

            CloseAllDialog();

            if (FindImage(npcViTriImagePath1)
                || FindImage(npcViTriImagePath2)
                || FindImage(npcViTriTenImagePath1)
                || FindImage(npcViTriTenImagePath2))
            {
                return true;
            }

            return false;
        }

        private void anNhanVat()
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
                        ClickImageByGroup("global", "annhanvat");
                        Thread.Sleep(Constant.TimeMediumShort);
                        return;
                    }
                }
            }

        }

        public bool isMoving()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            int i = 0;
            bool moving = true;
            while (moving && i < Constant.MaxLoop)
            {
                // Chụp màn hình
                var screen_first = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);
                screen_first = CaptureHelper.CropImage((Bitmap)screen_first, new Rectangle(180, 0, 250, 250));

                // Chờ 3s
                Thread.Sleep(1500);
                var screen_second = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);

                // Kiểm tra hình trước có trong hình sau hay không
                var p = ImageScanOpenCV.FindOutPoint((Bitmap)screen_second, (Bitmap)screen_first);
                if (p != null)
                {
                    moving = false;
                    return moving;
                }
            }

            return moving;
        }

        public void WriteStatus(string statusText)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }
            System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
            {
                mTextBoxStatus.Value += (mCharacter.Name + ": " + statusText + Environment.NewLine);
            });
        }

        public void MoMenuPhai()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            ClickImageByGroup("global", "momenuphai");
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
                au3.controlsend(mWindowName, "{ESC}");
            }
        }

        public void CloseFlash()
        {
            au3.winclose(mWindowName);
        }

        public void SendKey(string key, int wait = 1000)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            au3.controlsend(mWindowName, key);
            Thread.Sleep(wait);
        }

        public bool ClickRightToImage(string imagePath, int xRange = 0, int yRange = -20)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return false;
            }

            //imagePath = (mCharacter.IsChinese == 1 ? Constant.ChineseResourcePath : Constant.ResourcePath) + imagePath;
            imagePath = Constant.img_cn + imagePath;

            var screen = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);

            Bitmap iBtn = ImageScanOpenCV.GetImage(imagePath);
            var pBtn = ImageScanOpenCV.FindOutPoint((Bitmap)screen, iBtn);
            if (pBtn != null)
            {
                float scale = Helper.GetCurrentScale();
                au3.clickRight(mWindowName, 1, (int) ((pBtn.Value.X + xRange)* scale), (int) ((pBtn.Value.Y + yRange) * scale));
                Thread.Sleep(Constant.TimeShort);
                return true;
            }
            return false;
        }

        public void ClickToWindow(int xRange = 0, int yRange = -20, int numClick = 1, int wait = Constant.TimeShort)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            au3.click(mWindowName, numClick, xRange, yRange);
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
                au3.clickRight(mWindowName, numClick,(int) (x* scale), (int)(y * scale));
            }
            else
            {
                au3.click(mWindowName, numClick, (int)(x * scale), (int)(y * scale));
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

                au3.click(mWindowName, numClick, clickX, clickY);
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
            screen.Save(Constant.tracking + "/" + mCharacter.No + "_captured.png");
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

            ClickToImage(Constant.ImagePathGlobalBay);
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

            ClickToImage(Constant.ImagePathGlobalXuong);
        }

        public bool FindImageByGroup(string group, string name, bool active = false, bool hover = false)
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                WriteStatus("FindImageByGroup chưa có nhân vật nào dang chạy");
                return false;
            }
            //WriteStatus("tìm" + name);
            string groupPath = Constant.ImagePathGlobalFolder;
            switch (group)
            {
               case "bat_pet":
                    groupPath = Constant.ImagePathBatPetFolder;
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

            bool found = FindImage(groupPath + name + ".png") || (active && FindImage(groupPath + name + "_active.png")) || (hover && FindImage(groupPath + name + "_hover.png"));
            if (!found)
            {
               // WriteStatus("RindImageByGroup không tìm thấy " + groupPath + name + ".png");
            }

            return found;
        }

        public void ClickImageByGroup(string group, string name, bool active = false, bool hover = false, int numClick = 1, int x = 0, int y = -20)
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
            if (!FindImageByGroup(group, name, active, hover))
            {
                //WriteStatus("ClickImageByGroup không tìm thấy " + groupPath + name + ".png");
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
                    Helper.WriteStatus(mCharacter.LogText, $"{mCharacter.Name}", "Đã ngừng auto do game đã đóng");
                    thread.Abort();
                }
            }

        }
    }
}
