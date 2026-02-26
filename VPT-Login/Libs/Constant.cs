using Emgu.CV;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;

namespace VPT_Login.Libs
{
    public static class Constant
    {
        //public static string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string rootPath = string.Empty;
        public static IntPtr hwnd = IntPtr.Zero;

        public static ConcurrentDictionary<string, Bitmap> TemplateCache = new ConcurrentDictionary<string, Bitmap>();

        public const string asset = "Assets";
        public const string img_cn = "IMG/CN";
        public const string tracking = "tracking";
        public const string FLASH_NAME = "Adobe Flash Player 10";


        public static class FilePath
        {
            public static string XML_PATH = asset + "\\data.vpt";
            public static string FORCE_BIND_IP = asset + "\\ForceBindIP.exe";
            public static string FLASH_PLAYER = asset + "\\flash.exe";
        }

        public const int VeryTimeShort = 100; // Short time
        public const int TimeMediumShort = 300; // Short time
        public const int TimeShort = 1000; // Short time
        public const int TimeM = 2000; // Short time
        public const int TimeMedium = 3000; // Medium time
        public const int TimeLong = 5000; // Long time

        public const string ImagePathGlobalXuong = "/global/xuong.png";
        public const string ImagePathGlobalBay = "/global/bay.png";
        public const string ImagePathKhongTrongTranDau = "/global/khongtrongtrandau.png";


        public const string ImagePathGlobalFolder = "/global/";
        public const string ImagePathBatPetFolder = "/bat_pet/";
        public const string ImagePathTinhLinhFolder = "/tlinh/";
        public const string ImagePathThaiCoFolder = "/thai_co/";
        public const string ImagePathXuQueFolder = "/xu_que/";
        public const string ImagePathRutBoFolder = "/rutbo/";
        public const string ImagePathMapsFolder = "/maps/";
        public const string ImagePathPhuBanFolder = "/phu_ban/";
        public const string ImagePathMatBaoFolder = "/mat_bao/";
        public const string ImagePathCharNameFolder = "/char_name/";
        public const string ImagePathInMapFolder = "/in_map/";
        public const string ImagePathTriAnFolder = "/tri_an/";
        public const string ImagePathEventFolder = "/event/";
        public const string ImagePathNVHNFolder = "/nvhn/";
        public const string ImagePathSTMTFolder = "/stmt/";
        public const string ImagePathNLFolder = "/nguyen_lieu/";
        public const string ImagePathTMFolder = "/train_map/";

        public static Dictionary<string, string> PetList = new Dictionary<string, string>()
        {
             { "cao", "Cáo tuyết - Thần 4800" },
             { "ts", "TS Eskimos - Trí 4600" },
             { "cg", "Cơ Giáp 3000 - Xảo 4500" },
             { "mn", "TT Mã Não - Nhẫn 4800" },
             { "cl", "Bàn thạch Long - Thần 5000" },
             { "cm", "TS Cá mập - Lực 4930" },
             { "tkl", "TK Long - Lực 5200" },
             { "mt", "Ma thạch - Xảo  4600" },
             { "hcg", "Hỏa cơ giáp - Nhẫn  5000" },
        };

        public static Dictionary<string, string> PetOptions = new Dictionary<string, string>()
        {
            {"khong", "Không" },
            {"kep", "Kẹp" },
            {"dan", "Đan" },
        };

        public static Dictionary<string, string> NLOptions = new Dictionary<string, string>()
        {
            {"vai", "Vải thô" },
            {"long_thu", "Lông thú" },
            {"kim_loai", "Kim loại" },
            {"go", "Gỗ" },
            {"ngoc", "Ngọc" },
            {"gam_voc", "Gấm vóc" },
            {"da_thu", "Da thú" },
            {"kim_loai_hiem", "Kim loại hiếm" },
            {"go_tot", "Gỗ tốt" },
            {"pha_le", "Pha lê" },
        };

        public static Dictionary<string, string> ResetAutoOptions = new Dictionary<string, string>()
        {
            {"0", "Không nhóm" },
            {"1", "Nhóm 1" },
            {"2", "Nhóm 2" },
            {"3", "Nhóm 3" },
            {"4", "Nhóm 4" },
            {"5", "Nhóm 5" },
            {"6", "Nhóm 6" },
            {"7", "Nhóm 7" },
            {"8", "Nhóm 8" },
            {"9", "Nhóm 9" },
        };

        public static Dictionary<string, string> DoiNNOptions = new Dictionary<string, string>()
        {
            {"gam_voc", "Gấm vóc" },
            {"da_thu", "Da thú" },
            {"kim_loai_hiem", "Kim loại hiếm" },
            {"go_tot", "Gỗ tốt" },
            {"pha_le", "Pha lê" },
        };

        public static Dictionary<string, string> DicLoaiMB = new Dictionary<string, string>()
        {
            { "phapsuc",    "Pháp Sức" },
            { "vouu",       "Vô Ưu" },
            { "thanhdien",  "Thánh Điện" },
            { "hangdong",   "Hang Động" },
            { "daimac",     "Đại Mạc" },
            { "dicanh",     "Di Cảnh" },
            { "lietdiem",   "Liệt Diễm" },
            { "langhuyet",  "Lang Huyệt" },
            { "lacvien",    "Lạc Viên" },
            { "chientrang", "Chiến Trang" },
            { "thanbinh",   "Thần Binh" }
        };

        public static Dictionary<string, string> DicCapMB = new Dictionary<string, string>()
        {
            { "1",    "Cấp 1" },
            { "2",    "Cấp 2" },
            { "3",    "Cấp 3" },
            { "4",    "Cấp 4" },
            { "5",    "Cấp 5" },
            { "6",    "Cấp 6" },
        };

        public const int MaxLoopShort = 3;
        public const int MaxLoopQ = 10;
        public const int MaxLoop = 20;

        public const string ImagePathDoiThoai = "/doi_thoai/";
        public const string ImagePathViTriNPC = "/vi_tri_npc/";

        public static Dictionary<string, string> Maps = new Dictionary<string, string>()
        {
            { "0",    "Không" },
            { "MHL",    "Mị Hoặc Lâm" },
            { "TD",    "Thánh Đường" },
            { "VLS",    "Vân Lộc Sơn" },
            { "DVD",    "Đăng Văn Địa" },
            { "CT",    "Cổ Thành" },
        };

        public static List<Point> MapVLS = new List<Point>()
        {
            new Point(150,70),
            //new Point(140,140),
            //new Point(220,120),
            new Point(210,65),
            //new Point(130,110),
        };
        public static List<Point> MapCT = new List<Point>()
        {
            //new Point(150,50),
            //new Point(240,60),
            //new Point(180,90),
            new Point(200,150),
            new Point(260,110),
        };
        public static List<Point> MapDVD = new List<Point>()
        {
          //new Point(240,70),
       // new Point(130,80),
   //new Point(180,115),
    new Point(150,130),
    new Point(260,135),
        };
        public static List<Point> MapMHL = new List<Point>()
        {
            new Point(160,50),
    //new Point(260,150),
    new Point(230,90),
    //new Point(160,110),
    //new Point(260,110),
        };
        public static List<Point> MapTD = new List<Point>()
        {
             new Point(120,50),
    new Point(260,150),
    new Point(260,50),
    //new Point(180,100),
    new Point(120,150),
        };
    }
}
