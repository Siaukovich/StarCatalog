using System.Threading.Tasks;
using System.Windows.Controls;

namespace StarCatalog
{
    public interface IAsyncPageManager
    {
        Page CurrentPage  { get; set; }
        Page NextPage     { get; set; }
        Page PreviousPage { get; set; }
        Page FirstPage    { get; set; }
        Page LastPage     { get; set; }

        int CurrentPageNumber { get; set; }

        Task ShiftToNextPageAsync();
        Task ShiftToPreviousAsync();
        Task MoveToFirstAsync();
        Task MoveToLastAsync();
    }
}
