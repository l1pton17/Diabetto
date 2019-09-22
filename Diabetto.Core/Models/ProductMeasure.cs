using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Diabetto.Core.Models
{
    [Table("product_measures")]
    public sealed class ProductMeasure
    {
        [AutoIncrement]
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Indexed]
        [ForeignKey(typeof(Measure))]
        [Column("measure_id")]
        public int MeasureId { get; set; }

        [Indexed]
        [Column("product_measure_unit_id")]
        [ForeignKey(typeof(ProductMeasureUnit))]
        public int ProductMeasureUnitId { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public ProductMeasureUnit ProductMeasureUnit { get; set; }
    }
}