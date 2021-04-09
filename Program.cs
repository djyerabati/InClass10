using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InClass1V0
{
   
        class Order
        {
            public int OrderId { get; set; }
            public string Name { get; set; }
            public List<Store> Total_Products { get; set; }
        }
        class Product
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }



            public List<Store> Total_Orders { get; set; }
        }



        class Store
        {
            public int StoreID { get; set; }
            public Order Order { get; set; }
            public Product Product { get; set; }
            public int ProductCount { get; set; }
        }

        class OrderContext : DbContext
        { 
            public DbSet<Product> Products { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Store> Store { get; set; }



        string connectionstring = "Server=(localdb)\\MSSQLLocalDB; Database=InClass1V0;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionstring);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (OrderContext context = new OrderContext())
            {

                context.Database.EnsureCreated();



                Product MyProduct1 = new Product();
                MyProduct1.ProductId = 1;
                MyProduct1.Name = "P1";
                MyProduct1.Price = 10;



                Product MyProduct2 = new Product();
                MyProduct2.ProductId = 2;
                MyProduct2.Name = "P2";
                MyProduct2.Price = 18;



                Order MyOrder1 = new Order();
                MyOrder1.OrderId = 11;


                Order MyOrder2 = new Order();
                MyOrder2.OrderId = 22;




                Order MyOrder3 = new Order();
                MyOrder3.OrderId = 33;




                context.Products.Add(MyProduct1);
                context.Products.Add(MyProduct2);



                context.Orders.Add(MyOrder1);
                context.Orders.Add(MyOrder2);
                context.Orders.Add(MyOrder3);



                context.SaveChanges();
                // Display all orders where a product is sold
                var a = context.Orders
                    .Include(c => c.Total_Products)
                    .Where(c => c.Total_Products.Count != 0);
                foreach (var i in a)
                {
                    Console.WriteLine("OrderID={0},Name={1}", i.OrderId, i.Name);
                }

                // For a given product, find the order where it is sold the maximum.
                Order output = context.Store
                    .Where(c => c.Product.Name == "P1")
                    .OrderByDescending(c => c.ProductCount)
                    .Select(c => c.Order)
                    .First();
                Console.WriteLine("OrderID={0},Name={1}", output.OrderId, output.Name);

                // Find the orders where a given product is sold.
                var orders = context.Store
                    .Where(c => c.Product.Name == "P2");
                foreach (var i in orders)
                {
                    Console.Write(i.Order.OrderId + " ");
                }
            }

        }
    }
}
