using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApi.Models;
using RecipeApi.Models;

namespace RecipeApi.Models
{
    public class RecipeQuery
    {
        public int PageNumber { get; set; }=1;
        public int PageSize { get; set; }=12;
        public string? SearchPhase { get; set; }
		public RecipeSortBy? SortBy { get; set; } = RecipeSortBy.Title;
		public SortDirections? SortDirection { get; set; } = SortDirections.ASC;

        public List<int>? IngredientsIds { get; set; }
    }
}