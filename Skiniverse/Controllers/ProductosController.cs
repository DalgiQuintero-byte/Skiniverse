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
    }
}