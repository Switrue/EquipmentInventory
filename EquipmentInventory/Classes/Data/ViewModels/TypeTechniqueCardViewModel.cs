using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class TypeTechniqueCardViewModel : BaseCardViewModel<BaseUpdateDto>
{
    public TypeTechniqueCardViewModel(IDictionariesViewModel viewModel, object selectedItem)
        : base(viewModel, selectedItem)
    {
    }

    protected override async Task SaveData()
    {
        if (!Item.IsValid()) return;

        var response = Id.HasValue 
            ? await TypeTechniqueRequest.Update(Id.Value, Item) 
            : await TypeTechniqueRequest.Add(Item);

        await ProcessResult(response);
    }
}
