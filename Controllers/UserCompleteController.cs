using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
	DataContext _dapper;
	private readonly IConfiguration config;
		private readonly ReusableSql _reusableSql;
	public UserCompleteController(IConfiguration config)
	{
		_dapper = new DataContext(config);
		_reusableSql = new ReusableSql(config);
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

		if(_reusableSql.updateUser(user))
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




