using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesDatePrediction.UnitTest
{
    public static class TestData
    {
        public static Product CreateValidProduct(int productId = 1, decimal price = 10m)
        {
            return new Product
            {
                Productid = productId,
                Productname = "Test Product",
                Unitprice = price,
                Discontinued = false,
                Categoryid = 1,
                Supplierid = 1,
                Category = new Category
                {
                    Categoryid = 1,
                    Categoryname = "Test Category",
                    Description = "Dummy description"
                },
                Supplier = new Supplier
                {
                    Supplierid = 1,
                    Companyname = "Test Supplier",
                    Contactname = "Contact Name",
                    Contacttitle = "Contact Title",
                    Address = "Test Address",
                    City = "Test City",
                    Country = "Test Country",
                    Phone = "123456789"
                }
            };
        }
    }
}
