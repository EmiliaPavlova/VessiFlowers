namespace VessiFlowers.Business.Models.DomainModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using VessiFlowers.Business.Models.AutoMapperConfigurations;
    using VessiFlowers.Data.Entities;

    public class GalleryViewModel : IMapFrom<Gallery>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Display(Name = "Галерия")]
        public int GalleryTypeId { get; set; }

        public string GalleryTypeName { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително")]
        [Display(Name = "Позиция")]
        [Range(1, 50, ErrorMessage = "Позицията не е валидна")]
        public int Position { get; set; }

        [Display(Name = "Срок на изработка")]
        public string Duration { get; set; }

        [Display(Name = "Цена")]
        public string Price { get; set; }

        public virtual ICollection<MediaViewModel> Images { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Gallery, GalleryViewModel>()
             .ForMember(m => m.GalleryTypeName, opt => opt.MapFrom(m => m.GalleryType.Name))
             .ForMember(m => m.Images, opt => opt.MapFrom(m => m.Medias.OrderByDescending(o => o.IsPalette)));
        }
    }
}