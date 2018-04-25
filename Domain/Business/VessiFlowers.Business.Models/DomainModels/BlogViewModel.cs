namespace VessiFlowers.Business.Models.DomainModels
{
    using System.Collections.Generic;

    public class BlogViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }

        public Pager Pager { get; set; }
    }
}