using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class TableSwitcherInventoryViewModel : INotifyPropertyChanged
{
    private readonly NotificationService _notificationService;
    private ObservableCollection<TechniqueDto> _items;
    private int _absentStatusIndex;
    private int _repairStatusIndex;
    private string _selectedOption;
    private string _searchText;
    private string _fromCost;
    private string _toCost;
    private int _totalItems;
    private bool _isLoading;
    private int _currentPage = 1;
    private const int PageSize = 20;

    public ICommand CleanCommand { get; }
    public ICommand ForwardCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand SearchCommand { get; }
    public ObservableCollection<TechniqueDto> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }
    public int AbsentStatusIndex
    {
        get => _absentStatusIndex;
        set => SetField(ref _absentStatusIndex, value);
    }
    public int RepairStatusIndex
    {
        get => _repairStatusIndex;
        set => SetField(ref _repairStatusIndex, value);
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
    public bool IsLoading
    {
        get => _isLoading;
        set => SetField(ref _isLoading, value);
    }
    public int TotalItems
    {
        get => _totalItems;
        set
        {
            if (_totalItems != value)
            {
                _totalItems = value;
                OnPropertyChanged(nameof(TotalItems));
                OnPropertyChanged(nameof(TotalPages));
                UpdateCommandStates();
            }
        }
    }
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdateCommandStates();
            }
        }
    }
    private int TotalPages
        => TotalItems > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;

    public TableSwitcherInventoryViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        _items = new ObservableCollection<TechniqueDto>();

        SearchCommand = new RelayCommand(
            async () =>
            {
                await ExecuteSearch();
            },
            () => !IsLoading
        );

        CleanCommand = new RelayCommand(
            () =>
            {
                DefaultSelectedOption();
                DefaultStatusIndices();
                DefaultSearchFields();
                DefautlSearchInformation();
                ClearItems();
            }
        );

        ForwardCommand = new RelayCommand(
            async () =>
            {
                CurrentPage++;
                await LoadTechniqueData();
            },
            () => CurrentPage < TotalPages && !IsLoading
        );

        BackCommand = new RelayCommand(
            async () =>
            {
                CurrentPage--;
                await LoadTechniqueData();
            },
            () => CurrentPage > 1 && !IsLoading
        );

        DefaultSelectedOption();
        DefaultStatusIndices();
    }

    private void ClearItems()
        => _items.Clear();

    private void DefaultSelectedOption() 
        => SelectedOption = "TechName";

    private void DefaultStatusIndices()
    {
        AbsentStatusIndex = 1;
        RepairStatusIndex = 1;
    }

    private void DefaultSearchFields()
    {
        FromCost = string.Empty;
        ToCost = string.Empty;
        SearchText = string.Empty;
    }

    private void DefautlSearchInformation()
    {
        TotalItems = 0;
        CurrentPage = 1;
    }

    private void UpdateCommandStates()
    {
        (ForwardCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (BackCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private async Task ExecuteSearch()
    {
        try
        {
            IsLoading = true;
            CurrentPage = 1;
            await LoadTechniqueData();
        }
        finally
        {
            IsLoading = false;
            UpdateCommandStates();
        }
    }

    private async Task LoadTechniqueData()
    {
        var pagination = new PaginationModel
        {
            Page = CurrentPage,
            PageSize = PageSize
        };

        var filter = GetFilter();

        if (filter == null)
        {
            ProcessDataNotFound();
            return;
        }

        var result = await TechniqueRequest.GetTechnique(pagination, filter);

        if (result?.Items.Any() == true)
        {
            ClearItems();
            foreach (var item in result.Items) _items.Add(item);
            TotalItems = result.TotalCount;
        }
        else
        {
            ProcessDataNotFound();
        }
    }

    private void ProcessDataNotFound()
    {
        ClearItems();
        DefautlSearchInformation();
        TriggerANotification(Strings.DataNotFound);
    }

    private TechniqueFiltersDto GetFilter()
    {
        try
        {
            var filter = new TechniqueFiltersDto();

            switch (SelectedOption)
            {
                case "TechName":
                    filter.Name = SearchText;
                    break;
                case "TechType":
                    filter.TypeTechnique = SearchText;
                    break;
                case "TechNumber":
                    filter.Number = SearchText;
                    break;
                case "TechCost":
                    filter = ExtractCostFilter();
                    break;
                case "RespEmployee":
                    filter.Member = SearchText;
                    break;
                case "RespNumber":
                    filter.Office = int.Parse(SearchText);
                    break;
                case "CompNumber":
                    filter.Computer = int.Parse(SearchText);
                    break;
                case "DateAcquisition":
                    filter.DateOfPurchase = DateTime.Parse(SearchText);
                    break;
                case "DateProduction":
                    filter.DateOfManufacture = DateTime.Parse(SearchText);
                    break;
                case "DateOfUse":
                    filter.DateOfUse = DateTime.Parse(SearchText);
                    break;
                case "SupName":
                    filter.Supplier = SearchText;
                    break;
                default:
                    throw new Exception("The filter was not found");
            }

            if (AbsentStatusIndex != 1)
                filter.IsFastened = AbsentStatusIndex == 0
                    ? false : true;
            if (RepairStatusIndex != 1)
                filter.IsUnderRepair = RepairStatusIndex == 0
                    ? true : false;

            return filter;
        }
        catch
        {
            return null;
        }
    }

    private TechniqueFiltersDto ExtractCostFilter()
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

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
