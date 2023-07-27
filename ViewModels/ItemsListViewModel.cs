using System.Collections.Generic;
using DadsInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace DadsInventory.ViewModels
{
    public class ItemsListViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public string Title { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
    }
}
