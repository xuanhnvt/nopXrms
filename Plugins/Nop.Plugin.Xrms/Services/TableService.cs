using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Plugin.Xrms.Domain;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.ExportImport.Help;
using Nop.Services.Security;
using Nop.Services.Stores;
using OfficeOpenXml;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Table service
    /// </summary>
    public partial class TableService : ITableService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : table ID
        /// </remarks>
        private const string TABLES_BY_ID_KEY = "Nop.table.id-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string TABLES_PATTERN_KEY = "Nop.table.";

        #endregion

        #region Fields

        private readonly IRepository<Table> _tableRepository;

        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IAclService _aclService;
        private readonly CommonSettings _commonSettings;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="tableRepository">Table repository</param>
        /// <param name="tableRepository">Material repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="dbContext">DB context</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="catalogSettings">Catalog settings</param>
        public TableService(ICacheManager cacheManager,
            IRepository<Table> tableRepository,
            IRepository<AclRecord> aclRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IDbContext dbContext,
            IDataProvider dataProvider,
            IWorkContext workContext,
            IStoreContext storeContext,
            IEventPublisher eventPublisher,
            IStoreMappingService storeMappingService,
            IAclService aclService,
            CommonSettings commonSettings,
            CatalogSettings catalogSettings)
        {
            this._cacheManager = cacheManager;
            this._tableRepository = tableRepository;

            this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._dbContext = dbContext;
            this._dataProvider = dataProvider;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._eventPublisher = eventPublisher;
            this._storeMappingService = storeMappingService;
            this._aclService = aclService;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get property list by excel cells
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="worksheet">Excel worksheet</param>
        /// <returns>Property list</returns>
        public static IList<PropertyByName<T>> GetPropertiesByExcelCells<T>(ExcelWorksheet worksheet)
        {
            var properties = new List<PropertyByName<T>>();
            var poz = 1;
            while (true)
            {
                try
                {
                    var cell = worksheet.Cells[1, poz];

                    if (string.IsNullOrEmpty(cell?.Value?.ToString()))
                        break;

                    poz += 1;
                    properties.Add(new PropertyByName<T>(cell.Value.ToString()));
                }
                catch
                {
                    break;
                }
            }

            return properties;
        }


        /// <summary>
        /// Delete table
        /// </summary>
        /// <param name="Table">Table</param>
        public virtual void DeleteTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            table.Deleted = true;
            UpdateTable(table);

            //event notification
            _eventPublisher.EntityDeleted(table);
        }

        /// <summary>
        /// Gets all tables
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Table</returns>
        public virtual IPagedList<Table> GetAllTables(string tableName = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _tableRepository.Table;
            if (!string.IsNullOrWhiteSpace(tableName))
                query = query.Where(c => c.Name.Contains(tableName));
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

            var sortedTables = query.ToList();

            //paging
            return new PagedList<Table>(sortedTables, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        /// <param name="tableId">Table identifier</param>
        /// <returns>Table</returns>
        public virtual Table GetTableById(int tableId)
        {
            if (tableId == 0)
                return null;
            
            var key = string.Format(TABLES_BY_ID_KEY, tableId);
            return _cacheManager.Get(key, () => _tableRepository.GetById(tableId));
        }

        /// Gets a table from aggregate id
        /// </summary>
        /// <param name="aggregateId"> Aggregate identifier</param>
        /// <returns>Table</returns>
        public Table GetTableByAggregateId(Guid aggregateId)
        {
            if (aggregateId == Guid.Empty)
                return null;

            var query = from t in _tableRepository.Table
                        where t.AggregateId == aggregateId
                        select t;
            var table = query.FirstOrDefault();
            return table;
        }

        /// <summary>
        /// Inserts table
        /// </summary>
        /// <param name="table">Table</param>
        public virtual void InsertTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _tableRepository.Insert(table);

            //cache
            _cacheManager.RemoveByPattern(TABLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(table);
        }

        /// <summary>
        /// Updates the table
        /// </summary>
        /// <param name="table">Table</param>
        public virtual void UpdateTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _tableRepository.Update(table);

            //cache
            _cacheManager.RemoveByPattern(TABLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(table);
        }

        /// <summary>
        /// Returns a list of names of not existing tables
        /// </summary>
        /// <param name="tableNames">The names of the tables to check</param>
        /// <returns>List of names not existing tables</returns>
        public virtual string[] GetNotExistingTables(string[] tableNames)
        {
            if (tableNames == null)
                throw new ArgumentNullException(nameof(tableNames));

            var query = _tableRepository.Table;
            var queryFilter = tableNames.Distinct().ToArray();
            var filter = query.Select(c => c.Name).Where(c => queryFilter.Contains(c)).ToList();

            return queryFilter.Except(filter).ToArray();
        }

        /// <summary>
        /// Gets tables by identifier
        /// </summary>
        /// <param name="tableIds">Table identifiers</param>
        /// <returns>List of Table</returns>
        public virtual List<Table> GetTablesByIds(int[] tableIds)
        {
            if (tableIds == null || tableIds.Length == 0)
                return new List<Table>();

            var query = from p in _tableRepository.Table
                where tableIds.Contains(p.Id) && !p.Deleted
                select p;
            
            return query.ToList();
        }

        #endregion
    }
}
