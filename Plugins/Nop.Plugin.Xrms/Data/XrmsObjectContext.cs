using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Data;
using Nop.Data.Extensions;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Data.Mapping;
using Nop.Data.Mapping.Catalog;

namespace Nop.Plugin.Xrms.Data
{
    public class XrmsObjectContext : DbContext, IDbContext
    {
        public XrmsObjectContext(DbContextOptions<XrmsObjectContext> options) : base(options) { }

        #region Implementation of IDbContext

        #endregion
        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">Model muilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MaterialGroupMap());
            modelBuilder.ApplyConfiguration(new MaterialMap());
            modelBuilder.ApplyConfiguration(new ProductRecipeMap());
            modelBuilder.ApplyConfiguration(new MaterialQuantityHistoryMap());
            modelBuilder.ApplyConfiguration(new SupplierMap());

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Generate a script to create all tables for the current model
        /// </summary>
        /// <returns>A SQL script</returns>
        public virtual string GenerateCreateScript()
        {
            return this.Database.GenerateCreateScript();
        }

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Install object context
        /// </summary>
        public void Install()
        {
            //create tables
            this.ExecuteSqlScript(this.GenerateCreateScript());
        }

        /*public void Install()
        {
            //create the table
            var dbScript = CreateDatabaseScript();
            Database.ExecuteSqlCommand(dbScript);
            SaveChanges();
        }*/

        /// <summary>
        /// Uninstall object context
        /// </summary>
        public void Uninstall()
        {
            var tableName = this.GetTableName<MaterialQuantityHistory>();
            this.DropPluginTable(tableName);

            tableName = this.GetTableName<ProductRecipe>();
            this.DropPluginTable(tableName);

            //drop the table
            tableName = this.GetTableName<Material>();
            this.DropPluginTable(tableName);

            tableName = this.GetTableName<MaterialGroup>();
            this.DropPluginTable(tableName);

            tableName = this.GetTableName<Supplier>();
            this.DropPluginTable(tableName);
        }



        /// <summary>
        /// Creates a LINQ query for the query type based on a raw SQL query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <returns>An IQueryable representing the raw SQL query</returns>
        public virtual IQueryable<TQuery> QueryFromSql<TQuery>(string sql) where TQuery : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a LINQ query for the entity based on a raw SQL query
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>An IQueryable representing the raw SQL query</returns>
        public virtual IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the given SQL against the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="doNotEnsureTransaction">true - the transaction creation is not ensured; false - the transaction creation is ensured.</param>
        /// <param name="timeout">The timeout to use for command. Note that the command timeout is distinct from the connection timeout, which is commonly set on the database connection string</param>
        /// <param name="parameters">Parameters to use with the SQL</param>
        /// <returns>The number of rows affected</returns>
        public virtual int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                var result = this.Database.ExecuteSqlCommand(sql, parameters);
                transaction.Commit();

                return result;
            }
        }

        /*public System.Collections.Generic.IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }*/

        /// <summary>
        /// Detach an entity from the context
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /*public virtual bool ProxyCreationEnabled
        {
            get
            {
                return Configuration.ProxyCreationEnabled;
            }
            set
            {
                Configuration.ProxyCreationEnabled = value;
            }
        }

        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                Configuration.AutoDetectChangesEnabled = value;
            }
        }*/
    }
}
