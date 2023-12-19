using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
namespace DotnetAPI.Helpers;
public class AuthHelper
{
	private readonly IConfiguration _config;
	private readonly DataContext _dapper;
	
	public AuthHelper(IConfiguration config)
	{
		_config = config;
		_dapper = new DataContext(config);
	}
	public byte[] getPasswordHash(string password, byte[] passwordSalt)
	{
		string PasswordplusSalt = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
		return KeyDerivation.Pbkdf2(
			password: password,
			salt: Encoding.ASCII.GetBytes(PasswordplusSalt),
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 10000,
			numBytesRequested: 256 / 8
			);
	}

	public string getToken(int userId)
	{
		Claim[] claims1 = new Claim[] {
			new Claim("userId",userId.ToString())
		};
		string token = _config.GetSection("AppSettings:TokenKey").Value;
		SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));

		SigningCredentials signingCredentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);
		SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
		{
			Subject = new ClaimsIdentity(claims1),
			SigningCredentials = signingCredentials,
			Expires = DateTime.Now.AddDays(5)

		};
		JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

		SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

		return jwtSecurityTokenHandler.WriteToken(securityToken);
	}
	
	public bool SetPassword(UserLoginDto userLoginDto)
	{
		byte[] passwordSalt = new byte[128 / 8];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();

		{
			rng.GetNonZeroBytes(passwordSalt);
		}

		byte[] passwordHash = getPasswordHash(userLoginDto.Password, passwordSalt);


		string sqlToInsertInAuth = @"
			EXEC TutorialAppSchema.Auth_Upsert 
			@Email=@EmailParam,@PasswordSalt = @PasswordSaltParam ,@PasswordHash = @PasswordHashParam";

		// List<SqlParameter> sqlParameters = new List<SqlParameter>();
		DynamicParameters dynamicParameters = new DynamicParameters();
		dynamicParameters.Add("@PasswordSaltParam",passwordSalt,dbType:DbType.Binary);
		dynamicParameters.Add("@PasswordHashParam",passwordHash,dbType:DbType.Binary);
		dynamicParameters.Add("@EmailParam",userLoginDto.Email,dbType:DbType.String);
		//passwordSaltSqlParamter.Value = passwordSalt;
		
		// SqlParameter passwordHashsqlParamter = new SqlParameter("@PasswordHashParam", SqlDbType.VarBinary);
		// passwordHashsqlParamter.Value = passwordHash;
		// sqlParameters.Add(passwordHashsqlParamter);

		// SqlParameter emailParamter = new SqlParameter("@EmailParam", SqlDbType.VarChar);
		// emailParamter.Value = userRegisteration.Email;
		// sqlParameters.Add(emailParamter);
	

		return _dapper.ExecuteWithSqlParameter(sqlToInsertInAuth, dynamicParameters);
		
	}
}