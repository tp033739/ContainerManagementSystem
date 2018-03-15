using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerManagementSystem.Models
{
    public class ShippingSchedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Departure time")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DepartureTime { get; set; }

        [Display(Name = "Arrival time")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Departure location")]
        [Required]
        public string DepartureLocation { get; set; }

        [Display(Name = "Arrival location")]
        [Required]
        public string ArrivalLocation { get; set; }

        [Display(Name = "Vessel")]
        [Required]
        public int VesselId { get; set; }
        public virtual Vessel Vessel { get; set; }

        public string Description
        {
            get => $"{DepartureTime}: {DepartureLocation} -> {ArrivalLocation}";
        }
    }
}
