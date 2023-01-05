using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;

namespace PsssD.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext _dbContext;

        public ProductService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Product> GetProductList()
        {
            return _dbContext.Products.ToList();
        }
        public Product GetProductById(int id)
        {
            return _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
        }
        public Product AddProduct(Product product)
        {
            var result = _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return result.Entity;
        }
        public Product UpdateProduct(Product product)
        {
            var result = _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
            return result.Entity;
        }
        public bool DeleteProduct(int Id)
        {
            var filteredData = _dbContext.Products.Where(x => x.ProductId == Id).FirstOrDefault();
            var result = _dbContext.Remove(filteredData);
            _dbContext.SaveChanges();
            return result != null ? true : false;
        }

        public Object Get()
        {
          
            string v = _dbContext.Products.Count().ToString();
           
            return v;



        }
        public IEnumerable GetValues()
        {
            string v = _dbContext.Products.Count().ToString();

            return v;
        }
        //public IList<Product> Get()
        //{
        //    var results1 = (from c in this._dbContext.Products.Include(x => x.)
        //                    group c by c.ProductName into g
        //                    select new Product()
        //                    {
        //                        ProductName = g.Key,
        //                        ProductId = g.Distinct(),
        //                        ProductStock = g.Sum(ta => ta.ProductStock),

        //                        //  ProductId = g.Discount(),
        //                        ProductPrice = g.Max(q => q.ProductPrice),
        //                        //ProductStock = g.Min(q => q.ProductStock)
        //                    });


        //return results1.ToList();
        //var groupJoin = _.GroupJoin(studentList,  //inner sequence
        //                    std => std.StandardID, //outerKeySelector 
        //                    s => s.StandardID,     //innerKeySelector
        //                    (std, studentsGroup) => new // resultSelector 
        //                    {
        //                        Students = studentsGroup,
        //                        StandarFulldName = std.StandardName
        //                    });


    }

        
    }


