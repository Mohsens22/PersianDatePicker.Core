using System;
using System.ComponentModel;
using System.Globalization;

namespace Mohsen.PersianDateControls;

public class PersianDateConverter : TypeConverter
{
    // Overrides the CanConvertFrom method of TypeConverter.
    // The ITypeDescriptorContext interface provides the context for the
    // conversion. Typically, this interface is used at design time to 
    // provide information about the design-time container.
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {

        if (sourceType == typeof(string))
        {
            return true;
        }
        if (sourceType == typeof(DateTime))
        {
            return true;
        }
        return base.CanConvertFrom(context, sourceType);
    }

    // Overrides the ConvertFrom method of TypeConverter.
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string)
        {
            return PersianDate.Parse(value as string);
        }
        if (value is DateTime)
        {
            return new PersianDate((DateTime)value);
        }
        return base.ConvertFrom(context, culture, value);
    }

    // Overrides the ConvertTo method of TypeConverter.
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            PersianDate pd = (PersianDate)value;
            return value.ToString();
        }
        if (destinationType == typeof(DateTime))
        {
            PersianDate pd = (PersianDate)value;
            return pd.ToDateTime();
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}
