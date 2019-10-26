using System.IO;
using Diabetto.Core.Services;
using Foundation;

namespace Diabetto.iOS.Intents.Shared
{
    public sealed class SharedDatabaseConnectionStringHolder : IDatabaseConnectionStringHolder
    {
        /// <inheritdoc />
        public string DatabaseFilePath { get; }

        public SharedDatabaseConnectionStringHolder()
        {
            var fileName = "DiabettoDB.db3";
            var libraryPath = NSFileManager.DefaultManager.GetContainerUrl("group.diabetto");
            var path = Path.Combine(libraryPath.Path, fileName);

            DatabaseFilePath = path;
        }
    }
}