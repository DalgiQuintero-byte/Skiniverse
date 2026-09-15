using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Skiniverse.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _cadenaConexion;

        public AccountController(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("CadenaSQL") ?? "";
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string correo, string contrasena)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    string query = "SELECT COUNT(*) FROM Usuarios WHERE Correo = @Correo AND Contrasena = @Contrasena";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        cmd.Parameters.AddWithValue("@Contrasena", contrasena);

                        conexion.Open();
                        int resultado = (int)cmd.ExecuteScalar();

                        if (resultado > 0)
                        {
                            // ¡Login exitoso! Redirigimos al Test de Piel
                            return RedirectToAction("TestPiel", "Productos");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error de conexión con la base de datos: " + ex.Message;
                return View();
            }

            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View();
        }

        // GET: /Account/Registro
        public IActionResult Registro()
        {
            return View();
        }

        // POST: /Account/Registro
        [HttpPost]
        public IActionResult Registro(string correo, string contrasena)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    string query = "INSERT INTO Usuarios (Correo, Contrasena, FechaRegistro) VALUES (@Correo, @Contrasena, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        cmd.Parameters.AddWithValue("@Contrasena", contrasena);

                        conexion.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Exito"] = "¡Registro exitoso! Por favor inicia sesión.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "No se pudo registrar el usuario: " + ex.Message;
                return View();
            }
        }
    }
}