using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;

namespace ProjectEvalutionSystem.Helper
{
    public class CacheManager
    {
        public static T GetOrSet<T>
            (string cacheKey, Func<T> getItemCallback, string tableName)
            where T : class
        {
            var key = $"{cacheKey}";

            if (!(HttpContext.Current.Cache.Get(key) is T item))
            {
                var sqlCacheDependency = RegisterSqlCacheDependency(tableName);
                item = getItemCallback();

                HttpContext.Current.Cache.Insert(key, item, sqlCacheDependency,
                    DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            return item;
        }
        public static T GetOrSet<T>
            (string cacheKey, Func<T> getItemCallback, DateTimeOffset expiration, string tableName)
            where T : class
        {
            var key = $"{cacheKey}";
            if (!(HttpContext.Current.Cache.Get(key) is T item))
            {
                var sqlCacheDependency = RegisterSqlCacheDependency(tableName);
                item = getItemCallback();

                HttpContext.Current.Cache.Insert(key, item, sqlCacheDependency,
                    DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            return item;
        }
        public static T GetOrSet<T>
            (string cacheKey, T value, DateTimeOffset expiration, string tableName)
        {
            var key = $"{cacheKey}";
            if (!(HttpContext.Current.Cache.Get(key) is T item))
            {
                var sqlCacheDependency = RegisterSqlCacheDependency(tableName);
                item = value;
                HttpContext.Current.Cache.Insert(key, item, sqlCacheDependency,
                    DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            return item;
        }
        public static void Set<T>
            (string cacheKey, T value, DateTimeOffset expiration, string tableName)
        {
            var sqlCacheDependency = RegisterSqlCacheDependency(tableName);

            var key = $"{cacheKey}";

            HttpContext.Current.Cache.Insert(key, value, sqlCacheDependency,
                DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        }

        public static T GetOrSet<T>
            (string cacheKey, T value, string tableName)
        {
            var key = $"{cacheKey}";
            if (!(HttpContext.Current.Cache.Get(key) is T item))
            {
                var sqlCacheDependency = RegisterSqlCacheDependency(tableName);
                item = value;

                HttpContext.Current.Cache.Insert(key, item, sqlCacheDependency,
                    DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            return item;
        }
        public static T Get<T>(string cacheKey)
        {
            try
            {
                if (HttpContext.Current.Cache[$"{cacheKey}"] != null)
                {
                    T item = (T)HttpContext.Current.Cache[$"{cacheKey}"];
                    if (item == null)
                    {
                        throw new Exception("Cache key not found");
                    }
                    return item;
                }
                return default;
            }
            catch
            {
                return default;
            }
        }
        public static void Remove(string cacheKey)
        {
            try
            {
                var key = $"{cacheKey}";
                var item = HttpContext.Current.Cache.Get(key);
                if (item != null)
                {
                    HttpContext.Current.Cache.Remove(key);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// This method is for register the cache object in the database with associated table.
        /// </summary>
        public static SqlCacheDependency RegisterSqlCacheDependency(string tableName)
        {

            var currentConnectionString = ConfigurationManager.ConnectionStrings["PESCF"].ConnectionString;

            SqlCacheDependencyAdmin.EnableNotifications(currentConnectionString);

            var tablesAlreadyEnabledForNotifications = SqlCacheDependencyAdmin.GetTablesEnabledForNotifications(currentConnectionString);

            if (tablesAlreadyEnabledForNotifications.Any())
            {
                foreach (var table in tablesAlreadyEnabledForNotifications)
                {
                    if (tableName != table)
                    {
                        SqlCacheDependencyAdmin.EnableTableForNotifications(currentConnectionString, tableName);
                    }
                }
            }
            else
            {
                SqlCacheDependencyAdmin.EnableTableForNotifications(currentConnectionString, tableName);
            }


            var sqlCacheDependency = new SqlCacheDependency(
                databaseEntryName: GetCurrentDatabaseEntryName(),
                tableName: tableName);
            return sqlCacheDependency;
        }

        /// <summary>
        /// This method has returns the current <see cref="SqlCacheDependency"/> database entry name.
        /// </summary>
        public static string GetCurrentDatabaseEntryName()
        {
            string currentFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            var webConfig = XDocument.Load(currentFileName);

            foreach (var descendant in webConfig.Descendants("databases"))
            {
                var cachingElement = descendant.Element("add");
                if (cachingElement != null)
                {
                    var currentDatabaseEntryName =
                        cachingElement.Attribute("name");
                    return currentDatabaseEntryName?.Value;
                }
            }
            return string.Empty;
        }
    }
}