using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StarCatalog
{
    abstract class AsyncPageManager<T> : IAsyncPageManager 
    {
        public Page CurrentPage  { get; set; }
        public Page NextPage     { get; set; }
        public Page PreviousPage { get; set; }
        public Page FirstPage    { get; set; }
        public Page LastPage     { get; set; }

        public int CurrentPageNumber { get; set; }

        protected readonly List<T> Collection;

        protected AsyncPageManager(List<T> collection)
        {
            this.Collection = collection;
        }

        public abstract Task ShiftToNextPageAsync();
        public abstract Task ShiftToPreviousAsync();
        public abstract Task MoveToFirstAsync();
        public abstract Task MoveToLastAsync();

        protected void ShiftToNext()
        {
            Thread.Sleep(500);

            if (this.CurrentPageNumber == this.Collection.Count)
                return;

            this.PreviousPage = this.CurrentPage;
            this.CurrentPage = this.NextPage;
            this.NextPage = null;

            this.CurrentPageNumber++;
        }

        protected void ShiftToPrevious()
        {
            Thread.Sleep(500);

            if (this.CurrentPageNumber == 1)
                return;

            this.NextPage = this.CurrentPage;
            this.CurrentPage = this.PreviousPage;
            this.PreviousPage = null;

            this.CurrentPageNumber--;
        }

        protected void MoveToFirst()
        {
            Thread.Sleep(500);

            if (CurrentPageNumber == 1)
                return;

            this.CurrentPage = FirstPage;
            this.PreviousPage = null;
            this.NextPage = null;

            this.CurrentPageNumber = 1;
        }

        protected void MoveToLast()
        {
            Thread.Sleep(500);
            
            if (CurrentPageNumber == this.Collection.Count)
                return;

            this.CurrentPage = LastPage;
            this.PreviousPage = null;
            this.NextPage = null;

            this.CurrentPageNumber = this.Collection.Count;
        }
    }
}
