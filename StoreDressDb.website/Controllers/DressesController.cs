using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Data.SqlClient;
using System.Data;
using StoreDressDb.website.Models;

namespace StoreDressDb.website.Controllers
{
    public class DressesController : Controller
    {
        SqlConnection connection = new SqlConnection();

        public DressesController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("Dress");
        }

        public IActionResult Index(int id)
        {
            ViewBag.Categories = GetCategories();

            DressViewModel viewModel = new DressViewModel();
            viewModel.Dress = GetDress(id);
            viewModel.Dresses = GetDresses();
            viewModel.Category = GetCategory(viewModel.Dress.CategoryId);
            viewModel.Designer = GetDesigner(viewModel.Dress.DesignerId);

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

        private List<Dress> GetDresses()
        {
            SqlDataAdapter da = new SqlDataAdapter("select top 4 * from dbo.Dresses where IsSelected=1 order by Name", connection);
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
                    Price = Convert.ToDouble(row["Price"]),
                    DesignerId = Convert.ToInt32(row["DesignerId"]),
                    IsSelected = Convert.ToBoolean(row["IsSelected"]),
                    IsBestSeller = Convert.ToBoolean(row["IsBestSeller"])
                };

                dresses.Add(dress);
            }

            return dresses;
        }

        private Dress GetDress(int dressId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Dresses where Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", dressId);
            DataTable dt = new DataTable();
            da.Fill(dt);


            Dress dress = new Dress
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]),
                Description = Convert.ToString(dt.Rows[0]["Description"]),
                ImageURL = dt.Rows[0]["ImageURL"].ToString(),
                Name = dt.Rows[0]["Name"].ToString(),
                Price = Convert.ToDouble(dt.Rows[0]["Price"]),
                DesignerId = Convert.ToInt32(dt.Rows[0]["DesignerId"]),
                IsSelected = Convert.ToBoolean(dt.Rows[0]["IsSelected"]),
                IsBestSeller = Convert.ToBoolean(dt.Rows[0]["IsBestSeller"])
            };
            return dress;
        }

        private Designer GetDesigner(int designerId)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Designers where Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", designerId);
            DataTable dt = new DataTable();
            da.Fill(dt);

            Designer category = new Designer
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                NameSurname = dt.Rows[0]["NameSurname"].ToString()
            };

            return category;
        }
    }
}
