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
    }
}