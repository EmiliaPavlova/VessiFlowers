namespace VessiFlowers.Business.Models.DomainModels
{
    using System.Collections.Generic;
    using AutoMapper;
    using VessiFlowers.Business.Models.AutoMapperConfigurations;
    using VessiFlowers.Data.Entities;

    public class GalleryTypeViewModel : IMapFrom<GalleryType>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Keyword { get; set; }

        public string Name { get; set; }

        public ICollection<GalleryViewModel> Galleries { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<GalleryType, GalleryTypeViewModel>()
                .ForMember(m => m.Galleries, opt => opt.MapFrom(m => m.Galleries));
        }
    }
}