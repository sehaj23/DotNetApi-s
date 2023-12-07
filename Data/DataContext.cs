namespace DotnetAPI.Data;

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class DataContext
{
	private IConfiguration _config;
	public DataContext(IConfiguration config)
	{
		_config = config;
	}



	public IEnumerable<T> LoadData<T>(string sql)
	{
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection"));

		return db.Query<T>(sql);
	}
	public T LoadDataSingle<T>(string sql)
	{
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection"));

		return db.QuerySingle<T>(sql);
	}
	public bool Execute(string sql)
	{
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection")); ;

		return db.Execute(sql) > 0;
	}
	public int ExecuteWithRowCount(string sql)
	{
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection")); ;

		return db.Execute(sql);
	}

	public bool ExecuteWithSqlParameter(string sql, List<SqlParameter> parameters)
	{

		SqlCommand commandWithParams = new SqlCommand(sql);
		foreach (SqlParameter parameter in parameters)
		{
			commandWithParams.Parameters.Add(parameter);
		}
		
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection"));
		db.Open();
		commandWithParams.Connection = db;
		int rowsAffected = commandWithParams.ExecuteNonQuery();
		db.Close();

		return rowsAffected > 0;
	}
}