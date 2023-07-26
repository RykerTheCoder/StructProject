using CKK.Logic.Interfaces;

namespace CKK.Logic.Models
{
    public class Product : Entity
    {
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "New Price is less than 0.");
                }
                _price = value;
            }
        }
    }
}
