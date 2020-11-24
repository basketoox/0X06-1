using System.Reflection;
using System.Toolkit.Attributes;

namespace System.Toolkit.Helpers
{
    public class LocalizationHelper
    {
        public static string GetLocalizableDescription<T>(T value) //where T: struct enum
        {
            var fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                var attributes = (LocalizableDescriptionAttribute[])fi.GetCustomAttributes(typeof(LocalizableDescriptionAttribute), false);
                return ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description)))
                    ? attributes[0].Description
                    : value.ToString();
            }
            return string.Empty;
        }
    }
}
