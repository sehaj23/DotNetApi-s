namespace DotnetAPI.Dtos
{
partial class UserLoginDto
{
	string Email {get;set;}
	string Password {get;set;}
	


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