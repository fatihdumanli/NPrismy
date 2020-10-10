using System.Collections.Generic;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class ChangeTracker
    {
        private List<string> queries;

        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        public ChangeTracker()
        {
            queries = new List<string>();
            logger.LogInformation("Instantiated a new ChangeTracker.");
        }
        
        public void AddQuery(string query)
        {
            this.queries.Add(query);
        }

        public IEnumerable<string> GetQueries()
        {
            return queries;
        }

    }
}