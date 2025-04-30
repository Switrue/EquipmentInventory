using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Forms.Pages.Cards;
using EquipmentInventory.Properties;
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

public class DictionariesViewModel : IDictionariesViewModel
{
    private readonly NotificationService _notificationService;
    private static Dictionary<string, (Func<Task<IEnumerable<object>>> get,
                                       Func<object, UserControl> card,
                                       Func<long, Task<BaseResponse>> delete)> ModelActions;
    
    private UserControl _card;
    private ObservableCollection<object> _items;
    private object _selectedItem;
    private object _selectedModel;

    public ICommand ClearItemsCommand { get; }
    public ICommand AddItemCommand { get; }
    public ICommand EditItemCommand { get; }
    public ICommand DeleteItemCommand { get; }

    public UserControl Card
    {
        get => _card;
        set => SetField(ref _card, value);
    }

    public ObservableCollection<object> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }

    public object SelectedItem
    {
        get => _selectedItem;
        set => SetField(ref _selectedItem, value);
    }

    public object SelectedModel
    {
        get => _selectedModel;
        set
        {
            if (SetField(ref _selectedModel, value))
            {
                _ = UpdateItemsAsync();
            }
        }
    }

    public ObservableCollection<string> ModelTypes { get; set; }

    public DictionariesViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;

        ModelActions = new()
        {
            { Strings.TypeTecnique, (
                async () => await TypeTechniqueRequest.GetTypeTechniqueDataAsync(),
                param => new TypeTechniqueCard(this, param),
                TypeTechniqueRequest.Delete
            )},

            { Strings.Supplier, (
                async () => await SuppliersRequest.GetSuppliersDataAsync(),
                param => new SupplierCard(),
                SuppliersRequest.Delete
            )},

            { Strings.Employee, (
                async () => await MembersRequest.GetMembersDataAsync(),
                param => new MemberCard(),
                MembersRequest.Delete
            )},

            { Strings.Position, (
                async () => await PositionsRequest.GetPositionsDataAsync(),
                param => new PositionCard(),
                PositionsRequest.Delete
            )},

            { Strings.Computer, (
                async () => await ComputersRequest.GetComputersDataAsync(),
                param => new ComputerCard(),
                ComputersRequest.Delete
            )},

            { Strings.Office, (
                async () => await OfficesRequest.GetOfficesDataAsync(),
                param => new OfficeCard(),
                OfficesRequest.Delete
            )},

            { Strings.User, (
                async () => await UsersRequest.GetUsersDataAsync(),
                param => new UserCard(),
                UsersRequest.Delete
            )}
        };

        Card = new UserControl();
        Items = new ObservableCollection<object>();

        var types = ModelActions.Keys.ToList();
        ModelTypes = new ObservableCollection<string>(types);

        ClearItemsCommand = new RelayCommand(ClearItemsExecute);
        AddItemCommand = new RelayCommand(AddItem);
        EditItemCommand = new RelayCommand(EditItem);
        DeleteItemCommand = new RelayCommand(async () => await DeleteItem());
    }

    private void ClearItemsExecute()
        => SelectedModel = null;

    private void AddItem()
        => CardFactory();

    private void EditItem()
        => CardFactory(SelectedItem);

    private void CardFactory(object item = null)
    {
        try
        {
            if (SelectedModel is string modelTypes
                && ModelActions.TryGetValue(modelTypes, out var actions))
            {
                var (_, card, _) = actions;
                Card = card(item);
            }
            else
            {
                Card = new UserControl();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private async Task DeleteItem()
    {
        var dialogResult = CustomMessageBoxHelper.Show(Strings.Warning, Strings.DeleteItem, true);
        if (!dialogResult) return;

        try
        {
            var id = GetId() ?? throw new InvalidOperationException("Id cannot be null");

            if (SelectedModel is string modelTypes
                && ModelActions.TryGetValue(modelTypes, out var actions))
            {
                var (_, _, deleteAction) = actions;
                var response = await deleteAction(long.Parse(id.ToString()));

                if (response != null)
                {
                    await UpdateItemsAsync();
                    TriggerANotification(response.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private object GetId()
    {
        // Поиск с помощью рефлексии
        var idProperty = SelectedItem.GetType().GetProperty("Id");
        if (idProperty != null)
        {
            return idProperty.GetValue(SelectedItem);
        }

        return null;
    }

    public async Task UpdateItemsAsync()
    {
        try
        {
            if (SelectedModel is string modelTypes
                && ModelActions.TryGetValue(modelTypes, out var actions))
            {
                var (loadAction, _, _) = actions;
                var newData = await loadAction();
                Items = [.. newData];
            }
            else
            {
                Items = new ObservableCollection<object>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void TriggerANotification(string message)
        => _notificationService.Show(message);

    protected bool SetField<TField>(ref TField field, TField value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<TField>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
