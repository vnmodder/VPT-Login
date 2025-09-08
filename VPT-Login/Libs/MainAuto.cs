using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class MainAuto
    {
        private string mWindowName;
        public AutoFeatures mAuto;
        private DataModel mCharacter;
        private string[] mMembers;
        public GeneralFunctions mGeneralFunctions;
        ReactiveProperty<string> mTextBoxStatus;

        public MainAuto(DataModel character, ReactiveProperty<string> textBoxStatus)
        {
            mCharacter = character;
            mWindowName = $"{mCharacter.Server}-{mCharacter.Name}";
            mTextBoxStatus = textBoxStatus;
            mAuto = new AutoFeatures(character, mWindowName, textBoxStatus);
            mGeneralFunctions = new GeneralFunctions(mCharacter, mWindowName, textBoxStatus);
        }

        public void ChayHet()
        {
            mAuto.WriteStatus("Bắt đầu chạy các auto đã chọn");
            try
            {
                int total = 0;
                int complet = 0;
                for (int i = 0; i < 1; i++)
                {
                    if (mCharacter.RutOutfit.Value && !mCharacter.RutOutfitXong.Value)
                    {
                        total++;
                        mGeneralFunctions.rutBo();
                        if (mCharacter.RutOutfitXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.CheMatBao.Value && !mCharacter.CheMatBaoXong.Value)
                    {
                        total++;
                        mGeneralFunctions.runCheMatBao();
                        if (mCharacter.CheMatBaoXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.DieuKhac.Value && !mCharacter.DieuKhacXong.Value)
                    {
                        total++;
                        mGeneralFunctions.khongGianDieuKhac();
                        if (mCharacter.DieuKhacXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.TrongNL.Value && !mCharacter.TrongNLXong.Value)
                    {
                        total++;
                        mGeneralFunctions.trongNL();
                        if (mCharacter.TrongNLXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.HanhLang.Value && !mCharacter.HanhLangXong.Value)
                    {
                        total++;
                        mGeneralFunctions.nhanThuongHanhLang();
                        if (mCharacter.HanhLangXong.Value)
                        {
                            complet++;
                        }
                    }

                    if (mCharacter.PhuBan.Value && !mCharacter.PhuBanXong.Value)
                    {
                        total++;
                        mGeneralFunctions.runNhanAutoPB();
                        if (mCharacter.PhuBanXong.Value)
                        {
                            complet++;
                        }
                    }

                    if(complet>= total)
                    {
                        break;
                    }
                }

                mGeneralFunctions.ClickAnToan();
                while(mCharacter.TuHanh.Value && !mCharacter.TuHanhXong.Value)
                {
                    mGeneralFunctions.runAutoTuHanh();
                }

                mAuto.WriteStatus("Hoàn tất quá trình chạy auto");

            }
            catch (Exception ex)
            {
                Helper.WriteStatus(mTextBoxStatus, $"{mCharacter.Name}", $"Lỗi khi thực hiện auto: {ex.Message}");
            }
        }

        public void batPet()
        {
            runAction("batPet", () => mGeneralFunctions.batPet());
        }

        public void runCheMatBao()
        {
            runAction("runCheMatBao", () => mGeneralFunctions.runCheMatBao());
        }

        public void nhanThuongHanhLang()
        {
            runAction("nhanThuongHanhLang", () => mGeneralFunctions.nhanThuongHanhLang());
        }
        public void trongNL()
        {
            runAction("trongNL", () => mGeneralFunctions.trongNL());
        }

        public void runAutoTuHanh()
        {
            runAction("runAutoTuHanh", () => mGeneralFunctions.runAutoTuHanh());
        }
        public void runNhanAutoPB()
        {
            runAction("runNhanAutoPB", () => mGeneralFunctions.runNhanAutoPB());
        }

        public void rutBo()
        {
            runAction("rutBo", () => mGeneralFunctions.rutBo());
        }

        public void khongGianDieuKhac()
        {
            runAction("khongGianDieuKhac", () => mGeneralFunctions.khongGianDieuKhac());
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
                Helper.WriteStatus(mTextBoxStatus, $"{mCharacter.Name}", $"Lỗi khi thực hiện hành động {actionName}: {ex.Message}");
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
