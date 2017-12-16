using System.Threading.Tasks;
using System.Windows.Controls;

namespace StarCatalog
{
    public interface IPagesManager
    {
        Page CurrentPage  { get; }
        Page NextPage     { get; }
        Page PreviousPage { get; }
        Page FirstPage    { get; }
        Page LastPage     { get; }

        int CurrentPageNumber { get; set; }

        Task ShiftToNextPageAsync(TaskScheduler uiTaskScheduler);
        Task ShiftToPreviousPageAsync(TaskScheduler uiTaskScheduler);
        Task MoveToFirstAsync(TaskScheduler uiTaskScheduler);
        Task MoveToLastAsync(TaskScheduler uiTaskScheduler);
    }
}