namespace Northwind.WebApi2Services.Areas.Warehouse.Controllers.OData
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.OData;
    using EF6Models;

    /*
    若要为此控制器添加路由，请将这些语句合并到 WebApiConfig 类的 Register 方法中。请注意 OData URL 区分大小写。

    using System.Web.Http.OData.Builder;
    using Northwind.EF6Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Supplier>("Supplier");
    builder.EntitySet<Product>("Product"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SupplierController : ODataController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        // GET odata/Supplier
        [Queryable]
        public IQueryable<Supplier> GetSupplier()
        {
            return db.Suppliers;
        }

        // GET odata/Supplier(5)
        [Queryable]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            return SingleResult.Create(db.Suppliers.Where(supplier => supplier.SupplierId == key));
        }

        // PUT odata/Supplier(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != supplier.SupplierId)
            {
                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(supplier);
        }

        // POST odata/Supplier
        public async Task<IHttpActionResult> Post(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            return Created(supplier);
        }

        // PATCH odata/Supplier(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Supplier> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Supplier supplier = await ((DbSet<Supplier>)db.Suppliers).FindAsync(key);
            if (supplier == null)
            {
                return NotFound();
            }

            patch.Patch(supplier);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(supplier);
        }

        // DELETE odata/Supplier(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Supplier supplier = await ((DbSet<Supplier>)db.Suppliers).FindAsync(key);
            if (supplier == null)
            {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Supplier(5)/Products
        [Queryable]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Suppliers.Where(m => m.SupplierId == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int key)
        {
            return db.Suppliers.Count(e => e.SupplierId == key) > 0;
        }
    }
}
