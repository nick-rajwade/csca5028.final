using Microsoft.VisualStudio.TestTools.UnitTesting;
using csca5028.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csca5028.lib.Tests
{
    [TestClass()]
    public class SaleTests
    {
        [TestMethod()]
        public void SaleTest()
        {
            //create a list of items where items are {bacon, 1, 5.99}, {eggs, 1, 2.99}, {milk, 1, 3.99}, {bread, 1, 2.99}, {cheese, 1, 4.99}, {chicken, 1, 5.99}, {beef, 1, 6.99}, {pork, 1, 5.99}, {fish, 1, 7.99}, {apple, 1, 1.99}, {banana, 1, 1.99}, {orange, 1, 1.99}, {grape, 1, 1.99}, {strawberry, 1, 1.99}, {blueberry, 1, 1.99}, {raspberry, 1, 1.99}, {blackberry, 1, 1.99}, {watermelon, 1, 5.99}, {cantaloupe, 1, 5.99}, {honeydew, 1, 5.99}, {pineapple, 1, 5.99}, {kiwi, 1, 1.99}, {mango, 1, 1.99}, {papaya, 1, 1.99}, {pear, 1, 1.99}, {peach, 1, 1.99}, {plum, 1, 1.99}, {cherry, 1, 1.99}, {lemon, 1, 1.99}, {lime, 1, 1.99}, {coconut, 1, 1.99}, {avocado, 1, 1.99}, {potato, 1, 1.99}, {tomato, 1, 1.99}, {carrot, 1, 1.99}, {onion, 1, 1.99}, {lettuce, 1, 1.99}, {broccoli, 1, 1.99}, {cauliflower, 1, 1.99}, {corn, 1, 1.99}, {peas, 1, 1.99}, {green bean, 1, 1.99}, {green pepper, 1, 1.99}, {red pepper, 1, 1.99}, {yellow pepper, 1, 1.99}, {orange pepper, 1, 1.99}, {purple pepper, 1, 1.99}, {white pepper, 1, 1.99}, {black pepper, 1, 1.99}, {salt, 1, 1.99}, {pepper, 1, 1.99}, {sugar, 1, 1.99}, {flour, 1, 1.99}, {butter, 1, 1.99}, {oil, 1, 1.99}, {vinegar, 1, 1.99}, {soy sauce, 1, 1.99}, {ketchup, 1, 1.99}, {mustard, 1, 1.99}, {mayonnaise, 1, 1.99}, {ranch, 1, 1.99}, {italian, 1, 1.99}, {thousand island, 1, 1.99}, {blue cheese, 1, 1.99}, {caesar, 1, 1.99}, {russian, 1, 1.99}, {french, 1, 1.99}, {tartar, 1, 1.99}, {honey mustard, 1, 1.99}, {barbecue, 1, 1.99}, {honey barbecue, 1, 1.99}, {sweet and sour, 1, 1.99}, {teriyaki, 1, 1.99}, {sriracha, 1, 1.99}, {tobasco, 1, 1.99}, {worcestershire, 1, 1.99}, {salsa, 1, 1.99}, {guacamole, 1, 1.99}, {sour cream, 1, 1.99}, {cream cheese, 1, 1.99}, {syrup, 1, 1.99}, {jelly, 1, 1.99}, {jam, 1, 1.99}, {peanut butter, 1, 1.99}, {nutella, 1, 1.99}, {hummus, 1, 1.99}, {chocolate, 1, 1.99}, {caramel, 1,1.99}
            List<SaleItem> items = new List<SaleItem>();
            items.Add(new SaleItem() { name = "bacon", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "eggs", quantity = 1, price = 2.99M });
            items.Add(new SaleItem() { name = "milk", quantity = 1, price = 3.99M });
            items.Add(new SaleItem() { name = "bread", quantity = 1, price = 2.99M });
            items.Add(new SaleItem() { name = "cheese", quantity = 1, price = 4.99M });
            items.Add(new SaleItem() { name = "chicken", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "beef", quantity = 1, price = 6.99M });
            items.Add(new SaleItem() { name = "pork", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "fish", quantity = 1, price = 7.99M });
            items.Add(new SaleItem() { name = "apple", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "banana", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "orange", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "grape", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "strawberry", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "blueberry", quantity = 1, price = 1.99M });

            //create a sale with the list of items, payment type cash, storeID 1, and loyalty card false
            Sale sale = new Sale(items, Sale.PaymentType.Cash, new Guid("00000000-0000-0000-0000-000000000001"), false);
            Assert.AreEqual(15, sale.TotalItems);
            Assert.AreEqual(59.85M, sale.TotalPrice);


        }
    }
}