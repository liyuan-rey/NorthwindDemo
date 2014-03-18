// ProductController.cs

namespace Northwind.WebApi2Services.Areas.Warehouse.Controllers.Api
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Dto;
    using EF6Models;
    using Filters;
    using Models;

    [UnhandledExceptionFilter]
    public class ProductController : ApiController
    {
        // GET api/<controller>
        public async Task<IEnumerable<ProductListItemDto>> Get()
        {
            using (var ctx = new NorthwindDbContext())
            {
                IQueryable<ProductListItemDto> items = ctx.Products
                    .Include(p => p.Category)
                    .Select(ModelMapper.Product2ProductListItemDto);

                return await items.ToListAsync();
            }
        }

        // GET api/<controller>/5
        public async Task<ProductListItemDto> Get(int id)
        {
            using (var ctx = new NorthwindDbContext())
            {
                IQueryable<ProductListItemDto> items = ctx.Products
                    .Select(ModelMapper.Product2ProductListItemDto);
                    
                ProductListItemDto result = await items.FirstOrDefaultAsync(p => p.ProductId == id);

                if (result == null)
                {
                    throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return result;
            }
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}