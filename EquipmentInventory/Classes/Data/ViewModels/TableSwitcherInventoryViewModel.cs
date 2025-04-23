using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class TableSwitcherInventoryViewModel : TableSwitcherBaseViewModel<TechniqueDto>
{
    private ObservableCollection<string> _queryItems;
    private string _selectedQuery;
    private int _absentStatusIndex;
    private int _repairStatusIndex;
    private string _selectedOption;
    private string _fromCost;
    private string _toCost;

    public ICommand DeleteItemCommand { get; }
    public ICommand EditItemCommand { get; }

    public ObservableCollection<string> QueryItems
    {
        get => _queryItems;
        set => SetField(ref _queryItems, value);
    }

    public string SelectedQuery
    {
        get => _selectedQuery;
        set => SetField(ref _selectedQuery, value);
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

    public TableSwitcherInventoryViewModel(NotificationService notificationService)
        : base(notificationService)
    {
        _queryItems = new ObservableCollection<string>
        {
            Strings.OutdatedTechnology
        };

        DeleteItemCommand = new RelayCommand<TechniqueDto>(DeleteMessage);
        EditItemCommand = new RelayCommand<TechniqueDto>(EditMessage);

        DefaultSelectedOption();
        DefaultStatusIndices();
    }

    private void DeleteMessage(TechniqueDto item)
    {
        MessageBox.Show($"Удалить: {item.Id}");

       /* if (item != null && Items.Contains(item))
        {
            Items.Remove(item);
        }*/
    }

    private void EditMessage(TechniqueDto item)
    {
        MessageBox.Show($"Редактировать: {item.Id}");
    }

    protected override void ResetSearchParameters()
    {
        base.ResetSearchParameters();

        DefaultSelectedOption();
        DefaultStatusIndices();
        DefaultSearchFields();
        DefautlSearchInformation();
        DefaultSelectedItems();
        ClearItems();
    }

    protected override async Task LoadData()
    {
        var pagination = new PaginationModel { Page = CurrentPage, PageSize = PageSize };
        var result = new PaginatedResult<TechniqueDto>();

        object filter = SelectedQuery != null
            ? Settings.Default.YearOfObsolescence
            : GetFilter();

        if (filter == null)
        {
            ProcessDataNotFound();
            return;
        }

        result = SelectedQuery != null
            ? await TechniqueRequest.GetTechniqueOutdated(pagination, (int)filter)
            : await TechniqueRequest.GetTechniqueWithFiltration(pagination, (TechniqueFiltersDto)filter);

        ProcessResult(result);
    }

    private void DefaultSelectedOption() 
        => SelectedOption = "TechName";

    private void DefaultStatusIndices()
    {
        AbsentStatusIndex = 1;
        RepairStatusIndex = 1;
    }

    private void DefaultSelectedItems()
        => SelectedQuery = null;

    private void DefaultSearchFields()
    {
        FromCost = string.Empty;
        ToCost = string.Empty;
        SearchText = string.Empty;
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
}
