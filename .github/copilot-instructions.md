# Project: mono.shop

## Stack
- Backend: C#, ASP.NET Core (Program.cs / Startup.cs present) targeting .NET (6+ assumed)
- Data access: Entity Framework Core (StoreContext, migrations in Infrastructure/Identity/Data/Migrations)
- Architecture: Clean-ish separation into Core, Infrastructure, API projects
- Frontend: Angular (client/ directory, TypeScript)
- Utilities: AutoMapper (API/Helpers/MappingProfiles), JWT authentication (Identity/AppIdentityDbContext, TokenService)

## Architecture
- Core/
  - Contains domain entities, DTO interfaces, specifications and service interfaces.
  - Must remain free of Infrastructure or API dependencies.
- Infrastructure/
  - Contains EF Core DbContexts (StoreContext, AppIdentityDbContext), repositories (GenericRepository, ProductRepository), migrations, and service implementations (OrderService, TokenService, NovaPoshtaService).
  - Only layer that talks to the database or external services.
- API/
  - ASP.NET Core HTTP surface: Controllers, DTOs, middleware (ExceptionMiddleware), mapping profiles, DI wiring (ApplicationServicesExtensions, IdentityServiceExtensions).
  - Controllers should be thin: parse request, call Core-defined service interfaces (implemented in Infrastructure), return HTTP results.
- client/
  - Angular app: UI, HTTP clients, interceptors, guards. Communicates with API only.

## Rules (specific)
1. Business logic and data access live in Infrastructure (implementing interfaces in Core). Controllers must call service interfaces from Core, not perform business work.
2. Repositories and DbContext usage belong only in Infrastructure/Data. Use the UnitOfWork (Infrastructure/UnitOfWork.cs) or repository interfaces (IProductRepository, IBasketRepository) from services.
3. Use Specifications (Core/Specifications) and SpecificationEvaluator (Infrastructure/Data/SpecificationEvaluator.cs) to encapsulate query filtering and paging; controllers should not construct EF queries.
4. Mapping between domain models and external shapes must use AutoMapper profiles under API/Helpers/MappingProfiles.cs; controllers should accept/return DTOs (API/Dtos) only.
5. Register dependencies in API Startup (Extensions/ApplicationServicesExtensions.cs). Do not new-up services or DbContexts inside controllers — rely on DI.

## Forbidden
- Do NOT put filtering, calculations, or transactional business logic inside API/Controllers.
- Forbidden: Do NOT add project references from Core to Infrastructure or API (Core must be dependency-free of implementation projects).
- Do NOT access StoreContext or AppIdentityDbContext directly from controllers or Core.

## Example (correct pattern)
// API/Controllers/OrdersController.cs (excerpt)
[HttpPost]
public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
{
    var result = await _orderService.CreateOrderAsync(User.GetUserId(), orderDto);
    if (result == null) return BadRequest("Problem creating order");
    return Ok(result);
}

// Core/Interfaces/IOrderService.cs
public interface IOrderService
{
    Task<OrderToReturnDto> CreateOrderAsync(string buyerId, OrderDto orderDto);
}

// Infrastructure/Services/OrderService.cs (excerpt)
public async Task<OrderToReturnDto> CreateOrderAsync(string buyerId, OrderDto orderDto)
{
    // use repositories via IUnitOfWork, apply specs for queries, map domain->dto via AutoMapper
}
