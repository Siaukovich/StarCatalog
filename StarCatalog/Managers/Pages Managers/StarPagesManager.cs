using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarCatalog
{
    class StarPagesManager : AsyncPageManager<Star>
    {
        public StarPagesManager(List<Star> collection) : base(collection)
        {
            this.CurrentPageNumber = 1;

            this.CurrentPage = new StarViewPage(CurrentPageNumber);
            this.FirstPage = new StarViewPage(CurrentPageNumber);
            this.LastPage = new StarViewPage(collection.Count);

            if (collection.Count == 1)
                return;

            this.NextPage = new StarViewPage(CurrentPageNumber + 1);
        }

        public override Task ShiftToNextPageAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.ShiftToNext(); // NextPage is null after that.

                if (this.CurrentPageNumber == this.Collection.Count)
                    return;

                this.NextPage = new StarViewPage(this.CurrentPageNumber + 1);
               
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override Task ShiftToPreviousAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.ShiftToPrevious(); // PreviousPage is null after that.

                if (this.CurrentPageNumber == 1)
                    return;

                this.PreviousPage = new StarViewPage(this.CurrentPageNumber - 1);

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

        }

        public override Task MoveToFirstAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.MoveToFirst(); // PreviousPage and NextPage is null after that.

                if (this.Collection.Count == 1)
                    return;

                this.NextPage = new StarViewPage(this.CurrentPageNumber + 1);

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override Task MoveToLastAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.MoveToLast(); // PreviousPage and NextPage is null after that.

                if (this.Collection.Count == 1)
                    return;

                this.PreviousPage = new StarViewPage(this.CurrentPageNumber - 1);

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            
        }
    }
}
