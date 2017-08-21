using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using Database.Domain;
using NHibernate.Tool.hbm2ddl;

namespace Database
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var cfg = new Configuration();
                    cfg.Configure();
                    cfg.AddAssembly(typeof(Project).Assembly);

                    new SchemaUpdate(cfg);
                    _sessionFactory = cfg.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession openSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
