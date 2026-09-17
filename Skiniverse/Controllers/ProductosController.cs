using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Skiniverse.Models;
using System.Data;

namespace Skiniverse.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: /Producto/Catalogo?categoria=Limpiadores
        public IActionResult Catalogo(string? categoria)
        {
            List<Producto> listaProducto = new List<Producto>();
            string? conexionString = _configuration.GetConnectionString("ConexionSQL");
            if (string.IsNullOrEmpty(conexionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'ConexionSQL' no está configurada.");
            }

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Productos";

                if (!string.IsNullOrEmpty(categoria))
                {
                    query += " WHERE Categoria = @Categoria";
                }

                conexion.Open();
                SqlCommand cmd = new SqlCommand(query, conexion);

                if (!string.IsNullOrEmpty(categoria))
                {
                    cmd.Parameters.AddWithValue("@Categoria", categoria);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaProducto.Add(MapearProducto(reader));
                    }
                }
            }

            ViewData["CategoriaSeleccionada"] = categoria;
            return View(listaProducto);
        }

        // GET: /Producto/Admin (Vista de gestión para administrador)
        public IActionResult Admin()
        {
            List<Producto> listaProducto = new List<Producto>();
            string? conexionString = _configuration.GetConnectionString("ConexionSQL");
            if (string.IsNullOrEmpty(conexionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'ConexionSQL' no está configurada.");
            }

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Productos";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaProducto.Add(MapearProducto(reader));
                    }
                }
            }

            return View(listaProducto);
        }

        // GET: /Producto/Detalles/5
        public IActionResult Detalles(int id)
        {
            Producto? Producto = null;
            string? conexionString = _configuration.GetConnectionString("ConexionSQL");
            if (string.IsNullOrEmpty(conexionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'ConexionSQL' no está configurada.");
            }

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Productos WHERE IdProducto = @Id";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Producto = MapearProducto(reader);
                    }
                }
            }

            if (Producto == null)
            {
                return NotFound();
            }

            return View(Producto);
        }

        // GET: /Producto/TestPiel (Muestra la encuesta)
        public IActionResult TestPiel()
        {
            return View();
        }

        // POST: /Producto/ResultadoTest
        [HttpPost]
        public IActionResult ResultadoTest(EncuestaModel modelo)
        {
            return View();
        }

        // GET: /Producto/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Producto/Crear (Guarda el Producto en SQL Server)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Producto Producto)
        {
            if (ModelState.IsValid)
            {
                string? conexionString = _configuration.GetConnectionString("ConexionSQL");
                if (string.IsNullOrEmpty(conexionString))
                {
                    throw new InvalidOperationException("La cadena de conexión 'ConexionSQL' no está configurada.");
                }

                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    string query = @"INSERT INTO Productos
                                    (NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl) 
                                    VALUES 
                                    (@NombreProducto, @Categoria, @TipoPielRecomendado, @IngredientesActivos, @PrecioRegular, @StockActual, @ImagenUrl)";

                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@NombreProducto", Producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Categoria", Producto.Categoria);
                    cmd.Parameters.AddWithValue("@TipoPielRecomendado", Producto.TipoPielRecomendado);
                    cmd.Parameters.AddWithValue("@IngredientesActivos", Producto.IngredientesActivos);
                    cmd.Parameters.AddWithValue("@PrecioRegular", Producto.PrecioRegular);
                    cmd.Parameters.AddWithValue("@StockActual", Producto.StockActual);
                    cmd.Parameters.AddWithValue("@ImagenUrl", Producto.ImagenUrl as object ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Admin));
            }

            return View(Producto);
        }

        // GET: /Producto/Editar/5
        public IActionResult Editar(int id)
        {
            Producto? Producto = null;
            string? conexionString = _configuration.GetConnectionString("ConexionSQL");
            if (string.IsNullOrEmpty(conexionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'ConexionSQL' no está configurada.");
            }

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Productos WHERE IdProducto = @Id";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Producto = MapearProducto(reader);
                    }
                }
            }

            if (Producto == null) return NotFound();
            return View(Producto);
        }

        // POST: /Producto/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Producto Productos)
        {
            if (ModelState.IsValid)
            {
                string? conexionString = _configuration.GetConnectionString("ConexionSQL");
                if (string.IsNullOrEmpty(conexionString))
                {
                    throw new InvalidOperationException("La cadena de conexión 'ConexionSQL' no está configurada.");
                }

                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    string query = @"UPDATE Productos 
                                    SET NombreProducto = @NombreProducto, 
                                        Categoria = @Categoria, 
                                        TipoPielRecomendado = @TipoPielRecomendado, 
                                        IngredientesActivos = @IngredientesActivos, 
                                        PrecioRegular = @PrecioRegular, 
                                        StockActual = @StockActual, 
                                        ImagenUrl = @ImagenUrl 
                                    WHERE IdProducto = @IdProducto";

                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@IdProducto", Productos.IdProducto);
                    cmd.Parameters.AddWithValue("@NombreProducto", Productos.NombreProducto);
                    cmd.Parameters.AddWithValue("@Categoria", Productos.Categoria);
                    cmd.Parameters.AddWithValue("@TipoPielRecomendado", Productos.TipoPielRecomendado);
                    cmd.Parameters.AddWithValue("@IngredientesActivos", Productos.IngredientesActivos);
                    cmd.Parameters.AddWithValue("@PrecioRegular", Productos.PrecioRegular);
                    cmd.Parameters.AddWithValue("@StockActual", Productos.StockActual);
                    cmd.Parameters.AddWithValue("@ImagenUrl", Productos.ImagenUrl as object ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Admin));
            }

            return View(Productos);
        }

        // GET: /Producto/Eliminar/5
        public IActionResult Eliminar(int id)
        {
            string? conexionString = _configuration.GetConnectionString("ConexionSQL");
            if (string.IsNullOrEmpty(conexionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'ConexionSQL' no está configurada.");
            }

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "DELETE FROM Productos WHERE IdProducto = @Id";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction(nameof(Admin));
        }

        private Producto MapearProducto(SqlDataReader reader)
        {
            return new Producto
            {
                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                NombreProducto = reader["NombreProducto"].ToString() ?? "",
                Categoria = reader["Categoria"].ToString() ?? "",
                TipoPielRecomendado = reader["TipoPielRecomendado"].ToString() ?? "",
                IngredientesActivos = reader["IngredientesActivos"].ToString() ?? "",
                PrecioRegular = Convert.ToDecimal(reader["PrecioRegular"]),
                StockActual = Convert.ToInt32(reader["StockActual"]),
                ImagenUrl = reader["ImagenUrl"] != DBNull.Value ? reader["ImagenUrl"].ToString() : ""
            };

        }
        [HttpPost]
        [Authorize] // Si no ha iniciado sesión, ASP.NET Core lo enviará a /Account/Login automáticamente
        public IActionResult AgregarAlCarrito(int id)
        {
            // Lógica para guardar el producto en el carrito del usuario
            return RedirectToAction("Index", "Home");
        }
    }
}