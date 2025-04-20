using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    private float _searchFloat;

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
    public float SearchFloat
    {
        get => _searchFloat;
        set => SetField(ref _searchFloat, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public TableSwitcherInventoryViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        _items = new ObservableCollection<TechniqueDto>();
    }

    public ICommand Search => new RelayCommand<Button>(async (sender) =>
    {
        await UserAccountService.ExecuteTask(sender, LoadTechniqueData);
    });

    private async Task LoadTechniqueData()
    {
        var pagination = new PaginationModel { Page = 1, PageSize = 10 };
        var filter = GetFilter();

        var result = await TechniqueRequest.GetTechnique(pagination, filter);

        if (result?.Items.Any() == true)
        {
            _items.Clear();

            foreach (var item in result.Items)
            {
                _items.Add(item);
            }
            TriggerANotification($"Всего записей: {result.TotalCount}");
        }
        else
        {
            TriggerANotification("Данные не найдены");
        }
    }

    private TechniqueFiltersDto GetFilter()
    {w
        return SelectedOption switch
        {
            "TechName" => new TechniqueFiltersDto { Name = SearchText },
            "TechType" => new TechniqueFiltersDto { TypeTechnique = SearchText },
            "TechNumber" => new TechniqueFiltersDto { Number = SearchText },
            "TechCost" => new TechniqueFiltersDto { FromCost = SearchFloat },

            "DateProduction" => new TechniqueFiltersDto { DateOfManufacture = DateTime.Parse(SearchText) },
            _ => new TechniqueFiltersDto { Name = SearchText }
        };
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
