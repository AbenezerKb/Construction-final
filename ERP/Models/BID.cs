using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class BID
    {

        public string  BIDID { get; set; }
                        
        public DateTime initailDate { get; set; }        
        public DateTime finalDate { get; set; }

        public string WorkDescription { get; set; }

        public string ConBID { get; set; }
        
        public double EstimatedBID { get; set; }
        public double ActualCost { get; set; }    

        public string PenalityDescription { get; set; }
        public string Remark { get; set; }
        public string ProjectId { get; set; }

        public string fileName { get; set; }
    }
}

