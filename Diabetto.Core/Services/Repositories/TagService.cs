using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using SQLite;

namespace Diabetto.Core.Services.Repositories
{
    public interface ITagService
    {
        Task<List<Tag>> GetAll();
    }

    internal sealed class TagService : SQLiteConnection, ITagService
    {
        private static readonly object _lockObject = new object();

        public TagService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<Tag>();

            if (!Table<Tag>().Any())
            {
                SaveItem(CreateSystem("пробуждение"));
                SaveItem(CreateSystem("до завтрака"));
                SaveItem(CreateSystem("после завтрака"));
                SaveItem(CreateSystem("до второго завтрака"));
                SaveItem(CreateSystem("после второго завтрака"));
                SaveItem(CreateSystem("до обеда"));
                SaveItem(CreateSystem("после обеда"));
                SaveItem(CreateSystem("до полдника"));
                SaveItem(CreateSystem("после полдника"));
                SaveItem(CreateSystem("до ужина"));
                SaveItem(CreateSystem("после ужина"));
                SaveItem(CreateSystem("до позднего ужина"));
                SaveItem(CreateSystem("после позднего ужина"));
                SaveItem(CreateSystem("перед сном"));
                SaveItem(CreateSystem("ночь"));
                SaveItem(CreateSystem("контроль"));
                SaveItem(CreateSystem("до тренировки"));
                SaveItem(CreateSystem("после тренировки"));
            }
        }

        /// <inheritdoc />
        public Task<List<Tag>> GetAll()
        {
            lock (_lockObject)
            {
                var values = Table<Tag>().ToList();

                return Task.FromResult(values);
            }
        }

        public void SaveItem(Tag item)
        {
            lock (_lockObject)
            {
                if (item.Id == 0)
                {
                    Insert(item);
                }
                else
                {
                    Update(item);
                }
            }
        }

        private static Tag CreateSystem(string name)
        {
            return new Tag
            {
                Type = TagType.System,
                Name = name
            };
        }
    }
}