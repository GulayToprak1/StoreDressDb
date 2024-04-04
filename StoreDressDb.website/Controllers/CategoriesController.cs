using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using StoreDressDb.website.Models;

namespace StoreDressDb.website.Controllers
{
    public class CategoriesController : Controller
    {
        SqlConnection connection = new SqlConnection();

        public CategoriesController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("Dress");
        }
        public IActionResult Index(int id)
        {
            ViewBag.Categories = GetCategories();

            CategoryViewModel viewModel = new CategoryViewModel
            {
                Category = GetCategory(id),
                Dresses = GetDresses(id)
            };


            return View(viewModel);
        }

        private List<Category> GetCategories()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories order by Name", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Category> categories = new List<Category>();

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(new Category
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString()
                });
            }

            return categories;
        }

        private Category GetCategory(int categoryId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories where Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", categoryId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Category category = new Category
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Name = dt.Rows[0]["Name"].ToString()
            };

            return category;
        }

        private List<Dress> GetDresses(int categoryId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Dresses where CategoryId=@catId", connection);
            da.SelectCommand.Parameters.AddWithValue("catId", categoryId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Dress> dresses = new List<Dress>();

            foreach (DataRow row in dt.Rows)
            {
                Dress dress = new Dress
                {
                    Id = Convert.ToInt32(row["Id"]),
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    Description = Convert.ToString(row["Description"]),
                    ImageURL = row["ImageURL"].ToString(),
                    Name = row["Name"].ToString(),
                    Price= Convert.ToDouble(row["Price"]),
                    DesignerId = Convert.ToInt32(row["DesignerId"]),
                    IsSelected = Convert.ToBoolean(row["IsSelected"]),
                    IsBestSeller = Convert.ToBoolean(row["IsBestSeller"])
                };

                dresses.Add(dress);
            }

            return dresses;
        }
    }
}
