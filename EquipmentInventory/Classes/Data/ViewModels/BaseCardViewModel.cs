using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public abstract class BaseCardViewModel<T> : INotifyPropertyChanged where T : new()
{
    private IDictionariesViewModel _viewModel;
    private T _item;
    private long? _id;

    public ICommand SaveItemCommand { get; }

    public T Item
    {
        get => _item;
        set => SetField(ref _item, value);
    }

    public long? Id
    {
        get => _id;
        private set => SetField(ref _id, value);
    }

    public BaseCardViewModel(IDictionariesViewModel viewModel, object selectedItem)
    {
        _viewModel = viewModel;
        _item = new T();

        InitializeFromSelectedItem(selectedItem);

        SaveItemCommand = new RelayCommand<object>(
            async (sender) => await UserAccountService.ExecuteTask((Button)sender, SaveItemExecute));
    }

    private void InitializeFromSelectedItem(object selectedItem)
    {
        try
        {
            if (selectedItem != null)
            {
                // Извлечение Id
                var idProperty = selectedItem.GetType().GetProperty("Id");
                if (idProperty != null)
                {
                    Id = (long)idProperty.GetValue(selectedItem);
                }

                // Получаем свойства, исключая Id, для обоих типов
                var selectedItemProps = selectedItem.GetType()
                    .GetProperties()
                    .Where(p => p.Name != "Id")
                    .ToList();

                var itemProps = typeof(T)
                    .GetProperties()
                    .Where(p => p.CanWrite)
                    .ToList();

                // Проверка совпадения количества свойств
                if (selectedItemProps.Count != itemProps.Count)
                {
                    throw new InvalidOperationException("Property count mismatch");
                }

                // Копирование по порядку свойств
                for (int i = 0; i < selectedItemProps.Count; i++)
                {
                    var sourceProp = selectedItemProps[i];
                    var targetProp = itemProps[i];

                    object value = sourceProp.GetValue(selectedItem);

                    // Проверка совместимости типов
                    if (targetProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    {
                        targetProp.SetValue(Item, value);
                    }
                    else
                    {
                        Console.WriteLine($"Type mismatch: {sourceProp.Name} -> {targetProp.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    protected async Task ProcessResult(BaseResponse response)
    {
        if (response != null)
        {
            if (!Id.HasValue) ClearItem(); 
            _viewModel.TriggerANotification(response.Message);
            await _viewModel.UpdateItemsAsync();
        }
    }

    private void ClearItem() => Item = new T();

    private async Task SaveItemExecute() => await SaveData();

    protected abstract Task SaveData();

    protected bool SetField<TField>(ref TField field, TField value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<TField>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
