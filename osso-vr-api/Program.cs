using osso_vr_api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "localCors",
                  policy =>
                  {
                      policy.WithOrigins("https://localhost:5173", "http://localhost:5173");
                  });
});

builder.Services.AddSingleton<IDataStore>(new DataStore("c:\\data\\result.json"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    

    app.UseCors("localCors");

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
