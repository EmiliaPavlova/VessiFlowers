namespace VessiFlowers.Business.Services
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Models.DomainModels;
    using VessiFlowers.Common.Utilities;
    using VessiFlowers.Data.Repositories;

    public class PageService : IPageService
    {
        private readonly InstanceFactory<IVessiFlowersContext> contextFactory;
        private readonly IEmailService emailService;

        public PageService(InstanceFactory<IVessiFlowersContext> contextFactory, IEmailService emailService)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }


        public async Task<PageViewModel> GetPage(string name)
        {
            using (var context = this.contextFactory.Create())
            {
                return await context.Pages.ProjectTo<PageViewModel>().FirstOrDefaultAsync(p => p.Keyword == name);
            }
        }

        public async Task UpdatePage(PageViewModel pvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var page = await context.Pages.FirstOrDefaultAsync(p => p.Id == pvm.Id);
                if (page != null)
                {
                    page.Content = pvm.Content ?? string.Empty;
                    await context.SaveAsync();
                }
            }
        }

        public async Task<PageViewModel> GetBulletinPage(string months, string baseUrl, string blogImageUrl)
        {
            int bulletinPeriodInMonths;

            if (!int.TryParse("-" + months, out bulletinPeriodInMonths))
            {
                throw new InvalidCastException(nameof(bulletinPeriodInMonths));
            }

            DateTime bulletinPeriod = DateTime.Now.AddMonths(bulletinPeriodInMonths);

            using (var context = this.contextFactory.Create())
            {
                var posts = await context.Posts
                      .Where(p => p.Created > bulletinPeriod)
                      .OrderByDescending(p => p.Created)
                      .ProjectTo<PostViewModel>()
                      .ToListAsync();

                string filledPosts = string.Empty;
                string unsubscribe = string.Empty;
                var postForm = await context.Pages.Where(p => p.Keyword == "post").Select(p => p.Content).FirstOrDefaultAsync();
                foreach (var post in posts)
                {
                    var imgUrl = baseUrl + blogImageUrl;
                    var img = post.Images.FirstOrDefault();
                    if (img != null)
                    {
                        imgUrl = baseUrl + img.Url.Replace("/Images", "Images/thumbs");
                    }

                    var link = $"{baseUrl}blog/post/{post.Id.ToString()}";
                    filledPosts += postForm.Replace("@imageUrl", imgUrl)
                                           .Replace("@title", post.Title)
                                           .Replace("@date", post.Created.ToShortDateString())
                                           .Replace("@author", post.Author)
                                           .Replace("@content", post.Content.ToFixedLenght(350))
                                           .Replace("@link", link);
                }

                unsubscribe = $"За отписване кликнете <a href='{baseUrl}profile'>тук</a>";

                return new PageViewModel
                {
                    Title = "Новини от блогът на VessiFlowers.com",
                    Content = $"<table><tr><td><img src=\"{baseUrl}{blogImageUrl}\"></td><td>Здравейте приятели...</td></tr></table><hr />{filledPosts}{unsubscribe}"
                };
            }
        }

        public async Task<int> SendBulletinAsync(PageViewModel pvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var subscribers = await context.Users.Where(u => u.EmailConfirmed && u.IsSubscribed).ToListAsync();
                if (subscribers.Count > 0)
                {
                    var sender = ConfigurationManager.AppSettings["Email"];
                    foreach (var subscriber in subscribers)
                    {
                        this.emailService.Send(sender, subscriber.Email, pvm.Title, pvm.Content, true);
                    }
                }

                return subscribers.Count;
            }
        }
    }
}
