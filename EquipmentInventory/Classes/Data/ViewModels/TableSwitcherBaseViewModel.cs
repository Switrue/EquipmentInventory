using EquipmentInventory.Classes.Data.Models;
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
    private string _searchText;
    private int _totalItems;
    private bool _isLoading;
    private int _currentPage = 1;
    protected const int PageSize = 20;

    public ICommand CleanCommand { get; }
    public ICommand ForwardCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand SearchCommand { get; }

    public ObservableCollection<T> Items
    {
        get => _items;
        set => SetField(ref _items, value);
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

    protected int TotalPages => TotalItems > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;

    protected TableSwitcherBaseViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        _items = new ObservableCollection<T>();

        SearchCommand = new RelayCommand(async () => await ExecuteSearch(), () => !IsLoading);
        CleanCommand = new RelayCommand(ResetSearchParameters);
        ForwardCommand = new RelayCommand(async () => await ChangePage(1), () => CurrentPage < TotalPages && !IsLoading);
        BackCommand = new RelayCommand(async () => await ChangePage(-1), () => CurrentPage > 1 && !IsLoading);
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

    protected void ClearItems() => _items.Clear();

    protected void UpdateCommandStates()
    {
        (ForwardCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (BackCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    protected void ProcessResult(PaginatedResult<T> result)
    {
        if (result?.Items.Any() == true)
        {
            ClearItems();
            foreach (var item in result.Items) Items.Add(item);
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

    private void TriggerANotification(string message)
        => _notificationService.Show(message);

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
