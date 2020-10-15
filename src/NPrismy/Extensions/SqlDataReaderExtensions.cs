using System;
using Autofac;
using Microsoft.Data.SqlClient;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy.Extensions
{
    internal static class SqlDataReaderExtensions
    {
        //TODO: Elaborate with DateTime, boolean etc...
        internal static object GetTypeValueNonGeneric(this SqlDataReader reader, Type entityPropertyType, int ordinal)
        {
            object value = null;
            
            AutofacModule.Container.Resolve<ILogger>().LogInformation("entity property type: " + entityPropertyType);
            if(entityPropertyType == typeof(Int32))
            {
                value = reader.GetInt32(ordinal);
                return value;
            }

            else if(entityPropertyType == typeof(String))
            {
                value = reader.GetString(ordinal);    
                return value;        
            }

            else if(entityPropertyType == typeof(DateTime))
            {
                value = reader.GetDateTime(ordinal);
                return value;
            }

            return value;

        }
    }
}