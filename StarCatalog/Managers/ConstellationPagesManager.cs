using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarCatalog
{
    static class ConstellationPagesManager
    {
        public static ConstellationViewPage CurrentPage { get; private set; }
        public static ConstellationViewPage NextPage { get; private set; }
        public static ConstellationViewPage PreviousPage { get; private set; }

        public static int CurrentPageNumber;

        // Change return type to Task and add await where called.
        public static async Task UpdateAsync(int currentPageIndex)
        {
            //await Task.Delay(2000);

            int totalSize = ConstellationCollectionManager.Constellations.Count;

            if (totalSize == 0)
                throw new InvalidOperationException("Constellation collection is empty!");

            CurrentPageNumber = currentPageIndex;

            CurrentPage = new ConstellationViewPage(currentPageIndex);
            PreviousPage = null;
            NextPage = null;

            if (totalSize == 1)
                return;

            int currentIndexInCollection = currentPageIndex - 1;

            // If current element isn't the last one.
            if (currentIndexInCollection != totalSize - 1)
            {
                NextPage = new ConstellationViewPage(currentPageIndex + 1);
            }

            // If current element isn't the first one.
            if (currentIndexInCollection != 0)
            {
                PreviousPage = new ConstellationViewPage(currentPageIndex - 1);
            }
        }

        // Change return type to Task and add await where called.
        public static async Task UpdateToNextAsync(int currentPageIndex)
        {
            //await Task.Delay(2000);

            CurrentPageNumber = currentPageIndex;

            PreviousPage = CurrentPage;
            CurrentPage = NextPage;
            NextPage = null;

            int currentIndexInCollection = currentPageIndex - 1;
            int totalSize = ConstellationCollectionManager.Constellations.Count;

            // If current element isn't the last one.
            if (currentIndexInCollection != totalSize - 1)
            {
                NextPage = new ConstellationViewPage(currentPageIndex + 1);
            }
        }

        // Change return type to Task and add await where called.
        public static async Task UpdateToPrevAsync(int currentPageIndex)
        {
            //await Task.Delay(2000);

            CurrentPageNumber = currentPageIndex;

            NextPage = CurrentPage;
            CurrentPage = PreviousPage;
            PreviousPage = null;

            int currentIndexInCollection = currentPageIndex - 1;

            // If current element isn't the first one.
            if (currentIndexInCollection != 0)
            {
                PreviousPage = new ConstellationViewPage(currentPageIndex - 1);
            }
        }
    }
}