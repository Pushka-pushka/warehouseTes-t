using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehouseTestApp.Model
{
    public class Pallet
    {

        public int Id { get; }
        public double Width { get; }
        public double Height { get; }
        public double Depth { get; }
        public List<Box> Boxes { get; } = new();

        public DateOnly? ExpiryDate => Boxes.Count > 0 ? Boxes.Min(b => b.ExpiryDate) : null;
        public double Weight => 30 + Boxes.Sum(b => b.Weight);
        public double Volume => Width * Height * Depth + Boxes.Sum(b => b.Volume);

        public Pallet(int id, double width, double height, double depth)
        {
            if (width <= 0 || height <= 0 || depth <= 0)
                throw new ArgumentException("Размеры паллеты должны быть положительными");

            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
        }

        public void AddBox(Box box)
        {
            if (box.Width > Width || box.Depth > Depth)
                throw new InvalidOperationException("Коробка не помещается на паллету");

            Boxes.Add(box);
        }
    }

}
