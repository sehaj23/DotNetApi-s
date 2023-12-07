namespace DotnetAPI.Dtos
{
public partial class UserRegisterationDto
{
	public string Email {get;set;}
	public string Password {get;set;}
	
	public string PasswordConfirm {get;set;}
	
	public string FirstName { get; set; }

		public string LastName { get; set; }


		public string Gender { get; set; }




		

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
		if (FirstName == null)
			{
				FirstName = "";
			}
			if (LastName == null)
			{
				FirstName = "";
			}
			
			if (Gender == null)
			{
				Gender = "";
			}
	}
}

}