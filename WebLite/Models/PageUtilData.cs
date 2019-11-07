using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Models
{
    public class PageUtilData
    {
        public int Page { get; set; }

        public int Num { get; set; }

        public long Total { get; set; }

        public object Data { get; set; }

        public string Next { get; set; }

        public string Prev { get; set; }

        public bool HasPrev { get; set; }

        public bool HasNext { get; set; }

        public PageUtilData(int page, int num, long total, object data, string baseUrl)
        {
            this.Page = page;
            this.Num = num;
            this.Total = total;
            this.Data = data;

            if (page * num < total)
            {
                HasNext = true;
                Next = $"{baseUrl}?page={page + 1}&num={num}";
            }
            if (page > 1)
            {
                HasPrev = true;
                Prev = $"{baseUrl}?page={page - 1}&num={num}";
            }
        }

        public PageUtilData() { }

        public PageUtilData NoData()
        {
            this.Page = 1;
            this.Num = 0;
            this.Total = 0;
            this.Data = null;
            HasNext = false;
            Next = null;

            HasPrev = false;
            Prev = null;
            return this;
        }
    }
}
