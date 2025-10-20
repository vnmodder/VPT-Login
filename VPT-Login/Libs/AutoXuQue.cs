using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class AutoXuQue
    {
        private AutoFeatures mAuto;
        private DataModel mChar;

        public AutoXuQue(AutoFeatures auto, DataModel dataModel)
        {
            mAuto = auto;
            mChar = dataModel;
        }

        public void AutoLatThe()
        {
            mAuto.WriteStatus("Bắt đầu \"Lật thẻ\" ...");
            mAuto.CloseAllDialog();
            Thread.Sleep(Constant.TimeMediumShort);
            int check = 0;
            while (!mAuto.FindImageByGroup("xu_que", "latTheCheck") && check < 5)
            {
                // Mở xủ quẻ
                while (!mAuto.FindImageByGroup("xu_que", "xuQueTui"))
                {
                    // Kéo quick feature list lên trên cùng
                    while (mAuto.FindImageByGroup("global", "quickFeatureListUpArrow") &&
                        !mAuto.FindImageByGroup("global", "quickFeatureXuQue"))
                    {
                        mAuto.ClickImageByGroup("global", "quickFeatureListUpArrow");
                        Thread.Sleep(Constant.TimeShort);
                    }

                    // Kéo quick feature list xuống để tìm xủ quẻ
                    while (!mAuto.FindImageByGroup("global", "quickFeatureXuQue") && mAuto.FindImageByGroup("global", "quickFeatureListDownArrow"))
                    {
                        mAuto.ClickImageByGroup("global", "quickFeatureListDownArrow");
                        Thread.Sleep(Constant.TimeShort);
                    }

                    // Mở xủ quẻ
                    mAuto.ClickImageByGroup("global", "quickFeatureXuQue");
                    Thread.Sleep(Constant.TimeMediumShort);

                    //Mở lật thẻ
                    if (mAuto.FindImageByGroup("xu_que", "iconLatThe"))
                    {
                        mAuto.WriteStatus("Mở lật thẻ");
                        mAuto.ClickImageByGroup("xu_que", "iconLatThe");
                        Thread.Sleep(Constant.TimeMedium);
                        break;
                    }                    
                }

                check++;
            }
            check = 0;
            mAuto.WriteStatus("Đã mở");

            while ( check < 6)
            {
                mAuto.WriteStatus("Click mở thẻ");
                mAuto.ClickImageByGroup("xu_que", "latThe");
                Thread.Sleep(Constant.TimeMediumShort);

                if (mAuto.FindImageByGroup("xu_que", "latthe_ketqua_1",percent:.9) ||
                    mAuto.FindImageByGroup("xu_que", "latthe_ketqua_2", percent: .9) ||
                    mAuto.FindImageByGroup("xu_que", "latthe_ketqua_3", percent: .9))
                {
                    int count = 0;
                    while (count<5)
                    {
                        for (int i = 1; i < 7; i++)
                        {
                            if(mAuto.FindImageByGroup("xu_que", "latthe_so_" + i))
                            {
                                mAuto.ClickImageByGroup("xu_que", "latthe_so_" + i);
                                Thread.Sleep(Constant.TimeMediumShort);
                                count++;
                                continue;
                            }
                        }
                    }

                    mAuto.ClickImageByGroup("xu_que", "lat_thedoi");
                    Thread.Sleep(Constant.TimeMediumShort);
                }

                mAuto.ClickImageByGroup("xu_que", "nhanLatThe");
                Thread.Sleep(Constant.TimeMediumShort);

                mAuto.ClickImageByGroup("xu_que", "latTheCo");
                Thread.Sleep(Constant.TimeMediumShort);
                check++;

                if(mAuto.FindImageByGroup("xu_que", "hetLuot", percent: .9)){
                    break;
                }
            }

            Thread.Sleep(Constant.TimeShort);
            mAuto.CloseAllDialog();
            mChar.LatBaiXong.Value = true;
            mAuto.WriteStatus("Hoàn thành \"Lật thẻ\" ...");
        }
        public bool XuQue()
        {
            mAuto.WriteStatus("Bắt đầu \"Xủ Quẻ\" ...");
            mAuto.CloseAllDialog();

            // Mở xủ quẻ
            while (!mAuto.FindImageByGroup("xu_que", "xuQueTui"))
            {
                // Kéo quick feature list lên trên cùng
                while (mAuto.FindImageByGroup("global", "quickFeatureListUpArrow") &&
                    !mAuto.FindImageByGroup("global", "quickFeatureXuQue"))
                {
                    mAuto.ClickImageByGroup("global", "quickFeatureListUpArrow");
                    Thread.Sleep(Constant.TimeShort);
                }

                // Kéo quick feature list xuống để tìm xủ quẻ
                while (!mAuto.FindImageByGroup("global", "quickFeatureXuQue") && mAuto.FindImageByGroup("global", "quickFeatureListDownArrow"))
                {
                    mAuto.ClickImageByGroup("global", "quickFeatureListDownArrow");
                    Thread.Sleep(Constant.TimeShort);
                }

                // Mở xủ quẻ
                mAuto.ClickImageByGroup("global", "quickFeatureXuQue");
                Thread.Sleep(Constant.TimeMedium);
            }

            while (true)
            {
                // Phân giải nếu thấy
                while (mAuto.FindImageByGroup("xu_que", "xuQueTrang", percent: .9) ||
                    mAuto.FindImageByGroup("xu_que", "xuQueLuc", percent: .9) ||
                    mAuto.FindImageByGroup("xu_que", "xuQueLam", percent: .9))
                {
                    if (mAuto.FindImageByGroup("xu_que", "xuQueTrang", percent: .9))
                        mAuto.ClickImageByGroup("xu_que", "xuQueTrang", x: 22, y: 0, percent: .9);
                    if (mAuto.FindImageByGroup("xu_que", "xuQueLuc", percent: .9))
                        mAuto.ClickImageByGroup("xu_que", "xuQueLuc", x: 22, y: 0, percent: .9);
                    if (mAuto.FindImageByGroup("xu_que", "xuQueLam", percent: .9))
                    {
                        mAuto.ClickImageByGroup("xu_que", "xuQueLam", x: 22, y: 0, percent: .9);

                    }
                    while (mAuto.FindImageByGroup("xu_que", "xuQueXacNhanPhanGiai"))
                    {
                        mAuto.ClickImageByGroup("xu_que", "xuQuePhanGiai");
                    }
                }

                // Xủ quẻ :D
                int i = 0;
                while (i < 3)
                {
                    i++;
                    mAuto.ClickImageByGroup("xu_que", "xuQue3Sao");
                    mAuto.ClickImageByGroup("xu_que", "xuQue4Sao");
                    mAuto.ClickImageByGroup("xu_que", "xuQue5Sao");

                    int j = 0;
                    while (j < 2)
                    {
                        j++;
                        mAuto.ClickImageByGroup("xu_que", "xuQue1Sao");
                        mAuto.ClickImageByGroup("xu_que", "xuQue2Sao");
                    }
                }
            }

            return true;
        }
    }

}
