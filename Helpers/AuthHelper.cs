using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

public class AuthHelper
{
	private readonly IConfiguration _config;
	public AuthHelper(IConfiguration config)
	{
		_config = config;
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
}