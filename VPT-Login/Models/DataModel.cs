using Reactive.Bindings;
using System;
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
        public ReactiveProperty<bool> Active { get; } = new ReactiveProperty<bool>(false);
        [XmlIgnore]
        public ReactiveProperty<IntPtr> HWnd { get; } = new ReactiveProperty<IntPtr>(IntPtr.Zero);

    }
}
