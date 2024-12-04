namespace trackventory_backend.Services
{
  public class CookieManager
  {
    private readonly HttpContext _httpContext;

    public CookieManager(HttpContext httpContext) {
      _httpContext = httpContext;
    }

    // check exist (private)
    private bool CookieExists(string name) {

      return false;
    }

    // Add cookie

    // delete cookie

    // Update Cookie

    // list cookie

    // get cookie

    // set cookie

    // Clear all cookie

    // set expiary date
  }
}
