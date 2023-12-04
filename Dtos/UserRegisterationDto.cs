namespace DotnetAPI.Dtos
{
partial class UserRegisterationDto
{
	string Email {get;set;}
	string Password {get;set;}
	
	string PasswordConfirm {get;set;}

	public UserRegisterationDto()
	{
		if(Email ==null)
		{
			Email="";
		}
		if(Password ==null)
		{
			Email="";
		}
		if(PasswordConfirm ==null)
		{
			Email="";
		}
	}
}

}