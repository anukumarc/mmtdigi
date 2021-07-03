# Order Status API
The repository contains a simple Web API developed using asp.net core. The solution need to be optimized with the following for production environments:

  1. Authentication using JWT tockens.
  2. Logging using ILogger or Serilog.
  3. Implement CORS policies to prevent from Cross-Site Scripting(XSS) attacks.
  4. Enable AntiForgeryTokens to prevent Cross-Site Request Forgery(CSRF) attacks.
  5. Move connection strings to appSettings.json file and encrypt them.
