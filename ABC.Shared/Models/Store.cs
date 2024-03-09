using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ABC.Shared.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Store Name")]
        public string storeName { get; set; }

        [Required]

        [DisplayName("Contact Number")]
        public long storeContactNumber { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string storeEmail { get; set; }

        [Required]

        [DisplayName("Status")]
        public string storeStatus { get; set; }

        [DisplayName("Lot & Blk no.")]
        public string storeLotBlk { get; set; }

        [Required]
        [DisplayName("Street/Subdivision")]
        public string storeStreetSubdv { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Barangay")]
        public string storeBarangay { get; set; }


        [DisplayName("City")]
        public string storeCity { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Province")]
        public string storeProvince { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        public int storeZipCode { get; set; }
    }
}
