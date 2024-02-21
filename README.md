
# CorePagination

CorePagination is a lightweight and easy-to-use pagination library designed specifically for Entity Framework Core (EF Core). It offers a straightforward and extensible way to add pagination functionality to your .NET projects, improving efficiency and management of large datasets.

## Features

- **Easy to Integrate**: Seamlessly integrates with any EF Core project.
- **Semantic and Short Methods**: Makes the code understandable and easy to use.
- **Extensible**: Allows for customizations and extensions to fit specific needs.
- **Support for Pagination URLs**: Generates URLs for page navigation, ideal for REST APIs.
- **Asynchronous by Nature**: Designed for asynchronous operations, leveraging EF Core capabilities.

## Requirements

- .NET Core 3.1 or higher
- Entity Framework Core 3.1 or higher

## Basic Usage

To start using CorePagination, you simply need to call the `PaginateAsync` extension method on your `IQueryable` query.

```csharp
using CorePagination;
using Microsoft.EntityFrameworkCore;

var page = 1;
var pageSize = 10;

var paginatedResult = await dbContext.Entities.PaginateAsync(page, pageSize);
```

### Pagination with URLs

To use pagination that includes URLs for page navigation, you can use `PaginatorWithUrls` as follows:

```csharp
var baseUrl = "http://myapi.com/entities";
var paginatedResultWithUrls = await dbContext.Entities.PaginateUrlsAsync(page, pageSize, baseUrl);
```

## Upcoming Changes

🚀 **Version 0.2.0 Update Announcement**: We are excited to announce that CorePagination will soon be updated to version 0.2.0, bringing significant improvements and new features. Stay tuned for the release!

## Roadmap to Version 1.0

For the upcoming 1.0 release, we are planning to include:

- **Enhanced Validations**: Adding guards and validations across methods to ensure robustness and handle nulls more effectively.
- **Configurable Paginators**: Introducing stateful paginators with default configuration options.
- **Comprehensive Documentation**: Expanding code documentation and providing detailed usage examples in both English and Spanish.
- **NuGet and GitHub Packaging**: Making the library easily accessible and distributable through NuGet and GitHub packages.
- **Unit Testing**: Strengthening the library with thorough unit tests.
- **Branding**: Adding a logo to give CorePagination a unique identity.
- **Continuous Improvement**: We are committed to continuously improving the library based on community feedback and evolving needs.

### Beyond Version 1.0

- **Version 2.0 and Future Releases**: We are already planning ahead for version 2.0 and beyond, focusing on performance benchmarks and testing to ensure CorePagination remains efficient and scalable.

## Contributing

Contributions are welcome! If you have ideas for improving the library or have found a bug, feel free to create an issue or submit a pull request.

## License

CorePagination is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute it as you see fit.
