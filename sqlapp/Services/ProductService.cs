using Microsoft.FeatureManagement;
using sqlapp.Models;
using System.Data.SqlClient;
using System.Text.Json;

namespace sqlapp.Services
{

    // This service will interact with our Product data in the SQL database
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _feature;

        public ProductService(IConfiguration configuration, IFeatureManager feature)
        {
            _configuration = configuration;
            _feature = feature;
        }
        private SqlConnection GetConnection()
        {
            var x = _configuration["SQLConnection"];
            return new SqlConnection(x);
        }

        public async Task<bool> IsBeta()
        {
            if (await _feature.IsEnabledAsync("beta"))
            {
                return true;
            }
            else return false;
        }

        public async Task<List<Product>> GetProductList()
        {
            string funct = "https://planet-appfunction.azurewebsites.net/api/GetProducts?code=03z9ng-OYQ9UcQYamsuohnJ4jU57zFpV95fSM_hZ3VFDAzFul5BRfw==";
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(funct);
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        public List<Product> GetProducts()
        {            
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT ProductID,ProductName,Quantity from Products";
            SqlConnection _connection = GetConnection();

            _connection.Open();

            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        ProductID = _reader.GetInt32(0),
                        ProductName = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };

                    _product_lst.Add(_product);
                }
            }
            _connection.Close();
            return _product_lst;
        }

    }
}


