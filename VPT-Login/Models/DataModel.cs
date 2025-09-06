using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VPT_Login.Models
{
    public class DataModel
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string Version { get; set; }
        public string Link { get; set; }
        public int Status { get; set; }

        [XmlIgnore]
        public ReactiveProperty<IntPtr> HWnd { get; } = new ReactiveProperty<IntPtr>(IntPtr.Zero);
        [XmlIgnore]
        public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");
        [XmlIgnore]
        public ReactiveProperty<bool> IsChecked { get; } = new ReactiveProperty<bool>(false);
        [XmlIgnore]
        public ReactiveProperty<int> Running { get; } = new ReactiveProperty<int>(0);

        //Tùy chọn auto
        [XmlIgnore]
        public ReactiveProperty<string> PetKey { get; } = new ReactiveProperty<string>("cao") ;
        [XmlIgnore]
        public ReactiveProperty<string> PetOption { get; } = new ReactiveProperty<string>("khong") ;
        [XmlIgnore]
        public  ReactiveProperty<bool> ChiEp { get;  } = new ReactiveProperty<bool>();

        [XmlIgnore]
        public ReactiveProperty<string> NLKey { get;} = new ReactiveProperty<string>("vai") ;
        [XmlIgnore]
        public ReactiveProperty<string> NLDoiNNKey { get; } = new ReactiveProperty<string>("gam_voc") ;
        [XmlIgnore]
        public ReactiveProperty<string> LoaiMB { get; } = new ReactiveProperty<string>("thanbinh");
        [XmlIgnore]
        public ReactiveProperty<string> CapMB { get; } = new ReactiveProperty<string>("1");

        [XmlIgnore]
        public ReactiveProperty<bool> RutOutfit { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> RutOutfitXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> CheMatBao { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> CheMatBaoXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> DieuKhac { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> DieuKhacXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> HanhLang { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> HanhLangXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> TuHanh { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> TuHanhXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> KhoiPhuc { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> KhoiPhucXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> LatBai { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> LatBaiXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> PhuBan { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> PhuBanXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> NangNo { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> NangNoXong { get; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<bool> TrongNL { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> TrongNLXong { get; } = new ReactiveProperty<bool>();



        // Auto phụ bản
        [XmlIgnore]
        public ReactiveProperty<bool> MHD { get; } = new ReactiveProperty<bool>(false);
        [XmlIgnore]
        public ReactiveProperty<bool> MC { get; } = new ReactiveProperty<bool>(false);
        [XmlIgnore]
        public ReactiveProperty<bool> LTC { get; } = new ReactiveProperty<bool>(false);
        [XmlIgnore]
        public ReactiveProperty<bool> LD { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> VLD { get; } = new ReactiveProperty<bool>(true);
        [XmlIgnore]
        public ReactiveProperty<bool> Muoi { get; } = new ReactiveProperty<bool>(false);
        [XmlIgnore]
        public ReactiveProperty<bool> TGS { get; } = new ReactiveProperty<bool>(false);
        [XmlIgnore]
        public ReactiveProperty<bool> Tham { get; } = new ReactiveProperty<bool>(false);
    }
}
