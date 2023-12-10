

ALTER PROCEDURE TutorialAppSchema.spUser_Get 
    @UserId INT = NULL,
    @active BIT = NULL


AS
BEGIN

DROP TABLE IF EXISTS #avgDepartmentSalary

SELECT [UserJobInfo].Department, AVG(UserSalary.Salary) 
avgSalary INTO #avgDepartmentSalary
 FROM TutorialAppSchema.Users AS Users
LEFT JOIN TutorialAppSchema.UserSalary As UserSalary
ON UserSalary.UserId = Users.UserId
LEFT JOIN TutorialAppSchema.UserJobInfo As UserJobInfo
ON UserJobInfo.UserId = Users.UserId
GROUP BY UserJobInfo.Department 

CREATE CLUSTERED INDEX cix_avg_salary_department on #avgDepartmentSalary(Department)
SELECT [Users].UserId,[Users].FirstName,[Users].LastName,[Users].Email,[Users].Gender,[UserSalary].Salary,[UserJobInfo].JobTitle,avgSalary.avgSalary FROM TutorialAppSchema.Users AS Users 
LEFT JOIN TutorialAppSchema.UserSalary As UserSalary
ON UserSalary.UserId = Users.UserId
LEFT JOIN TutorialAppSchema.UserJobInfo As UserJobInfo
ON UserJobInfo.UserId = Users.UserId
LEFT JOIN #avgDepartmentSalary as AvgSalary
ON AvgSalary.department = UserJobInfo.Department
-- OUTER APPLY (
--    SELECT AVG(UserSalary2.Salary) avgSalary FROM TutorialAppSchema.Users AS Users
-- LEFT JOIN TutorialAppSchema.UserSalary As UserSalary2
-- ON UserSalary2.UserId = Users.UserId
-- LEFT JOIN TutorialAppSchema.UserJobInfo As UserJobInfo2
-- ON UserJobInfo2.UserId = Users.UserId
-- WHERE UserJobInfo2.Department = UserJobInfo.Department
-- GROUP BY UserJobInfo2.Department 
-- ) AS AVGSALARY
WHERE Users.UserId =ISNULL(@UserId,Users.UserId) AND Users.Active = ISNULL(@active,Users.Active);
END




