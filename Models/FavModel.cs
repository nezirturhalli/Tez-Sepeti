using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TezSepeti.Models
{
    public class FavModel
    {
        public int ID { get; set; }
        public int projectID { get; set; }
        public int userID { get; set; }        
        public DateTime createdDate { get; set; }

    }
}
