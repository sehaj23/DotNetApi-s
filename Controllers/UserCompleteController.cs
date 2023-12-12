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
		string parameter = "";
		if (userId != 0)
		{
			sql += "@userId=" + userId;
		}
		return _dapper.LoadData<UserComplete>(sql);
		//return new string[] {"user1","user2",value};
	}

	[HttpPut("UpsertUser")]
	public IActionResult updateUser(UserComplete user)
	{

		string sql = @"
			EXEC TutorialAppSchema.spUser_Upsert 
			@FirstName='" + user.FirstName +
			"',@LastName = '" + user.LastName +
			"',@Email = '" + user.Email +
			"',@Gender = '" + user.Gender +
			"',@Active= '" + user.Active +
			"',@JobTitle= '" + user.JobTitle +
			"',@Department= '" + user.Departments +
			"',@Salary= '" + user.Salary +
			"',@UserId= '" + user.UserId;
		bool result = _dapper.Execute(sql);
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




