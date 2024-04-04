using Microsoft.AspNetCore.Mvc;
using StoreDressDb.website.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace StoreDressDb.website.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection connection = new SqlConnection();
        public HomeController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("Dress");
        }

        public IActionResult Index()
        {
            ViewBag.Categories = GetCategories();

            HomePageViewModel model = new HomePageViewModel();
            model.SizinIcinSectiklerimiz = GetDresses(false);
            model.CokSatanlar = GetDresses(true);

            return View(model);
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

        private List<Dress> GetDresses(bool isBestSeller)
        {
            string sqlCommand = "select top 4 * from dbo.Dresses where IsSelected=1 order by Name";
            if (isBestSeller)
                sqlCommand = "select top 4 * from dbo.Dresses where IsBestSeller=1 order by Name";

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand, connection);
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
       
    }
}