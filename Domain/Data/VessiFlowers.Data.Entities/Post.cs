namespace VessiFlowers.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vf.Posts")]
    public partial class Post
    {
        public Post()
        {
            this.Medias = new HashSet<Media>();
        }

        public int Id { get; set; }

        public DateTime Created { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsActive { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Media> Medias { get; set; }
    }
}
