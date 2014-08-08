using Lucene.Net.Documents;
using Sitecore.ContentSearch;
using Sitecore.Diagnostics;


namespace UserIndexer.Model
{
  public class PropertyIndexOperations : IIndexOperations
  {
    public bool LowerCaseFieldNames
    {
      get;
      set;
    }

    public PropertyIndexOperations()
    {
      LowerCaseFieldNames = true;
    }

    public void Add(IIndexable indexable, IProviderUpdateContext context, ProviderIndexConfiguration indexConfiguration)
    {
      Assert.ArgumentNotNull(indexable, "indexable");
      Assert.ArgumentNotNull(context, "context");
      Assert.ArgumentNotNull(indexConfiguration, "indexConfiguration");
      Document document = new Document();
      foreach (IIndexableDataField field in indexable.Fields)
      {
        string name = LowerCaseFieldNames ? field.Name.ToLowerInvariant() : field.Name;
        string value = field.Value == null ? string.Empty : field.Value.ToString();
        document.Add(new Field(name, value, Field.Store.YES, Field.Index.ANALYZED));
      }
      context.AddDocument(document, new CultureExecutionContext(indexable.Culture));
    }

    public void Delete(IIndexableUniqueId indexableUniqueId, IProviderUpdateContext context)
    {
      Assert.ArgumentNotNull(indexableUniqueId, "indexableUniqueId");
      Assert.ArgumentNotNull(context, "context");
      context.Delete(indexableUniqueId);
    }

    public void Delete(IIndexableId id, IProviderUpdateContext context)
    {
      Assert.ArgumentNotNull(id, "id");
      Assert.ArgumentNotNull(context, "context");
      context.Delete(id);
    }

    public void Delete(IIndexable indexable, IProviderUpdateContext context)
    {
      Assert.ArgumentNotNull(indexable, "indexable");
      Assert.ArgumentNotNull(context, "context");
      context.Delete(indexable.UniqueId);
    }

    public void Update(IIndexable indexable, IProviderUpdateContext context, ProviderIndexConfiguration indexConfiguration)
    {
      Assert.ArgumentNotNull(indexable, "indexable");
      Assert.ArgumentNotNull(context, "context");
      Assert.ArgumentNotNull(indexConfiguration, "indexConfiguration");
      Delete(indexable, context);
      Add(indexable, context, indexConfiguration);
    }
  }
}