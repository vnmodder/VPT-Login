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
            mAuto.WriteStatus("Bắt đầu ép pet");
            epPetByColor("trang");
            mAuto.WriteStatus($"Ép {Constant.PetList[mCharacter.PetKey]} trắng xong");
            epPetByColor("luc");
            mAuto.WriteStatus($"Ép {Constant.PetList[mCharacter.PetKey]}  xanh lá xong");
        }

        public void epPetByColor( string color)
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
    }
}
