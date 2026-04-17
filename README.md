  DinderMVC

  The backend REST API for the Dinder dining-matching app. DinderMVC handles user accounts, friend relationships, parties, meal catalogs, swipe choices, and token-based authentication. It is consumed by 
  DinderWeb (the web frontend) and any mobile clients.

  This repo also includes the DataStructure/ folder containing the DinderDLL source — the shared class library of DTOs, request/response models, and data models used across the Dinder ecosystem.

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Table of Contents

   1. Overview
   2. Domain Model
   3. API Endpoints
   4. Authentication
   5. Project Structure
   6. Technology Stack
   7. Running Locally

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Overview

  DinderMVC is an ASP.NET Core Web API following a hypermedia-driven (HATEOAS-style) design. Every response includes navigational links to related resources, and the root endpoint (GET /api/v1) returns a
  full directory of available endpoints. The API is versioned under /api/v1/.

  The app is documented with XML comments and ships with a DinderMVC.xml documentation file. An API.ods spreadsheet also serves as a manual reference for the API surface.

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Domain Model

  The core entities and their relationships:

   - User — registered account with a display name and credentials
   - UserFriend — a directional friendship link between two users, with an acceptance status
   - AppInstall — represents a registered device or client installation; used to scope token issuance
   - DinderToken — a bearer token associated with a user and app install, stored in the database with an expiration date
   - GlobalMeal — a platform-curated meal/dish in the shared catalog
   - UserMeal — a meal that a specific user has added to their personal list
   - Party — a group session where members collectively swipe on meals; has a status lifecycle (open, voting, decided, etc.)
   - PartyInvite — an invitation to join a party, with an acceptance/rejection status
   - PartyMeal — a meal that has been added to a party's voting pool
   - PartyChoice — a user's swipe decision (yes/no) on a meal within a party
   - SwipeChoice — lookup table for swipe direction values
   - PartySettingMatrix / PartySettingType / PartySettingValue — a flexible EAV-style configuration system for per-party settings

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  API Endpoints

  The API root returns navigational links to all available resources. Key endpoint groups:

  Token

   - GET /api/v1/Token/{AppInstallID} — exchange Basic auth credentials for a bearer token

  Users

   - Full CRUD on users, their meals, friends, and the parties they belong to

  Parties

   - Full CRUD on parties, invites, choices, meals, and settings

  Global Meals

   - Read-only access to the platform meal catalog

  Root / Discovery

   - GET /api — available API versions
   - GET /api/v1 — full list of v1 endpoints with HATEOAS links

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Authentication

  The API uses a two-scheme authentication model, both implemented as custom ASP.NET Core authentication handlers.

  Basic Authentication (BasicAuthenticationHandler) is used only for the token endpoint. The client sends a Base64-encoded username:password header. If valid, the handler populates the user's claims
  (including their GUID) and the request proceeds to TokenController.

  Bearer Authentication (BearerAuthenticationHandler) is used for all other endpoints. The client sends the token returned by the token endpoint. TokenValidationService validates it against the database, and
  BearerAuthenticationOptions carries the configuration for expiry and signing.

  Token issuance is handled by TokenIssuerService. When a new token is issued, all previous tokens for that user are deleted from the database — only one active token per user at a time.

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Project Structure

   - Controllers/ — RootController (discovery), TokenController, UsersController (37 KB), PartiesController (46 KB), GlobalMealsController; all inherit from a DinderControllerBase that provides shared 
  logging helpers
   - Authentication/ — BasicAuthenticationHandler, BearerAuthenticationHandler, TokenIssuerService, TokenValidationService, and their options classes
   - DataStructure/ — source for DinderDLL: entity classes, EF Core configurations, DTOs, and the DinderDLL.csproj project file
   - DMs/ — Domain Model wrappers that map EF entities to DTOs for serialisation
   - COs/ — Command Objects used to compose response payloads (e.g. PartySettingsViewCO, PartyInviteViewCO)
   - Models/ — DinderContext (EF DbContext), DapperQueries and Queries for raw SQL alongside EF, and request/response extension methods
   - Services/ — APIServices (HttpClient factory), UserIdentity (current user resolution)

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Technology Stack

   - .NET (ASP.NET Core Web API)
   - Entity Framework Core (primary ORM)
   - Dapper (used alongside EF for performance-sensitive queries)
   - SQL Server
   - Custom Basic and Bearer authentication handlers
   - XML documentation

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Running Locally

  The API expects a SQL Server instance. The default connection string in appsettings.json targets a local instance:

   server=DESKTOP-ITC50UT\MSSQLSERVER01;database=Dinder-Alpha

  Update AppSettings.ConnectionString in appsettings.json to point to your own SQL Server instance before running. A commented-out Azure SQL connection string is also present in the config file as a
  reference for cloud deployment.

   dotnet run

  The API will be available at https://localhost:44362/. Browse to GET /api/v1 to see all available endpoints.

 ~           
