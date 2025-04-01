using System.Collections.Generic;
using System.Linq;

namespace EquipmentInventory.Classes.Models.ViewModels
{
    public class ComboBoxesViewModel
    {
        public IList<int> LongIntegerList { get; }

        public ComboBoxesViewModel()
        {
            LongIntegerList = new List<int>(Enumerable.Range(0, 1000));
        }
    }
}
