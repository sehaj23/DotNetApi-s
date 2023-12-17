namespace DotnetAPI.Data;

using System.Data;
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

	public bool ExecuteWithSqlParameter(string sql, DynamicParameters parameters)
	{
	
		
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection"));
		return db.Execute(sql,parameters) > 0;

	
	}
	public IEnumerable<T> LoadDataWithParameters<T>(string sql,DynamicParameters dynamicParameters)
	{
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection"));

		return db.Query<T>(sql,dynamicParameters);
	}
	public T LoadDataSinglWithParamterse<T>(string sql,DynamicParameters dynamicParameters)
	{
		SqlConnection db = new SqlConnection(_config.GetConnectionString("defaultConnection"));

		return db.QuerySingle<T>(sql,dynamicParameters);
	}
}