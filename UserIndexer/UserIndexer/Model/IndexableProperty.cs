using System;
using System.Reflection;
using Sitecore.ContentSearch;

namespace UserIndexer.Model
{
  public class IndexableProperty : IIndexableDataField
  {
    private readonly PropertyInfo _property;
    private readonly object _value;

    public IndexableProperty(PropertyInfo property, object value)
    {
      _property = property;
      _value = value;
    }

    public Type FieldType
    {
      get
      {
        return _property.PropertyType;
      }
    }

    public object Id
    {
      get
      {
        return _property.DeclaringType.FullName + "." + _property.Name;
      }
    }

    public string Name
    {
      get
      {
        string indexFieldName = _property.GetCustomAttribute<IndexFieldAttribute>().IndexFieldName;
        return string.IsNullOrEmpty(indexFieldName) ? _property.Name : indexFieldName;
      }
    }

    public string TypeKey
    {
      get
      {
        return _property.PropertyType.FullName;
      }
    }

    public object Value
    {
      get
      {
        return _value;
      }
    }
  }
}