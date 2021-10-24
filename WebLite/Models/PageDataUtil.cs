using System;

namespace WebLite.Models
{
    public class PageDataUtil<T>
    {
        // 总数量
        public long TotalSize { get; set; }
        // 当前页码
        public int Page
        {
            get => Page;
            set
            {
                if (value < 1 || value > 65535)
                {
                    throw new ArgumentException("invalid page");
                }
                Page = value;
            }
        }
        // 每页数量
        public int PageSize
        {
            get => PageSize;
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentException("invalid pageSize");
                }
                PageSize = value;
            }
        }

        // 核心数据
        public T Data { get; set; }
        // 下一页
        public string Next { get; set; }
        // 上一页
        public string Prev { get; set; }
        // 是否有上一页
        public bool HasPrev { get; set; }
        // 是否有下一页
        public bool HasNext { get; set; }

        public PageDataUtil(int page, int pageSize, long totalSize, T data, string baseUrl)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.TotalSize = totalSize;
            this.Data = data;

            if (page * pageSize < totalSize)
            {
                HasNext = true;
                Next = $"{baseUrl}?page={page + 1}&num={pageSize}";
            }
            if (page > 1)
            {
                HasPrev = true;
                Prev = $"{baseUrl}?page={page - 1}&num={pageSize}";
            }
        }

        public PageDataUtil() { }

        public static PageDataUtil<T> NoData()
        {
            return new PageDataUtil<T>(
                1,
                0,
                0,
                default(T),
                null
            );
        }
    }
}
