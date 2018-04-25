namespace VessiFlowers.Business.Models.DomainModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using VessiFlowers.Business.Models.AutoMapperConfigurations;
    using VessiFlowers.Data.Entities;

    public class MediaViewModel : IMapFrom<Media>
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string DataSize { get; set; }

        public int? GalleryId { get; set; }

        public int? PostId { get; set; }

        public bool IsPalette { get; set; }

        [Required]
        [Display(Name = "Снимка")]
        public HttpPostedFileBase File { get; set; }
    }
}