using Sitecore.ContentSearch;

namespace UserIndexer.Model
{
  public class IndexableSitecoreUser : IndexableObject
  {
    [IndexField("_id")]
    public override IIndexableId Id
    {
      get
      {
        return new IndexableId<string>(UserName);
      }
    }

    [IndexField("_uniqueid")]
    public override IIndexableUniqueId UniqueId
    {
      get
      {
        return new IndexableUniqueId<string>(UserName.ToLowerInvariant().Replace("\\", ""));
      }
    }

    [IndexField("UserName")]
    public string UserName
    {
      get;
      internal set;
    }

    [IndexField("Email")]
    public string Email
    {
      get;
      internal set;
    }
  }
}