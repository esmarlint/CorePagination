
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
var paginatedResultWithUrls = await myDbContext.Entities.PaginateUrlsAsync(page, pageSize, baseUrl);
```

## Customization

CorePagination is extensible, allowing you to create your own paginator implementations if you need specific behaviors. Simply implement the `IPaginator<T>` interface in your custom class.

## Contributing

Contributions are welcome! If you have ideas for improving the library or have found a bug, feel free to create an issue or submit a pull request.

## License

CorePagination is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute it as you see fit.
