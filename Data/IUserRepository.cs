using DotnetAPI.Models;

namespace DotnetAPI.Data;

public interface IUserRepository
{
	public bool SaveChanges();
	public void AddEntity<T>(T entity);
	public void RemoveEntity<T>(T entity);

	public IEnumerable<User> GetAllUsers();

	public User GetSingleUser(int userId);

	public UsersSalary GetSingleUserSalary(int userId);

	public IEnumerable<UsersSalary> GetAllUsersSalary();
};

