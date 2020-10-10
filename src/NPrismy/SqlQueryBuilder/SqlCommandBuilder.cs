using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class SqlCommandBuilder : ISqlCommandBuilder
    {
        private WhereBuilder _whereBuilder = AutofacModule.Container.Resolve<WhereBuilder>();
        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        public string BuildInsertQuery<T>(T obj)
        {
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ");
            sb.Append(tableDefinition.GetTableName());
            sb.Append(" ");
            var columns = tableDefinition.GetColumnDefinitions();
            var columnNames = columns.Select(w => w.ColumnName);
            var columnNamesSplittedWithComma = string.Join(',', columnNames);
            sb.Append(string.Format("( {0} )", columnNamesSplittedWithComma));
            sb.Append(" ");
            sb.Append("VALUES");
            
            List<string> values = new List<string>();
            /* BEGIN: Obtaining entity values */
            foreach(var column in columns)
            {
                logger.LogInformation("COLUMN PROPERTY NAME: " + column.PropertyName);
                //May need to apply quotes
                var objPropValue = obj.GetType().GetProperty(column.PropertyName).GetValue(obj);
                logger.LogInformation("Entity PROPERTY VALUE: " + objPropValue);

                if(objPropValue == null)
                {
                    values.Add("null");
                }

                else
                {
                    logger.LogInformation(objPropValue + " appended to values");

                    //Is entity value is non-numeric, it must be decorated with quotes ('')
                    if(objPropValue.ToString().IsNumeric())
                    {
                        values.Add(objPropValue.ToString());
                    }

                    else
                    {
                        values.Add(objPropValue.ToString().DecorateWithQuotes());
                    }
                }
            }
            /* END: Obtaining entity values */
            sb.Append(string.Format("( {0} )", string.Join(',', values)));
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

     
    }
}