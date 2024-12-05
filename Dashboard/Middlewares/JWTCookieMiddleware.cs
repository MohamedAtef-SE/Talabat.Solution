namespace Dashboard.Middlewares
{
    public class JWTCookieMiddleware(RequestDelegate _next)
    {
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.ContainsKey("jwt_token"))
            {
                var token = httpContext.Request.Cookies["jwt_token"];

                if (!String.IsNullOrEmpty(token))
                {
                    httpContext.Request.Headers["Authorization"] = $"Bearer {token}";
                }
            }
            await _next.Invoke(httpContext);
        }
    }
}
