using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Security;
using Sitecore.Events;

namespace UserIndexer.Model.Repositories
{
  public class SitecoreUserRepository
  {
    private const string SitecoreUserIndexName = "sitecore_user_index";

    public IEnumerable<IndexableSitecoreUser> Get()
    {
      using (IProviderSearchContext context = CreateSearchContext())
      {
        return context.GetQueryable<IndexableSitecoreUser>().ToArray();
      }
    }

    public IProviderDeleteContext CreateDeleteContext()
    {
      return ContentSearchManager.GetIndex(SitecoreUserIndexName).CreateDeleteContext();
    }

    public IProviderUpdateContext CreateUpdateContext()
    {
      return ContentSearchManager.GetIndex(SitecoreUserIndexName).CreateUpdateContext();
    }

    private IProviderSearchContext CreateSearchContext()
    {
      return ContentSearchManager.GetIndex(SitecoreUserIndexName).CreateSearchContext(SearchSecurityOptions.DisableSecurityCheck);
    }

    public void AddUser(IndexableSitecoreUser user)
    {
      IndexableSitecoreUser forumUser;
      using (IProviderSearchContext context = CreateSearchContext())
      {
        forumUser = context.GetQueryable<IndexableSitecoreUser>().FirstOrDefault(searchResultItem => searchResultItem.UserName.Contains(user.UserName));
      }
      if (forumUser != null)
        return;
      using (IProviderUpdateContext updateContext = CreateUpdateContext())
      {
        var index = ContentSearchManager.GetIndex(SitecoreUserIndexName);
        Event.RaiseEvent("indexing:adding", index.Name, user.Email);
        new PropertyIndexOperations().Add(user, updateContext, index.Configuration);
        Event.RaiseEvent("indexing:added", index.Name, user.Email);
        updateContext.Commit();
      }
    }
 
    public void DeleteUser(IIndexableUniqueId userName)
    {
      using (IProviderDeleteContext deleteContext = CreateDeleteContext())
      {
        deleteContext.Delete(userName);
        deleteContext.Commit();
      }
    }
  }
}