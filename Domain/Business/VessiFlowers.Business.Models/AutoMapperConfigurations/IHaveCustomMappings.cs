namespace VessiFlowers.Business.Models.AutoMapperConfigurations
{
    using AutoMapper;

    public interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}