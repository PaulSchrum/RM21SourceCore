using System.Linq;
using UpdateControls.Fields;

namespace ptsCogo.Models
{
    public class Selection
    {
        private Independent<Item> _selectedItem = new Independent<Item>();

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem.Value = value; }
        }
    }
}
