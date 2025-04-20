using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class TableSwitcherInventoryViewModel : INotifyPropertyChanged
{
    private ObservableCollection<TechniqueDto> _items;

    public ObservableCollection<TechniqueDto> Items
    {
        get => _items;
        set
        {
            if (_items != value)
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public TableSwitcherInventoryViewModel()
    {
        Items = new ObservableCollection<TechniqueDto>();
    }

    public ICommand Search => new RelayCommand<Button>(async (sender) =>
    {
        await UserAccountService.ExecuteTask(sender, LoadTechniqueData);
    });

    private async Task LoadTechniqueData()
    {
        var pagination = new PaginationModel { Page = 1, PageSize = 10 };
        var filter = new TechniqueFiltersDto { Number = "13", IsFastened = true };

        var result = await TechniqueRequest.GetTechnique(pagination, filter);

        if (result?.Items.Any() == true)
        {
            Console.WriteLine($"Всего записей: {result.TotalCount}");

            Items.Clear();

            foreach (var item in result.Items)
            {
                Items.Add(item);
            }
        }
        else
        {
            CustomMessageBoxHelper.Show("Данные не найдены.");
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
