using System;

namespace Bookstore.Service
{
    public class RatingResult
    {
        public Guid BookId { get; set; }
        public decimal Value { get; set; }
    }
}