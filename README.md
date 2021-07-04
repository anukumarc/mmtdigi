# Order Status API
The repository contains a simple Web API developed using asp.net core.  I have chosen Dapper over Entity Framework to create the database context which is flexible enough to accommodate future modifications to the database while being faster and lightweight.  The Unit Tests project uses 'Fluent Assertions' nuget package for assertions.  I would sugggest to optimize the solution with the following for production environments:

  1. Authentication using JWT tockens.
  2. Logging using ILogger or Serilog.
  3. Implement CORS policies to prevent from Cross-Site Scripting(XSS) attacks.
  4. Enable AntiForgeryTokens to prevent Cross-Site Request Forgery(CSRF) attacks.
  5. Move connection strings to appSettings.json file and encrypt them.
  6. Enanble Docker support for containeraized deployments
  7. Integrate with Azure DevOps for Continuous Integration and Deployments (CI/CD)
