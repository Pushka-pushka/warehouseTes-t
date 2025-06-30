using warehouseTestApp.Model;

namespace WerehauseAppTest
{
    public class WarehouseTest
    {
        [Fact]
        public void BoxCreate()
        {
            //arrange
            var productionDate = new DateOnly(2025, 6, 30);

            //act
            var box = new Box(1, 10, 10, 10, 5, productionDate);
          
            //assert
            Assert.Equal(1, box.Id);
            Assert.Equal(10, box.Width);
            Assert.Equal(1000, box.Volume);
            Assert.Equal(productionDate.AddDays(100), box.ExpiryDate); // productionDate + 100 дней

        }

        [Fact]
        public void PalleteCreate() 
        {
            //act
            var pallete = new Pallet(4, 20, 20, 20);

            //assert
            Assert.Equal(4, pallete.Id);
            Assert.Equal(30, pallete.Weight);
            Assert.Equal(8000, pallete.Volume);

        }

        [Fact]
        public void PaleteWithTwoBoxs() 
        {
            var productionDate = new DateOnly(2025, 6, 30);

            var box = new Box(1, 10, 10, 10, 15, productionDate);
            var boxTwo = new Box(2, 10, 10, 10, 25, productionDate);

            var pallete = new Pallet(4, 20, 20, 20);
            pallete.AddBox(box);
            pallete.AddBox(boxTwo);

            Assert.Equal(70, pallete.Weight);
            Assert.Equal(10000, pallete.Volume);


        }

        [Fact]
        public void WarehouseCreateBox()
        {
            //arrange
            var productionDate = new DateOnly(2025, 6, 30);
            var expiryDate = new DateOnly(2028, 8, 11);
            var warehouse = new Warehouse();


            //act
            warehouse.AddBox(11, 22, 33, 16, productionDate, null);
            warehouse.AddBox(11, 22, 33, 16, null, expiryDate);

            //assert
            Assert.Equal(2, warehouse.BoxesWithoutPallet.Count());

        }


        [Fact]
        public void WarehouseCreatePallete()
        {
            var warehouse = new Warehouse();

            warehouse.AddPallet(10, 10, 10);
            warehouse.AddPallet(20,20,20);  
            warehouse.AddPallet(30,30,30);

            Assert.Equal(3, warehouse.Pallets.Count());

        }

    }
}
