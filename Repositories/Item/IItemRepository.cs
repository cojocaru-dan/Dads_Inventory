using System.Collections.Generic;

namespace DadsInventory.Models
{
    public interface IItemRepository
    {
        IEnumerable<Item> AllItems { get; }
        Item GetItemById(int id);

    }
}
