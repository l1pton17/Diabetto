﻿using Diabetto.Core.Models;
using SQLite;

namespace Diabetto.Core.Services.Repositories
{
    internal interface IProductCategoryService
    {

    }

    internal sealed class ProductCategoryService : SQLiteConnection, IProductCategoryService
    {
        public ProductCategoryService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<ProductCategory>();
        }
    }
}