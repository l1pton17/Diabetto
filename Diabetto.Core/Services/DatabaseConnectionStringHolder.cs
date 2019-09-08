using System;
using System.IO;

namespace Diabetto.Core.Services
{
    internal interface IDatabaseConnectionStringHolder
    {
        string DatabaseFilePath { get; }
    }

    internal sealed class DatabaseConnectionStringHolder : IDatabaseConnectionStringHolder
    {
        /// <inheritdoc />
        public string DatabaseFilePath { get; }

        public DatabaseConnectionStringHolder()
        {
            var fileName = "DiabettoDB.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);

            DatabaseFilePath = path;
        }
    }
}