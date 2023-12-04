using DotnetAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository,UserRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors((options) =>
{
	options.AddPolicy("DevCors", (corsBuilder) =>
	{
		corsBuilder.WithOrigins("http://localhost:5000", "https://localhost:5001").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
	});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseCors("DevCors");
}
else
{
	app.UseHttpsRedirection();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
