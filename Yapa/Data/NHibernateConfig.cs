using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;

namespace Yapa.Data;

public sealed class NHibernateConfig
{
    private const int TARGET_VERSION = 1;
    
    public static ISessionFactory CreateSessionFactory(string databaseFilePath)
    {
        var configuration = Fluently.Configure()
            .Database(SQLiteConfiguration.Standard.UsingFile(databaseFilePath))
            .Mappings(m =>
            {
                var classMaps = FindTypes();

                foreach (var map in classMaps)
                    m.FluentMappings.Add(map);
            })
            .ExposeConfiguration(UpdateSchema)
            .BuildConfiguration();
        
        return configuration.BuildSessionFactory();
    }

    private static void UpdateSchema(Configuration config)
    {
        
        using var sessionFactory = config.BuildSessionFactory();
        using var session = sessionFactory.OpenSession();
        
        SchemaVersion currentVersion;
        
        try
        {
            currentVersion = session.Query<SchemaVersion>().OrderByDescending(v => v.VersionNumber).FirstOrDefault();
        }
        catch (GenericADOException ex) when (ex.InnerException is SQLiteException sqliteEx && sqliteEx.Message.Contains("no such table"))
        {
            currentVersion = null;
        }

        if (currentVersion == null ||currentVersion.VersionNumber < TARGET_VERSION)
        {
            var schemaUpdate = new SchemaUpdate(config);
            schemaUpdate.Execute(false, true);
            
            using var transaction = session.BeginTransaction();
            session.Save(new SchemaVersion {VersionNumber = TARGET_VERSION});
            transaction.Commit();
        }
    }

    private static IEnumerable<Type> FindTypes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.BaseType != null && 
                        t.BaseType.IsGenericType && 
                        t.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<>));
    }
}