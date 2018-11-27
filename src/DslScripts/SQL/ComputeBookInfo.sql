SELECT
    b.ID,
    NumberOfComments = COUNT(bc.ID)
FROM
    Bookstore.Book b
    LEFT JOIN Bookstore.Comment bc ON bc.BookID = b.ID
GROUP BY
    b.ID
