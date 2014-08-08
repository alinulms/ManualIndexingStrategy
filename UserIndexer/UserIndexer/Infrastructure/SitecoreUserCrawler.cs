using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.Events;
using Sitecore.Security.Domains;
using UserIndexer.Model;
using UserIndexer.Model.Factories;

namespace UserIndexer.Infrastructure
{
  public class SitecoreUserCrawler : IProviderCrawler
  {
    private ISearchIndex _index;
    private IIndexOperations _operations;

    public void Initialize(ISearchIndex index)
    {
      _index = index;
      _operations = new PropertyIndexOperations();
    }

    public void RebuildFromRoot(IProviderUpdateContext context, IndexingOptions indexingOptions = IndexingOptions.Default)
    {
      try
      {
        foreach (IndexableSitecoreUser user in GetAllUsers())
        {
          Event.RaiseEvent("indexing:adding", _index.Name, user.Email);
          _operations.Add(user, context, _index.Configuration);
          Event.RaiseEvent("indexing:added", _index.Name, user.Email);
        }
      }
      catch (Exception ex)
      {
        CrawlingLog.Log.Error(string.Format("{0}: Error rebuilding index \"{1}\" from root.", GetType(), _index.Name), ex);
      }
    }

    private IEnumerable<IndexableSitecoreUser> GetAllUsers()
    {
      SitecoreUserFactory factory = new SitecoreUserFactory();
      var domain = Domain.GetDomain("extranet");
      var users = domain.GetUsers().ToArray();
      return users.ToArray().Select(factory.Create);
    }

    public void RefreshFromRoot(IProviderUpdateContext context, IIndexable indexableStartingPoint, IndexingOptions indexingOptions = IndexingOptions.Default)
    {
      CrawlingLog.Log.Warn(string.Format("[Index={0}] Refresh From Root Requested - Update is not supported for SitecoreUserCrawler", _index.Name));
    }

    public void Update(IProviderUpdateContext context, IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions = IndexingOptions.Default)
    {
      CrawlingLog.Log.Warn(string.Format("[Index={0}] Update Requested - Update is not supported for SitecoreUserCrawler", _index.Name));
    }

    public void Delete(IProviderUpdateContext context, IIndexableId indexableId, IndexingOptions indexingOptions = IndexingOptions.Default)
    {
      CrawlingLog.Log.Warn(string.Format("[Index={0}] Delete Requested - Update is not supported for SitecoreUserCrawler", _index.Name));
    }

    public void Delete(IProviderUpdateContext context, IIndexableUniqueId indexableUniqueId, IndexingOptions indexingOptions = IndexingOptions.Default)
    {
      CrawlingLog.Log.Warn(string.Format("[Index={0}] Delete Requested - Update is not supported for SitecoreUserCrawler", _index.Name));
    }

    public void StopIndexing()
    {
      CrawlingLog.Log.Warn(string.Format("[Index={0}] Stop Indexing Requested - Update is not supported for SitecoreUserCrawler", _index.Name));
    }

    public void PauseIndexing()
    {
      CrawlingLog.Log.Warn(string.Format("[Index={0}] Pause Indexing Requested - Update is not supported for SitecoreUserCrawler", _index.Name));
    }

    public void ResumeIndexing()
    {
      CrawlingLog.Log.Warn(string.Format("[Index={0}] Resume Indexing Requested - Update is not supported for SitecoreUserCrawler", _index.Name));
    }
  }
}