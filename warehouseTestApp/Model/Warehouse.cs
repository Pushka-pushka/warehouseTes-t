using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehouseTestApp.Model
{
    public class Warehouse
    {

        public enum ExpiryCategory
        {
            FirstCategory,  //срок от 0 до 20 дней
            SecondCategory, // срок от 21 до 60 дней
            LastCategory   // срок 61 день и выше

        }


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
            var box = new Box(_nextBoxId, width, height, depth, weight, productionDate, expiryDate);
            _boxes.Add(box);
            _nextBoxId++;
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
        public IEnumerable<IGrouping<DateOnly, Pallet>> GetPalletsGroupedByExpiryAndDay()
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


        public static ExpiryCategory GetExpiryCategory(DateOnly expiryDate)
        {
            int daysUntilExpiry = expiryDate.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber;

            if (daysUntilExpiry <= 20)
                return ExpiryCategory.FirstCategory;
            else if (daysUntilExpiry <=60)
                return ExpiryCategory.SecondCategory;
            else
                return ExpiryCategory.LastCategory;
        }




        public static Dictionary<ExpiryCategory, List<Pallet>> SortPalletsByExpiryCategories(IEnumerable<Pallet> pallets)
        {
            var result = new Dictionary<ExpiryCategory, List<Pallet>>()
        {
            { ExpiryCategory.FirstCategory, new List<Pallet>() },
            { ExpiryCategory.SecondCategory, new List<Pallet>() },
            { ExpiryCategory.LastCategory, new List<Pallet>() }
        };

            foreach (var pallet in pallets)
            {
                if (pallet.ExpiryDate.HasValue)
                {
                    var category = GetExpiryCategory(pallet.ExpiryDate.Value);
                    result[category].Add(pallet);
                }
                else
                {
                    // Паллеты без срока годности (без коробок) можно добавить в отдельную категорию
                    // или игнорировать
                }
            }

            // Сортировка внутри каждой категории
            foreach (var category in result.Keys.ToList())
            {
                result[category] = result[category]
                    .OrderBy(p => p.ExpiryDate)
                    .ThenBy(p => p.Weight)
                    .ToList();
            }

            return result;
        }
    }







}