using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Diabetto.Core.Models
{
    [Table("products")]
    public sealed class Product
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("glycemic_index")]
        public int? GlycemicIndex { get; set; }

        [Indexed]
        [Column("product_category_id")]
        [ForeignKey(typeof(ProductCategory))]
        public int? ProductCategoryId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public ProductCategory ProductCategory { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ProductMeasureUnit> Units { get; set; }
    }
}