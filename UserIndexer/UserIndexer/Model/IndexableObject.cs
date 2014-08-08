using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Sitecore.ContentSearch;

namespace UserIndexer.Model
{
  public abstract class IndexableObject : IIndexable
  {
    private IEnumerable<IIndexableDataField> _fields;

    public virtual void LoadAllFields()
    {
      bool loaded = Fields.Any();
    }

    public virtual IIndexableDataField GetFieldById(object fieldId)
    {
      return Fields.FirstOrDefault(field => field.Id == fieldId);
    }

    public virtual IIndexableDataField GetFieldByName(string fieldName)
    {
      return Fields.FirstOrDefault(field => string.Equals(field.Name, fieldName, StringComparison.InvariantCultureIgnoreCase));
    }

    public abstract IIndexableId Id
    {
      get;
    }

    public abstract IIndexableUniqueId UniqueId
    {
      get;
    }

    public virtual string DataSource
    {
      get;
      set;
    }

    public virtual string AbsolutePath
    {
      get;
      set;
    }

    public virtual CultureInfo Culture
    {
      get;
      set;
    }

    public virtual IEnumerable<IIndexableDataField> Fields
    {
      get
      {
        return _fields ?? (_fields = GetIndexableProperties());
      }
    }

    private IEnumerable<IIndexableDataField> GetIndexableProperties()
    {
      return (from property in GetType().GetProperties()
              where property.GetCustomAttribute<IndexFieldAttribute>() != null
              let value = GetConvertedValue(property)
              select new IndexableProperty(property, value)).ToArray();
    }

    private object GetConvertedValue(PropertyInfo property)
    {
      TypeConverterAttribute converterAttribute = property.GetCustomAttribute<TypeConverterAttribute>();
      if (converterAttribute == null)
        return property.GetValue(this);
      TypeConverter typeConverter = (TypeConverter)Activator.CreateInstance(Type.GetType(converterAttribute.ConverterTypeName));
      return typeConverter.ConvertTo(null, Culture ?? CultureInfo.CurrentCulture, property.GetValue(this), typeof(string));
    }
  }
}