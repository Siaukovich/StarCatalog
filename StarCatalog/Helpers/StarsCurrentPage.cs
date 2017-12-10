using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog
{
    class StarsCurrentPage : List<Star>
    {
        public StarsCurrentPage()
        {
            int currentConstellationIndex = ConstellationPagesManager.CurrentPageNumber - 1;
            this.AddRange(ConstellationCollectionManager.Constellations[currentConstellationIndex].Stars);
        }
    }
}
