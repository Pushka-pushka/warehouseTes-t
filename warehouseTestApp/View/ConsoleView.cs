using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehouseTestApp.Model;
using warehouseTestApp.View.Interface;

namespace warehouseTestApp.View
{
    public class ConsoleView : IView
    {
       
        public void ShowMenu()
        {
           
            Console.Clear();

            Console.WriteLine("Система управления складом");
            Console.WriteLine("1. Добавить паллету");
            Console.WriteLine("2. Добавить коробку");
            Console.WriteLine("3. Разместить коробку на паллете");
            Console.WriteLine("4. Показать все паллеты");
            Console.WriteLine("5. Показать коробки без паллет");
            Console.WriteLine("6. Показать паллеты, сгруппированные по сроку годности");
            Console.WriteLine("7. Показать паллеты с ближайшим сроком годности");
            Console.WriteLine("8. Выход");
            Console.WriteLine("_____________________");
            Console.Write("Выберите действие: ");
           
        }

        public void ShowMessage(string message) => Console.WriteLine(message);

        public void DisplayPallets(IEnumerable<Pallet> pallets)
        {
            Console.WriteLine("\nСписок паллет:");
            foreach (var pallet in pallets)
            {
                Console.WriteLine($"Паллета ID: {pallet.Id}, Объем: {pallet.Volume/1000000:F2} M.куб, Вес: {pallet.Weight:F2}кг., Срок: {pallet.ExpiryDate?.ToString("dd.MM.yyyy") ?? "Нет"}");
                foreach (var box in pallet.Boxes)
                {
                    Console.WriteLine($"  Коробка ID: {box.Id}, Объем: {box.Volume/1000000:F2} M.куб, Вес: {box.Weight:F2}кг., Срок: {box.ExpiryDate?.ToString("dd.MM.yyyy")}");
                }
            }
            Console.WriteLine("_____________________");

        }

        public void DisplayBoxes(IEnumerable<Box> boxes)
        {
            Console.WriteLine("\nКоробки без паллет:");
            foreach (var box in boxes)
            {
                Console.WriteLine($"ID: {box.Id}, Объем: {box.Volume/1000000:F2} М.куб, Вес: {box.Weight:F2} кг, Срок: {box.ExpiryDate?.ToString("dd.MM.yyyy")}");
            }

            Console.WriteLine("_____________________");
        }

        public void DisplayPalletsGroupedByExpiry(IEnumerable<IGrouping<DateOnly, Pallet>> groups)
        {
            Console.WriteLine("\nПаллеты, сгруппированные по сроку годности:");
            foreach (var group in groups)
            {
                Console.WriteLine($"Срок годности: {group.Key:dd.MM.yyyy}");
                foreach (var pallet in group.OrderBy(p => p.Volume))
                {
                    Console.WriteLine($"  Паллета ID: {pallet.Id}, Объем: {pallet.Volume/1000000:F2} М.куб, Вес: {pallet.Weight:F2}кг.");
                }
            }
            Console.WriteLine("_____________________");
        }

        public string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
