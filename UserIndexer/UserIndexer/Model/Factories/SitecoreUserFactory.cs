using System.Globalization;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;

namespace UserIndexer.Model.Factories
{
  public class SitecoreUserFactory
  {
    public IndexableSitecoreUser Create(User sitecoreUser)
    {
      Assert.ArgumentNotNull(sitecoreUser, "sitecoreUser");
      return new IndexableSitecoreUser
      {
        UserName = sitecoreUser.Profile.UserName,
        Email = sitecoreUser.Profile.Email ?? "_no_email_",
        Culture = sitecoreUser.Profile.Culture ?? CultureInfo.InvariantCulture
      };
    }
  }
}