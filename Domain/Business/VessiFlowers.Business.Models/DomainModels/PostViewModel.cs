namespace VessiFlowers.Business.Models.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using VessiFlowers.Business.Models.AutoMapperConfigurations;
    using VessiFlowers.Data.Entities;

    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително")]
        [Display(Name = "Съдържание")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Публикувай")]
        public bool IsActive { get; set; }

        public string Author { get; set; }

        public virtual ICollection<MediaViewModel> Images { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(m => m.User.FullName))
                .ForMember(m => m.Images, opt => opt.MapFrom(m => m.Medias.OrderBy(o => o.IsPalette)));
        }
    }
}