namespace DotnetAPI.Dtos
{
public partial class UserLoginDto
{
	public string Email {get;set;}
	public string Password {get;set;}
	


	public UserLoginDto()
	{
		if(Email ==null)
		{
			Email="";
		}
		if(Password ==null)
		{
			Email="";
		}

	}
}

}