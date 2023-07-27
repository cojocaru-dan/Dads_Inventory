using System.Collections.Generic;

namespace DadsInventory.Models
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> AllCategories =>
            new List<Category>
            {
                new Category{CategoryId = 1, CategoryName = "Large" },
                new Category{CategoryId = 2, CategoryName =  "Medium" },
                new Category{CategoryId = 3, CategoryName = "Small" },
            };
    }
}
