using System.Collections.Generic;
using System.Web.Http;

namespace TfsWatchDog.Service
{
    public class WatchDogController : ApiController
    {
        public IList<Product> GetIterations()
        {
            return new List<Product>(){
            new Product(){ID = 1, Name="Product 1", Description="Desc 1"},
            new Product(){ID = 2, Name="Product 2", Description="Desc 2"},
            new Product(){ID = 3, Name="Product 3", Description="Desc 3"},
    };
        }
    }

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
