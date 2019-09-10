using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace Diabetto.Core.Services
{
    public interface IProductService
    {
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