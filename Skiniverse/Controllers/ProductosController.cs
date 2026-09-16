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

        // GET: /Productos/Catalogo?categoria=Limpiadores
        public IActionResult Catalogo(string? categoria)
        {
            List<Producto> listaProductos = new List<Producto>();
            string conexionString = _configuration.GetConnectionString("ConexionSQL");

            using (SqlConnection conexion = new SqlConnection(conexionString))
            {
                // Si viene el parámetro categoria, añadimos el filtro WHERE en la consulta SQL
                string query = "SELECT IdProducto, NombreProducto, Categoria, TipoPielRecomendado, IngredientesActivos, PrecioRegular, StockActual, ImagenUrl FROM Productos";

                if (!string.IsNullOrEmpty(categoria))
                {
                    query += " WHERE Categoria = @Categoria";
                }

                SqlCommand cmd = new SqlCommand(query, conexion);

                if (!string.IsNullOrEmpty(categoria))
                {
                    cmd.Parameters.AddWithValue("@Categoria", categoria);
                }

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

            ViewData["CategoriaSeleccionada"] = categoria;
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