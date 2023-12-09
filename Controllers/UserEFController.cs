using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
	DataContextEF entityFrameWork;
	IMapper _mapper;

	IUserRepository _userRespository;
	//private readonly ILogger<WeatherForecastController> _logger;
	private readonly IConfiguration config;
	public UserEFController(IConfiguration config, IUserRepository userRepository)
	{
		entityFrameWork = new DataContextEF(config);
		_mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.CreateMap<UserDTO, User>();
		}));
		_userRespository = userRepository;
	}


	[HttpGet("getUsers/{userId}")]
	public User GetUser(int userId)
	{
		return _userRespository.GetSingleUser(userId);
	}

	[HttpGet("getUsers")]
	public IEnumerable<User> GetAllUsers()
	{
		return _userRespository.GetAllUsers();

	}

	[HttpPut("updateUser")]

	public IActionResult updateUser(User user)
	{
		User userDB = _userRespository.GetSingleUser(user.UserId);
		if (userDB != null)
		{
			userDB.FirstName = user.FirstName;
			userDB.Gender = user.Gender;
			userDB.LastName = user.LastName;
			userDB.Email = user.Email;
			userDB.Active = user.Active;

			if (_userRespository.SaveChanges())
			{
				return Ok();
			}
			else
			{
				throw new Exception("Failed to update");
			}
		}
		else
		{
			throw new Exception("User Not Found");
		}

	}
	[HttpPost("AddUser")]

	public IActionResult AddUser(UserDTO user)
	{
		User userDB = new User();
		userDB.FirstName = user.FirstName;
		userDB.Gender = user.Gender;
		userDB.LastName = user.LastName;
		userDB.Email = user.Email;
		userDB.Active = user.Active;
		_userRespository.AddEntity<User>(userDB);
		if (_userRespository.SaveChanges())
		{
			return Ok();
		}

		throw new Exception("Failed to Add");


	}


	[HttpPost("AddUserSalary")]
	public IActionResult AddUserSalary(UsersSalary usersSalary)
	{
		UsersSalary userDB = new UsersSalary();
		userDB.Departments = usersSalary.Departments;
		userDB.UserId = usersSalary.UserId;
		userDB.Salary = usersSalary.Salary;

		_userRespository.AddEntity<UsersSalary>(userDB);
		if (_userRespository.SaveChanges())
		{
			return Ok();
		}

		throw new Exception("Failed to Add");


	}
	// [HttpDelete("DeleteUserSalary/{userId}")]
	// public IActionResult deleteUserSalary(int userId)
	// {
	// 	UsersSalary userDB = _userRespository.GetSingleUserSalary(userId);
	// 	if (userDB != null)
	// 	{
	// 		_userRespository.RemoveEntity<UsersSalary>(userDB);
	// 	}
	// 	if (_userRespository.SaveChanges())
	// 	{
	// 		return Ok();
	// 	}

	// 	throw new Exception("Failed to delete");


	// }

	[HttpGet("getUserSalary/{userId}")]
	public UsersSalary GetUserSalary(int userId)
	{
		return _userRespository.GetSingleUserSalary(userId);
	}

	[HttpGet("getUsersSalary")]
	public IEnumerable<UsersSalary> GetAllUsersSalary()
	{
		return _userRespository.GetAllUsersSalary();

	}

	[HttpPut("updateUserSalary")]

	public IActionResult updateUserSalary(UsersSalary usersSalary)
	{
		UsersSalary userDB = _userRespository.GetSingleUserSalary(usersSalary.UserId);
		if (userDB != null)
		{
			userDB.Departments = usersSalary.Departments;
			userDB.UserId = usersSalary.UserId;
			userDB.Salary = usersSalary.Salary;


			if (_userRespository.SaveChanges())
			{
				return Ok();
			}
			else
			{
				throw new Exception("Failed to update");
			}
		}
		else
		{
			throw new Exception("User Not Found");
		}

	}




	[HttpDelete("DeleteUserSalary/{userId}")]
	public IActionResult deleteUserSalary(int userId)
	{
		UsersSalary userDB = _userRespository.GetSingleUserSalary(userId);
		if (userDB != null)
		{
			_userRespository.RemoveEntity<UsersSalary>(userDB);
		}
		if (_userRespository.SaveChanges())
		{
			return Ok();
		}

		throw new Exception("Failed to delete");


	}

}




// string sql = @"INSERT INTO TutorialAppSchema.Users (
// 				[FirstName],
// 				[LastName],
// 				[Email],
// 				[Gender],
// 				[Active]) VALUES (" + 
// 				"'" + user.FirstName +
// 		 "','" + user.LastName + 
// 		 "','" + user.Email + 
// 		 "','" + user.Gender + 
// 		 "','" + user.Active +
// 		 "');";