# ASPNetCoreApi.Infrastructure
Extension methods to assist with configuration ASP.Net Core API sites.

## Dependencies

AspNetCoreApi.Infrastructure is a .Net Standard 2.1 class library that has the following dependencies.

This is not a full-dependency tree, but just the major dependencies listed as close to the top of the tree as possible.

- [AspNetCore.Mediatr](https://www.nuget.org/packages/AspNetCore.Mediatr/): Mediatr configuration
  - [MediatR](https://www.nuget.org/packages/MediatR/)
  - [EntityManagement](https://www.nuget.org/packages/EntityManagement/): `EntityFramework.Core` repository pattern
  - [EntityManagement.Core](https://www.nuget.org/packages/EntityManagement.Core/): Base entites and interfaces for `EntityManagment` written in a DDD-style
    - [FluentValidation](https://www.nuget.org/packages/FluentValidation/): Used for validation rules in domain entities
  - [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
- [Serilog](https://www.nuget.org/packages/Serilog/)
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/)
