using Lithium.Api.Gallery.Controllers;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<GalleryConfiguration>(sp =>
    builder.Configuration.GetSection("Gallery").Get<GalleryConfiguration>());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("My-Request-Header");
    logging.ResponseHeaders.Add("My-Response-Header");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();
app.UseHttpLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(_ => _
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod());
var galleryConfiguration = app.Services.GetRequiredService<GalleryConfiguration>();
app.Configuration.Bind("Gallery", galleryConfiguration);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(galleryConfiguration.RootDirectory),
    RequestPath = galleryConfiguration.BaseUrl,
    OnPrepareResponse = _ =>
    {
        var fi = _.File;
    }
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
