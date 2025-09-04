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
            mAuto = new AutoFeatures(character, mWindowName , textBoxStatus);
            mGeneralFunctions = new GeneralFunctions(mCharacter, mWindowName, textBoxStatus);
        }
        public void batPet()
        {
            runAction("batPet", () => mGeneralFunctions.batPet());
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
