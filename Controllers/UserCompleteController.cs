using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
	DataContext _dapper;
	private readonly IConfiguration config;
	public UserCompleteController(IConfiguration config)
	{
		_dapper = new DataContext(config);
	}

	[HttpGet("getUsers/{userId}")]
	public IEnumerable<UserComplete> GetAllUsers(int userId)
	{
		string sql = "EXEC TutorialAppSchema.spUser_Get ";
		DynamicParameters dynamicParameters = new DynamicParameters();
		string parameter = "";
		if (userId != 0)
		{
			dynamicParameters.Add("@userIdParam",userId,DbType.Int32);
			sql += "@UserId=@userIdParam";
		}
		return _dapper.LoadDataWithParameters<UserComplete>(sql,dynamicParameters);
		//return new string[] {"user1","user2",value};
	}

	[HttpPut("UpsertUser")]
	public IActionResult updateUser(UserComplete user)
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
		if (result)
		{
			return Ok();
		}
		throw new Exception("Unable to process!");
	}

	[HttpDelete("DeleteUser/{userId}")]
	public bool deleteUser(int userId)
	{
		string sql = @"TutorialAppSchema.spUser_Delete " + userId.ToString();
		return _dapper.Execute(sql);

	}
}




