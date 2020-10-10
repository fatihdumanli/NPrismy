using System;
using System.Data.SqlClient;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
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
            }

            if(entityPropertyType == typeof(String))
            {
                value = reader.GetString(ordinal);            
            }

            return value;
        }
    }
}