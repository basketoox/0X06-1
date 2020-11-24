using System.Database.CustomAttributes;

namespace System.Database
{
    [Serializable]
    [Table(Name ="BatteryModel")]
    public class BatteryModel
    {
        //主键 INDENTITY自动增长标识
        [Id(Name = "id", Strategy = GenerationType.INDENTITY)]
        public int? Id { get; set; }

        [Column]
        public string SN { get; set; }

        [Column]
        public int? subSNNum { get; set; }
        //对应数据库中的名字为U_Name
        [Column]
        public string[] subSN { get; set; }
        [Column]
        public int? Updatestatus { get; set; }

        [Column]
        public DateTime? Created { get; set; }
    }
}
