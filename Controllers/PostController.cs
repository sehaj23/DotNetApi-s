
namespace DotnetAPI.Controllers;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PostController : ControllerBase
{
	DataContext _dapper;
	//private readonly ILogger<WeatherForecastController> _logger;
	private readonly IConfiguration config;
	public PostController(IConfiguration config)
	{
		_dapper = new DataContext(config);
	}

	[HttpPost("UpsertPost")]

	public IActionResult Addpost(PostToEditDto post)
	{

		string userId = User.FindFirst("userId")?.Value;


		string sql = @"
			EXEC TutorialAppSchema.Post_Upsert 
			@PostTitle='" + post.PostTitle +
			"',@PostContent = '" + post.PostContent +
			"',@PostId = '" + post.PostId +
			"',@UserId = '" + userId + "'";

		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		throw new Exception("Unable to Add Post!");
	}
	// [HttpPut
	// ("EditPost")]

	// public IActionResult Editpost(PostToEditDto post)
	// {
	// 	string userId = User.FindFirst("userId")?.Value;
	// 	string sql = @"UPDATE TutorialAppSchema.Post SET  
	// 		PostContent = '" + post.PostContent + "',PostTitle = '" +
	// 	 post.PostTitle + "',PostUpdate = GETDATE() WHERE PostId =" + post.PostId + "AND UserId = " + userId;


	// 	bool result = _dapper.Execute(sql);
	// 	if (result)
	// 	{
	// 		return Ok();
	// 	}
	// 	throw new Exception("Unable to Edit Post!");
	// }

	[HttpDelete("DeletePost/{postId}")]
	public bool deletePost(int postId)
	{
		string userId = User.FindFirst("userId")?.Value;
		string sql = @"EXEC TutorialAppSchema.spPost_delete @PostId =" + postId + ",@UserId = " + userId; ;
		return _dapper.Execute(sql);

	}

	[HttpGet("getAllPosts/{postId}/{userId}/{searchParams}")]
	public IEnumerable<Post> GetAllPost(int postId=0, int userId=0, string searchParams = "None")
	{

		string sql = @"EXEC TutorialAppSchema.spPost_Get";
		string parameter = "";
		if (postId != 0)
		{
			parameter += ", @PostId = " + postId.ToString();
		};
		if (userId != 0)
		{
			parameter += ", @UserId = " + userId.ToString();
		};
		if (searchParams != "None")
		{
			parameter += ", @searchParam = " + searchParams;
		}
		if (parameter.Length > 0)
		{
			sql += parameter.Substring(1);
		}
		return _dapper.LoadData<Post>(sql);
	}




	[HttpGet("getMyPosts")]
	public IEnumerable<Post> GetMyUsers()
	{
		string userId = User.FindFirst("userId")?.Value;
		return _dapper.LoadData<Post>("EXEC TutorialAppSchema.spPost_Get @UserId ="+userId);
		//return new string[] {"user1","user2",value};
	}


}