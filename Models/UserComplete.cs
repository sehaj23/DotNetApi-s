namespace DotnetAPI.Models

{
	public partial class UserComplete
	{
		public int UserId { get; set; }
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string Gender { get; set; }

		public bool Active { get; set; }
		
		public string JobTitle { get; set; }

		public string Departments { get; set; }
		public int Salary  {get;set;}
		
	//	public int avgSalary {get;set;}


		public UserComplete()
		{
			if (FirstName == null)
			{
				FirstName = "";
			}
			if (LastName == null)
			{
				FirstName = "";
			}
			if (Email == null)
			{
				FirstName = "";
			}
			if (Gender == null)
			{
				Gender = "";
			}
			if (JobTitle == null)
			{
				JobTitle = "";
			}
			if (Departments == null)
			{
				Departments = "";
			}

		}
	}
}
