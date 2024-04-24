using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using RecipeApi.Exceptions;

namespace RecipeApi.Models
{
    public class PageResult<T>
	{
		public List<T> Items { get; set; }
		public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }
        public PageResult(List<T> items, int totalCount, int pageSize, int pageNumber)
		{
			Items = items;
			TotalItemsCount = totalCount;
			ItemsFrom = pageSize * (pageNumber - 1) + 1;
            if(ItemsFrom > totalCount) throw new BadRequestException("Page number out of range");
            var calculatedItemsTo = ItemsFrom + pageSize - 1;
			ItemsTo = (calculatedItemsTo > totalCount) ? totalCount : calculatedItemsTo;
			TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
		}
	}
}