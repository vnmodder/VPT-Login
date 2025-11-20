using Reactive.Bindings;
using System;

namespace VPT_Login.Models
    {
    public class DataModel
    {
        public ReactiveProperty<int> Id { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> Server { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> Version { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> Link { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<int> Status { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Online { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<bool> Relog { get; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<int> No { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<IntPtr> HWnd { get; } = new ReactiveProperty<IntPtr>(IntPtr.Zero);
        public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<int> Running { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<bool> RutOutfitXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> CheMatBaoXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> DieuKhacXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> HanhLangXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> TuHanhXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> KhoiPhucXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> LatBaiXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> PhuBanXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> NangNoXong { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> TrongNLXong { get; } = new ReactiveProperty<bool>();


        public ReactiveProperty<bool> IsChecked { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<string> PetKey { get; } = new ReactiveProperty<string>("cao");
        public ReactiveProperty<string> PetOption { get; } = new ReactiveProperty<string>("khong");
        public ReactiveProperty<bool> ChiEp { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> NLKey { get; } = new ReactiveProperty<string>("kim_loai");
        public ReactiveProperty<string> NLDoiNNKey { get; } = new ReactiveProperty<string>("gam_voc");
        public ReactiveProperty<string> LoaiMB { get; } = new ReactiveProperty<string>("thanbinh");
        public ReactiveProperty<string> CapMB { get; } = new ReactiveProperty<string>("1");
        public ReactiveProperty<bool> RutOutfit { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> CheMatBao { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> DieuKhac { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> HanhLang { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> TuHanh { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> KhoiPhuc { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> LatBai { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> PhuBan { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> NangNo { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> TrongNL { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> MHD { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> MC { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> LTC { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> LD { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> VLD { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> Muoi { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> TGS { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> Tham { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<string> NhomAuto { get; } = new ReactiveProperty<string>("0");
        public ReactiveProperty<bool> CheckAuto { get; } = new ReactiveProperty<bool>(false);

        public DataModel(int id, string name, string server, string version, string link, int status)
        {
            this.Id.Value = id;
            this.Name.Value = name;
            this.Server.Value = server;
            this.Version.Value = version;
            this.Link.Value = link;
            this.Status.Value = status;
        }

        public DataModel(XMLDataModel xml)
        {
            this.Id.Value = xml.Id;
            this.Name.Value = xml.Name;
            this.Server.Value = xml.Server;
            this.Version.Value = xml.Version;
            this.Link.Value = xml.Link;
            this.Status.Value = xml.Status;

            this.IsChecked.Value = xml.IsChecked;
            this.PetKey.Value = xml.PetKey;
            this.PetOption.Value = xml.PetOption;
            this.ChiEp.Value = xml.ChiEp;
            this.NLKey.Value = xml.NLKey;
            this.NLDoiNNKey.Value = xml.NLDoiNNKey;
            this.LoaiMB.Value = xml.LoaiMB;
            this.CapMB.Value = xml.CapMB;

            this.RutOutfit.Value = xml.RutOutfit;
            this.CheMatBao.Value = xml.CheMatBao;
            this.DieuKhac.Value = xml.DieuKhac;
            this.HanhLang.Value = xml.HanhLang;
            this.TuHanh.Value = xml.TuHanh;
            this.KhoiPhuc.Value = xml.KhoiPhuc;
            this.LatBai.Value = xml.LatBai;
            this.PhuBan.Value = xml.PhuBan;
            this.NangNo.Value = xml.NangNo;
            this.TrongNL.Value = xml.TrongNL;

            this.MHD.Value = xml.MHD;
            this.MC.Value = xml.MC;
            this.LTC.Value = xml.LTC;
            this.LD.Value = xml.LD;
            this.VLD.Value = xml.VLD;
            this.Muoi.Value = xml.Muoi;
            this.TGS.Value = xml.TGS;
            this.Tham.Value = xml.Tham;
        }

        public XMLDataModel ToXmlModel()
        {
            return new XMLDataModel
            {
                Id = this.Id.Value,
                Name = this.Name.Value,
                Server = this.Server.Value,
                Version = this.Version.Value,
                Link = this.Link.Value,
                Status = this.Status.Value,

                IsChecked = this.IsChecked.Value,
                PetKey = this.PetKey.Value,
                PetOption = this.PetOption.Value,
                ChiEp = this.ChiEp.Value,
                NLKey = this.NLKey.Value,
                NLDoiNNKey = this.NLDoiNNKey.Value,
                LoaiMB = this.LoaiMB.Value,
                CapMB = this.CapMB.Value,

                RutOutfit = this.RutOutfit.Value,
                CheMatBao = this.CheMatBao.Value,
                DieuKhac = this.DieuKhac.Value,
                HanhLang = this.HanhLang.Value,
                TuHanh = this.TuHanh.Value,
                KhoiPhuc = this.KhoiPhuc.Value,
                LatBai = this.LatBai.Value,
                PhuBan = this.PhuBan.Value,
                NangNo = this.NangNo.Value,
                TrongNL = this.TrongNL.Value,

                MHD = this.MHD.Value,
                MC = this.MC.Value,
                LTC = this.LTC.Value,
                LD = this.LD.Value,
                VLD = this.VLD.Value,
                Muoi = this.Muoi.Value,
                TGS = this.TGS.Value,
                Tham = this.Tham.Value
            };
        }
    }
}
