using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehouseTestApp.Model;

namespace warehouseTestApp.View.Interface
{
    public interface IView
    {
        void ShowMenu();
        void ShowMessage(string message);
        void DisplayPallets(IEnumerable<Pallet> pallets);
        void DisplayBoxes(IEnumerable<Box> boxes);
        void DisplayPalletsGroupedByExpiry(IEnumerable<IGrouping<DateOnly, Pallet>> groups);
        string GetUserInput(string prompt);
    }
}
