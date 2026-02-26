namespace VPT_Login.Models
{
    public class XMLDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string Version { get; set; }
        public string Link { get; set; }
        public int Status { get; set; }

        public bool IsChecked { get; set; } = false;
        public string PetKey { get; set; } = "cao";
        public string PetOption { get; set; } = "khong";
        public bool ChiEp { get; set; }
        public string NLKey { get; set; } = "vai";
        public string NLDoiNNKey { get; set; } = "gam_voc";
        public string LoaiMB { get; set; } = "thanbinh";
        public string CapMB { get; set; } = "1";
        public bool RutOutfit { get; set; } = true;
        public bool CheMatBao { get; set; } = true;
        public bool DieuKhac { get; set; } = true;
        public bool HanhLang { get; set; } = true;
        public bool TuHanh { get; set; } = true;
        public bool KhoiPhuc { get; set; }
        public bool LatBai { get; set; }
        public bool PhuBan { get; set; } = true;
        public bool NangNo { get; set; }
        public bool TrongNL { get; set; } = true;
        public bool CheckAuto { get; set; } = false;
        public string NhomAuto { get; set; } = "0";
        public string map { get; set; } = "0";

        // Auto phụ bản
        public bool MHD { get; set; } = false;
        public bool MC { get; set; } = false;
        public bool LTC { get; set; } = false;
        public bool LD { get; set; } = false;
        public bool VLD { get; set; } = false;
        public bool Muoi { get; set; } = false;
        public bool TGS { get; set; } = false;
        public bool Tham { get; set; } = false;
    }
}
