using System;
namespace Order.API.Common
{
    public static class Constants
    {
        public const string CUSTOMERAPI_BASE_URL = "https://customer-account-details.azurewebsites.net";
        public const string CUSTOMERAPI_REQUEST_URL = "/api/GetUserDetails?code=1CrsOooSHlV15C7OYnLY0DHjBHyjzoI8LNHITV04cNCyNCahecPDhw==";
        public const string CONNECTION_STRING = "Server=tcp:sse.database.windows.net,1433;Initial Catalog=SSE_Test;Persist Security Info=False;User ID=mmt-sse-test;Password=database-user-01;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
