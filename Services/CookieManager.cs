
using Microsoft.Net.Http.Headers;

namespace trackventory_backend.Services
{
  public class CookieManager
  {
    private readonly HttpContext _httpContext;

    public CookieManager(HttpContext httpContext) {
      _httpContext = httpContext;
    }

    // check exist (private)
    private bool CookieExists(string key) => _httpContext.Request.Cookies.ContainsKey(key);

    // Add cookie

    public void AddCookie(string key, string value, int ExpiredTimeInMinutes) {
      var cookie = new CookieHeaderValue(key, value);

      var cookieOptions = new CookieOptions {
        HttpOnly = true,
        IsEssential = true,
        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
        Secure = true,
        Expires = DateTime.UtcNow.AddMinutes(ExpiredTimeInMinutes)
      };
      // https://medium.com/@nwonahr/working-with-sessions-and-cookies-in-asp-net-core-013b24037d91

      _httpContext.Response.Cookies.Append(key, value, cookieOptions);
    }

    // get cookie
    public string? GetCookie(string key) => _httpContext.Request.Cookies[key];

    // delete cookie
    public void DeleteCookie(string key) => _httpContext.Response.Cookies.Delete(key);


    // list cookie
    public IEnumerable<Object> ListCookies() {
      var cookies = _httpContext.Request.Cookies;

      return cookies.Select(c => new { Name = c.Key, Value = c.Value });

    }


    // Clear all cookie
    public void ClearAllCookies() {
      var cookies = _httpContext.Request.Cookies;
      foreach (var cookie in cookies) {
        DeleteCookie(cookie.Key);
      }
    }

    // set expiary date
  }
}
