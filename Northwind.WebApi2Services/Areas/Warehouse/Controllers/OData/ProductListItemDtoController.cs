// ProductListItemDtoController.cs

namespace Northwind.WebApi2Services.Areas.Warehouse.Controllers.OData
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.OData;
    using Dto;
    using EF6Models;
    using Models;

    /*
    若要为此控制器添加路由，请将这些语句合并到 WebApiConfig 类的 Register 方法中。请注意 OData URL 区分大小写。

    using System.Web.Http.OData.Builder;
    using Northwind.EF6Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Product>("Product");
    builder.EntitySet<Category>("Categories"); 
    builder.EntitySet<OrderDetail>("OrderDetail"); 
    builder.EntitySet<Supplier>("Suppliers"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */

    public class ProductListItemDtoController : ODataController
    {
        private readonly NorthwindDbContext _db = new NorthwindDbContext();

        // GET odata/Product
        [Queryable]
        public IQueryable<ProductListItemDto> GetProductListItemDto()
        {
            using (var ctx = new NorthwindDbContext())
            {
                IQueryable<ProductListItemDto> items = ctx.Products
                    .Include(p => p.Category)
                    .Select(ModelMapper.Product2ProductListItemDto);

                return items;
            }
        }

        // GET odata/Product(5)
        [Queryable]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Products.Where(product => product.ProductId == key));
        }

        // PUT odata/Product(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != product.ProductId)
            {
                return BadRequest();
            }

            _db.Entry(product).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(product);
        }

        // POST odata/Product
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Created(product);
        }

        // PATCH odata/Product(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await ((DbSet<Product>) _db.Products).FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Patch(product);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(product);
        }

        // DELETE odata/Product(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Product product = await ((DbSet<Product>) _db.Products).FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Product(5)/Category
        [Queryable]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Products.Where(m => m.ProductId == key).Select(m => m.Category));
        }

        // GET odata/Product(5)/OrderDetails
        [Queryable]
        public IQueryable<OrderDetail> GetOrderDetails([FromODataUri] int key)
        {
            return _db.Products.Where(m => m.ProductId == key).SelectMany(m => m.OrderDetails);
        }

        // GET odata/Product(5)/Supplier
        [Queryable]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Products.Where(m => m.ProductId == key).Select(m => m.Supplier));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int key)
        {
            return _db.Products.Count(e => e.ProductId == key) > 0;
        }
    }
}