using Reactive.Bindings;
using System;
using System.Threading;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class MainAuto
    {
        private string mWindowName;
        public AutoFeatures mAuto;
        private DataModel mCharacter;
        public GeneralFunctions mGeneralFunctions;
        ReactiveProperty<string> mTextBoxStatus;

        public MainAuto(DataModel character, ReactiveProperty<string> textBoxStatus)
        {
            mCharacter = character;
            mWindowName = $"{mCharacter.Server.Value}-{mCharacter.Name.Value}";
            mTextBoxStatus = textBoxStatus;
            mAuto = new AutoFeatures(character, mWindowName, textBoxStatus);
            mGeneralFunctions = new GeneralFunctions(mCharacter, mWindowName, textBoxStatus);
        }

        public void ChayHet()
        {
            mAuto.WriteStatus("Bắt đầu chạy các auto đã chọn");

            Thread.Sleep(Constant.TimeShort);
            mAuto.AnNhanVat();
            try
            {
                int total = 0;
                int complet = 0;
                for (int i = 0; i < 1; i++)
                {
                    if (mCharacter.RutOutfit.Value && !mCharacter.RutOutfitXong.Value)
                    {
                        total++;
                        mGeneralFunctions.RutOutfit();
                        if (mCharacter.RutOutfitXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.CheMatBao.Value && !mCharacter.CheMatBaoXong.Value)
                    {
                        total++;
                        mGeneralFunctions.RunCheMatBao();
                        if (mCharacter.CheMatBaoXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.DieuKhac.Value && !mCharacter.DieuKhacXong.Value)
                    {
                        total++;
                        mGeneralFunctions.KhongGianDieuKhac();
                        if (mCharacter.DieuKhacXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.TrongNL.Value && !mCharacter.TrongNLXong.Value)
                    {
                        total++;
                        mGeneralFunctions.TrongNL();
                        if (mCharacter.TrongNLXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.NangNo.Value && !mCharacter.NangNoXong.Value)
                    {
                        total++;
                        mGeneralFunctions.ChoTLAn();
                        if (mCharacter.NangNoXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.LatBai.Value && !mCharacter.LatBaiXong.Value)
                    {
                        total++;
                        mGeneralFunctions.AutoLatThe();
                        if (mCharacter.LatBaiXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.KhoiPhuc.Value && !mCharacter.KhoiPhucXong.Value)
                    {
                        total++;
                        mGeneralFunctions.hoiPhuc();
                        if (mCharacter.KhoiPhucXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.HanhLang.Value && !mCharacter.HanhLangXong.Value)
                    {
                        total++;
                        mGeneralFunctions.NhanThuongHanhLang();
                        if (mCharacter.HanhLangXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.PhuBan.Value && !mCharacter.PhuBanXong.Value)
                    {
                        total++;
                        mGeneralFunctions.RunNhanAutoPB();
                        if (mCharacter.PhuBanXong.Value)
                        {
                            complet++;
                        }
                    }

                   

                    if (complet >= total)
                    {
                        break;
                    }
                }

                mGeneralFunctions.ClickAnToan();
                while (mCharacter.TuHanh.Value && !mCharacter.TuHanhXong.Value)
                {
                    mGeneralFunctions.RunAutoTuHanh();
                }

                mAuto.WriteStatus($"Hoàn tất quá trình chạy {complet}/{total} auto");

            }
            catch (Exception ex)
            {
                Helper.WriteStatus(mTextBoxStatus, $"{mCharacter.Name.Value}", $"Lỗi khi thực hiện auto: {ex.Message}");
            }
        }

        public void batPet()
        {
            runAction("batPet", () => mGeneralFunctions.BatPet());
        }

        public void AutoLatThe()
        {
            runAction("AutoLatThe", () => mGeneralFunctions.AutoLatThe());
        }

        public void hoiPhuc()
        {
            runAction("hoiPhuc", () => mGeneralFunctions.hoiPhuc());
        } 

        public void nuoiTL()
        {
            runAction("nuoiTL", () => mGeneralFunctions.ChoTLAn());
        }

        public void trainMap()
        {
            runAction("trainMap", () => mGeneralFunctions.TrainQuai());
        }

        public void xuQue()
        {
            runAction("xuQue", () => mGeneralFunctions.xuQue());
        }

        public void runCheMatBao()
        {
            runAction("runCheMatBao", () => mGeneralFunctions.RunCheMatBao());
        }

        public void nhanThuongHanhLang()
        {
            runAction("nhanThuongHanhLang", () => mGeneralFunctions.NhanThuongHanhLang());
        }
        public void trongNL()
        {
            runAction("trongNL", () => mGeneralFunctions.TrongNL());
        }

        public void runAutoTuHanh()
        {
            runAction("runAutoTuHanh", () => mGeneralFunctions.RunAutoTuHanh());
        }
        public void runNhanAutoPB()
        {
            runAction("runNhanAutoPB", () => mGeneralFunctions.RunNhanAutoPB());
        }

        public void rutBo()
        {
            runAction("rutBo", () => mGeneralFunctions.RutOutfit());
        }

        public void khongGianDieuKhac()
        {
            runAction("khongGianDieuKhac", () => mGeneralFunctions.KhongGianDieuKhac());
        }

        private void runAction(String actionName, Action action)
        {
            //if (mCharacter.HWnd != IntPtr.Zero)
            //{
            //    Helper.WriteStatus(mTextBoxStatus, mCharacter.No.ToString(), "Nhân vật " + mCharacter.Name + " đang không được chạy hoặc đang chạy auto khác như: event, ...");
            //    return;
            //}
            // startGameIfNotExists();

            try
            {
                action();
            }
            catch (Exception ex)
            {
                Helper.WriteStatus(mTextBoxStatus, $"{mCharacter.Name.Value}", $"Lỗi khi thực hiện hành động {actionName}: {ex.Message}");
            }

            //mCharacter.Running = 0;
            //Helper.saveSettingsToXML(mCharacter);
            //foreach (var thread in Helper.threadList)
            //{
            //    if (thread.Name == (mCharacter.ID + actionName))
            //    {
            //        Helper.writeStatus(mTextBoxStatus, mCharacter.ID, "Đã ngừng auto");
            //        thread.Abort();
            //    }
            //}
        }
    }
}
