using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DadsInventory.Models
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _appDbContext;

        public ItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Item> AllItems
        {
            get { return _appDbContext.Items.Include(c => c.Category); }
        }

        public Item GetItemById(int Id)
        {
            return _appDbContext.Items.Include(c => c.Category).FirstOrDefault(p => p.Id == Id);
        }
    }
}