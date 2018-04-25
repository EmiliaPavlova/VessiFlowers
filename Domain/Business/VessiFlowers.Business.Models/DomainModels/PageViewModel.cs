namespace VessiFlowers.Business.Models.DomainModels
{
    using System.ComponentModel.DataAnnotations;
    using VessiFlowers.Business.Models.AutoMapperConfigurations;
    using VessiFlowers.Data.Entities;

    public class PageViewModel : IMapFrom<Page>
    {
        public int Id { get; set; }

        public string Keyword { get; set; }

        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Съдържание")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}