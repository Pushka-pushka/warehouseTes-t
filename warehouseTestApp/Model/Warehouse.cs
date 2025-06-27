using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehouseTestApp.Model
{
    public class Warehouse
    {
        private List<Pallet> _pallets = new();
        private List<Box> _boxes = new();
        private int _nextPalletId = 1;
        private int _nextBoxId = 1;

        public IReadOnlyList<Pallet> Pallets => _pallets.AsReadOnly();
        public IReadOnlyList<Box> BoxesWithoutPallet => _boxes.AsReadOnly();

        public void AddPallet(double width, double height, double depth)
        {
            var pallet = new Pallet(_nextPalletId++, width, height, depth);
            _pallets.Add(pallet);
        }

        public void AddBox(double width, double height, double depth, double weight,
                         DateOnly? productionDate, DateOnly? expiryDate)
        {
            var box = new Box(_nextBoxId++, width, height, depth, weight, productionDate, expiryDate);
            _boxes.Add(box);
        }

        public void PlaceBoxOnPallet(int boxId, int palletId)
        {
            var box = _boxes.FirstOrDefault(b => b.Id == boxId);
            var pallet = _pallets.FirstOrDefault(p => p.Id == palletId);

            if (box == null || pallet == null)
                throw new ArgumentException("Коробка или паллета не найдены");

            pallet.AddBox(box);
            _boxes.Remove(box);
        }

        public IEnumerable<IGrouping<DateOnly, Pallet>> GetPalletsGroupedByExpiry()
        {
            return _pallets
                .Where(p => p.ExpiryDate.HasValue)
                .GroupBy(p => p.ExpiryDate.Value)
                .OrderBy(g => g.Key);
        }

        public IEnumerable<Pallet> GetPalletsSortedByExpiryThenByWeight(int? count = null)
        {
            var query = _pallets
                .Where(p => p.ExpiryDate.HasValue)
                .OrderBy(p => p.ExpiryDate)
                .ThenBy(p => p.Weight);

            return count.HasValue ? query.Take(count.Value) : query;
        }

    }
}
