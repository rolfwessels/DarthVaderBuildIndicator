using System.Collections.Generic;

namespace BuildIndicatron.Core.Api.Model
{
    public class ChangeSet
    {
        public List<Item> items { get; set; }
    }

    public class Author
    {
        public string fullName { get; set; }
    }

    public class Item
    {
        public Author author { get; set; }
    }
}