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

	private readonly IConfiguration _config;
	
	private readonly AuthHelper _authHelper;

	public AuthController(IConfiguration configuration)
	{
		_dapper = new DataContext(configuration);
		_config = configuration;
		_authHelper =  new AuthHelper(configuration);
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
		byte[] passwordSalt = new byte[128 / 8];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();

		{
			rng.GetNonZeroBytes(passwordSalt);
		}
		
		byte[] passwordHash =_authHelper.getPasswordHash(userRegisteration.Password, passwordSalt);


		string sqlToInsertInAuth = @"INSERT INTO TutorialAppSchema.Auth ([Email],[PasswordHash],[PasswordSalt]) VALUES 
	('" + userRegisteration.Email + "',@PasswordHash,@PasswordSalt)";
		List<SqlParameter> sqlParameters = new List<SqlParameter>();
		SqlParameter passwordSaltSqlParamter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
		passwordSaltSqlParamter.Value = passwordSalt;
		SqlParameter passwordHashsqlParamter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
		passwordHashsqlParamter.Value = passwordHash;
		sqlParameters.Add(passwordSaltSqlParamter);
		sqlParameters.Add(passwordHashsqlParamter);
		bool result = _dapper.ExecuteWithSqlParameter(sqlToInsertInAuth, sqlParameters);
		if (result == false)
		{
			throw new Exception("Unable to insert in auth table");
		}
		string sqlToAddDetailsOfUser = @"INSERT INTO TutorialAppSchema.Users ([FirstName],
[LastName],
										[Email],
										[Gender],
										[Active]) VALUES (" +
										"'" + userRegisteration.FirstName +
										"','" + userRegisteration.LastName +
										"','" + userRegisteration.Email +
										"','" + userRegisteration.Gender +
										"','" + 1 +
										"');";


		bool resultToAddDetailsOfUser = _dapper.Execute(sqlToAddDetailsOfUser);
		if (resultToAddDetailsOfUser == false)
		{
			throw new Exception("Failed to Add Details Of user");
		}
		return Ok();
	}
	[AllowAnonymous]
	[HttpPost("Login")]
	public IActionResult Login(UserLoginDto userLogin)
	{
		string sqlToGetHashAndSalt = "SELECT [PasswordHash],[PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '" + userLogin.Email + "'";
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
