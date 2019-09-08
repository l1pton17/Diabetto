using SQLite;

namespace Diabetto.Core.Models
{
    [Table("product_categories")]
    public sealed class ProductCategory
    {
        [AutoIncrement]
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}