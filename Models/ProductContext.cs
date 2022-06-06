using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace pump.Models
{
    public class ProductContext
    {

        private string cs = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=ANf3Mgx6nseCbZpu;";

        public async Task<List<ProductModel>> GetByCompanyId(string id)
        {
            List<ProductModel> productsModel = new List<ProductModel>();

            await using var cn = new NpgsqlConnection(cs);
            await cn.OpenAsync();
            await using (var cmd = new NpgsqlCommand("SELECT id, name, price, unit, img FROM public.products WHERE company_id = '" + id + "' ORDER BY ts DESC", cn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    productsModel.Add(new ProductModel()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Price = double.Parse(reader.GetDecimal(2).ToString()),
                        Unit = reader.GetString(3),
                        Img = reader.GetString(4),
                    });
                }
            }

            return productsModel;
        }

        public async void SetProduct(ProductModel product)
        {
            await using var cn = new NpgsqlConnection(cs);
            await cn.OpenAsync();

            await using (var cmd = new NpgsqlCommand("INSERT INTO public.products (name, price, unit, img, company_id, ts) VALUES (@name, @price, @unit, @img, @company_id, now())", cn))
            {
                cmd.Parameters.AddWithValue("name", product.Name);
                cmd.Parameters.AddWithValue("price", decimal.Parse(product.Price.ToString()));
                cmd.Parameters.AddWithValue("unit", product.Unit);
                cmd.Parameters.AddWithValue("img", product.Img);
                cmd.Parameters.AddWithValue("company_id", new Guid(product.Company_Id));
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
