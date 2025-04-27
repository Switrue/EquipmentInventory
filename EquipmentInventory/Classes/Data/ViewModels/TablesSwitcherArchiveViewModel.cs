using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class TablesSwitcherArchiveViewModel : TableSwitcherBaseViewModel<ArchiveDto>
{
    public TablesSwitcherArchiveViewModel(NotificationService notificationService)
        : base(notificationService)
    {
    }

    protected override void ResetSearchParameters()
    {
        base.ResetSearchParameters();

        DefaultSearchFields();
        DefautlSearchInformation();
        ClearItems();
    }

    protected override async Task LoadData()
    {
        var pagination = new PaginationModel { Page = CurrentPage, PageSize = Settings.Default.PageSize };
        var filter = SearchText;

        var result = await ArchiveRequest.GetArchive(pagination, filter);
        ProcessResult(result);
    }

    private void DefaultSearchFields()
        => SearchText = string.Empty;
}
