namespace VessiFlowers.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vf.Slugs")]
    public partial class Slug
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; }

        [Required]
        [StringLength(64)]
        public string Controller { get; set; }

        [StringLength(64)]
        public string Action { get; set; }

        public int? Param { get; set; }
    }
}
