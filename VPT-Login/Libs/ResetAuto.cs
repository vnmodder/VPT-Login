using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class ResetAuto
    {
        private ReactiveProperty<bool> isChecked;
        private ReactiveCollection<DataModel> characters;

        public ResetAuto(ReactiveProperty<bool> isChecked, ReactiveCollection<DataModel> characters)
        {
            this.isChecked = isChecked;
            this.characters = characters;
        }

        public void RunResetAuto()
        {
            while (isChecked.Value)
            {
                resetAutoAll();
                Thread.Sleep(1000 * 30);
            }
        }

        private void resetAutoAll()
        {
            var list = characters
             .Where(x => x.CheckAuto.Value)
             .ToList();

            foreach (var ch in list)
            {
                Task.Run(() =>
                {
                    var auto = new AutoFeatures(ch);
                    try
                    {
                        if (ch.HWnd.Value == IntPtr.Zero) return;
                        int i = 0;
                        while (ch.CheckAuto.Value && i < 10 && isChecked.Value)
                        {
                            i++;
                            if (!auto.FindImageByGroup("global", "khongtrongtrandau", false, true))
                            {
                                auto.ClickImageByGroup("global", "inbattleauto");
                                Thread.Sleep(2000);
                                continue;
                            }

                            auto.CloseAllDialog();
                            if (auto.FindImageByGroup("global", "outbattletatauto", percent: .9))
                                auto.ClickImageByGroup("global", "outbattletatauto");

                            Thread.Sleep(Constant.VeryTimeShort);
                            auto.ClickImageByGroup("global", "outbattletatauto_not", true, true);
                            Thread.Sleep(Constant.VeryTimeShort);
                            if (auto.FindImageByGroup("global", "outbattletatauto_not", true, true))
                                auto.ClickImageByGroup("global", "outbattletatauto_not");

                            if (auto.FindImageByGroup("global", "thugon_kynang"))
                                auto.ClickImageByGroup("global", "thugon_kynang");

                            auto.WriteStatus("Đã reset auto");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        auto.WriteStatus("Lỗi: " + ex.Message);
                    }
                });
            }
        }
    }
}
