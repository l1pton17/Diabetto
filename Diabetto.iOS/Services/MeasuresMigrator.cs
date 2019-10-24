using System;
using System.IO;
using System.Threading.Tasks;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.iOS.Intents.Shared;

namespace Diabetto.iOS.Services
{
    public sealed class MeasuresMigrator
    {
        private sealed class OldDatabaseConnectionStringHolder : IDatabaseConnectionStringHolder
        {
            public string DatabaseFilePath { get; }

            public OldDatabaseConnectionStringHolder()
            {
                var fileName = "DiabettoDB.db3";
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var libraryPath = Path.Combine(documentsPath, "..", "Library");
                var path = Path.Combine(libraryPath, fileName);

                DatabaseFilePath = path;
            }
        }

        private readonly IHealthKitService _healthKitService;

        public MeasuresMigrator(IHealthKitService healthKitService)
        {
            _healthKitService = healthKitService ?? throw new ArgumentNullException(nameof(healthKitService));
        }

        public async Task Init()
        {
            var oldService = new MeasureService(new OldDatabaseConnectionStringHolder());
            var newService = new MeasureService(new SharedDatabaseConnectionStringHolder());

            var oldMeasures = await oldService.GetAllAsync();

            using (_healthKitService.Suppress())
            {
                foreach (var measure in oldMeasures)
                {
                    await newService.AddAsync(measure);
                }
            }
        }
    }
}