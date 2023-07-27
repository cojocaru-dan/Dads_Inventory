using System.Collections.Generic;

namespace DadsInventory.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> AllCategories { get; }
    }
}
