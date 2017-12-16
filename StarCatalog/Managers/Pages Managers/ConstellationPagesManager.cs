using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarCatalog
{
    class ConstellationPagesManager : AsyncPageManager
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

        public override Task ShiftToNextPageAsync(TaskScheduler uiTaskScheduler)
        {
            this.ShiftToNext(); // NextPage is null after that.

            if (this.CurrentPageNumber == this.Collection.Count)
                return Task.CompletedTask;
            
            return Task.Run(async () => this.NextPage = await this.LoadNextPageAsync(uiTaskScheduler));
        }

        public override Task ShiftToPreviousPageAsync(TaskScheduler uiTaskScheduler)
        {
            this.ShiftToPrevious(); // PreviousPage is null after that.

            if (this.CurrentPageNumber == 1)
                return Task.CompletedTask;

            return Task.Run(async () => this.PreviousPage = await this.LoadNextPageAsync(uiTaskScheduler));
        }

        public override Task MoveToFirstAsync(TaskScheduler uiTaskScheduler)
        {
            this.MoveToFirst(); // PreviousPage and NextPage is null after that.

            if (this.Collection.Count == 1)
                return Task.CompletedTask;

            return Task.Run(async () => this.NextPage = await this.LoadNextPageAsync(uiTaskScheduler));
        }

        public override Task MoveToLastAsync(TaskScheduler uiTaskScheduler)
        {
            MoveToLast(); // PreviousPage and NextPage is null after that.

            if (this.Collection.Count == 1) 
                return Task.CompletedTask;

            return Task.Run(async () => this.PreviousPage = await this.LoadNextPageAsync(uiTaskScheduler));
        }

        protected Task<ConstellationViewPage> LoadNextPageAsync(TaskScheduler taskScheduler)
        {
            return Task.Factory.StartNew(
                () => new ConstellationViewPage(this.CurrentPageNumber + 1),
                CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }

        protected Task<ConstellationViewPage> LoadPreviousPage(TaskScheduler taskScheduler)
        {
           return Task.Factory.StartNew(
                () =>new ConstellationViewPage(this.CurrentPageNumber - 1),
                CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }
    }
}