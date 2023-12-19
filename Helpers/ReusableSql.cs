
using Dapper;
using DotnetAPI.Models;
using DotnetAPI.Data;
namespace DotnetAPI.Helpers;
public class ReusableSql
{
	DataContext _dapper;
	private readonly IConfiguration config;
	public ReusableSql(IConfiguration config)
	{
		_dapper = new DataContext(config);
	}
	
	public bool updateUser(UserComplete user)
	{
		string sql = @"
			EXEC TutorialAppSchema.spUser_Upsert 
			@FirstName= @FirstNameParam
			,@LastName= @LastNameParam 
			,@Email = @EmailParam
			,@Gender = @GenderParam
			,@Active= 1
			,@JobTitle= @JobTitleParam
			,@Department= @DepartmentParam
			,@Salary= @SalaryParam
			,@UserId= @UserIdParam";
			DynamicParameters dynamicParameters = new DynamicParameters();
			dynamicParameters.Add("@FirstNameParam",user.FirstName);
			dynamicParameters.Add("@LastNameParam ",user.LastName);
			dynamicParameters.Add("@EmailParam",user.Email);
			dynamicParameters.Add("@GenderParam",user.Gender);
			dynamicParameters.Add("@JobTitleParam",user.JobTitle);
			dynamicParameters.Add("@DepartmentParam",user.Departments);
			dynamicParameters.Add("@SalaryParam",user.Salary);
			dynamicParameters.Add("@UserIdParam",user.UserId.ToString());
			
			bool result = _dapper.ExecuteWithSqlParameter(sql,dynamicParameters);
			return result;
	}
}