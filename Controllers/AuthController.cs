using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers;


public class AuthController : ControllerBase
{
	private readonly DataContext _dapper;

	private readonly IConfiguration _config;

	public AuthController(IConfiguration configuration)
	{
		_dapper = new DataContext(configuration);
		_config = configuration;
	}

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
		byte[] passwordHash = getPasswordHash(userRegisteration.Password,passwordSalt);

		
		string sqlToInsertInAuth = @"INSERT INTO TutorialAppSchema.Auth ([Email],[PasswordHash],[PasswordSalt]) VALUES 
	('" + userRegisteration.Email + "',@PasswordHash,@PasswordSalt)";
		List<SqlParameter> sqlParameters = new List<SqlParameter>();
		SqlParameter passwordSaltSqlParamter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
		passwordSaltSqlParamter.Value = passwordSalt;
		SqlParameter passwordHashsqlParamter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
		passwordHashsqlParamter.Value = passwordHash;
		sqlParameters.Add(passwordSaltSqlParamter);
		sqlParameters.Add(passwordHashsqlParamter);
		bool result = _dapper.ExecuteWithSqlParameter(sqlToInsertInAuth,sqlParameters);
		if (result == false)
		{
			throw new Exception("Unable to insert in auth table");
		}
			string sqlToAddDetailsOfUser= @"INSERT INTO TutorialAppSchema.Users ([FirstName],
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
		if (resultToAddDetailsOfUser==false)
		{
			throw new Exception("Failed to Add Details Of user");
		}
		return Ok();
	}

	[HttpPost("Login")]
	public IActionResult Login(UserLoginDto userLogin)
	{
		string sqlToGetHashAndSalt = "SELECT [PasswordHash],[PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '" + userLogin.Email + "'";
		UserLoginConfirmationDto userLoginConfirmation = _dapper.LoadDataSingle<UserLoginConfirmationDto>(sqlToGetHashAndSalt);
		byte[] passwordHash = getPasswordHash(userLogin.Password,userLoginConfirmation.PasswordSalt);
		for(var i = 0;i<passwordHash.Length;i++)
		{
			if(passwordHash[i] != userLoginConfirmation.PasswordHash[i])
			{
				return StatusCode(401,"Incorrect Password!");
			}
		}
		string getUser = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" + userLogin.Email + "'";;
		int userData =  _dapper.LoadDataSingle<int>(getUser);
		
		return Ok(new Dictionary<string,string>
		{
			{"token",getToken(userData)}
		});
	}
	
	private byte[] getPasswordHash(string password,byte[] passwordSalt)
	{
		string PasswordplusSalt = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
		return  KeyDerivation.Pbkdf2(
			password: password,
			salt: Encoding.ASCII.GetBytes(PasswordplusSalt),
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 10000,
			numBytesRequested: 256 / 8
			);
	}
	
	private string getToken(int userId)
	{
		Claim[] claims1 = new Claim[] {
			new Claim("userId",userId.ToString())
		};
		string token = _config.GetSection("AppSettings:TokenKey").Value;
		SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));
		
		SigningCredentials signingCredentials =  new SigningCredentials(tokenKey,SecurityAlgorithms.HmacSha512Signature);
		SecurityTokenDescriptor securityTokenDescriptor= new SecurityTokenDescriptor()
		{
			Subject= new ClaimsIdentity(claims1),
			SigningCredentials=signingCredentials,
			Expires=DateTime.Now.AddDays(5)	
		
		};
		JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
		
		SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
		
		return jwtSecurityTokenHandler.WriteToken(securityToken);
	}
}