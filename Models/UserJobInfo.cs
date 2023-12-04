
namespace DotnetAPI.Models

{
	public partial class UsersJobInfo
	{
		public int UserId { get; set; }
		public string JobTitle { get; set; }

		public string Departments { get; set; }




		public UsersJobInfo()
		{
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

