using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Table service interface
    /// </summary>
    public partial interface ITableService
    {
        /// <summary>
        /// Delete table
        /// </summary>
        /// <param name="Table">Table</param>
        void DeleteTable(Table table);

        /// <summary>
        /// Gets all tables
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Tables</returns>
        IPagedList<Table> GetAllTables(string tableName = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a table
        /// </summary>
        /// <param name="tableId">Table identifier</param>
        /// <returns>Table</returns>
        Table GetTableById(int tableId);

        /// Gets a table from aggregate id
        /// </summary>
        /// <param name="aggregateId"> Aggregate identifier</param>
        /// <returns>Table</returns>
        Table GetTableByAggregateId(Guid aggregateId);

        /// <summary>
        /// Inserts table
        /// </summary>
        /// <param name="table">Table</param>
        void InsertTable(Table table);

        /// <summary>
        /// Updates the table
        /// </summary>
        /// <param name="table">Table</param>
        void UpdateTable(Table table);

        /// <summary>
        /// Returns a list of names of not existing tables
        /// </summary>
        /// <param name="tableNames">The names of the tables to check</param>
        /// <returns>List of names not existing tables</returns>
        string[] GetNotExistingTables(string[] tableNames);

        /// <summary>
        /// Gets tables by identifier
        /// </summary>
        /// <param name="tableIds">Table identifiers</param>
        /// <returns>List of Table</returns>
        List<Table> GetTablesByIds(int[] tableIds);

    }
}
