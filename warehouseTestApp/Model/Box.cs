using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehouseTestApp.Model
{
    public class Box
    {
        public int Id { get; }
        public double Width { get; }
        public double Height { get; }
        public double Depth { get; }
        public double Weight { get; }
        public DateOnly? ProductionDate { get; }
        public DateOnly? ExpiryDate { get; }
        private DateOnly nowDate = DateOnly.FromDateTime(DateTime.Now);


        public double Volume => Width * Height * Depth;

        public Box(int id, double width, double height, double depth, double weight,
                  DateOnly? productionDate = null, DateOnly? expiryDate = null)
        {
            if (width <= 0 || height <= 0 || depth <= 0 || weight <= 0)
                throw new ArgumentException("Все параметры должны быть положительными");

            if (productionDate == null && expiryDate == null)
                throw new ArgumentException("Необходимо указать дату производства или срок годности");
            else if (expiryDate <= nowDate)
                throw new ArgumentException("коробка просрочена");


            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
            Weight = weight;
            ProductionDate = productionDate;
            ExpiryDate = expiryDate ?? productionDate?.AddDays(100);
        }
    }
}
