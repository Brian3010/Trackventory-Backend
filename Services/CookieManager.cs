
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
      // https://medium.com/@nwonahr/working-with-sessions-and-cookies-in-asp-net-core-013b24037d91

    }

    // delete cookie

    // Update Cookie

    // list cookie

    // get cookie

    // set cookie

    // Clear all cookie

    // set expiary date
  }
}
