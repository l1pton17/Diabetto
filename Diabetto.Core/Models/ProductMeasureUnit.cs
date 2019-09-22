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

        [Column("name")]
        public string Name { get; set; }

        [Column("shortName")]
        public string ShortName { get; set; }

        [Column("carbohydrates")]
        public float Carbohydrates { get; set; }

        [Column("is_grams")]
        public bool IsGrams { get; set; }

        [Column("fats")]
        public float? Fats { get; set; }

        [Column("proteins")]
        public float? Proteins { get; set; }

        [Column("calories")]
        public float? Calories { get; set; }

        public override string ToString()
        {
            return $"Id: [{Id}] Product id: [{ProductId}] Name: [{Name}] Carbs: [{Carbohydrates}]";
        }
    }
}