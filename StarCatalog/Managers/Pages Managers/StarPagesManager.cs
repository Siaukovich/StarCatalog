using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarCatalog
{
    class StarPagesManager : AsyncPageManager
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

        public override async Task ShiftToNextPageAsync(TaskScheduler uiTaskScheduler)
        {
            this.ShiftToNext(); // NextPage is null after that.

            if (this.CurrentPageNumber == this.Collection.Count)
                return;

            this.NextPage = await Task.Factory.StartNew(
                () => new StarViewPage(this.CurrentPageNumber + 1),
                CancellationToken.None, TaskCreationOptions.None, uiTaskScheduler);
        }

        public override async Task ShiftToPreviousPageAsync(TaskScheduler uiTaskScheduler)
        {
            this.ShiftToPrevious(); // PreviousPage is null after that.

            if (this.CurrentPageNumber == 1)
                return;

            this.NextPage = await Task.Factory.StartNew(
                () => new ConstellationViewPage(this.CurrentPageNumber + 1),
                CancellationToken.None, TaskCreationOptions.None, uiTaskScheduler);

            this.PreviousPage = await Task.Factory.StartNew(
                () => new StarViewPage(this.CurrentPageNumber - 1),
                CancellationToken.None, TaskCreationOptions.None, uiTaskScheduler);
        }

        public override async Task MoveToFirstAsync(TaskScheduler uiTaskScheduler)
        {
            this.MoveToFirst(); // PreviousPage and NextPage is null after that.

            if (this.Collection.Count == 1)
                return;

            if (this.Collection.Count == 1)
                return;

            this.NextPage = await Task.Factory.StartNew(
                () => new StarViewPage(this.CurrentPageNumber + 1),
                CancellationToken.None, TaskCreationOptions.None, uiTaskScheduler);
        }

        public override async Task MoveToLastAsync(TaskScheduler uiTaskScheduler)
        {
            MoveToLast();

            if (this.Collection.Count == 1) // PreviousPage and NextPage is null after that.
                return;

            this.PreviousPage = await Task.Factory.StartNew(
                () => new StarViewPage(this.CurrentPageNumber - 1),
                CancellationToken.None, TaskCreationOptions.None, uiTaskScheduler);
        }

         protected async void LoadNextPageAsync(TaskScheduler taskScheduler)
        {
            this.NextPage = await Task.Factory.StartNew(
                () => new StarViewPage(this.CurrentPageNumber + 1),
                CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }

        protected async void LoadPreviousPageAsync(TaskScheduler taskScheduler)
        {
            this.PreviousPage = await Task.Factory.StartNew(
                () => new StarViewPage(this.CurrentPageNumber - 1),
                CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }
    }
}
