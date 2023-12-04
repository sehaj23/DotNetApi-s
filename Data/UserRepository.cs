using AutoMapper;
using DotnetAPI.Models;

namespace DotnetAPI.Data;

public class UserRepository : IUserRepository
{
	DataContextEF entityFrameWork;

	private readonly IConfiguration config;
	public UserRepository(IConfiguration config)
	{
		entityFrameWork = new DataContextEF(config);

	}
	public bool SaveChanges()
	{
		return entityFrameWork.SaveChanges() > 0;
	}
	public void AddEntity<T>(T entity)
	{
		if (entity != null)
		{
			entityFrameWork.Add(entity);
		}
	}
	public void RemoveEntity<T>(T entity)
	{
		if (entity != null)
		{
			entityFrameWork.Remove(entity);
		}
	}

	public IEnumerable<User> GetAllUsers()
	{
		return entityFrameWork.Users.ToList();

	}
	
	public User GetSingleUser(int userId)
	{
		User? user = entityFrameWork.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
		if (user != null)
		{
			return user;
		}
		throw new Exception("User Not Found");
	}
	
	public UsersSalary GetSingleUserSalary(int userId)
	{
		UsersSalary usersSalary = entityFrameWork.UsersSalary.Where(u => u.UserId == userId).FirstOrDefault<UsersSalary>();
		if (usersSalary != null)
		{
			return usersSalary;
		}
		throw new Exception("User Salary Not Found");
	}
	public IEnumerable<UsersSalary> GetAllUsersSalary()
	{
		return entityFrameWork.UsersSalary.ToList();

	}

  
}