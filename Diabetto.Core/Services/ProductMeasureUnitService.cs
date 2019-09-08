using System.Collections.Generic;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using SQLite;

namespace Diabetto.Core.Services
{
    public interface IProductMeasureUnitService
    {
        Task<List<ProductMeasureUnit>> GetByProductId(int productId);
    }

    internal sealed class ProductMeasureUnitService : SQLiteConnection, IProductMeasureUnitService
    {
        private static readonly object _lockObject = new object();

        public ProductMeasureUnitService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<ProductMeasureUnit>();
        }

        /// <inheritdoc />
        public Task<List<ProductMeasureUnit>> GetByProductId(int productId)
        {
            lock (_lockObject)
            {
                var values = Table<ProductMeasureUnit>()
                    .Where(v => v.ProductId == productId)
                    .ToList();

                return Task.FromResult(values);
            }
        }
    }
}