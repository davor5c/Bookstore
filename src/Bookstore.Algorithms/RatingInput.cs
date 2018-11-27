using System;

namespace Bookstore.Algorithms
{
    public class RatingInput
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public bool IsForeign { get; set; }
    }
}