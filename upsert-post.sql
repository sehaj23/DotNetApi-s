CREATE OR ALTER PROCEDURE TutorialAppSchema.Post_Upsert

    @UserId  INT,
    @PostTitle  NVARCHAR(255),
    @PostContent  NVARCHAR(MAX),
    @PostId INT = NULL
AS
BEGIN

    IF NOT EXISTS (SELECT *
    FROM TutorialAppSchema.Post
    WHERE PostId = @PostId)
   BEGIN
        INSERT INTO TutorialAppSchema.Post
            (
            UserId,PostTitle,PostContent,PostCreated,PostUpdate
            )
        VALUES
            (
                @UserId, @PostTitle, @PostContent, GETDATE(), GETDATE()
   )
    END
ELSE
    BEGIN
        UPDATE  TutorialAppSchema.Post SET
        PostTitle =  @PostTitle,
        PostContent = @PostContent,
        PostUpdate = GETDATE()
        WHERE PostId = @PostId
    END


END

        