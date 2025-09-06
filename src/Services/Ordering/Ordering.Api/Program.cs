var builder = WebApplication.CreateBuilder(args);
//Configure services to the container
var app = builder.Build();

//Configure HttP request piprline;

app.Run();
