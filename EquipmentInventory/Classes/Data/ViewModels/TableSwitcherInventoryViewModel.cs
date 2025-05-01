using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class TableSwitcherInventoryViewModel : TableSwitcherBaseViewModel<TechniqueDto>
{
    #region Fields
    private TechniqueUpdateDto _techniqueUpdate;
    private ObservableCollection<string> _queryItems;
    private ObservableCollection<BaseDto> _typeTechniqueItems;
    private ObservableCollection<BaseDto> _suppliersItems;
    private ObservableCollection<MemberDto> _membersItems;
    private ObservableCollection<OfficeDto> _officesItems;
    private ObservableCollection<ComputerDto> _computersItems;
    private string _selectedQuery;
    private int _absentStatusIndex;
    private int _repairStatusIndex;
    private string _selectedOption;
    private string _fromCost;
    private string _toCost;
    private long? id;
    #endregion

    #region Properties
    public ICommand DeleteItemCommand { get; }
    public ICommand EditItemCommand { get; }
    public ICommand AddItemCommand { get; }
    public ICommand SaveDataCommand { get; }

    public TechniqueUpdateDto TechniqueUpdate
    {
        get => _techniqueUpdate;
        set => SetField(ref _techniqueUpdate, value);
    }

    public ObservableCollection<string> QueryItems
    {
        get => _queryItems;
        set => SetField(ref _queryItems, value);
    }

    public ObservableCollection<BaseDto> TypeTechniqueItems
    {
        get => _typeTechniqueItems;
        set => SetField(ref _typeTechniqueItems, value);
    }

    public ObservableCollection<BaseDto> SuppliersItems
    {
        get => _suppliersItems;
        set => SetField(ref _suppliersItems, value);
    }

    public ObservableCollection<MemberDto> MembersItems
    {
        get => _membersItems;
        set => SetField(ref _membersItems, value);
    }

    public ObservableCollection<OfficeDto> OfficesItems
    {
        get => _officesItems;
        set => SetField(ref _officesItems, value);
    }

    public ObservableCollection<ComputerDto> ComputersItems
    {
        get => _computersItems;
        set => SetField(ref _computersItems, value);
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
    #endregion

    public TableSwitcherInventoryViewModel(NotificationService notificationService)
        : base(notificationService)
    {
        _techniqueUpdate = new TechniqueUpdateDto();
        _queryItems = new ObservableCollection<string>
        {
            Strings.OutdatedTechnology
        };

        DeleteItemCommand = new RelayCommand<TechniqueDto>(async (item) => await DeleteItem(item));
        EditItemCommand = new RelayCommand<TechniqueDto>(EditItem);
        AddItemCommand = new RelayCommand(AddItem);
        SaveDataCommand = new RelayCommand<object>(async (sender) => await UserAccountService.ExecuteTask((Button)sender, SaveData));

        DefaultSelectedOption();
        DefaultStatusIndices();
    }

    public static async Task<TableSwitcherInventoryViewModel> CreateAsync(NotificationService notificationService)
    {
        var viewModel = new TableSwitcherInventoryViewModel(notificationService);
        await viewModel.InitializeAsync();
        return viewModel;
    }

    public async Task InitializeAsync()
    {
        TypeTechniqueItems = await TypeTechniqueRequest.GetTypeTechniqueItemsAsync();
        SuppliersItems = await SuppliersRequest.GetSuppliersItemsAsync();
        MembersItems = await MembersRequest.GetMembersItemsAsync();
        OfficesItems = await OfficesRequest.GetOfficesItemsAsync();
        ComputersItems = await ComputersRequest.GetComputersItemsAsync();
    }

    private async Task SaveData()
    {
        var isAdd = id is null;

        if (!TechniqueUpdate.IsValid()) return;

        var response = new BaseResponse();

        response = isAdd
                ? await TechniqueRequest.Add(TechniqueUpdate)
                : await TechniqueRequest.Update(TechniqueUpdate, id.Value);

        if (response != null)
        {
            if (isAdd) ResetTechniqueUpdate();
            TriggerANotification(response.Message);
            await LoadData();
        }
    }

    private void ResetTechniqueUpdate()
        => TechniqueUpdate = new TechniqueUpdateDto();

    private async Task DeleteItem(TechniqueDto item)
    {
        var dialogResult = CustomMessageBoxHelper.Show(Strings.Warning, Strings.DeleteItem, true);

        if (!dialogResult) return;

        var result = await TechniqueRequest.Delete(item.Id);
        if (result != null)
        {
            TriggerANotification(result.Message);
            await LoadData();
        }
    }

    private void EditItem(TechniqueDto item)
    {
        id = item.Id;
        TechniqueUpdate = ConvertToUpdateDto(item);
    }

    private void AddItem()
    {
        id = null;
        ResetTechniqueUpdate();
    }

    private TechniqueUpdateDto ConvertToUpdateDto(TechniqueDto techniqueDto)
    {
        if (techniqueDto == null)
        {
            throw new ArgumentNullException(nameof(techniqueDto));
        }

        var typeTechnique = TypeTechniqueItems?
            .FirstOrDefault(item => item.Name == techniqueDto.TypeTechnique);
        var member = MembersItems?
            .FirstOrDefault(item => item.FullName == techniqueDto.Member);
        var office = OfficesItems?
            .FirstOrDefault(item => item.Number == techniqueDto.Office);
        var computer = ComputersItems?
            .FirstOrDefault(item => item.Number == techniqueDto.Computer);
        var supplier = SuppliersItems?
            .FirstOrDefault(item => item.Name == techniqueDto.Supplier);

        return new TechniqueUpdateDto
        {
            Number = techniqueDto.Number,
            IdTypeTechnique = typeTechnique.Id,
            Name = techniqueDto.Name,
            IdMember = member?.Id,
            IdOffice = office?.Id,
            IdComputer = computer?.Id,
            DateOfPurchase = techniqueDto.DateOfPurchase,
            DateOfManufacture = techniqueDto.DateOfManufacture,
            DateOfUse = techniqueDto.DateOfUse.HasValue ? techniqueDto.DateOfUse.Value : null,
            IdSupplier = supplier.Id,
            Cost = techniqueDto.Cost,
            UnderRepair = techniqueDto.UnderRepair
        };
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
        var pagination = new PaginationModel { Page = CurrentPage, PageSize = Settings.Default.PageSize };
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

        filter = null;
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
