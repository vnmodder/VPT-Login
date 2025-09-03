using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPT_Login.Libs
{
    public static class Constant
    {
        //public static string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string rootPath = string.Empty;

        public const string asset = "Assets";
        public const string img_cn = "IMG/CN";
        public const string tracking = "tracking";
        public const string FLASH_NAME = "Adobe Flash Player 10";


        public static class FilePath
        {
            public static string XML_PATH =asset +"\\data.vpt";
            public static string FORCE_BIND_IP = asset + "\\ForceBindIP.exe";
            public static string FLASH_PLAYER = asset + "\\flash.exe";
        }

        public const int VeryTimeShort = 100; // Short time
        public const int TimeMediumShort = 300; // Short time
        public const int TimeShort = 1000; // Short time
        public const int TimeMedium = 3000; // Medium time
        public const int TimeLong = 5000; // Long time

        public const string ImagePathGlobalXuong = img_cn + "/global/xuong.png";
        public const string ImagePathGlobalBay = "/global/bay.png";
        public const string ImagePathKhongTrongTranDau = "/global/khongtrongtrandau.png";

        public const string ImagePathGlobalFolder = "/global/";

        public const string ImagePathBatPetFolder = "/bat_pet/";

        public static Dictionary<string, string> PetList = new Dictionary<string, string>()
        {
             { "cao", "Cáo Trắng Tuyết Nguyên" },
             { "ts", "Thợ Săn Eskimos" },
             { "cg", "Cơ Giáp Phá Băng Loại 300" },
             { "mn", "Tuyết Thú Mã Não" },            
        };

    }
}
