using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class MemberCardViewModel : BaseCardViewModel<MemberUpdateDto>
{
    private ObservableCollection<BaseDto> _positionItems;

    public ObservableCollection<BaseDto> PositionItems
    {
        get => _positionItems;
        set => SetField(ref _positionItems, value);
    }

    private MemberCardViewModel(IDictionariesViewModel viewModel, object selectedItem)
        : base(viewModel, selectedItem)
    {
    }

    public static async Task<MemberCardViewModel> CreateAsync(IDictionariesViewModel dictionariesView, object selectedItem)
    {
        var viewModel = new MemberCardViewModel(dictionariesView, selectedItem);
        await viewModel.InitializeAsync();
        await viewModel.InitializeSelectedPosition(selectedItem);
        return viewModel;
    }

    private Task InitializeSelectedPosition (object selectedItem)
    {
        if (selectedItem == null) return Task.CompletedTask;

        try
        {
            var selectedPosition = DataService.GetProperty(selectedItem, "Должность")?.ToString();
            var position = PositionItems?
                .FirstOrDefault(item => item.Name == selectedPosition);
            Item.IdPosition = position.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return Task.CompletedTask;
    }

    private async Task InitializeAsync()
        => PositionItems = await PositionsRequest.GetPositionsItemsAsync();

    protected async override Task SaveData()
    {
        if (!Item.IsValid()) return;

        var response = Id.HasValue
            ? await MembersRequest.Update(Id.Value, Item)
            : await MembersRequest.Add(Item);

        await ProcessResult(response);
    }
}
