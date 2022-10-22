using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels
{
    public class VwAddress
    {
        public long AddressId { get; set; }

        [Required]
        [StringLength(500)]
        public string AddressDescription { get; set; }

        [StringLength(15)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        public long DistrictId { get; set; }
        public string? District { get; set; }

        public long? TehsilId { get; set; }
        public string? Tehsil { get; set; }

        [Required]
        public long AddressTypeId { get; set; }
        public string? AddressType { get; set; }

        [JsonIgnore]
        public long? PersonId { get; set; }

        [JsonIgnore]
        public long? BusinessId { get; set; }
    }
}
