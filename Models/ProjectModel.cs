using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TezSepeti.Models
{
    public class ProjectModel
    {
        [Key]
        public int ID { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string mediaJson { get; set; }
        public string textJson { get; set; }
        public string fileJson { get; set; }
 

        public string tags { get; set; }
        public string supportersList { get; set; }

        public decimal totalValue { get; set; }
        public decimal currentValue { get; set; }

        public int personelCount { get; set; }
        public string time { get; set; }
        
        public int viewed { get; set; }
        public int totalminute { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public int userID { get; set; }
    }
}
