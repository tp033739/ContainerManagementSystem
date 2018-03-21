using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerManagementSystem.Models
{
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Cargo description")]
        [Required]
        public string CargoDescription { get; set; }

        [Display(Name = "Cargo weight (kg)")]
        [Required]
        public float CargoWeightInKilograms { get; set; }

        [Display(Name = "Number of containers")]
        [Required]
        public int NumberOfContainers { get; set; }

        [Display(Name = "Customer")]
        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "Shipping schedule")]
        [Required]
        public int ShippingScheduleId { get; set; }
        [Display(Name = "Shipping schedule")]
        public virtual ShippingSchedule ShippingSchedule { get; set; }

        public string AgentUserId { get; set; }
    }
}
