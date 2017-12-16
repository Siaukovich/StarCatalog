using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StarCatalog
{
    public abstract class AsyncPageManager
    {
        public Page CurrentPage { get; set; }
        public Page NextPage { get; set; }
        public Page PreviousPage { get; set; }
        public Page FirstPage { get; set; }
        public Page LastPage { get; set; }

        public int CurrentPageNumber { get; set; }

        protected readonly IList Collection;

        protected AsyncPageManager(IList collection)
        {
            this.Collection = collection;
        }

        public abstract Task ShiftToNextPageAsync(TaskScheduler uiTaskScheduler);
        public abstract Task ShiftToPreviousPageAsync(TaskScheduler uiTaskScheduler);
        public abstract Task MoveToFirstAsync(TaskScheduler uiTaskScheduler);
        public abstract Task MoveToLastAsync(TaskScheduler uiTaskScheduler);

        protected void ShiftToNext()
        {
            this.PreviousPage = this.CurrentPage;
            this.CurrentPage = this.NextPage;
            this.NextPage = null;

            this.CurrentPageNumber++;
        }

        protected void ShiftToPrevious()
        {
            this.NextPage = this.CurrentPage;
            this.CurrentPage = this.PreviousPage;
            this.PreviousPage = null;

            this.CurrentPageNumber--;
        }

        protected void MoveToFirst()
        {
            this.CurrentPage = FirstPage;
            this.PreviousPage = null;
            this.NextPage = null;

            this.CurrentPageNumber = 1;
        }

        protected void MoveToLast()
        {
            this.CurrentPage = LastPage;
            this.PreviousPage = null;
            this.NextPage = null;

            this.CurrentPageNumber = this.Collection.Count;
        }
    }
}
