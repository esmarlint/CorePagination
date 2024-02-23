
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

# CorePagination Usage Examples

## Simple Paginator Example

```csharp
using CorePagination.Paginators.SimplePaginator;
using System.Linq;
using System.Threading.Tasks;

var context = GetYourDbContext();  // Replace with your method to obtain DbContext

// Simple pagination applying a filter
var paginator = new SimplePaginator<Entity>();
var entities = context.Entities.Where(x => x.IsActive);
var result = await paginator.PaginateAsync(entities, new PaginatorParameters { Page = 1, PageSize = 10 });
```

## PaginateAsync Extension Method Example

```csharp
using CorePagination.Extensions;
using System.Linq;
using System.Threading.Tasks;

var context = GetYourDbContext(); // Obtain your database context

// Direct usage of PaginateAsync on a query
var result = await context.Entities
    .Where(entity => entity.Category == "Category1")
    .PaginateAsync(pageSize: 10, pageNumber: 1);
```

## CursorPaginateAsync Example

```csharp
using CorePagination.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

var context = GetYourDbContext(); // Get your database context

// Cursor-based pagination directly on a query
var result = await context.Entities
    .Where(entity => entity.Status == "Active")
    .CursorPaginateAsync(
        keySelector: x => x.Id,
        pageSize: 5,
        currentCursor: 10, // Assuming an integer-based cursor
        order: PaginationOrder.Ascending);
```

## Pagination Transformer Example

```csharp
using CorePagination.Extensions;
using CorePagination.Tranformation.Transformers;
using System.Linq;
using System.Threading.Tasks;

var context = GetYourDbContext(); // Database context

// Assuming you already have a pagination result
var paginationResult = await context.Entities
    .Where(x => x.IsActive)
    .PaginateAsync(pageSize: 10, pageNumber: 1);

// Apply a transformer to the result
var baseUrl = "http://example.com/api/entities";
var transformer = new SimpleUrlResultTransformer<Entity>(baseUrl);
var urlResult = transformer.Transform(paginationResult);
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
