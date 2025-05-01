using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class ComputerCardViewModel : BaseCardViewModel<ComputerUpdateDto>
{
    public ComputerCardViewModel(IDictionariesViewModel viewModel, object selectedItem)
        : base(viewModel, selectedItem)
    {
    }

    protected async override Task SaveData()
    {
        if (!Item.IsValid()) return;

        var response = Id.HasValue
            ? await ComputersRequest.Update(Id.Value, Item)
            : await ComputersRequest.Add(Item);

        await ProcessResult(response);
    }
}
