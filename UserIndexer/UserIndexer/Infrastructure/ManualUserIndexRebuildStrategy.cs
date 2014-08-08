using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance.Strategies;
using Sitecore.Security.Accounts;
using UserIndexer.Model;
using UserIndexer.Model.Factories;
using UserIndexer.Model.Repositories;

namespace UserIndexer.Infrastructure
{
  public class ManualUserIndexRebuildStrategy : IIndexUpdateStrategy
  {
    private readonly SitecoreUserRepository _forumUserRepository = new SitecoreUserRepository();
    private readonly SitecoreUserFactory _forumUserFactory = new SitecoreUserFactory();

    public void Initialize(ISearchIndex index)
    {
    }

    public void AddToIndex(User user)
    {
      IndexableSitecoreUser indexableUser = _forumUserFactory.Create(user);
      _forumUserRepository.AddUser(indexableUser);
    }

    public void DeleteFromIndex(IIndexableUniqueId id)
    {
      _forumUserRepository.DeleteUser(id);
    }
  }
}