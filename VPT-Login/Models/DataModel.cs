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
        public ReactiveProperty<IntPtr> HWnd { get; set; } = new ReactiveProperty<IntPtr>(IntPtr.Zero);
        [XmlIgnore]
        public ReactiveProperty<string> PetKey { get; set; } = new ReactiveProperty<string>("cao") ;
        [XmlIgnore]
        public ReactiveProperty<string> PetOption { get; set; } = new ReactiveProperty<string>("khong") ;
        [XmlIgnore]
        public ReactiveProperty<string> NLKey { get; set; } = new ReactiveProperty<string>("vai") ;
        [XmlIgnore]
        public  ReactiveProperty<bool> ChiEp { get; set; } = new ReactiveProperty<bool>();
        [XmlIgnore]
        public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");
    }
}
