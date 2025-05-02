using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public abstract class TableSwitcherBaseViewModel<T> : INotifyPropertyChanged
{
    protected readonly NotificationService _notificationService;
    private ObservableCollection<T> _items;
    private T _selectedItem;
    private string _searchText;
    private int _totalItems;
    private bool _isLoading;
    private int _currentPage = 1;

    public ICommand CleanCommand { get; }
    public ICommand ForwardCommand { get; }
    public ICommand LastPageCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand FirstPageCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ExportToWordCommand { get; }
    public ICommand ExportToExcelCommand { get; }

    public ObservableCollection<T> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }

    public T SelectedItem
    {
        get => _selectedItem;
        set => SetField(ref _selectedItem, value);
    }

    // Общие свойства
    public string SearchText
    {
        get => _searchText;
        set => SetField(ref _searchText, value);
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
            if (_totalItems == value) return;
            _totalItems = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TotalPages));
            UpdateCommandStates();
        }
    }

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage == value) return;
            _currentPage = value;
            OnPropertyChanged();
            UpdateCommandStates();
        }
    }

    protected int TotalPages => TotalItems > 0 ? (int)Math.Ceiling((double)TotalItems / Settings.Default.PageSize) : 0;

    protected TableSwitcherBaseViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        _items = new ObservableCollection<T>();

        SearchCommand = new RelayCommand(async () => await ExecuteSearch(), () => !IsLoading);
        CleanCommand = new RelayCommand(ResetSearchParameters);
        ForwardCommand = new RelayCommand(async () => await ChangePage(1), CanForwardCommand);
        LastPageCommand = new RelayCommand(async () => await ChangePage(TotalPages - CurrentPage), CanForwardCommand);
        BackCommand = new RelayCommand(async () => await ChangePage(-1), CanBackCommand);
        FirstPageCommand = new RelayCommand(async () => await ChangePage(-(CurrentPage - 1)), CanBackCommand);
        ExportToWordCommand = new RelayCommand(ExportToWord);
        ExportToExcelCommand = new RelayCommand(ExportToExcel);
    }

    protected void ExportToWord()
    {
        if (Items.Count > 0) ExportHelper.Word(Items.ToList());
    }

    protected void ExportToExcel()
    {
        if (Items.Count > 0) ExportHelper.Excel(Items.ToList());
    }

    protected virtual async Task ExecuteSearch()
    {
        try
        {
            IsLoading = true;
            CurrentPage = 1;
            await LoadData();
        }
        finally
        {
            IsLoading = false;
            UpdateCommandStates();
        }
    }

    protected bool CanBackCommand()
        => CurrentPage > 1 && !IsLoading;

    protected bool CanForwardCommand() 
        => CurrentPage < TotalPages && !IsLoading;

    protected async Task ChangePage(int delta)
    {
        CurrentPage += delta;
        await LoadData();
    }

    protected virtual void ResetSearchParameters()
    {
        SearchText = string.Empty;
        DefautlSearchInformation();
        ClearItems();
    }

    protected abstract Task LoadData();

    protected void ClearItems() 
    {
        Items.Clear();
        SelectedItem = default;
    }

    protected void UpdateCommandStates()
    {
        (ForwardCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (LastPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (BackCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (FirstPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    protected void ProcessResult(PaginatedResult<T> result)
    {
        if (result?.Items.Any() == true)
        {
            Items = [.. result.Items];
            TotalItems = result.TotalCount;
        }
        else
        {
            ProcessDataNotFound();
        }
    }

    protected void DefautlSearchInformation()
    {
        TotalItems = 0;
        CurrentPage = 1;
    }

    protected void ProcessDataNotFound()
    {
        ClearItems();
        DefautlSearchInformation();
        TriggerANotification(Strings.DataNotFound);
    }

    protected void TriggerANotification(string message)
        =>  _notificationService.Show(message);

    // Общие методы для INotifyPropertyChanged
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
