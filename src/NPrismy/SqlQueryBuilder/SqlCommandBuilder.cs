using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Autofac;
using NPrismy.Extensions;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class SqlCommandBuilder : ISqlCommandBuilder
    {
        private WhereBuilder _whereBuilder = AutofacModule.Container.Resolve<WhereBuilder>();
        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();

        public string BuildDeleteQuery<T>(Expression<Func<T, bool>> expression)
        {
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("DELETE FROM {0}", tableDefinition.GetTableName()));
            var whereClause = _whereBuilder.ToSql<T>(expression);
            sb.Append(" WHERE ");
            sb.Append(whereClause);
            return sb.ToString();
        }

        public string BuildDeleteQuery<T>(object primaryKey)
        {
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            var pkColumn = tableDefinition.GetPrimaryKeyColumnDefinition();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("DELETE FROM {0} ", tableDefinition.GetTableName()));
            sb.Append(string.Format("WHERE {0} = {1}", pkColumn.ColumnName, primaryKey.ToString().DecorateWithQuotes()));
            return sb.ToString();
        }

        public string BuildFindByPrimaryKeyQuery<T>(object pkValue)
        {
            var tableDefinition= TableRegistry.Instance.GetTableDefinition<T>();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("SELECT * FROM {0} ", tableDefinition.GetTableName()));
            sb.Append(" WHERE ");       
            var pkColumnn = tableDefinition.GetPrimaryKeyColumnDefinition();
            sb.Append(string.Format("{0} = {1}", pkColumnn.ColumnName, pkValue.ToString().DecorateWithQuotes()));
            return sb.ToString();
        }

        public string BuildInsertQuery<T>(T obj)
        {
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            StringBuilder sb = new StringBuilder();

            if(tableDefinition.IsIdentityInsertEnabled() && 
                (tableDefinition.GetPrimaryKeyColumnDefinition().EntityPropertyType.Equals(typeof(Int32)) 
                || tableDefinition.GetPrimaryKeyColumnDefinition().EntityPropertyType.Equals(typeof(Int64))))
             {
                string enableIdentityInsertStatement = string.Format("SET IDENTITY_INSERT {0} ON ", tableDefinition.GetTableName()); 
                sb.Append(enableIdentityInsertStatement);
            }
            
            sb.Append("INSERT INTO ");
            sb.Append(tableDefinition.GetTableName());
            sb.Append(" ");

            //We shouldn't use navigation properties when building an INSERT query.
            //Include identity if identity insert enabled for table.
            var columns = tableDefinition.GetColumnDefinitions(includeIdentity: tableDefinition.IsIdentityInsertEnabled(), includeNavigationProperties: false);
            var columnNames = columns.Select(w => w.ColumnName).ToArray();
            columnNames.DecorateWithSquareBrackets();
            var columnNamesSplittedWithComma = string.Join(',', columnNames);
            sb.Append(string.Format("( {0} )", columnNamesSplittedWithComma));
            sb.Append(" ");
            sb.Append("VALUES");
            
            List<string> values = new List<string>();
            /* BEGIN: Obtaining entity values */
            foreach(var column in columns)
            {
                //May need to apply quotes
                object objPropValue = null;

                try
                {
                     objPropValue = obj.GetType().GetProperty(column.PropertyName)
                    .GetValue(obj);
                }

                catch(Exception e)
                {

                }

                //Trying twice for private fields.
                //Trying get private field in case of exception.
                try
                {

                    BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Static;
                    FieldInfo field = obj.GetType().GetField(column.PropertyName, bindFlags);
                    

                    if(field != null)
                        objPropValue = field.GetValue(obj);
                    
                }
                catch(Exception e)
                {

                }
                    
                Type entityPropertyType = null;
                //Check entity property type
                try
                {
                    entityPropertyType = obj.GetType()
                        .GetProperty(column.PropertyName).PropertyType;
                }

                catch(Exception e)
                {
                    
                }

                //Trying twice for private fields.
                try
                {
                    BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Static;
                    FieldInfo field = obj.GetType().GetField(column.PropertyName, bindFlags);
                    if(field != null)
                        entityPropertyType = field.FieldType;
                }

                catch (Exception e)
                {
                    
                }
                 

                string valueToAddQuery = null;

                if(objPropValue == null)
                {
                    valueToAddQuery = "null";
                }
                
                else
                {

                     //Is entity value is non-numeric, it must be decorated with quotes ('')
                     //DecorateWithQuotes() decides whether the value is numeric or not
                     if(entityPropertyType == typeof(DateTime))
                     {
                         valueToAddQuery = objPropValue.ConvertToSqlDateTime();
                         valueToAddQuery = valueToAddQuery.DecorateWithQuotes();
                     }

                     else
                     {
                         valueToAddQuery = objPropValue.ToString().DecorateWithQuotes();
                     }
                    
                }

                values.Add(valueToAddQuery);

            }
            /* END: Obtaining entity values */
            sb.Append(string.Format("( {0} )", string.Join(',', values)));


            /* BEGIN: Adding some SQL statement to obtain database-generated id */
            sb.Append("; SELECT SCOPE_IDENTITY();");
            return sb.ToString();           

        }

        //Without WHERE clause
        public string BuildReadQuery<T>()
        {   
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("SELECT * FROM {0} ", tableDefinition.GetTableName()));
            return sb.ToString();    
        }

        //With WHERE clause
        public string BuildReadQuery<T>(Expression<Func<T, bool>> expr)
        {
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();

            StringBuilder sb = new StringBuilder();
            var whereClause = _whereBuilder.ToSql<T>(expr);
            sb.Append(string.Format("SELECT * FROM {0} ", tableDefinition.GetTableName()));
            sb.Append("WHERE");
            sb.Append(whereClause);
            return sb.ToString();          
        }

        public string BuildUpdateQuery<T>(T obj)
        {
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("UPDATE {0} ", tableDefinition.GetTableName()));
            
            var columns = tableDefinition.GetColumnDefinitions().ToList();

            var columnValue = typeof(T).GetProperty(columns[0].PropertyName).GetValue(obj).ToString();
             sb.Append(string.Format("SET [{0}] = {1}, ", columns[0].ColumnName, columnValue.DecorateWithQuotes()));  

            for(int i = 1; i < columns.Count(); i++)
            {
                columnValue = typeof(T).GetProperty(columns[i].PropertyName).GetValue(obj).ToString();
                sb.Append(string.Format("[{0}] = {1}", columns[i].ColumnName, columnValue.DecorateWithQuotes()));

                if(i < columns.Count - 1)
                {
                    sb.Append(", ");
                }
            }
                
            /* BEGIN: Adding where clause with PK */
            var pkColumn = tableDefinition.GetPrimaryKeyColumnDefinition();
            var pkValue = typeof(T).GetProperty(pkColumn.PropertyName).GetValue(obj);
            sb.Append(string.Format(" WHERE {0} = {1}", pkColumn.ColumnName, pkValue.ToString().DecorateWithQuotes()));
            /* EMD: Adding where clause with PK */
            
            return sb.ToString();
        }
    }
}