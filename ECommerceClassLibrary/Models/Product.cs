namespace ECommerceClassLibrary.Models
{
    public class Product
    {
        private int id;

        private string name;

        private string description;

        private decimal price;

        private decimal quantity;

        private int sellerId;



        public int Id
        {
            get => id; set => id = value;
        }

        public string Name
        {
            get => name; set => name = value;
        }

        public string Description
        {
            get => description; set => description = value;
        }

        public decimal Price
        {
            get => price; set => price = value;
        }

        public decimal Quantity
        {
            get => quantity; set => quantity = value;
        }

        public int SellerId
        {
            get => sellerId; set => sellerId = value;
        }


        public Product(int id, string name, string desc, decimal price, decimal quantity, int sellerId)
        {
            this.id = id;
            this.name = name;
            this.description = desc;
            this.price = price;
            this.quantity = quantity;
            this.sellerId = sellerId;
        }
    }
}
