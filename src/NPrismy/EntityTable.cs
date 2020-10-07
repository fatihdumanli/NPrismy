using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    public class EntityTable<T> 
    {
        ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        private IConnection connection = AutofacModule.Container.Resolve<IConnection>();

        public async Task<IEnumerable<T>> GetAll()
        { 
            var query = string.Format("SELECT * From def.processDefinitions");

            await connection.QueryAsync(query);
            logger.LogInformation(query);

            return null;
        }


        private string _tableName 
        {
            get 
            {
                return typeof(T).Name;
            }
        }
    }

}