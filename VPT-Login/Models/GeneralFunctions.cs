using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VPT_Login.Models
{
    public class GeneralFunctions
    {
        private string mWindowName;
        private ReactiveProperty<DataModel> mCharacter = new ReactiveProperty<DataModel>();

        public GeneralFunctions( DataModel character, string mWindowName)
        {
            mCharacter.Value = character;
            this.mWindowName = mWindowName;
        }



    }
}
