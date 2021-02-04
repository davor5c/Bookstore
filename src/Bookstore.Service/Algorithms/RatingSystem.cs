using System;
using System.Collections.Generic;

namespace Bookstore.Service
{
    public class RatingSystem
    {
        public IEnumerable<RatingResult> ComputeRating(IEnumerable<RatingInput> books)
        {
            var ratingResults = new List<RatingResult>();

            foreach (var book in books)
            {
                decimal rating = 0;

                if (book.Title?.IndexOf("super", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    rating += 100;

                if (book.Title?.IndexOf("great", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    rating += 50;

                if (book.IsForeign)
                    rating *= 1.2m;

                ratingResults.Add(new RatingResult { BookId = book.BookId, Value = rating });
            }

            return ratingResults;
        }
    }
}