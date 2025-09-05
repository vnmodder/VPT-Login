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
        public ReactiveProperty<string> PetKey { get; } = new ReactiveProperty<string>("cao") ;
        [XmlIgnore]
        public ReactiveProperty<string> PetOption { get; } = new ReactiveProperty<string>("khong") ;
        [XmlIgnore]
        public ReactiveProperty<string> NLKey { get;} = new ReactiveProperty<string>("vai") ;
        [XmlIgnore]
        public ReactiveProperty<string> NLDoiNNKey { get; } = new ReactiveProperty<string>("gam_voc") ;
        [XmlIgnore]
        public ReactiveProperty<string> LoaiMB { get; } = new ReactiveProperty<string>("thanbinh");
        [XmlIgnore]
        public ReactiveProperty<string> CapMB { get; } = new ReactiveProperty<string>("1");
        [XmlIgnore]
        public  ReactiveProperty<bool> ChiEp { get;  } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");
    }
}
