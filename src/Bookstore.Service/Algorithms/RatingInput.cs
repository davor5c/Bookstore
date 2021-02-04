using System;

namespace Bookstore.Service
{
    public class RatingInput
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public bool IsForeign { get; set; }
    }
}