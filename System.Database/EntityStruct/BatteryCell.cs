using System.Database.CustomAttributes;

namespace System.Database
{
    [Serializable]
    [Table(Name ="batterycell")]
    public class BatteryCell
    {
        //主键 INDENTITY自动增长标识
        [Id(Name = "Id", Strategy = GenerationType.INDENTITY)]
        public int? Id { get; set; }

        [Column]
        public string SN { get; set; }

        [Column]
        public double? Voltage { get; set; }

        [Column] // double? 允许int为NULL时不会报错
        public double? Resistance { get; set; }

        [Column]
        public string ResultVol { get; set; }

        [Column]
        public string ResultRes { get; set; }

        [Column]
        public string ResultTest { get; set; }

        [Column]
        public int? Updatestatus { get; set; }

        [Column(Name = "Created")]
        public DateTime? Created { get; set; }
    }
}
