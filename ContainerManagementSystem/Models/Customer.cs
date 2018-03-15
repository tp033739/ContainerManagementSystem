using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContainerManagementSystem.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Organization name")]
        [Required]
        public string OrganizationName { get; set; }

        public string AgentUserId { get; set; }
    }
}
