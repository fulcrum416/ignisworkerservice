using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgnisWorkerService.Data.DbModel
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Type { get; set; } // Temperature, Pressure, Flow Rate, Totalizer, Speed, Level
        public string? Unit { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal? TagValue { get; set; }
        public bool Snapshot {  get; set; }
        public DateTime InDate { get; set; }
        public DateTime LogDate { get; set; }


        
    }
}
