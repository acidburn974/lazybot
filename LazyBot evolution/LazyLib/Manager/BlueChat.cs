using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Plugins.ExtraLazy
{
    public class BlueChat
    {
        public static String readChat()
        {
            return Memory.ReadUtf8StringRelative(Convert.ToUInt32((uint)Pointers.BlueChat.BlueBase), 128);
        }
    }
}
