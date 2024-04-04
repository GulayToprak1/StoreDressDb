using Microsoft.AspNetCore.Mvc;
using StoreDressDb;
using System.Data.SqlClient;
using System.Data;

namespace StoreDress.panel.Controllers
{
    public class DesignersController : Controller
    {
        SqlConnection connection = new SqlConnection();

        public DesignersController(IConfiguration configuration)
        {
            connection.ConnectionString = configuration.GetConnectionString("Dress");
        }
        public IActionResult Index()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Designers order by NameSurname", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Designer> designers = new List<Designer>();

            foreach (DataRow row in dt.Rows)
            {
                Designer designer = new Designer
                {
                    Id = Convert.ToInt32(row["Id"]),
                    NameSurname = row["NameSurname"].ToString()
                };

                designers.Add(designer);
            }
            return View(designers);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Designer model)
        {
            if (ModelState.IsValid)
            {
                SqlCommand cmd = new SqlCommand("insert into dbo.Designers values (@name)", connection);
                cmd.Parameters.AddWithValue("name", model.NameSurname);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
        public IActionResult Edit(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Designers where Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
                return RedirectToAction(nameof(Index));
            else
            {
                Designer designer = new Designer
                {
                    Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                    NameSurname = dt.Rows[0]["NameSurname"].ToString()
                };

                return View(designer);
            }
        }


        [HttpPost]
        public IActionResult Edit(Designer model)
        {
            if (ModelState.IsValid)
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Designers where Id=@id", connection);
                da.SelectCommand.Parameters.AddWithValue("id", model.Id);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, model.Id + " numaralı kayıt bulunamadı.");
                    return View(model);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("update dbo.Designers set NameSurname=@name where Id=@id", connection);
                    cmd.Parameters.AddWithValue("id", model.Id);
                    cmd.Parameters.AddWithValue("name", model.NameSurname);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    return RedirectToAction(nameof(Index));
                }
            }
            else
                return View(model);
        }
        public IActionResult Delete(int id)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Designers where Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
                return RedirectToAction(nameof(Index));
            else
            {
                Designer designer = new Designer
                {
                    Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                    NameSurname = dt.Rows[0]["NameSurname"].ToString()
                };

                return View(designer);
            }
        }

        [HttpPost]
        public IActionResult Delete(Designer model)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from dbo.Designers where Id=@id", connection);
            da.SelectCommand.Parameters.AddWithValue("id", model.Id);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                ModelState.AddModelError(string.Empty, model.Id + " numaralı kayıt bulunamadı.");
                return View(model);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("delete from dbo.Designers where Id=@id", connection);
                cmd.Parameters.AddWithValue("id", model.Id);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction(nameof(Index));
            }

        }
    }
}
