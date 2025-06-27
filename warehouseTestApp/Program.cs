using warehouseTestApp.Controller;
using warehouseTestApp.Model;
using warehouseTestApp.View;

namespace warehouseTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Инициализация компонентов MVC
            var warehouse = new Warehouse();
            var view = new ConsoleView();
            var controller = new WarehouseController(warehouse, view);

            // Запуск приложения
            controller.Run();
        }
    }
}
