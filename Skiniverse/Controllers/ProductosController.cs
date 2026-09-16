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

        // GET: /Productos/Detalles/5
        public IActionResult Detalles(int id)
        {
            Producto producto = null;
            string conexionString = _configuration.GetConnectionString("ConexionSQL");

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Productos WHERE IdProducto = @Id";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                conexion.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto = new Producto
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
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Productos";
                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaProductos.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            NombreProducto = reader["NombreProducto"].ToString() ?? "",
                            Categoria = reader["Categoria"].ToString() ?? "",
                            TipoPielRecomendado = reader["TipoPielRecomendado"].ToString() ?? "",
                            IngredientesActivos = reader["IngredientesActivos"].ToString() ?? "",
                            PrecioRegular = Convert.ToDecimal(reader["PrecioRegular"]),
                            StockActual = Convert.ToInt32(reader["StockActual"]),
                            ImagenUrl = reader["ImagenUrl"] != DBNull.Value ? reader["ImagenUrl"].ToString() : ""
                        });
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
                    string query = @"INSERT INTO Productos 
                                    (NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl) 
                                    VALUES 
                                    (@NombreProducto, @Categoria, @TipoPielRecomendado, @IngredientesActivos, @PrecioRegular, @StockActual, @ImagenUrl)";

                    SqlCommand cmd = new SqlCommand(query, conexion);

                    // Parámetros seguros para prevenir inyección SQL
                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    cmd.Parameters.AddWithValue("@TipoPielRecomendado", producto.TipoPielRecomendado);
                    cmd.Parameters.AddWithValue("@IngredientesActivos", producto.IngredientesActivos);
                    cmd.Parameters.AddWithValue("@PrecioRegular", producto.PrecioRegular);
                    cmd.Parameters.AddWithValue("@StockActual", producto.StockActual);
                    cmd.Parameters.AddWithValue("@ImagenUrl", (object)producto.ImagenUrl ?? DBNull.Value);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Catalogo));
            }

            return View(producto);
        }
    }
}