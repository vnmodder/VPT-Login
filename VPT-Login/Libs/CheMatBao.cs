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
    public class CheMatBao
    {
        private DataModel mChar;
        private AutoFeatures mAuto;
        private string mLoaiMB;
        private int mCapMB;

        public CheMatBao(DataModel mChar, AutoFeatures auto)
        {
            this.mChar = mChar;
            mAuto = auto;
        }

        public bool che()
        {
            int loop = 1;

            // Mở bảng theo cấp mật bảo cần chế
            mAuto.ClickImageByGroup("mat_bao", "tieudecapmatbao", false, false, 1, 20, -20 + (mCapMB * 25));

            // Click điểm an toàn
            mAuto.ClickImageByGroup("mat_bao", "clickantoan");

            // Mở bảng theo loại mật bảo cần chế
            mAuto.ClickImageByGroup("mat_bao", mLoaiMB);

            // Chế mật bảo
            //!mAuto.findImageByGroup("mat_bao", "hetluotche") &&
            while (loop <= Constant.MaxLoopQ)
            {
                // Click tự đặt nguyên liệu
                mAuto.ClickImageByGroup("mat_bao", "tudongdatnguyenlieu");

                // Click chế tạo
                mAuto.ClickImageByGroup("mat_bao", "chetaomatbao", false, false);

                // Click điểm an toàn
                mAuto.ClickImageByGroup("mat_bao", "clickantoan");
                Thread.Sleep(2000);
                loop++;
            }

            return true;
        }

        public bool moBangCheMB()
        {
            mAuto.CloseAllDialog();

            // Mở bảng nhân vật
            mAuto.ClickImageByGroup("global", "nhanvat", false, false);

            // Mở bảng hồn khí
            mAuto.ClickImageByGroup("mat_bao", "honkhi", false, false);

            // Chờ 5s
            Thread.Sleep(5000);

            // Mở bảng mật bảo
            mAuto.ClickImageByGroup("mat_bao", "matbao", true, true);

            // Mở bảng chế tạo
            mAuto.ClickImageByGroup("mat_bao", "chetao", true, true);

            // Kiểm tra đã mở dc bảng chế tao mật bảo chưa ?
            if (!mAuto.FindImageByGroup("mat_bao", "chetaomatbao", false, true))
            {
                moBangCheMB();
            }

            return true;
        }

        public void setLoaiMB(string loaiMB)
        {
            switch (loaiMB)
            {
                case "Pháp Sức":
                    mLoaiMB = "phapsuc";
                    break;
                case "Vô Ưu":
                    mLoaiMB = "vouu";
                    break;
                case "Thánh Điện":
                    mLoaiMB = "thanhdien";
                    break;
                case "Hang Động":
                    mLoaiMB = "hangdong";
                    break;
                case "Đại Mạc":
                    mLoaiMB = "daimac";
                    break;
                case "Di Cảnh":
                    mLoaiMB = "dicanh";
                    break;
                case "Liệt Diễm":
                    mLoaiMB = "lietdiem";
                    break;
                case "Lang Huyệt":
                    mLoaiMB = "langhuyet";
                    break;
                case "Lạc Viên":
                    mLoaiMB = "lacvien";
                    break;
                case "Chiến Trang":
                    mLoaiMB = "chientrang";
                    break;
                case "Thần Binh":
                default:
                    mLoaiMB = "thanbinh";
                    break;
            }
        }

        public void setCapMB(int capMB)
        {
            mCapMB = capMB;
        }
    }
}
