using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarCatalog
{
    class ConstellationPagesManager : AsyncPageManager<Constellation>
    {
        public ConstellationPagesManager(List<Constellation> collection) : base(collection)
        {
            this.CurrentPageNumber = 1;

            this.CurrentPage = new ConstellationViewPage(CurrentPageNumber);
            this.FirstPage = new ConstellationViewPage(1);
            this.LastPage = new ConstellationViewPage(collection.Count);

            if (collection.Count == 1)
                return;
            
            this.NextPage = new ConstellationViewPage(CurrentPageNumber + 1);
        }

        public override Task ShiftToNextPageAsync()
        {
            return Task.Factory.StartNew(() =>
            {

                this.ShiftToNext(); // NextPage is null after that.

                if (this.CurrentPageNumber == this.Collection.Count)
                    return;

                this.NextPage = new ConstellationViewPage(this.CurrentPageNumber + 1);

                ConstellationCollectionManager.Current = this.CurrentPageNumber + 1;

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override Task ShiftToPreviousAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.ShiftToPrevious(); // PreviousPage is null after that.

                if (this.CurrentPageNumber == 1)
                    return;

                this.PreviousPage = new ConstellationViewPage(this.CurrentPageNumber - 1);

                ConstellationCollectionManager.Current = this.CurrentPageNumber - 1;

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override Task MoveToFirstAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.MoveToFirst(); // PreviousPage and NextPage is null after that.

                if (this.Collection.Count == 1)
                    return;

                this.NextPage = new ConstellationViewPage(this.CurrentPageNumber + 1);

                ConstellationCollectionManager.Current = 0;

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override Task MoveToLastAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.MoveToLast(); // PreviousPage and NextPage is null after that.

                if (this.Collection.Count == 1)
                    return;

                this.PreviousPage = new ConstellationViewPage(this.CurrentPageNumber - 1);

                ConstellationCollectionManager.Current = this.CurrentPageNumber - 1;

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}