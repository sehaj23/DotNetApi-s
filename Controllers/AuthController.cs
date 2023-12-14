using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]

public class AuthController : ControllerBase
{
	private readonly DataContext _dapper;



	private readonly AuthHelper _authHelper;

	public AuthController(IConfiguration configuration)
	{
		_dapper = new DataContext(configuration);
		_authHelper = new AuthHelper(configuration);
	}
	[AllowAnonymous]
	[HttpPost("Register")]
	public IActionResult Register(UserRegisterationDto userRegisteration)
	{
		if (userRegisteration.Password != userRegisteration.PasswordConfirm)
		{
			throw new Exception("Password do not match!");
		}
		string sql = "SELECT * FROM  TutorialAppSchema.Auth WHERE Email ='" + userRegisteration.Email + "'";
		IEnumerable<string> existingUser = _dapper.LoadData<string>(sql);
		if (existingUser.Count() != 0)
		{
			throw new Exception("User Already Exists!");
		}
		UserLoginDto userLoginDto = new UserLoginDto()
		{
			Email= userRegisteration.Email,
			Password = userRegisteration.Password,
			
		};
		if (_authHelper.SetPassword(userLoginDto)==false)
		{
			throw new Exception("Unable to insert in auth table");
		}
		
		string sqlToAddDetailsOfUser = @"
			EXEC TutorialAppSchema.spUser_Upsert 
			@FirstName='" + userRegisteration.FirstName +
			"',@LastName = '" + userRegisteration.LastName +
			"',@Email = '" + userRegisteration.Email +
			"',@Gender = '" + userRegisteration.Gender +
			"',@Active= 1" +
			",@JobTitle= '" + userRegisteration.JobTitle +
			"',@Department= '" + userRegisteration.Departments +
			"',@Salary= '" + userRegisteration.Salary +"'";

		bool resultToAddDetailsOfUser = _dapper.Execute(sqlToAddDetailsOfUser);
		if (resultToAddDetailsOfUser == false)
		{
			throw new Exception("Failed to Add Details Of user");
		}
		return Ok();
	}
	
	[HttpPut("ResetPassword")]
	public IActionResult ResetPassword(UserLoginDto userLogin)
	{
		if(_authHelper.SetPassword(userLogin))
		{
			return Ok();
		};
		throw new Exception("Unable to Update Password");
		
	}
	
	[AllowAnonymous]
	[HttpPost("Login")]
	public IActionResult Login(UserLoginDto userLogin)
	{
		//string sqlToGetHashAndSalt = "SELECT [PasswordHash],[PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '" + userLogin.Email + "'";
		string sqlToGetHashAndSalt = "EXEC TutorialAppSchema.spLogin_Confirmation @Email='" + userLogin.Email.ToString() + "';";
		UserLoginConfirmationDto userLoginConfirmation = _dapper.LoadDataSingle<UserLoginConfirmationDto>(sqlToGetHashAndSalt);
		byte[] passwordHash = _authHelper.getPasswordHash(userLogin.Password, userLoginConfirmation.PasswordSalt);
		for (var i = 0; i < passwordHash.Length; i++)
		{
			if (passwordHash[i] != userLoginConfirmation.PasswordHash[i])
			{
				return StatusCode(401, "Incorrect Password!");
			}
		}
		string getUser = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" + userLogin.Email + "'"; ;
		int userData = _dapper.LoadDataSingle<int>(getUser);

		return Ok(new Dictionary<string, string>
		{
			{"token",_authHelper.getToken(userData)}
		});
	}



	[HttpGet("refreshToken")]
	public IActionResult refreshToken()
	{
		string sqlGetUser = "SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" + User.FindFirst("userId")?.Value + "'";
		int userId = _dapper.LoadDataSingle<int>(sqlGetUser);

		return Ok(new Dictionary<string, string>
		{
			{"token",_authHelper.getToken(userId)}
		});
	}

}
