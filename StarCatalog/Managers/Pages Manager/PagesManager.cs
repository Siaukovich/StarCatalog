using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StarCatalog
{
    public class PagesManager<T> : IPageManager 
                         where T : Page, IViewPage, new()
    {
        private T _currentPage;
        private T _nextPage;
        private T _previousPage;

        private readonly T _firstPage;
        private readonly T _lastPage;

        public Page CurrentPage  => _currentPage;
        public Page NextPage     => _nextPage;
        public Page PreviousPage => _previousPage;
        public Page FirstPage    => _firstPage;
        public Page LastPage     => _lastPage;

        public int CurrentPageNumber { get; set; }

        private readonly IList _collection;

        public PagesManager(IList collection)
        {
            this._collection = collection;

            this.CurrentPageNumber = 1; // First page.

            this._currentPage = new T();
            this._currentPage.SetDataContext(CurrentPageNumber);

            this._firstPage = this._currentPage;

            this._lastPage = new T();
            this._lastPage.SetDataContext(collection.Count);

            if (collection.Count == 0)
                return;

            this._nextPage = new T();
            this._nextPage.SetDataContext(CurrentPageNumber + 1);
        }

        public Task ShiftToNextPageAsync(TaskScheduler uiTaskScheduler)
        {
            this.ShiftToNext(); // NextPage is null after that.

            if (this.CurrentPageNumber == this._collection.Count)
                return Task.CompletedTask;

            return Task.Run(async () => this._nextPage = await LoadNextPageAsync(uiTaskScheduler));
        }

        public Task ShiftToPreviousPageAsync(TaskScheduler uiTaskScheduler)
        {
            this.ShiftToPrevious(); // PreviousPage is null after that.

            if (this.CurrentPageNumber == 1)
                return Task.CompletedTask;

            return Task.Run(async () => this._previousPage = await LoadPreviousPageAsync(uiTaskScheduler));
        }

        public Task MoveToFirstAsync(TaskScheduler uiTaskScheduler)
        {
            this.MoveToFirst(); // PreviousPage and NextPage is null after that.

            if (this._collection.Count == 1)
                return Task.CompletedTask;

            return Task.Run(async () => this._nextPage = await LoadNextPageAsync(uiTaskScheduler));
        }

        public Task MoveToLastAsync(TaskScheduler uiTaskScheduler)
        {
            MoveToLast(); // PreviousPage and NextPage is null after that.

            if (this._collection.Count == 1)
                return Task.CompletedTask;

            return Task.Run(async () => this._previousPage = await LoadPreviousPageAsync(uiTaskScheduler));
        }

        private void ShiftToNext()
        {
            this._previousPage = this._currentPage;
            this._currentPage = this._nextPage;
            this._nextPage = null;

            this.CurrentPageNumber++;
        }

        private void ShiftToPrevious()
        {
            this._nextPage = this._currentPage;
            this._currentPage = this._previousPage;
            this._previousPage = null;

            this.CurrentPageNumber--;
        }

        private void MoveToFirst()
        {
            this._currentPage = _firstPage;
            this._previousPage = null;
            this._nextPage = null;

            this.CurrentPageNumber = 1;
        }

        private void MoveToLast()
        {
            this._currentPage = _lastPage;
            this._previousPage = null;
            this._nextPage = null;

            this.CurrentPageNumber = this._collection.Count;
        }

        private Task<T> LoadNextPageAsync(TaskScheduler taskScheduler)
        {
            return Task.Factory.StartNew(
                () => 
                {
                    T nextPage = new T();
                    nextPage.SetDataContext(CurrentPageNumber + 1);
                    return nextPage;
                },
                CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }

        private Task<T> LoadPreviousPageAsync(TaskScheduler taskScheduler)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    T prevPage = new T();
                    prevPage.SetDataContext(CurrentPageNumber - 1);
                    return prevPage;
                },
                CancellationToken.None, TaskCreationOptions.None, taskScheduler);
        }
    }
}
