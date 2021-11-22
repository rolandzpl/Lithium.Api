using Lithium.Api.AspNetCore;
using Lithium.Api.Accounts.AspNetCore;
using Lithium.Api.Blog.AspNetCore;
using Lithium.Api.Galleries.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAccountsSupport(builder.Configuration);
builder.Services.AddBlogSupport(builder.Configuration);
builder.Services.AddGalleriesSupport(builder.Configuration);
builder.Services.AddControllers(opt => opt.Filters.Add(new HttpResponseExceptionFilter()));
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
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAccess", policy => policy.RequireUserName("roland"));
    options.AddPolicy("EditorAccess", policy => policy.RequireRole("Editor"));
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
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
});
app.UseAuthentication();
app.UseAuthorization();
app.Configuration.Bind("Gallery", galleryConfiguration);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(galleryConfiguration.RootDirectory),
    RequestPath = galleryConfiguration.BaseUrl
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public class FakeEntryPoint { }