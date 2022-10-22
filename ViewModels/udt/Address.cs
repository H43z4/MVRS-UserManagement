using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels.udt
{
    public class Address : BaseModel
    {
        [Key]
        public long AddressId { get; set; }

        [Required]
        [StringLength(500)]
        public string AddressDescription { get; set; }

        [StringLength(15)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }
        public long DistrictId { get; set; }
        public long? TehsilId { get; set; }
        public long AddressTypeId { get; set; }
        public long? PersonId { get; set; }
        public long? BusinessId { get; set; }
    }
}
