using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehouseTestApp.Model;
using warehouseTestApp.View.Interface;

namespace warehouseTestApp.Controller
{
    public class WarehouseController
    {
        private readonly Warehouse _warehouse;
        private readonly IView _view;

        public WarehouseController(Warehouse warehouse, IView view)
        {
            _warehouse = warehouse;
            _view = view;
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                try
                {
                    _view.ShowMenu();
                    var input = _view.GetUserInput("");

                    switch (input)
                    {
                        case "1":
                            AddPallet();
                            break;
                        case "2":
                            AddBox();
                            break;
                        case "3":
                            _view.DisplayBoxes(_warehouse.BoxesWithoutPallet);
                            _view.DisplayPallets(_warehouse.Pallets);
                            PlaceBoxOnPallet();
                            break;
                        case "4":
                            _view.DisplayPallets(_warehouse.Pallets);
                            _view.DisplayBoxes(_warehouse.BoxesWithoutPallet);
                            break;
                        case "5":
                            _view.DisplayBoxes(_warehouse.BoxesWithoutPallet);
                            break;
                        case "6":
                            _view.DisplayPalletsGroupedByExpiry(_warehouse.GetPalletsGroupedByExpiry());
                            break;
                        case "7":
                            ShowPalletsWithNearestExpiry();
                            break;
                        case "8":
                            _view.SortPalletsByExpiryCategories(_warehouse.Pallets); // сортировка по категориям
                            break;

                        case "9":  
                           
                            running = false;
                            break;
                        default:
                            _view.ShowMessage("Неверный выбор");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _view.ShowMessage($"Ошибка: {ex.Message}");
                }

                if (running)
                {
                    _view.ShowMessage("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void AddPallet()
        {
            double width = GetPositiveDouble("Введите ширину паллеты (см.): ");
            double height = GetPositiveDouble("Введите высоту паллеты (см.): ");
            double depth = GetPositiveDouble("Введите глубину паллеты (см.): ");

            _warehouse.AddPallet(width, height, depth);
            _view.ShowMessage("Паллета успешно добавлена");
        }

        private void AddBox()
        {
            double width = GetPositiveDouble("Введите ширину коробки (см.): ");
            double height = GetPositiveDouble("Введите высоту коробки(см.): ");
            double depth = GetPositiveDouble("Введите глубину коробки(см.): ");
            double weight = GetPositiveDouble("Введите вес коробки (кг.): ");

            var dateChoice = _view.GetUserInput("Укажите дату производства (1) или срок годности (2)? ");
            DateOnly? productionDate = null;
            DateOnly? expiryDate = null;
            DateOnly nowDate = DateOnly.FromDateTime(DateTime.Now); ;

            if (dateChoice == "1")
            {

                productionDate = GetDate("Введите дату производства (дд.мм.гггг): ");
                
                if (productionDate > nowDate)
                    throw new ArgumentException("ошибка записи");
               
            }
            else if (dateChoice == "2")
            {
                expiryDate = GetDate("Введите срок годности (дд.мм.гггг): ");
                
            }
            else
            {
                throw new ArgumentException("Неверный выбор");
            }

            _warehouse.AddBox(width, height, depth, weight, productionDate, expiryDate);
            _view.ShowMessage("Коробка успешно добавлена");
        }

        private void PlaceBoxOnPallet()
        {
            int boxId = GetPositiveInt("Введите ID коробки: ");
            int palletId = GetPositiveInt("Введите ID паллеты: ");

            _warehouse.PlaceBoxOnPallet(boxId, palletId);
            _view.ShowMessage("Коробка успешно размещена на паллете");
        }

        private void ShowPalletsWithNearestExpiry()
        {
            int count = GetPositiveInt("Введите количество паллет для отображения: ");
            var pallets = _warehouse.GetPalletsSortedByExpiryThenByWeight(count);
            _view.DisplayPallets(pallets);
        }

        private double GetPositiveDouble(string prompt)
        {
            while (true)
            {
                var input = _view.GetUserInput(prompt);
                if (double.TryParse(input, out double result) && result > 0)
                    return result;
                _view.ShowMessage("Неверный ввод. Введите положительное число.");
            }
        }

        private int GetPositiveInt(string prompt)
        {
            while (true)
            {
                var input = _view.GetUserInput(prompt);
                if (int.TryParse(input, out int result) && result > 0)
                    return result;
                _view.ShowMessage("Неверный ввод. Введите положительное целое число.");
            }
        }

        private DateOnly GetDate(string prompt)
        {
            while (true)
            {
                var input = _view.GetUserInput(prompt);
                if (DateOnly.TryParse(input, out DateOnly result))
                    return result;
                _view.ShowMessage("Неверный формат даты. Используйте дд.мм.гггг.");
            }
        }
    }
}
