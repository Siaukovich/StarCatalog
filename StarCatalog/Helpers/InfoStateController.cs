using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Helpers
{
    public static class InfoStateController
    {
        public static bool InfoWasChanged { get; private set; } = false;
        public static void InfoChanged() => InfoWasChanged = true;
        public static void Reset() => InfoWasChanged = false;
    }
}
