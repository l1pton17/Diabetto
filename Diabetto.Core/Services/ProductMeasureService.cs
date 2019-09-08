using Diabetto.Core.Models;
using SQLite;

namespace Diabetto.Core.Services
{
    public interface IProductMeasureService
    {

    }

    internal sealed class ProductMeasureService : SQLiteConnection, IProductMeasureService
    {
        public ProductMeasureService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<ProductMeasure>();
        }
    }
}