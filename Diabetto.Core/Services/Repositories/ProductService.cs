using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace Diabetto.Core.Services.Repositories
{
    public interface IProductService
    {
        Task<Product> GetAsync(int id);

        Task<List<Product>> GetByNameAsync(string name);

        Task AddAsync(Product value);
    }

    internal sealed class ProductService : SQLiteConnection, IProductService
    {
        private static readonly object _lockObject = new object();

        public ProductService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<Product>();
        }

        /// <inheritdoc />
        public Task<Product> GetAsync(int id)
        {
            lock (_lockObject)
            {
                var result = this.GetWithChildren<Product>(id);

                return Task.FromResult(result);
            }
        }

        /// <inheritdoc />
        public Task<List<Product>> GetByNameAsync(string name)
        {
            lock (_lockObject)
            {
                var results = Table<Product>()
                    .Where(v => v.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return Task.FromResult(results);
            }
        }

        /// <inheritdoc />
        public Task AddAsync(Product value)
        {
            lock (_lockObject)
            {
                this.InsertWithChildren(value, recursive: false);
            }

            return Task.CompletedTask;
        }
    }
}