using System;
using System.Collections.Generic;
using Autofac;
using NPrismy.IOC;

namespace NPrismy
{
    public abstract class EntityTable<T>
    {
        private IConnection _connection;
        private EntityTable()
        {      
            _connection = AutofacModule.Container.ResolveOptional<IConnection>();
        }

        public IEnumerable<T> GetAll()
        { 
            throw new Exception(this.GetType().Name + " ERRRRORR");
            _connection.Open();
            string query = "SELECT * From []";
            return null;
        }
    }

}