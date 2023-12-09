
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

	[HttpPost("AddPost")]

	public IActionResult Addpost(PostToAddDto post)
	{

		string userId = User.FindFirst("userId")?.Value;
		string sql = @"INSERT INTO TutorialAppSchema.Post (
										[UserId],
										[PostContent],
										[PostTitle],
										[PostCreated],
										[PostUpdate]
										) VALUES (" +
											"'" + Convert.ToInt32(userId) +
											"','" + post.PostContent +
											"','" + post.PostTitle +
											"',GETDATE(),GETDATE() );";

		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		throw new Exception("Unable to Add Post!");
	}
	[HttpPut
	("EditPost")]

	public IActionResult Editpost(PostToEditDto post)
	{
		string userId = User.FindFirst("userId")?.Value;
		string sql = @"UPDATE TutorialAppSchema.Post SET  
			PostContent = '" + post.PostContent + "',PostTitle = '" +
		 post.PostTitle + "',PostUpdate = GETDATE() WHERE PostId =" + post.PostId + "AND UserId = " + userId;


		bool result = _dapper.Execute(sql);
		if (result)
		{
			return Ok();
		}
		throw new Exception("Unable to Edit Post!");
	}

	[HttpDelete("DeletePost/{postId}")]
	public bool deletePost(int postId)
	{
		string userId = User.FindFirst("userId")?.Value;
		string sql = @"DELETE FROM 
						TutorialAppSchema.Post
						WHERE PostId =" + postId + "AND UserId = " + userId; ;
		return _dapper.Execute(sql);

	}

	[HttpGet("getAllPosts")]
	public IEnumerable<Post> GetAllPost()
	{
		return _dapper.LoadData<Post>("select * from TutorialAppSchema.Post");
		//return new string[] {"user1","user2",value};
	}

	[HttpGet("PostSingle/{postId}")]
	public Post GetUser(int postId)
	{
		return _dapper.LoadDataSingle<Post>("select * from TutorialAppSchema.Post where PostId = " + postId);
		//return new string[] {"user1","user2",value};
	}

	[HttpGet("getMyPosts")]
	public IEnumerable<Post> GetMyUsers()
	{
		string userId = User.FindFirst("userId")?.Value;
		return _dapper.LoadData<Post>("select * from TutorialAppSchema.Post WHERE UserId =  " + Convert.ToInt32(userId));
		//return new string[] {"user1","user2",value};
	}

	[HttpGet("SeachPost/{searchParam}")]
	public IEnumerable<Post> SearchPost(string searchParam)
	{

		return _dapper.LoadData<Post>(@"select * from TutorialAppSchema.Post
		 WHERE PostTitle LIKE  '%" + searchParam + "%' OR PostContent LIKE '%" + searchParam + "%'");
		//return new string[] {"user1","user2",value};
	}

}