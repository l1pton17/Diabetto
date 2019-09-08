using Diabetto.Core.Models;
using SQLite;

namespace Diabetto.Core.Services
{
    internal interface IProductService
    {

    }

    internal sealed class ProductService : SQLiteConnection, IProductService
    {
        public ProductService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<Product>();
        }
    }
}