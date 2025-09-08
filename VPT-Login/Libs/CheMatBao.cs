using System.Threading;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class CheMatBao
    {
        private DataModel mChar;
        private AutoFeatures mAuto;

        public CheMatBao(DataModel mChar, AutoFeatures auto)
        {
            this.mChar = mChar;
            mAuto = auto;
        }

        public bool CheTaoMatBao()
        {
            int loop = 1;

            // Mở bảng theo cấp mật bảo cần chế
            mAuto.ClickImageByGroup("mat_bao", "tieudecapmatbao", false, false, 1, 20, -20 + (int.Parse(mChar.CapMB.Value) * 25));

            // Click điểm an toàn
            mAuto.ClickImageByGroup("mat_bao", "clickantoan");

            // Mở bảng theo loại mật bảo cần chế
            mAuto.ClickImageByGroup("mat_bao", mChar.LoaiMB.Value);

            // Chế mật bảo
            if(mAuto.FindImageByGroup("mat_bao", "hetluotche"))
            {
                mAuto.WriteStatus("Hôm nay đã hết lượt chế");
                return false;
            }

            while (loop <= Constant.MaxLoopQ)
            {

                // Click tự đặt nguyên liệu
                mAuto.ClickImageByGroup("mat_bao", "tudongdatnguyenlieu");

                // Click chế tạo
                mAuto.ClickImageByGroup("mat_bao", "chetaomatbao", false, false);

                if(mAuto.FindImageByGroup("mat_bao", "khongdunguyenlieu"))
                {
                    mAuto.WriteStatus("Đã dừng vì không đủ nguyên liệu");
                    Thread.Sleep(Constant.TimeMediumShort);
                    mAuto.ClickImageByGroup("mat_bao", "xacnhankhongdu", false, true);
                    return false;
                }

                // Click điểm an toàn
                mAuto.ClickImageByGroup("mat_bao", "clickantoan");
                Thread.Sleep(2000);
                loop++;
            }
            mChar.CheMatBaoXong.Value = true;
            mAuto.WriteStatus("Chế mật bảo hoàn thành");
            return true;
        }

        public bool MoBangCheMB()
        {
            mAuto.CloseAllDialog();

            // Mở bảng nhân vật
            mAuto.ClickImageByGroup("global", "nhanvat", false, false);

            // Mở bảng hồn khí
            mAuto.ClickImageByGroup("mat_bao", "honkhi", false, false);

            // Chờ 5s
            Thread.Sleep(4000);

            // Mở bảng mật bảo
            mAuto.ClickImageByGroup("mat_bao", "matbao", true, true);

            // Mở bảng chế tạo
            mAuto.ClickImageByGroup("mat_bao", "chetao", true, true);

            // Kiểm tra đã mở dc bảng chế tao mật bảo chưa ?
            if (!mAuto.FindImageByGroup("mat_bao", "chetaomatbao", false, true))
            {
                MoBangCheMB();
            }

            return true;
        }
    }
}
