using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPT_Login.Models;
using static Emgu.CV.OCR.Tesseract;

namespace VPT_Login.Libs
{
    public class AutoPhuBan
    {
        private DataModel mCharacter;
        private AutoFeatures mAuto;
        private string[] mPhuBan = new string[8];

        public AutoPhuBan(DataModel mChar, AutoFeatures mAuto)
        {
            this.mCharacter = mChar;
            this.mAuto = mAuto;
        }

        public void NhanPhuBanTLTByNVHN()
        {
            int i = 0;
            while (mPhuBan[i] != null && i <= Constant.MaxLoopQ)
            {
                

                while (!mAuto.IsTalkWithNPC("sugiamophuban"))
                {
                    mAuto.TalkToNPC("sugiamophuban", mapName: "tienlapthanh");
                }

                // Click vào nhiệm vụ được trả hoặc chưa nhận
                mAuto.ClickImageByGroup("phu_ban", mPhuBan[i], false, true);

                // Click nhận nhiệm vụ và trả nhiệm vụ
                mAuto.ClickImageByGroup("phu_ban", "nhannhiemvu", false, true);
                mAuto.ClickImageByGroup("phu_ban", "xong", false, true);

                mAuto.WriteStatus("Nhận phụ bản thành công ...");
                Thread.Sleep(Constant.TimeShort);
                i++;
            }
        }

        public bool auto()
        {
            nhanThuongPhuBan();

            // Chuyển trang 1
            mAuto.ClickImageByGroup("phu_ban", "prevpage");
            int i = 0;
            while (mPhuBan[i] != null && i <= Constant.MaxLoopQ)
            {
                

                // Chuyển trang
                if (mPhuBan[i] == "thamhiem" || mPhuBan[i] == "thegioiso")
                {
                    mAuto.ClickImageByGroup("phu_ban", "nextpage");
                }
                else
                {
                    mAuto.ClickImageByGroup("phu_ban", "prevpage");
                }


                // Chọn độ khó
                if (mPhuBan[i] == "lietdiemthamuyen"
                    || mPhuBan[i] == "trolailanghuyet"
                    || mPhuBan[i] == "quyhutmau" || mPhuBan[i] == "thegioiso")
                {
                    mAuto.ClickImageByGroup("phu_ban", "batdau" + mPhuBan[i], false, false, 1, 120);
                    Thread.Sleep(Constant.TimeShort);
                    //mAuto.ClickRightToImage("/phu_ban/batdau" + mPhuBan[i]+".png", 120, 35);
                    mAuto.ClickImageByGroup("phu_ban", "batdau" + mPhuBan[i], false, false, 1, 120, 35);
                }

                // Auto phụ bản
                //mAuto.ClickRightToImage("/phu_ban/batdau" + mPhuBan[i]+".png", 40, 40);
                mAuto.ClickImageByGroup("phu_ban", "batdau" + mPhuBan[i], false, false, 1, 45, 37);
                mAuto.ClickImageByGroup("global", "xacnhanco", false, true);
                Thread.Sleep(Constant.TimeShort);
                i++;
            }

            if (i >= Constant.MaxLoopQ)
            {
                return false;
            }

            return true;
        }

        public bool nhanThuongPhuBan()
        {
            mAuto.CloseAllDialog();

            // Mở auto phụ bản
            while (!mAuto.FindImageByGroup("phu_ban", "hoanthanhphuban_check"))
            {
                // Kéo quick feature list lên trên cùng
                while (mAuto.FindImageByGroup("global", "quickFeatureListUpArrow") && !mAuto.FindImageByGroup("phu_ban", "hoanthanhphuban"))
                {
                    mAuto.WriteStatus("Kéo lên đầu quick feature list");
                    mAuto.ClickImageByGroup("global", "quickFeatureListUpArrow");
                    Thread.Sleep(Constant.TimeShort);
                }

                // Kéo quick feature list xuống để tìm Auto Phụ bản
                while (!mAuto.FindImageByGroup("phu_ban", "hoanthanhphuban") && mAuto.FindImageByGroup("global", "quickFeatureListDownArrow"))
                {
                    mAuto.WriteStatus("Không tìm thấy Auto Phụ bản, di chuyển sang trang tiếp");
                    mAuto.ClickImageByGroup("global", "quickFeatureListDownArrow");
                    Thread.Sleep(Constant.TimeShort);
                }

                // Mở auto phụ bản
                mAuto.ClickImageByGroup("phu_ban", "hoanthanhphuban");
                Thread.Sleep(Constant.TimeMedium);
            }

            int i = 0;
            // Nhận hoàn thành phụ bản nếu có
            while (mAuto.FindImageByGroup("phu_ban", "nhanthuong") && i <= Constant.MaxLoopQ)
            {
                mAuto.ClickImageByGroup("phu_ban", "nhanthuong");
                Thread.Sleep(Constant.TimeMedium);
                i++;
            }

            // Chuyển trang 2 và nhận hoàn thành phụ bản nếu có
            mAuto.ClickImageByGroup("phu_ban", "nextpage");
            while (mAuto.FindImageByGroup("phu_ban", "nhanthuong") && i <= Constant.MaxLoopQ + 5)
            {
                mAuto.ClickImageByGroup("phu_ban", "nhanthuong");
                Thread.Sleep(Constant.TimeMedium);
                i++;
            }

            return true;
        }

        public void SetPhuBan()
        {
            mAuto.WriteStatus("Lấy thông tin các phụ bản sẽ auto ...");
            int i = 0;
            if (mCharacter.MHD.Value)
            {
                mPhuBan[i]="mehuyendong";
                i++;
            }

            if (mCharacter.MC.Value)
            {
                mPhuBan[i] = "khobaudaimac";
                i++;
            }

            if (mCharacter.LTC.Value)
            {
                mPhuBan[i] = "luctiencanh";
                i++;
            }

            if (mCharacter.LD.Value)
            {
                mPhuBan[i] = "lietdiemthamuyen";
                i++;
            }

            if (mCharacter.VLD.Value)
            {
                mPhuBan[i] = "trolailanghuyet";
                i++;
            }

            if (mCharacter.Muoi.Value)
            {
                mPhuBan[i] = "quyhutmau";
                i++;
            }

            if (mCharacter.TGS.Value)
            {
                mPhuBan[i] = "thegioiso";
                i++;
            }

            if (mCharacter.Tham.Value)
            {
                mPhuBan[i] = "thamhiem";
                i++;
            }
        }
    }
}
