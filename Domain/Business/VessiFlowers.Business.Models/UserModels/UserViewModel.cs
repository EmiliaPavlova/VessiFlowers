namespace VessiFlowers.Business.Models.UserModels
{
    using System.ComponentModel.DataAnnotations;
    using VessiFlowers.Business.Models.AutoMapperConfigurations;
    using VessiFlowers.Data.Entities;

    public class UserViewModel : IMapFrom<User>
    {
        [Required]
        public string Id { get; set; }

        public string Email { get; set; }

        [Display(Name = "Абонамент за бюлетин")]
        public bool IsSubscribed { get; set; }
    }
}
