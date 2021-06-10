using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TezSepeti.Models
{
    public class FavLikeViewModel
    {
        public int ID { get; set; }
        public int likeCount { get; set; }
        public string liked { get; set; }
        public int favCount { get; set; }
        public string faved { get; set; }
        public int viewed { get; set; }
        public string img { get; set; }
        public ProjectModel p { get; set; }

    }
}
