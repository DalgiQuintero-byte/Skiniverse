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

        // GET: /Productos/Admin (Vista de gestión para administrador)
        public IActionResult Admin()
        {
            List<Producto> listaProductos = new List<Producto>();
            string conexionString = _configuration.GetConnectionString("ConexionSQL");

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Producto";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaProductos.Add(MapearProducto(reader));
                    }
                }
            }

            return View(listaProductos);
        }

        // GET: /Productos/Detalles/5
        public IActionResult Detalles(int id)
        {
            Producto producto = null;
            string conexionString = _configuration.GetConnectionString("ConexionSQL");

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Producto WHERE IdProducto = @Id";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto = MapearProducto(reader);
                    }
                }
            }

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: /Productos/Catalogo
        public IActionResult Catalogo()
        {
            List<Producto> listaProductos = new List<Producto>();
            string conexionString = _configuration.GetConnectionString("ConexionSQL");

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Producto";

                conexion.Open();
                SqlCommand cmd = new SqlCommand(query, conexion);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaProductos.Add(MapearProducto(reader));
                    }
                }
            }

            return View(listaProductos);
        }

        // GET: /Productos/TestPiel (Muestra la encuesta)
        public IActionResult TestPiel()
        {
            return View();
        }

        // POST: /Productos/ResultadoTest (Recibe el test y muestra los dos botones de elección)
        [HttpPost]
        public IActionResult ResultadoTest(EncuestaModel modelo)
        {
            return View();
        }

        // GET: /Productos/Crear (Muestra el formulario)
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Productos/Crear (Guarda el producto en SQL Server)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                string conexionString = _configuration.GetConnectionString("ConexionSQL");

                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    string query = @"INSERT INTO Producto 
                                    (NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl) 
                                    VALUES 
                                    (@NombreProducto, @Categoria, @TipoPielRecomendado, @IngredientesActivos, @PrecioRegular, @StockActual, @ImagenUrl)";

                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);

                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    cmd.Parameters.AddWithValue("@TipoPielRecomendado", producto.TipoPielRecomendado);
                    cmd.Parameters.AddWithValue("@IngredientesActivos", producto.IngredientesActivos);
                    cmd.Parameters.AddWithValue("@PrecioRegular", producto.PrecioRegular);
                    cmd.Parameters.AddWithValue("@StockActual", producto.StockActual);
                    cmd.Parameters.AddWithValue("@ImagenUrl", (object)producto.ImagenUrl ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Admin));
            }

            return View(producto);
        }

        // GET: /Productos/Editar/5 (Carga datos para editar)
        public IActionResult Editar(int id)
        {
            Producto producto = null;
            string conexionString = _configuration.GetConnectionString("ConexionSQL");

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Producto WHERE IdProducto = @Id";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto = MapearProducto(reader);
                    }
                }
            }

            if (producto == null) return NotFound();
            return View(producto);
        }

        // POST: /Productos/Editar/5 (Actualiza el producto en SQL Server)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Producto producto)
        {
            if (ModelState.IsValid)
            {
                string conexionString = _configuration.GetConnectionString("ConexionSQL");

                using (SqlConnection conexion = new SqlConnection(conexionString))
                {
                    string query = @"UPDATE Producto 
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

                    cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    cmd.Parameters.AddWithValue("@TipoPielRecomendado", producto.TipoPielRecomendado);
                    cmd.Parameters.AddWithValue("@IngredientesActivos", producto.IngredientesActivos);
                    cmd.Parameters.AddWithValue("@PrecioRegular", producto.PrecioRegular);
                    cmd.Parameters.AddWithValue("@StockActual", producto.StockActual);
                    cmd.Parameters.AddWithValue("@ImagenUrl", (object)producto.ImagenUrl ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Admin));
            }

            return View(producto);
        }

        // GET: /Productos/Eliminar/5 (Elimina directamente el producto)
        public IActionResult Eliminar(int id)
        {
            string conexionString = _configuration.GetConnectionString("ConexionSQL");

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "DELETE FROM Producto WHERE IdProducto = @Id";
                conexion.Open();

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction(nameof(Admin));
        }

        // Método auxiliar privado para evitar la duplicación de código de lectura de datos
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
    }
}