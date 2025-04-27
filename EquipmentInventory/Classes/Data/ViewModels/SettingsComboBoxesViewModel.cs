using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class SettingsComboBoxesViewModel : INotifyPropertyChanged
{
    private LanguageItem _selectedLanguage;
    private int _selectedLongInteger;
    private int _selectedShortInteger;
    private SettingsBuffer _buffer;
    public event PropertyChangedEventHandler PropertyChanged;

    public IList<int> LongIntegerList { get; }
    public IList<int> ShortIntegerList { get; }
    public List<LanguageItem> Languages { get; }

    public LanguageItem SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (_selectedLanguage != value)
            {
                _selectedLanguage = value;
                _buffer.CultureInfo = value.Code;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }
    }

    public int SelectedLongInteger
    {
        get => _selectedLongInteger;
        set
        {
            if (_selectedLongInteger != value)
            {
                _selectedLongInteger = value;
                _buffer.YearOfObsolescence = value;
                _buffer.ApplyToYearOfObsolescence();
                OnPropertyChanged(nameof(SelectedLongInteger));
            }
        }
    }

    public int SelectedShortInteger
    {
        get => _selectedShortInteger;
        set
        {
            if (_selectedShortInteger != value)
            {
                _selectedShortInteger = value;
                _buffer.NumberOfRecords = value;
                _buffer.ApplyNumberOfRecords();
                OnPropertyChanged(nameof(SelectedLongInteger));
            }
        }
    }

    public SettingsComboBoxesViewModel()
    {
        _buffer = new SettingsBuffer();

        LongIntegerList = new List<int>(Enumerable.Range(1, 100));
        ShortIntegerList = new List<int>(Enumerable.Range(1, 10));

        Languages = new List<LanguageItem>()
        {
            new LanguageItem(
                name: "Русский", 
                code: "ru-RU"),

            new LanguageItem(
                name: "English", 
                code: "en-US")
        };

        SelectedLanguage = Languages.FirstOrDefault(l => l.Code == _buffer.CultureInfo) 
            ?? Languages[0];
        SelectedLongInteger = LongIntegerList.Contains(_buffer.YearOfObsolescence)
            ? _buffer.YearOfObsolescence
            : LongIntegerList[4];
        SelectedShortInteger = ShortIntegerList.Contains(_buffer.NumberOfRecords)
            ? _buffer.NumberOfRecords 
            : ShortIntegerList[4];
    }

    public ICommand ApplyLanguageCommand => new RelayCommand(() =>
    {
        var dialogResult = CustomMessageBoxHelper.Show(Strings.Warning, Strings.LanguageChange, true);

        if (dialogResult)
        {
            _buffer.ApplyToCultureInfo();

            WindowService.RestoreApp();
        }
    });

    protected virtual void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
