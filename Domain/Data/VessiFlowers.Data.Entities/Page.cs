namespace VessiFlowers.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vf.Pages")]
    public partial class Page
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Keyword { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
