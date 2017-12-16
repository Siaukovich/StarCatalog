using System.Collections.Generic;

namespace StarCatalog
{
    class StarsCurrent : List<Star>
    {
        public StarsCurrent() => this.AddRange(CollectionManager.GetCurrentStars());
    }
}
