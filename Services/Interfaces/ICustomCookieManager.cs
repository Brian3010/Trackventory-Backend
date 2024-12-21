namespace trackventory_backend.Services.Interfaces
{
  public interface ICustomCookieManager
  {

    public void AddCookie(string key, string value, int ExpiredTimeInMinutes);

    public string? GetCookie(string key);

    public void DeleteCookie(string key);

    public IEnumerable<KeyValuePair<string, string>> ListCookies();


    public void ClearAllCookies();
  }
}
