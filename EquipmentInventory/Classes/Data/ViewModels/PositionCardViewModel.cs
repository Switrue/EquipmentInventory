using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class PositionCardViewModel : BaseCardViewModel<BaseUpdateDto>
{
    public PositionCardViewModel(IDictionariesViewModel dictionariesView, object selectedItem)
        : base(dictionariesView, selectedItem)
    {
    }

    protected async override Task SaveData()
    {
        if (!Item.IsValid()) return;

        var response = Id.HasValue
            ? await PositionsRequest.Update(Id.Value, Item)
            : await PositionsRequest.Add(Item);

        await ProcessResult(response);
    }
}
