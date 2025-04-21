using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class TableSwitcherInventoryViewModel : INotifyPropertyChanged
{
    private readonly NotificationService _notificationService;
    private ObservableCollection<TechniqueDto> _items;
    private string _selectedOption;
    private string _searchText;
    private string _fromCost;
    private string _toCost;

    public ObservableCollection<TechniqueDto> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }
    public string SelectedOption
    {
        get => _selectedOption;
        set => SetField(ref _selectedOption, value);
    }
    public string SearchText
    {
        get => _searchText;
        set => SetField(ref _searchText, value);
    }
    public string FromCost
    {
        get => _fromCost;
        set => SetField(ref _fromCost, value);
    }
    public string ToCost
    {
        get => _toCost;
        set => SetField(ref _toCost, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public TableSwitcherInventoryViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        _items = new ObservableCollection<TechniqueDto>();

        DefaultSelectedOption();
    }

    public void DefaultSelectedOption() 
        => SelectedOption = "TechName";

    public ICommand Search => new RelayCommand<Button>(async (sender) =>
    {
        await UserAccountService.ExecuteTask(sender, LoadTechniqueData);
    });

    private async Task LoadTechniqueData()
    {
        var pagination = new PaginationModel { Page = 1, PageSize = 10 };
        var filter = GetFilter();

        if (filter == null)
        {
            DataNotFound();
            return;
        }

        var result = await TechniqueRequest.GetTechnique(pagination, filter);

        if (result?.Items.Any() == true)
        {
            ClearItems();

            foreach (var item in result.Items)
            {
                _items.Add(item);
            }
            TriggerANotification($"Всего записей: {result.TotalCount}");
        }
        else
        {
            DataNotFound();
        }
    }

    private void DataNotFound()
    {
        ClearItems();
        TriggerANotification("Данные не найдены");
    }

    private void ClearItems()
        => _items.Clear();

    private TechniqueFiltersDto GetFilter()
    {
        try
        {
            return SelectedOption switch
            {
                "TechName" => new TechniqueFiltersDto { Name = SearchText },
                "TechType" => new TechniqueFiltersDto { TypeTechnique = SearchText },
                "TechNumber" => new TechniqueFiltersDto { Number = SearchText },
                "TechCost" => GetCostFilter(),
                "RespEmployee" => new TechniqueFiltersDto { Member = SearchText },
                "RespNumber" => new TechniqueFiltersDto { Office = int.Parse(SearchText) },
                "CompNumber" => new TechniqueFiltersDto { Computer = int.Parse(SearchText) },
                "DateAcquisition" => new TechniqueFiltersDto { DateOfPurchase = DateTime.Parse(SearchText) },
                "DateProduction" => new TechniqueFiltersDto { DateOfManufacture = DateTime.Parse(SearchText) },
                "DateOfUse" => new TechniqueFiltersDto { DateOfUse = DateTime.Parse(SearchText) },
                "SupName" => new TechniqueFiltersDto { Supplier = SearchText },
                _ => new TechniqueFiltersDto()
            };
        }
        catch
        {
            return null;
        }
    }

    private TechniqueFiltersDto GetCostFilter()
    {
        TechniqueFiltersDto techCost = null;

        var hasFrom = !string.IsNullOrWhiteSpace(FromCost);
        var hasTo = !string.IsNullOrWhiteSpace(ToCost);

        if (hasFrom || hasTo)
        {
            techCost = new TechniqueFiltersDto();

            if (hasFrom)
            {
                techCost.FromCost = float.Parse(FromCost, CultureInfo.InvariantCulture);
            }

            if (hasTo)
            {
                techCost.ToCost = float.Parse(ToCost, CultureInfo.InvariantCulture);
            }
        }

        return techCost;
    }

    private void TriggerANotification(string message)
        => _notificationService.Show(message);

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
