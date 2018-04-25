namespace VessiFlowers.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vf.GalleryType")]
    public partial class GalleryType
    {
        public GalleryType()
        {
            this.Galleries = new HashSet<Gallery>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Keyword { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public virtual ICollection<Gallery> Galleries { get; set; }
    }
}
