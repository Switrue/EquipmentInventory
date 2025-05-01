using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class OfficeCardViewModel : BaseCardViewModel<OfficeUpdateDto>
{
    public OfficeCardViewModel(IDictionariesViewModel viewModel, object selectedItem)
        : base(viewModel, selectedItem)
    {
    }

    protected async override Task SaveData()
    {
        if (!Item.IsValid()) return;

        var response = Id.HasValue
            ? await OfficesRequest.Update(Id.Value, Item)
            : await OfficesRequest.Add(Item);

        await ProcessResult(response);
    }
}
