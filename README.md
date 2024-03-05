![Banner](docs/images/banner.png)

# CorePagination

CorePagination is a lightweight and easy-to-use pagination library designed specifically for Entity Framework Core (EF Core). It offers a straightforward and extensible way to add pagination functionality to your .NET projects, improving efficiency and management of large datasets. By leveraging CorePagination, developers can avoid the complexity of creating custom pagination logic from scratch, benefiting from pre-built methods that are optimized for performance and flexibility. Additionally, the library's support for transformers, like the `UrlResultTransformer`, provides out-of-the-box solutions for common pagination challenges, such as generating navigation URLs, thereby saving development time and enhancing user experience. This makes CorePagination an ideal choice for projects that require reliable, scalable, and easy-to-implement pagination solutions.

## Table of Contents

- [Introduction](#CorePagination)
- [Features](#features)
- [Requirements](#requirements)
- [CorePagination Usage Examples](#corepagination-usage-examples)
    - [Using `PaginateAsync`](#using-paginateasync)
    - [Using `SimplePaginateAsync`](#using-simplepaginateasync)
    - [Using `CursorPaginateAsync`](#using-cursorpaginateasync)
- [Paginators](#paginators)
    - [SimplePaginator](#simplepaginator)
    - [SizeAwarePaginator](#sizeawarepaginator)
    - [CursorPaginator](#cursorpaginator)
    - [Creating Your Own Paginator](#creating-your-own-paginator)
- [Transformers](#transformers-in-corepagination)
    - [What are Transformers?](#what-are-transformers)
    - [Using Existing Transformers](#using-existing-transformers)
    - [Using Inline Transformations](#using-inline-transformations)
    - [Creating and Using Your Own Transformers](#creating-and-using-your-own-transformers)
- [Upcoming Changes](#upcoming-changes)
- [Roadmap to Version 1.0](#roadmap-to-version-10)
- [Contributing](#contributing)
- [License](#license)

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

//paginationResult: SizeAwarePaginationResult<Product>
var paginationResult = await products.PaginateAsync(pageNumber, pageSize);

// paginationResult includes:
// Items: List of products on the current page.
// TotalItems: Total count of products.
// TotalPages: Total number of pages.
// Page: Current page number.
// PageSize: Number of items per page.
```

### Using `SimplePaginateAsync`

`SimplePaginateAsync` provides a basic pagination mechanism without the total count of items or pages, typically offering faster performance than PaginateAsync by eliminating the need for total count calculations.

#### Example with `SimplePaginateAsync`

```csharp
var context = new ApplicationDbContext();
var products = context.Products;

int pageNumber = 1;
int pageSize = 10;

//paginationResult: PaginationResult<Product>
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

# Paginators

Paginators are the core components of the CorePagination library, designed to abstract the complexity of pagination logic, making it easy and efficient to paginate large datasets. They provide a robust and flexible framework for implementing various pagination strategies, tailored to different application requirements and optimization needs. While paginators represent the core machinery for pagination, extensions are provided to streamline their use in everyday coding, offering a simpler interface that abstracts away some of the underlying complexities.

## Using Paginators

To leverage paginators directly in your application, you first need to understand the available paginator types and how to apply them according to your specific data retrieval and presentation needs.

### SimplePaginator

The `SimplePaginator` provides basic pagination functionality, fetching a specified page of data without computing the total number of items or pages. This approach is particularly efficient when you do not need to display total counts in your UI.

#### Example with `SimplePaginator`:

```csharp
var context = new ApplicationDbContext();
var productsQuery = context.Products.AsQueryable();
var simplePaginator = new SimplePaginator<Product>();

int pageNumber = 1;
int pageSize = 10;

var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
var simplePaginationResult = await simplePaginator.PaginateAsync(productsQuery, parameters);
```

This result provides a straightforward set of items for the specified page, alongside basic pagination metadata like page number and page size.

### SizeAwarePaginator

The `SizeAwarePaginator` extends the basic pagination functionality by also computing the total number of items and the total number of pages. This paginator is suitable for interfaces that require detailed pagination controls.

#### Example with `SizeAwarePaginator`:

```csharp
var context = new ApplicationDbContext();
var productsQuery = context.Products.AsQueryable();
var sizeAwarePaginator = new SizeAwarePaginator<Product>();

int pageNumber = 1;
int pageSize = 10;

var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
var sizeAwarePaginationResult = await sizeAwarePaginator.PaginateAsync(productsQuery, parameters);
```

In this example, `sizeAwarePaginationResult` includes detailed pagination information, facilitating the creation of more informative and interactive UI pagination components.

### CursorPaginator

The `CursorPaginator` is ideal for scenarios where continuous data loading is required, such as infinite scrolling or cursor-based navigation. It paginates data based on a cursor, typically an identifier or a timestamp, allowing for efficient retrieval of subsequent data segments.

#### Example with `CursorPaginator`:

```csharp
var context = new ApplicationDbContext();
var productsQuery = context.Products.OrderBy(p => p.Id).AsQueryable();
var cursorPaginator = new CursorPaginator<Product, int>(p => p.Id);

int pageSize = 10;
int? currentCursor = null;

var cursorPaginationResult = await cursorPaginator.PaginateAsync(productsQuery, new CursorPaginationParameters<int> { PageSize = pageSize, CurrentCursor = currentCursor });
```

Here, `cursorPaginationResult` provides not only the current page of items but also the cursor for accessing the next segment, optimizing data loading for user experiences that require seamless data fetching.

### Creating Your Own Paginator

To create your own paginator, you need to implement the `IPagination<T, TParameters, TResult>` interface. This custom paginator can then be tailored to specific requirements, such as a unique pagination strategy or data source.

#### Example: Creating a Custom Paginator:

```csharp
public class MyCustomPaginator<T> : IPagination<T, MyCustomPaginatorParameters, MyCustomPaginationResult<T>>
{
    public async Task<MyCustomPaginationResult<T>> PaginateAsync(IQueryable<T> query, MyCustomPaginatorParameters parameters)
    {
        // Implement your custom pagination logic here
        var items = await query.Skip(parameters.Page * parameters.PageSize).Take(parameters.PageSize).ToListAsync();

        return new MyCustomPaginationResult<T>
        {
            Items = items,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            // Add additional pagination-related information if needed
        };
    }
}
```

In this example, `MyCustomPaginator` provides a template for implementing pagination logic that fits your specific needs, offering flexibility beyond the built-in paginators.


### Transformers in CorePagination

#### What are transformers

Transformers in CorePagination are designed to modify or enhance the pagination results, allowing for additional data manipulation or formatting tailored to specific requirements. They provide a powerful way to adapt the paginated results into different formats or structures, facilitating their integration into various application contexts.

### Using Existing Transformers

CorePagination includes a set of predefined transformers that can be applied to your pagination results for common use cases. For instance, the `UrlResultTransformer` can be used to enrich pagination results with navigational URLs, which is particularly useful for web APIs.

#### Example: Applying `UrlResultTransformer`

```csharp
var context = new ApplicationDbContext();
var products = context.Products;
string baseUrl = "http://example.com/products";

var paginationResult = await products.PaginateAsync(pageNumber, pageSize);
var urlPaginationResult = paginationResult.Transform(new UrlResultTransformer<Product>(baseUrl));
```

In this example, `UrlResultTransformer` is used to append navigation URLs to the pagination result, enhancing its integration capabilities for client-side applications or APIs.

### Using Inline Transformations

Inline transformations allow you to apply custom transformations directly within your code, offering a quick and flexible way to adjust the output of pagination results.

#### Example: Inline Transformation

```csharp
var paginationResult = await products.PaginateAsync(pageNumber, pageSize);
var customResult = paginationResult.Transform(result => new {
    SimpleItems = result.Items.Select(item => new { item.Id, item.Name }),
    result.Page,
    result.PageSize,
    result.TotalItems
});
```

This inline transformation simplifies the paginated result, selecting only the `Id` and `Name` from each item, which might be particularly useful for reducing payload sizes in API responses.

### Creating and Using Your Own Transformers

You can extend CorePagination by creating your own transformers, implementing the `IPaginationTransformer<T, TResult>` interface to define custom logic for transforming pagination results.

#### Example: Creating a Custom Transformer

```csharp
public class MyCustomTransformer : IPaginationTransformer<Product, MyCustomProductResult>
{
    public MyCustomProductResult Transform(IPaginationResult<Product> paginationResult)
    {
        // Custom transformation logic
        return new MyCustomProductResult {
            CustomItems = paginationResult.Items.Select(item => new CustomItem { ... }),
            paginationResult.TotalItems
        };
    }
}
```

```csharp
var paginationResult = await products.PaginateAsync(pageNumber, pageSize);
var myCustomResult = paginationResult.Transform(new MyCustomTransformer());
```

This section demonstrates how to create a `MyCustomTransformer` that applies specific transformation logic to the pagination results, illustrating the extensibility of CorePagination for various application needs.

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

CorePagination is licensed under the [Apache License](LICENSE). Feel free to use, modify, and distribute it as you see fit.
