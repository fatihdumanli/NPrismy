using System;
using System.Collections.Generic;
using System.Linq;

namespace NPrismy
{

    public static class PersistanceProviderFactory
    {
        private static List<KeyValuePair<PersistanceProvider, Type>> _providers 
            = new List<KeyValuePair<PersistanceProvider, Type>>()
            {
                new KeyValuePair<PersistanceProvider, Type>(PersistanceProvider.SqlServer, typeof(SqlServerConnection)),
                new KeyValuePair<PersistanceProvider, Type>(PersistanceProvider.Oracle, typeof(IConnection)),
                new KeyValuePair<PersistanceProvider, Type>(PersistanceProvider.MySql, typeof(IConnection))
            };

        public static Type GetProvider(PersistanceProvider providerEnum)
        {
            return _providers.Where(p => p.Key == providerEnum).Single().Value;
        }
    }

    public enum PersistanceProvider
    {
        SqlServer = 1,
        Oracle = 2,
        MySql = 3
    }
}