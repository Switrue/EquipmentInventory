using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Properties;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class DictionariesViewModel : INotifyPropertyChanged
{
    private ObservableCollection<object> _items;
    private object _selectedModel;

    public ICommand ClearItems { get; }

    public ObservableCollection<object> Items
    {
        get => _items;
        set => SetField(ref _items, value);
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

    public DictionariesViewModel()
    {
        Items = new ObservableCollection<object>();

        ModelTypes = new ObservableCollection<string> 
        { 
            Strings.TypeTecnique, 
            Strings.Supplier,
            Strings.Employee,
            Strings.Position,
            Strings.Computer,
            Strings.Office,
            Strings.User
        };

        ClearItems = new RelayCommand(ClearItemsExecute);
    }

    public async Task UpdateItemsAsync()
    {
        try
        {
            if (SelectedModel == null) Items = new ObservableCollection<object>();

            if (SelectedModel is string modelTypes)
            {
                IEnumerable<object> newData = modelTypes switch
                {
                    var t when t == Strings.TypeTecnique => await TypeTechniqueRequest.GetTypeTechniqueDataAsync(),
                    var t when t == Strings.Supplier => await SuppliersRequest.GetSuppliersDataAsync(),
                    var t when t == Strings.Employee => await MembersRequest.GetMembersDataAsync(),
                    var t when t == Strings.Position => await PositionsRequest.GetPositionsDataAsync(),
                    var t when t == Strings.Computer => await ComputersRequest.GetComputersDataAsync(),
                    var t when t == Strings.Office => await OfficesRequest.GetOfficesDataAsync(),
                    var t when t == Strings.User => await UsersRequest.GetUsersDataAsync(),
                    _ => new List<object>()
                };

                Items = [.. newData];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void ClearItemsExecute()
        => SelectedModel = null;

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
