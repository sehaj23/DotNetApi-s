using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
	DataContext _dapper;
	//private readonly ILogger<WeatherForecastController> _logger;
	private readonly IConfiguration config;
	public UserController(IConfiguration config)
	{
		_dapper = new DataContext(config);
	}
	[HttpGet("test")]
	public DateTime test()
	{
		return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
	}

[HttpGet("testConnection")]
	public String testConnectionString()
	{
		return "Your Application is running";
	}
	

	[HttpGet("getUsers/{userId}")]
	public User GetUser(int userId)
	{
		return _dapper.LoadDataSingle<User>("select * from TutorialAppSchema.Users where UserId = " + userId);
		//return new string[] {"user1","user2",value};
	}

	[HttpGet("getUsers")]
	public IEnumerable<User> GetAllUsers()
	{
		return _dapper.LoadData<User>("select * from TutorialAppSchema.Users");
		//return new string[] {"user1","user2",value};
	}

	[HttpPut("updateUser")]

	public IActionResult updateUser(User user)
	{
		string sql = @"
			UPDATE TutorialAppSchema.Users SET [FirstName] ='" + user.FirstName +
			 "',[LastName] = '" + user.LastName +
			 "',[Email] = '" + user.Email +
			 "',[Gender] = '" + user.Gender +
			 "',[Active]= '" + user.Active +
			 "' WHERE UserId = " + user.UserId;
		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		return UnprocessableEntity();

	}

	[HttpPost("AddUser")]

	public IActionResult AddUser(UserDTO user)
	{

	
		return UnprocessableEntity();
	}
	[HttpDelete("DeleteUser/{userId}")]
	public bool deleteUser(int userId)
	{
		string sql = @"DELETE FROM 
						TutorialAppSchema.Users
						WHERE UserId =" + userId;
		return _dapper.Execute(sql);

	}

	[HttpGet("getUsersSalary/{userId}")]
	public UsersSalary GetUserSalary(int userId)
	{
		return _dapper.LoadDataSingle<UsersSalary>("select * from TutorialAppSchema.UserSalary where UserId = " + userId);
		//return new string[] {"user1","user2",value};
	}

	[HttpDelete("DeleteUserSalary/{userId}")]
	public bool deleteUserSalary(int userId)
	{
		string sql = @"DELETE FROM 
						TutorialAppSchema.UserSalary
						WHERE UserId =" + userId;
		return _dapper.Execute(sql);

	}
	[HttpPut("updateUserSalary")]

	public IActionResult updateUserSalary(UsersSalary usersSalary)
	{
		string sql = @"
			UPDATE TutorialAppSchema.UserSalary SET [Salary] = " + usersSalary.Salary + "WHERE UserId = " + usersSalary.UserId;
			
		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		return UnprocessableEntity();

	}
	
	[HttpPost("AddUserSalary")]

	public IActionResult AddUserSalary(UsersSalary user)
	{

		string sql = @"INSERT INTO TutorialAppSchema.UserSalary ([FirstName],
[LastName],
										[UserId],
										[Salary],
										[Department]) VALUES (" +
											"'" + user.UserId +
											"','" + user.Salary +
											"','" + user.Departments +
											"');";

		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		return UnprocessableEntity();
	}
	
		[HttpGet("GetUserJobInfo/{userId}")]
	public UsersJobInfo GetUserJobInfo(int userId)
	{
		return _dapper.LoadDataSingle<UsersJobInfo>("select * from TutorialAppSchema.UserJobInfo where UserId = " + userId);
		//return new string[] {"user1","user2",value};
	}

	[HttpDelete("updateUserJobInfo/{userId}")]
	public bool deleteUserJobInfo(int userId)
	{
		string sql = @"DELETE FROM 
						TutorialAppSchema.UserJobInfo
						WHERE UserId =" + userId;
		return _dapper.Execute(sql);

	}
	[HttpPut("updateUserJobInfo")]

	public IActionResult updateUserJobInfo(UsersJobInfo usersJobInfo)
	{
		string sql = @"
			UPDATE TutorialAppSchema.UserJobInfo SET [JobTitle] = " + usersJobInfo.JobTitle + "WHERE UserId = " + usersJobInfo.UserId;
			
		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		return UnprocessableEntity();

	}
	
	[HttpPost("AddUserJobInfo")]

	public IActionResult AddUserJobInfo(UsersJobInfo user)
	{

		string sql = @"INSERT INTO TutorialAppSchema.UserJobInfo ([FirstName],
[LastName],
										[UserId],
										[JobTitle],
										[Department]) VALUES (" +
											"'" + user.UserId +
											"','" + user.JobTitle +
											"','" + user.Departments +
											"');";

		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		return UnprocessableEntity();
	}
}




