using System.Collections.Generic;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{

    internal class ChangeTrackerItem
    {
        public object Item { get; set; }     
        public string Query { get; set; }   

        public ChangeTrackerItem(object item, string query)
        {
            this.Item = item;
            this.Query = query;            
        }

        public ChangeTrackerItem(string query)
        {
            this.Query = query;            
        }
    }

    internal class ChangeTracker
    {
        private List<ChangeTrackerItem> items = new List<ChangeTrackerItem>();

        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        public ChangeTracker()
        {
            logger.LogInformation("Instantiated a new ChangeTracker.");
        }

        internal IEnumerable<ChangeTrackerItem> GetChanges()
        {
            return items;
        }

        internal void AddItem(string query)
        {
            var item = new ChangeTrackerItem(query);
            items.Add(item);            
        }
        internal void AddItem(object entity, string query)
        {
            var item = new ChangeTrackerItem(entity, query);
            this.items.Add(item);
        }
        
        

    }
}