namespace DotnetAPI.Dtos
{
public partial class UserLoginConfirmationDto
{
	public byte[] PasswordHash {get;set;}
	public byte[] PasswordSalt {get;set;}
	


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