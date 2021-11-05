## Introduction

API project for delivering content on web pages. It supports:
- blogging
- galleries

__Check swagger to find out the current API capabilities.__
## Database

Brief memo on how to work with database and Entity Framework:
```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## TODO:

- [ ] Authentication
- [ ] Authorization
- [ ] Serving images in galleries
