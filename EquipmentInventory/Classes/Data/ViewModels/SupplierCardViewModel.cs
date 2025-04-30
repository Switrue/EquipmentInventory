using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class SupplierCardViewModel : BaseCardViewModel<BaseUpdateDto>
{
    public SupplierCardViewModel(IDictionariesViewModel viewModel, object selectedItem) 
        : base(viewModel, selectedItem)
    {
    }

    protected async override Task SaveData()
    {
        if (!Item.IsValid()) return;

        var response = Id.HasValue
            ? await SuppliersRequest.Update(Id.Value, Item)
            : await SuppliersRequest.Add(Item);

        await ProcessResult(response);
    }
}
