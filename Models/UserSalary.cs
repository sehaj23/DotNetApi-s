namespace DotnetAPI.Models

{
	public partial class UsersSalary
	{
		public int UserId {get;set;}
		public int Salary  {get;set;}
		
		public string Departments  {get;set;}
		
		
		
	
	public UsersSalary()
	{
		
		if(Departments == null)
		{
			Departments ="";
		}
		
		
	}
	}
}
