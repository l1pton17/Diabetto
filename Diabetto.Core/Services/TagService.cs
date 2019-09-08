using Diabetto.Core.Models;
using SQLite;

namespace Diabetto.Core.Services
{
    internal interface ITagService
    {

    }

    internal sealed class TagService : SQLiteConnection, ITagService
    {
        public TagService(IDatabaseConnectionStringHolder connectionStringHolder)
            : base(connectionStringHolder.DatabaseFilePath)
        {
            CreateTable<Tag>();
        }
    }
}