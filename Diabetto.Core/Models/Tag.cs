using SQLite;

namespace Diabetto.Core.Models
{
    public enum TagType
    {
        System,
        User
    }

    [Table("tags")]
    public class Tag
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Unique]
        public string Name { get; set; }

        [Column("type")]
        public TagType Type { get; set; }
    }
}