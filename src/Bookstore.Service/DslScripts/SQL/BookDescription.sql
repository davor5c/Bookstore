SELECT
    b.ID,
    Description = ISNULL(CAST(b.NumberOfPages AS nvarchar(100)), '?') + ' pages'
        + CASE WHEN cb.ID IS NOT NULL THEN ', children ' + ISNULL(CAST(cb.AgeFrom AS nvarchar(100)), '?') + '-' + ISNULL(CAST(cb.AgeTo AS nvarchar(100)), '?') + ' y/o' ELSE '' END
        + CASE WHEN fb.ID IS NOT NULL THEN ', foreign (' + ISNULL(fb.OriginalLanguage, '?') + ')' ELSE '' END
FROM
    Bookstore.Book b
    LEFT JOIN Bookstore.ChildrensBook cb ON cb.ID = b.ID
    LEFT JOIN Bookstore.ForeignBook fb ON fb.ID = b.ID
