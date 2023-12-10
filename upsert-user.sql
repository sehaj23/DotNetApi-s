CREATE OR ALTER PROCEDURE TutorialAppSchema.spUser_Upsert

    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(50),
    @Gender NVARCHAR (50),
    @Active BIT,
    @JobTitle NVARCHAR(50),
    @Department NVARCHAR(50),
    @Salary DECIMAL(18, 4),
    @UserId int = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT *
    FROM TutorialAppSchema.Users
    WHERE UserId = @UserId)
    BEGIN
        IF NOT EXISTS (SELECT *
        FROM TutorialAppSchema.Users
        WHERE Email = @Email)
    BEGIN
            DECLARE @outputUserId INT
            INSERT INTO TutorialAppSchema.Users
                (
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active])
            VALUES
                (
                    @FirstName, @LastName, @Email, @Gender, @Active
            )
            SET @outputUserId = @@IDENTITY
            INSERT INTO TutorialAppSchema.UserSalary
                (
                UserId,Salary
                )
            VALUES
                (
                    @outputUserId, @Salary
           )
            INSERT INTO TutorialAppSchema.UserJobInfo
                (
                UserId,department,JobTitle
                )
            VALUES
                (
                    @outputUserId, @Department, @JobTitle
           )
        END
    END
    ELSE
        BEGIN
        UPDATE TutorialAppSchema.Users SET
             FirstName = @FirstName,
             LastName = @LastName,
             Gender= @Gender,
             Email = @Email,
             Active = @Active
             WHERE UserId = @UserId
        UPDATE TutorialAppSchema.UserSalary SET
            Salary = @Salary  WHERE UserId = @UserId

        UPDATE TutorialAppSchema.UserJobInfo SET
            department = @Department,JobTitle=@JobTitle  WHERE UserId = @UserId

    END
END

        