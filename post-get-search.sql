CREATE OR ALTER PROCEDURE TutorialAppSchema.spPost_Get
    @UserId INT = NULL,
    @searchParam NVARCHAR(max) = NULL,
    @PostId INT = NULL
AS
BEGIN
    SELECT *
    FROM TutorialAppSchema.Post
    WHERE UserId = ISNULL(@UserId,Post.UserId) AND PostId = ISNULL(@PostId,Post.PostId) AND (@searchParam IS NULL
        OR
        Post.PostContent LIKE '%' + @searchParam + '%'
        OR
        Post.PostTitle LIKE '%' + @searchParam + '%'
)
END


