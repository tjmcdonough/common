using System;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Utilities.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<string> GetPropertyNamesWithoutAttribute<TClass, TAttribute>() 
            where TClass : class 
            where TAttribute : Attribute
        {
            return typeof(TClass).GetProperties().Where(
                    prop => !Attribute.IsDefined(prop, typeof(TAttribute)))
                .Select(info => info.Name.ToLower()).ToList();
        }
    }
}