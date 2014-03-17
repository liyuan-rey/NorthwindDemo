// CategoryController.cs

namespace Northwind.WebApi2Services.Areas.Warehouse.Controllers
{
    using System;
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
    public class CategoryController : ApiController
    {
        // GET api/<controller>
        public async Task<IEnumerable<CategoryListItemDto>> Get()
        {
            using (var ctx = new NorthwindDbContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                List<CategoryListItemDto> result =
                    await ctx.Categories
                        .Select(ModelMapper.Category2CategoryListItemDto)
                        .ToListAsync();

                return result;
            }
        }

        // GET api/<controller>/5
        public async Task<CategoryListItemDto> Get(int id)
        {
            using (var ctx = new NorthwindDbContext())
            {
                CategoryListItemDto result = await ctx.Categories.AsNoTracking()
                    .Select(ModelMapper.Category2CategoryListItemDto)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);

                if (result == null)
                {
                    throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return result;
            }
        }

        // POST api/<controller>
        public async Task Post([FromBody] CategoryNewDto value)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            using (var ctx = new NorthwindDbContext())
            {
                Func<CategoryNewDto, Category> func = ModelMapper.CategoryNewDto2Category.Compile();
                Category categoty = func(value);

                ctx.Categories.Add(categoty);

                await ctx.SaveChangesAsync();
            }
        }

        // PUT api/<controller>/5
        public async Task Put(int id, [FromBody] CategoryUpdateDto value)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            using (var ctx = new NorthwindDbContext())
            {
                Func<CategoryUpdateDto, Category> func = ModelMapper.CategoryUpdateDto2Category.Compile();
                Category categoty = func(value);

                ctx.Categories.Attach(categoty);

                value.UpdatedProperties.ForEach(prop =>
                    ctx.Entry(categoty).Property(prop).IsModified = true);

                await ctx.SaveChangesAsync();
            }
        }

        // DELETE api/<controller>/5
        public async Task Delete(int id)
        {
            using (var ctx = new NorthwindDbContext())
            {
                var category = new Category
                {
                    CategoryId = id
                };

                ctx.Categories.Attach(category);
                ctx.Categories.Remove(category);

                await ctx.SaveChangesAsync();
            }
        }
    }
}

/*
public User UpdateUser(User user, IEnumerable<Expression<Func<User, object>>> properties)
        {
            if (string.IsNullOrEmpty(user.UserId))
            {
                throw new InvalidOperationException("user does not exist");
            }
            else
            {
                db.Users.Attach(user);
                foreach (var selector in properties)
                {
                    string propertyName = Helpers.PropertyToString(selector.Body);
                    db.Entry(user).Property(propertyName).IsModified = true;
                }
                db.SaveChanges();
            }
            return user;
        }
 
 */