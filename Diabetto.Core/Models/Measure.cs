using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Diabetto.Core.Models
{
    [Table("measures")]
    public sealed class Measure
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        [Indexed]
        public DateTime Date { get; set; }

        [Column("bread_units")]
        public float BreadUnits { get; set; }

        [ForeignKey(typeof(Tag))]
        [Column("tag_id")]
        public int? TagId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public Tag Tag { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ProductMeasure> Products { get; set; } = new List<ProductMeasure>();

        [Column("level")]
        public int? Level { get; set; }

        [Column("long_insulin")]
        public int LongInsulin { get; set; }

        [Column("short_insulin")]
        public int ShortInsulin { get; set; }
    }
}