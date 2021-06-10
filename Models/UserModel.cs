using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TezSepeti.Models
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int userTypeID { get; set; }

        public string phone{ get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }

        public string? imageName { get; set; }
        
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
        [NotMapped]
        public IFormFile? photo { get; set; }

    }
}
