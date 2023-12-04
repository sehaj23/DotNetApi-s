namespace DotnetAPI.Dtos
{
partial class UserLoginConfirmationDto
{
	byte[] PasswordHash {get;set;}
	byte[] PasswordSalt {get;set;}
	


	public UserLoginConfirmationDto()
	{
		if(PasswordHash ==null)
		{
			PasswordHash=new byte[0];
		}
		if(PasswordSalt ==null)
		{
			PasswordSalt=new byte[0];
		}

	}
}

}