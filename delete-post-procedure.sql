CREATE OR ALTER PROCEDURE TutorialAppSchema.spPost_delete
    @PostId INT,
    @UserId INT
AS
BEGIN
    DELETE FROM TutorialAppSchema.Post WHERE PostId = @PostId AND UserId =  @UserId
END