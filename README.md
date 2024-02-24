
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

Import the CorePagination.Extensions namespace to get started:

```csharp
using CorePagination.Extensions;
```

### Using `PaginateAsync`

`PaginateAsync` is a comprehensive pagination method that provides detailed pagination results, including total item and page counts. It is particularly useful for user interfaces that require detailed pagination controls.

#### Example with `PaginateAsync`

Below is an example demonstrating how to use `PaginateAsync` to paginate a list of `Product` entities:

```csharp
var context = new ApplicationDbContext();
var products = context.Products;

int pageNumber = 1;
int pageSize = 10;

var paginationResult = await products.PaginateAsync(pageNumber, pageSize);

// paginationResult includes:
// Items: List of products on the current page.
// TotalItems: Total count of products.
// TotalPages: Total number of pages.
// Page: Current page number.
// PageSize: Number of items per page.
```

### Using `SimplePaginateAsync`

`SimplePaginateAsync` provides a basic pagination mechanism without the total count of items or pages.

#### Example with `SimplePaginateAsync`

```csharp
var context = new ApplicationDbContext();
var products = context.Products;

int pageNumber = 1;
int pageSize = 10;

var paginationResult = await products.SimplePaginateAsync(pageNumber, pageSize);

// paginationResult includes:
// Items: Current page's list of products.
// Page: Current page number.
// PageSize: Number of items per page.
```

### Using `CursorPaginateAsync`

`CursorPaginateAsync` is ideal for efficient and stateful pagination, such as infinite scrolling.

#### Example with `CursorPaginateAsync`

```csharp
var context = new ApplicationDbContext();
var products = context.Products.OrderBy(p => p.Id);

int pageSize = 10;
int? currentCursorId = null;

var paginationResult = await products.CursorPaginateAsync(
    p => p.Id, pageSize, currentCursorId, PaginationOrder.Ascending);

// paginationResult includes:
// Items: List of products for the current segment.
// PageSize: Number of items per segment.
// Cursor: Current cursor position.
```

These examples aim to provide clear and concise guidance for using CorePagination effectively in your applications.

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
