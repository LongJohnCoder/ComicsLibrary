﻿CREATE PROCEDURE [ComicsLibrary].[GetHomeBooks]
	@SeriesID INT = NULL
AS
BEGIN

	SELECT 
		 B.[Id]
		  ,S.[Id] SeriesId
		  ,s.Title SeriesTitle
		  ,CASE
			WHEN b.BookTypeID = 1 THEN CONCAT('#', CAST(b.Number AS VARCHAR))
			WHEN b.BookTypeID = 2 THEN CONCAT('Vol. ', CAST(b.Number AS VARCHAR))
			ELSE ''
			END IssueTitle
		  ,b.ImageUrl
		  ,b.ReadUrl
		  ,(SELECT COUNT(BookId) FROM [ComicsLibrary].[SeriesUnreadBooks] WHERE SeriesId = S.Id) UnreadIssues
		  ,b.Creators
	FROM [ComicsLibrary].[Series] S
		INNER JOIN [ComicsLibrary].[SeriesUnreadBooks] U ON U.[SeriesId] = S.[Id]
		INNER JOIN [ComicsLibrary].[Books] B ON B.[Id] = U.[BookId]
	WHERE ((@SeriesID IS NULL AND S.[Abandoned] = 0) OR S.[Id] = @SeriesID)
		AND U.[Rank] = 1
	ORDER BY S.[Title]

END
GO