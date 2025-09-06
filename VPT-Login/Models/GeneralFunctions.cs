﻿using Emgu.CV;
using KAutoHelper;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Threading;
using System.Windows.Media.Media3D;
using VPT_Login.Libs;

namespace VPT_Login.Models
{
    public class GeneralFunctions
    {
        private string mWindowName;
        private DataModel mCharacter;
        private ReactiveProperty<string> textBoxStatus;
        private AutoFeatures mAuto;
        private CheMatBao mCheMatBao;
        private TrongNL mTrongNL;

        public GeneralFunctions(DataModel character, string mWindowName, ReactiveProperty<string> textBoxStatus)
        {
            this.mCharacter = character;
            this.mWindowName = mWindowName;
            this.textBoxStatus = textBoxStatus;
            mAuto = new AutoFeatures(mCharacter, mWindowName, textBoxStatus);
            mCheMatBao = new CheMatBao(mCharacter, mAuto);
            mTrongNL = new TrongNL(mCharacter, mAuto);
        }

        public void runAutoTuHanh()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            string npc = "truonglaovouutoc";
            string location = "autotuhanh";
            string map = "thientinhdia";
            mAuto.WriteStatus("Bắt đầu \"Auto Tu Hành\"");
            mAuto.CloseAllDialog();

            // Chạy đến Thiên Tĩnh Địa
            if (!mAuto.MoveToMap(map, 7, -18))
            {
                mAuto.WriteStatus("Không thể di chuyển đến Thiên Tĩnh Địa, thử lại ...");
                runAutoTuHanh();
            }

            // Bay lên
            mAuto.Bay();

            // Chạy đến NPC
            if (!mAuto.MoveToNPC(npc, location))
            {
                mAuto.WriteStatus("Không thể di chuyển đến vị trí auto tu hành");
                runAutoTuHanh();
            }

            // Bay xuống
            mAuto.BayXuong();

            // Nói chuyện với NPC
            if (mAuto.TalkToNPC(npc, mapName: map))
            {
                // Chọn Auto Tu Hành
                mAuto.ClickImageByGroup("global", "autotuhanh", false, true);

                // Bấm bắt đầu
                mAuto.ClickImageByGroup("global", "batdauautotuhanh", false, false);

                // Bấm có
                mAuto.ClickImageByGroup("global", "xacnhanco", false, true);
            }
        }

        public void nhanThuongHanhLang()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            string npc = "conghanhlang";
            string location = "nhanquahanhlang";
            mAuto.WriteStatus("Bắt đầu \"Nhận thưởng hành lang\"");
            mAuto.CloseAllDialog();

            // Di chuyển đến Quyến Cố Thành
            if (!mAuto.MoveToMap("quyencothanh", 5))
            {
                mAuto.WriteStatus("Không thể di chuyển đến Quyến Cố Thành, thử lại ...");
                nhanThuongHanhLang();
            }

            // Bay lên
            mAuto.Bay();

            // Di chuyển đến vị trí nhận thưởng hàng ngày
            if (!mAuto.MoveToNPC(npc, location))
            {
                mAuto.WriteStatus("Không thể di chuyển đến vị trí nhận quà hành lang");
                nhanThuongHanhLang();
            }

            // Bay xuống
            mAuto.BayXuong();

            // Nói chuyện với NPC
            if (mAuto.TalkToNPC(npc, 0, 0, -40, mapName: location))
            {
                // Kéo xuống dưới
                //mAuto.ClickImageByGroup("global", "keoxuong", false, true, 3);

                Thread.Sleep(Constant.TimeMediumShort);
                // Bấm nhận thưởng hành lang
                mAuto.ClickImageByGroup("global", "nhanthuonghanhlang", false, true);

                mAuto.WriteStatus("Đã nhận xong");
            }
        }

        public void runCheMatBao()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            mAuto.WriteStatus("Bắt đầu \"Chế mật bảo\"");

            // Mở bảng chế mật bảo
            if (mCheMatBao.moBangCheMB())
            {
                // Mở bảng chế mật bảo cần chế
                mCheMatBao.che();
            }
        }
        public void trongNL()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            mAuto.WriteStatus("Bắt đầu \"Trồng Nguyên Liệu\" ...");
            mTrongNL.moTrangVien();
            if (mTrongNL.kiemTraSoDatTrong())
            {
                mTrongNL.moNuoiTrong();
                mAuto.WriteStatus("Chọn nguyên liệu để trồng ...");
                mAuto.ClickImageByGroup("nguyen_lieu", mCharacter.NLKey.Value);
                Thread.Sleep(Constant.TimeShort);
                mTrongNL.trong();
            }
            mTrongNL.thuHoach();
            mTrongNL.dongTrangVien();
        }

        public void batPet()
        {
            if (mCharacter.ChiEp.Value)
            {
                epPet();
                return;
            }

            // Mở bảng nhân vật
            mAuto.CloseAllDialog();
            bool mainDungSau = true;

            List<Point> mapPoints = collectMiniMapPointsForTrain();
            while (true)
            {
                epPet();

                mAuto.WriteStatus("Đang tìm và bắt pet...");
                mAuto.CloseAllDialog();

                int batPetLoop = 0;
                while (batPetLoop < 10)
                {
                    int round_count = 0;
                    foreach (Point p in mapPoints)
                    {
                        if(mAuto.FindImageByGroup("global", "khongtrongtrandau", hover: true))
                        {
                            round_count = 0;
                        }

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
                            if (!mAuto.FindImageByGroup("global", "auto_check") &&
                                mAuto.FindImageByGroup("global", "inbattleauto"))
                            {
                                mAuto.ClickImageByGroup("global", "inbattlebatpet");
                                Thread.Sleep(Constant.TimeShort);

                                var list = mAuto.FindImages("/bat_pet/pet_" + mCharacter.PetKey.Value + ".png");

                                if (list == null || list.Count < 1 ||
                                    mCharacter.PetOption.Value == "dan" && round_count > 5)
                                {
                                    mAuto.ClickPoint(50, 50);
                                    mAuto.ClickImageByGroup("global", "inbattleauto");
                                    Thread.Sleep(Constant.TimeShort);
                                    mAuto.ClickImageByGroup("global", "inbattletatauto");
                                }
                                else
                                {

                                    //mAuto.ClickImageByGroup("bat_pet", "pet_" + mCharacter.PetKey.Value);
                                    mAuto.ClickPoint(list[0].X, list[0].Y - 20);
                                    round_count++;

                                    Thread.Sleep(Constant.VeryTimeShort);
                                    if (mAuto.FindImageByGroup("global", "inbattleauto"))
                                    {
                                        bool daBat = false;
                                        for (int i = 1; i < list.Count; i++)
                                        {
                                            mAuto.ClickImageByGroup("global", "inbattlebatpet");
                                            Thread.Sleep(Constant.TimeShort);
                                            mAuto.ClickPoint(list[i].X, list[i].Y - 20);
                                            Thread.Sleep(Constant.VeryTimeShort);
                                            if (!mAuto.FindImageByGroup("global", "inbattleauto"))
                                            {
                                                daBat = true;
                                                break;
                                            }
                                        }
                                        if (!daBat)
                                        {
                                            mAuto.ClickPoint(50, 50);
                                            mAuto.ClickImageByGroup("global", "inbattleauto");
                                            Thread.Sleep(Constant.TimeShort);
                                            mAuto.ClickImageByGroup("global", "inbattletatauto");
                                        }
                                    }
                                    if (mCharacter.PetOption.Value != "khong" &&
                                        (round_count == 1 || round_count == 8))
                                    {
                                        
                                        if (mAuto.FindImageByGroup("bat_pet", "batpet_" + mCharacter.PetOption.Value))
                                        {
                                            mAuto.ClickImageByGroup("bat_pet", "batpet_" + mCharacter.PetOption.Value);
                                            Thread.Sleep(Constant.TimeShort);
                                            //{X = 537 Y = 461}
                                            mAuto.ClickPoint(540, mainDungSau ? 440 : 470);
                                            Thread.Sleep(Constant.TimeShort);
                                            if (mAuto.FindImageByGroup("bat_pet", "batpet_" + mCharacter.PetOption.Value))
                                            {
                                                mainDungSau = !mainDungSau;
                                                mAuto.ClickImageByGroup("bat_pet", "batpet_" + mCharacter.PetOption.Value);
                                                Thread.Sleep(Constant.TimeShort);
                                                mAuto.ClickPoint(540, mainDungSau ? 440 : 470);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        mAuto.SendKey("d");
                                    }
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
            epPetByColor("luc");
            mAuto.WriteStatus("Hoàn thành ép pet");
        }

        private void epPetByColor(string color)
        {

            if (mAuto.DangTrongTranDau())
            {
                mAuto.ClickPoint(50, 50);
                mAuto.ClickImageByGroup("global", "inbattleauto");
                Thread.Sleep(Constant.TimeShort);
                mAuto.ClickImageByGroup("global", "inbattletatauto");
            }

            int petNumber = 0;
            do
            {
                mAuto.CloseAllDialog();
                while (!mAuto.FindImageByGroup("bat_pet", "eppet_bang_check"))
                {
                    mAuto.ClickImageByGroup("bat_pet", "eppet_bang");
                    Thread.Sleep(Constant.TimeShort);
                }

                mAuto.ClickImageByGroup("bat_pet", "eppet_dunghop", true);

                if (mAuto.FindImageByGroup("bat_pet", "eppet_morongtuipet"))
                    mAuto.ClickImageByGroup("bat_pet", "eppet_morongtuipet");

                mAuto.ClickImageByGroup("bat_pet", "eppet_" + color, true);
                Thread.Sleep(Constant.TimeMedium);


                for (int i = 1; i < 5; i++)
                {
                    List<Point> pets = mAuto.FindImages("/bat_pet/pet_ep_" + mCharacter.PetKey.Value + ".png");
                    petNumber = pets.Count;
                    string cl = color == "trang" ? "trắng" : "xanh lá";
                    mAuto.WriteStatus($"Đang có {petNumber} pet {Constant.PetList[mCharacter.PetKey.Value]} màu {cl}");
                    if (petNumber >= 5)
                    {
                        Thread.Sleep(Constant.VeryTimeShort);
                        int petInUse = 0;
                        while (petInUse < 5)
                        {
                            mAuto.ClickPoint(pets[petInUse].X, pets[petInUse].Y - 20, 2);
                            mAuto.ClickPoint(pets[petInUse].X, pets[petInUse].Y - 20, 2);
                            Thread.Sleep(Constant.TimeMediumShort);
                            //mAuto.ClickImageByGroup("global", "eppet_bang_check");
                            petInUse++;
                        }

                        mAuto.ClickImageByGroup("bat_pet", "eppet_hop");
                        Thread.Sleep(Constant.VeryTimeShort);
                        //mAuto.clickImageByGroup("global", "co");

                        mAuto.ClickImageByGroup("bat_pet", "eppet_" + (color == "trang" ? "luc" : "trang"), true);
                        Thread.Sleep(Constant.VeryTimeShort);
                        mAuto.ClickImageByGroup("bat_pet", "eppet_" + color, true);

                        if (mAuto.FindImageByGroup("bat_pet", "tile_" + (color == "trang" ? 80 : 50)))
                        {
                            Thread.Sleep(Constant.VeryTimeShort);
                            mAuto.ClickImageByGroup("bat_pet", "pet_chinh", y: 20);
                        }

                        Thread.Sleep(Constant.VeryTimeShort);
                        pets = mAuto.FindImages("/bat_pet/pet_ep_" + mCharacter.PetKey.Value + ".png");
                        continue;
                    }

                    if (!mAuto.FindImageByGroup("bat_pet", "eppet_nextpage"))
                    {
                        break;
                    }
                    mAuto.ClickImageByGroup("bat_pet", "eppet_nextpage");
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

            var full_screen = CaptureHelper.CaptureWindow(mCharacter.HWnd.Value);

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

            mAuto.WriteStatus("có " + mapPoints.Count + " điểm di chuyển");
            return mapPoints;
        }

        public void rutBo()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            mAuto.WriteStatus("Bắt đầu Rút oufit");
            mAuto.CloseAllDialog();

            // Mở bảng nhân vật
            mAuto.ClickImageByGroup("global", "nhanvat", false, false);

            // Mở tủ đồ
            mAuto.ClickImageByGroup("rutbo", "tudo", false, true);

            Thread.Sleep(2000);

            // Mở bảng rút bộ
            mAuto.ClickImageByGroup("rutbo", "rutbo", true, true);

            // Bấm rút thưởng
            mAuto.ClickImageByGroup("rutbo", "rutthuongbo", false, true);

            // Bấm xác nhận
            mAuto.ClickImageByGroup("rutbo", "rutboxacnhan", false, true);

            mAuto.WriteStatus("Rút oufit hoàn thành");
        }

        public void khongGianDieuKhac()
        {
            if (mCharacter.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            mAuto.WriteStatus("Bắt đầu \"Đổi không gian điêu khắc\"");
            mAuto.CloseAllDialog();

            // Mở bảng KGDK
            if (!findTheFeatureFromQuickFeatures("khonggiandieukhac"))
            {
                return;
            }

            Thread.Sleep(Constant.TimeMedium);
            // Chọn đổi
            mAuto.ClickImageByGroup("global", "khonggiandieukhacdoi", false, false);

            // Chọn có
            mAuto.ClickImageByGroup("global", "luachonco", false, true);

            mAuto.WriteStatus("Đã hoàn thành \"Đổi không gian điêu khắc\"");
        }

        private bool findTheFeatureFromQuickFeatures(String featureName)
        {
            int loop = 0;
            while (!mAuto.FindImageByGroup("global", featureName + "_check") && loop < Constant.MaxLoopShort)
            {
                while (mAuto.FindImageByGroup("global", "quickFeatureListUpArrow") &&
                    !mAuto.FindImageByGroup("global", featureName))
                {
                    mAuto.WriteStatus("Kéo lên đầu quick feature list");
                    mAuto.ClickImageByGroup("global", "quickFeatureListUpArrow");
                    Thread.Sleep(Constant.TimeShort);
                }
                while (!mAuto.FindImageByGroup("global", featureName) &&
                    mAuto.FindImageByGroup("global", "quickFeatureListDownArrow"))
                {
                    mAuto.WriteStatus("Không tìm thấy tính năng, di chuyển sang trang tiếp");
                    mAuto.ClickImageByGroup("global", "quickFeatureListDownArrow");
                    Thread.Sleep(Constant.TimeMedium);
                }

                if (mAuto.FindImageByGroup("global", featureName))
                {
                    mAuto.WriteStatus("Đã tìm thấy tính năng, mở tính năng " + featureName);
                    mAuto.ClickImageByGroup("global", featureName);
                    Thread.Sleep(Constant.TimeMedium);

                    return true;
                }
                else
                {
                    mAuto.WriteStatus("Không tìm thấy tính năng " + featureName);
                }
                loop++;
            }

            return false;
        }
    }
}
