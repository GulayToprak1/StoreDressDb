using Microsoft.AspNetCore.Mvc;
using StoreDressDb;
using System.Data.SqlClient;
using System.Data;
using StoreDress.panel.Models;

namespace StoreDress.panel.Controllers
{
    public class DressesController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        SqlConnection connection = new SqlConnection();
        public DressesController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            connection.ConnectionString = configuration.GetConnectionString("Dress");
            _environment = environment;
        }
        public IActionResult Index()
        {
            SqlDataAdapter da = new SqlDataAdapter("select b.*, c.Name as CategoryName, w.NameSurname as DesignerName from dbo.Dresses as b inner join dbo.Categories as c on c.Id=b.CategoryId inner join dbo.Designers as w on w.Id=b.DesignerId order by b.Name", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<DressViewModel> dress = new List<DressViewModel>();

            foreach (DataRow row in dt.Rows)
            {
                DressViewModel dres = new DressViewModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    Description = Convert.ToString(row["Description"]),
                    ImageURL = row["ImageUrl"].ToString(),
                    Name = row["Name"].ToString(),
                    Price = Convert.ToDouble(row["Price"]),
                    DesignerId = Convert.ToInt32(row["DesignerId"]),
                    CategoryName = row["CategoryName"].ToString(),
                    DesignerName = row["DesignerName"].ToString(),
                    IsSelected = Convert.ToBoolean(row["IsSelected"]),
                    IsBestSeller = Convert.ToBoolean(row["IsBestSeller"])
                };

                dress.Add(dres);
            }
            return View(dress);
        }
        public IActionResult Create()
        {
            DressCreateModel createModel = new DressCreateModel
            {
                Dress = new Dress(),
                Categories = GetCategories(),
                Designers = GetDesigners()
            };

            return View(createModel);
        }

        [HttpPost]
        public IActionResult Create(DressCreateModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //resim.png
                string wwwRootPath = _environment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string newFileName = fileName + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + extension;
                //resim-20240208211800123.png

                string path = Path.Combine(wwwRootPath + "/uploads/dress/", newFileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                SqlCommand cmd = new SqlCommand("insert into dbo.Dresses values (@name, @categoryId, @designerId, @description,@price, @imageUrl, '', '', @isSelected, @isBestSeller)", connection);
                cmd.Parameters.AddWithValue("name", model.Dress.Name);
                cmd.Parameters.AddWithValue("categoryId", model.Dress.CategoryId);
                cmd.Parameters.AddWithValue("designerId", model.Dress.DesignerId);
                cmd.Parameters.AddWithValue("description", model.Dress.Description);
                cmd.Parameters.AddWithValue("price", model.Dress.Price);
                cmd.Parameters.AddWithValue("imageUrl", newFileName);
                cmd.Parameters.AddWithValue("isSelected", model.Dress.IsSelected);
                cmd.Parameters.AddWithValue("isBestSeller", model.Dress.IsBestSeller);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {

                DressCreateModel createModel = new DressCreateModel
                {
                    Dress = model.Dress,
                    Categories = GetCategories(),
                    Designers = GetDesigners()
                };

                return View(createModel);
            }
        }
        public List<Category> GetCategories()
        {
            List<Category> list = new List<Category>();

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Categories order by Name", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Category
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString()
                });
            }


            return list;
        }

        public List<Designer> GetDesigners()
        {
            List<Designer> list = new List<Designer>();

            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Designers order by NameSurname", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Designer
                {
                    Id = Convert.ToInt32(row["Id"]),
                    NameSurname = row["NameSurname"].ToString()
                });
            }
            return list;
        }
        public Dress GetDress(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Dresses where Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", id);
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
        public DressViewModel GetDressViewModel(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select b.*, c.Name as CategoryName, w.NameSurname as DesignerName from dbo.Dresses as b inner join dbo.Categories as c on c.Id=b.CategoryId inner join dbo.Designers as w on w.Id=b.DesignerId where b.Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", id);
            DataTable dt = new DataTable();
            da.Fill(dt);


            DressViewModel dress = new DressViewModel
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]),
                Description = Convert.ToString(dt.Rows[0]["Description"]),
                ImageURL = dt.Rows[0]["ImageURL"].ToString(),
                Name = dt.Rows[0]["Name"].ToString(),
                DesignerId = Convert.ToInt32(dt.Rows[0]["DesignerId"]),
                Price= Convert.ToDouble(dt.Rows[0]["Price"]),
                CategoryName = dt.Rows[0]["CategoryName"].ToString(),
                DesignerName = dt.Rows[0]["DesignerName"].ToString(),
                IsSelected = Convert.ToBoolean(dt.Rows[0]["IsSelected"]),
                IsBestSeller = Convert.ToBoolean(dt.Rows[0]["IsBestSeller"])
            };

            return dress;
        }
        public IActionResult Edit(int id)
        {
            ViewBag.KategoriListesi = GetCategories();
            ViewBag.TasarimciListesi = GetDesigners();

            return View(GetDress(id));
        }

        [HttpPost]
        public IActionResult Edit(Dress model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    model.ImageURL = GetDress(model.Id).ImageURL;
                }
                else
                {
                    //resim.png
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string newFileName = fileName + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + extension;
                    //resim-20240208211800123.png
                    model.ImageURL = newFileName;

                    string path = Path.Combine(wwwRootPath + "/uploads/dress/", newFileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }


                SqlCommand cmd = new SqlCommand("update dbo.Dresses set Name=@name, CategoryId=@categoryId, DesignerId=@DesignerId, Description=@description,Price=@price, ImageURL=@imageUrl, IsSelected=@isSelected, IsBestSeller=@isBestSeller where Id=@id", connection);
                cmd.Parameters.AddWithValue("id", model.Id);
                cmd.Parameters.AddWithValue("name", model.Name);
                cmd.Parameters.AddWithValue("categoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("designerId", model.DesignerId);
                cmd.Parameters.AddWithValue("description", model.Description);
                cmd.Parameters.AddWithValue("price", model.Price);
                cmd.Parameters.AddWithValue("imageUrl", model.ImageURL);
                cmd.Parameters.AddWithValue("isSelected", model.IsSelected);
                cmd.Parameters.AddWithValue("isBestSeller", model.IsBestSeller);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.KategoriListesi = GetCategories();
                ViewBag.TasarimciListesi = GetDesigners();

                return View(model);
            }
        }
        public IActionResult Delete(int id)
        {
            return View(GetDressViewModel(id));
        }

        [HttpPost]
        public IActionResult Delete(Dress dress)
        {
            DressViewModel dress1 = GetDressViewModel(dress.Id);
            if (dress1 == null)
            {
                ModelState.AddModelError(string.Empty, dress.Id + " numaralı kayıt sistemde bulunamadı.");
                return View(dress1);
            }
            else
            {
                string wwwRootPath = _environment.WebRootPath;
                string path = Path.Combine(wwwRootPath + "/uploads/dress/", dress1.ImageURL);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);


                SqlCommand cmd = new SqlCommand("delete from dbo.Dresses where Id=@id", connection);
                cmd.Parameters.AddWithValue("id", dress.Id);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
