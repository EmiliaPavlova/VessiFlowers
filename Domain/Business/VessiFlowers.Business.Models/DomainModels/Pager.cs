﻿namespace VessiFlowers.Business.Models.DomainModels
{
    using System;
    using System.Configuration;

    public class Pager
    {
        public Pager(int totalItems, int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            int pagerLength = int.Parse(ConfigurationManager.AppSettings["PagerLength"]);

            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;

            var isEven = pagerLength % 2 == 0;
            var startPage = 0;
            var endPage = 0;

            if (isEven)
            {
                startPage = currentPage - (int)(pagerLength / 2) + 1;
                endPage = currentPage + (int)(pagerLength / 2);
            }
            else
            {
                startPage = currentPage - (int)Math.Floor((decimal)pagerLength / 2);
                endPage = currentPage + (int)Math.Ceiling((decimal)pagerLength / 2) - 1;
            }

            if (totalPages >= pagerLength)
            {
                if (startPage <= 0)
                {
                    startPage = 1;
                    endPage = pagerLength;
                }

                if (endPage >= totalPages)
                {
                    endPage = totalPages;
                    startPage = totalPages - pagerLength + 1;
                }
            }
            else
            {
                startPage = 1;
                endPage = totalPages;
            }

            this.TotalItems = totalItems;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
            this.StartPage = startPage;
            this.EndPage = endPage;
        }

        public int TotalItems { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }
    }
}