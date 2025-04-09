using EquipmentInventory.Classes.Data.Interfaces;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Services;

public class ProgramInformationService : IProgramInformationService
{
    private GroupBox _groupBox;
    private TextBox _textBox;

    public ProgramInformationService() { }

    public void RegisterControls(GroupBox groupBox, TextBox textBox)
    {
        _groupBox = groupBox;
        _textBox = textBox;
    }

    public void SetTitle(string header) => _groupBox.Header = header;

    public void SetDescription(string description) => _textBox.Text = description;
}
