using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Diabetto.Core.Models
{
    [Table("product_measure_units")]
    public sealed class ProductMeasureUnit
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Indexed]
        [ForeignKey(typeof(Product))]
        [Column("product_id")]
        public int ProductId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Product Product { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("shortName")]
        public string ShortName { get; set; }

        [Column("carbohydrates")]
        public float Carbohydrates { get; set; }

        [Column("fats")]
        public float? Fats { get; set; }

        [Column("proteins")]
        public float? Proteins { get; set; }

        [Column("calories")]
        public float? Calories { get; set; }

        public ProductMeasureUnit()
        {
        }

        public ProductMeasureUnit(ProductMeasureUnit source)
        {
            Id = source.Id;
            ProductId = source.ProductId;
            Product = source.Product;
            Name = source.Name;
            ShortName = source.ShortName;
            Carbohydrates = source.Carbohydrates;
            Fats = source.Fats;
            Proteins = source.Proteins;
            Calories = source.Calories;
        }

        public override string ToString()
        {
            return $"Id: [{Id}] Product id: [{ProductId}] Name: [{Name}] Carbs: [{Carbohydrates}]";
        }
    }
}