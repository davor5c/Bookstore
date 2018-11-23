SELECT
    k.ID,
    NumberOfComments = COUNT(kom.ID)
FROM
    Bookstore.Book k
    LEFT JOIN Bookstore.Comment kom ON kom.BookID = k.ID
GROUP BY
    k.ID
