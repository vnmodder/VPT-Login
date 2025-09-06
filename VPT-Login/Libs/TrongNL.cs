using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class TrongNL
    {
        private DataModel _model;
        private AutoFeatures mAuto;

        public TrongNL(DataModel model, AutoFeatures mAuto)
        {
            _model = model;
            this.mAuto = mAuto;
        }

        public void thuHoach()
        {
            mAuto.WriteStatus("Thu hoạch nguyên liệu ...");
            mAuto.ClickImageByGroup("nguyen_lieu", "thu_hoach");
            Thread.Sleep(Constant.TimeShort);
            mAuto.ClickImageByGroup("nguyen_lieu", "diem_thu_hoach");
            mAuto.WriteStatus("Đã trồng xong nguyên liệu ...");
        }

        public void trong()
        {
            mAuto.WriteStatus("Trồng Nguyên Liệu ...");
            bool trong = true;
            int i = 0;
            while (trong && i < Constant.MaxLoop)
            {
                trong = mAuto.ClickToImage(Constant.ImagePathNLFolder + "dat_trong.png", 0, -25);
                i++;
            }
        }

        public bool kiemTraSoDatTrong()
        {
            mAuto.WriteStatus("Kiểm tra còn đất trống để trồng hay không ?");
            return mAuto.FindImage(Constant.ImagePathNLFolder + "dat_trong.png");
        }

        public void moNuoiTrong()
        {
            mAuto.WriteStatus("Mở bảng nuôi trồng nguyên liệu ...");
            mAuto.ClickImageByGroup("nguyen_lieu", "nuoi_trong");
            Thread.Sleep(Constant.TimeShort);
        }

        public void dongTrangVien()
        {
            mAuto.WriteStatus("Đóng trang viên ...");
            mAuto.ClickImageByGroup("nguyen_lieu", "trang_vien");
        }

        public void moTrangVien()
        {
            mAuto.CloseAllDialog();
            // Mở menu phải
            if (mAuto.FindImageByGroup("global", "momenuphai"))
            {
                mAuto.WriteStatus("Mở menu phải ...");
                mAuto.MoMenuPhai();
            }
            mAuto.ClickImageByGroup("nguyen_lieu", "trang_vien", false, true);
            mAuto.WriteStatus("Chờ 1 thời gian để flash tải thông tin mở trang viên ...");
            Thread.Sleep(Constant.TimeMedium);
        }

    }
}
