using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
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

public class TablesSwitcherArchiveViewModel : INotifyPropertyChanged
{
    private readonly NotificationService _notificationService;
    private ObservableCollection<ArchiveDto> _items;
    private string _searchText;
    private int _totalItems;
    private bool _isLoading;
    private int _currentPage = 1;
    private const int PageSize = 20;

    public ICommand CleanCommand { get; }
    public ICommand ForwardCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand SearchCommand { get; }

    public ObservableCollection<ArchiveDto> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }

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

    public TablesSwitcherArchiveViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        _items = new ObservableCollection<ArchiveDto>();

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
                DefaultSearchFields();
                DefautlSearchInformation();
                ClearItems();
            }
        );

        ForwardCommand = new RelayCommand(
            async () =>
            {
                CurrentPage++;
                await LoadArchiveData();
            },
            () => CurrentPage < TotalPages && !IsLoading
        );

        BackCommand = new RelayCommand(
            async () =>
            {
                CurrentPage--;
                await LoadArchiveData();
            },
            () => CurrentPage > 1 && !IsLoading
        );
    }

    private async Task ExecuteSearch()
    {
        try
        {
            IsLoading = true;
            CurrentPage = 1;
            await LoadArchiveData();
        }
        finally
        {
            IsLoading = false;
            UpdateCommandStates();
        }
    }

    private async Task LoadArchiveData()
    {
        var pagination = new PaginationModel
        {
            Page = CurrentPage,
            PageSize = PageSize,
        };

        var filter = SearchText;

        var result = await ArchiveRequest.GetArchive(pagination, filter);

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

    private void ClearItems()
        => _items.Clear();

    private void ProcessDataNotFound()
    {
        DefautlSearchInformation();
        TriggerANotification(Strings.DataNotFound);
    }

    private void DefaultSearchFields()
        => SearchText = string.Empty;

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
