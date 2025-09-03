using Emgu.CV;
using KAutoHelper;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xaml.Permissions;
using VPT_Login.Libs;

namespace VPT_Login.Models
{
    public class GeneralFunctions
    {
        private string mWindowName;
        private DataModel mCharacter;
        private ReactiveProperty<string> textBoxStatus;
        public AutoFeatures mAuto;

        public GeneralFunctions(DataModel character, string mWindowName, ReactiveProperty<string> textBoxStatus)
        {
            this.mCharacter = character;
            this.mWindowName = mWindowName;
            this.textBoxStatus = textBoxStatus;
            mAuto = new AutoFeatures(mCharacter, mWindowName, textBoxStatus);
        }

        public void batPet()
        {
            mAuto.WriteStatus("Bắt đầu tìm pet để bắt");
            List<Point> mapPoints = collectMiniMapPointsForTrain();
            while (true)
            {
                epPet();

                int batPetLoop = 0;
                while (batPetLoop < 10)
                {
                    foreach (Point p in mapPoints)
                    {
                        if (!mAuto.DangTrongTranDau())
                        {
                            mAuto.ClickImageByGroup("global", "outbattletatauto");
                            if (!mAuto.FindImageByGroup("global", "map_top"))
                            {
                                mAuto.SendKey("~");

                                if (!mAuto.FindImageByGroup("global", "map_top"))
                                {
                                    mAuto.ClickImageByGroup("maps", "map");
                                }
                            }
                            mAuto.ClickPoint(p.X, p.Y);
                            Thread.Sleep(Constant.TimeMedium);
                        }

                        while (mAuto.DangTrongTranDau())
                        {
                            if (!mAuto.FindImageByGroup("global", "auto_check") && mAuto.FindImageByGroup("global", "inbattleauto"))
                            {
                                mAuto.ClickImageByGroup("global", "inbattlebatpet");
                                Thread.Sleep(Constant.TimeShort);
                                if (!mAuto.FindImageByGroup("bat_pet", "pet"))
                                {
                                    mAuto.ClickPoint(50, 50);
                                    mAuto.ClickImageByGroup("global", "inbattleauto");
                                    Thread.Sleep(Constant.TimeShort);
                                    mAuto.ClickImageByGroup("global", "inbattletatauto");
                                }
                                else
                                {
                                    mAuto.ClickImageByGroup("bat_pet", "pet");
                                    mAuto.SendKey("d");
                                }
                            }
                            Thread.Sleep(Constant.TimeMedium);
                        }
                    }
                    batPetLoop++;
                }
            }
        }

        private void epPet()
        {
            mAuto.WriteStatus("Bắt đầu ép pet");
            epPetByColor("trang");
            mAuto.WriteStatus($"Ép {Constant.PetList[mCharacter.PetKey]} trắng xong");
            epPetByColor("luc");
            mAuto.WriteStatus($"Ép {Constant.PetList[mCharacter.PetKey]}  xanh lá xong");
        }

        private void epPetByColor( string color)
        {
            int petNumber = 0;
            do
            {
                mAuto.CloseAllDialog();
                while (!mAuto.FindImageByGroup("global", "eppet_bang_check"))
                {
                    mAuto.ClickImageByGroup("global", "eppet_bang");
                    Thread.Sleep(Constant.TimeShort);
                }

                mAuto.ClickImageByGroup("global", "eppet_dunghop", true);

                if (mAuto.FindImageByGroup("global", "eppet_morongtuipet"))
                    mAuto.ClickImageByGroup("global", "eppet_morongtuipet");

                mAuto.ClickImageByGroup("global", "eppet_" + color, true);
                Thread.Sleep(Constant.TimeMedium);


                for (int i = 1; i < 5; i++)
                {
                    List<Point> pets = mAuto.FindImages("/bat_pet/pet_ep_"+ mCharacter.PetKey + ".png");
                    petNumber = pets.Count;
                    mAuto.WriteStatus($"Đang có {petNumber} pet {Constant.PetList[mCharacter.PetKey]} màu {color}");
                    if (petNumber >= 5)
                    {
                        int petInUse = 0;
                        while (petInUse < 5)
                        {
                            mAuto.ClickPoint(pets[petInUse].X, pets[petInUse].Y - 20, 2);
                            mAuto.ClickPoint(pets[petInUse].X, pets[petInUse].Y - 20, 2);
                            Thread.Sleep(Constant.TimeMediumShort);
                            //mAuto.ClickImageByGroup("global", "eppet_bang_check");
                            petInUse++;
                        }

                        mAuto.ClickImageByGroup("global", "eppet_hop");
                        Thread.Sleep(Constant.TimeMediumShort);
                        //mAuto.clickImageByGroup("global", "co");

                        mAuto.ClickImageByGroup("global", "eppet_" +(color == "trang"?"luc":"trang"), true);
                        Thread.Sleep(Constant.TimeMediumShort);
                        mAuto.ClickImageByGroup("global", "eppet_" + color, true);
                    }

                    mAuto.ClickImageByGroup("global", "eppet_nextpage");


                }
            } while (petNumber >= 5);
        }

        public List<Point> collectMiniMapPointsForTrain()
        {
            //string resourcePath = mCharacter.IsChinese == 1 ? "cn_resources" : "resources";

            mAuto.WriteStatus("Thu thập điểm trên bản đồ");
            List<Point> mapPoints = new List<Point>();

            // Mở bảng đồ mini;
            if (!mAuto.FindImageByGroup("global", "map_top"))
            {
                mAuto.SendKey("~");

                if (!mAuto.FindImageByGroup("global", "map_top"))
                {
                    mAuto.ClickImageByGroup("maps", "map");
                }
            }

            var full_screen = CaptureHelper.CaptureWindow(mCharacter.HWnd);

            // Tắt các bảng nổi
            mAuto.CloseAllDialog();

            Bitmap iBtnMapTop = ImageScanOpenCV.GetImage(Constant.img_cn + "/global/map_top.png");
            var pBtnMapTop = ImageScanOpenCV.FindOutPoint((Bitmap)full_screen, iBtnMapTop);

            Bitmap iBtnMapBottom = ImageScanOpenCV.GetImage(Constant.img_cn + "/global/map_bottom.png");
            var pBtnMapBottom = ImageScanOpenCV.FindOutPoint((Bitmap)full_screen, iBtnMapBottom);

            if (iBtnMapTop != null && iBtnMapBottom != null)
            {
                mapPoints.Add(new Point(pBtnMapTop.Value.X + 190, pBtnMapTop.Value.Y + 190));
                mapPoints.Add(new Point(pBtnMapTop.Value.X + 80, pBtnMapTop.Value.Y + 80));
                mapPoints.Add(new Point(pBtnMapTop.Value.X + 300, pBtnMapTop.Value.Y + 80));
                mapPoints.Add(new Point(pBtnMapBottom.Value.X + 80, pBtnMapBottom.Value.Y - 100));
                mapPoints.Add(new Point(pBtnMapBottom.Value.X + 300, pBtnMapBottom.Value.Y - 100));
            }

            mAuto.WriteStatus("Thu thập điểm trên bản đồ xong, có " + mapPoints.Count);
            return mapPoints;
        }
    }
}
