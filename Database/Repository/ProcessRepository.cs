using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Domain;
using NHibernate;

namespace Database.Repository
{
    public class ProcessRepository
    {
        public void Add(Process newProcess)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newProcess);
                    transaction.Commit();
                }
            }
        }

        public IList<Process> GetAllProcesses()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                var processes = session.CreateQuery("from Process").List<Process>();
                return processes;
            }

        }


        public void Update(Process newProcess)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(newProcess);
                    transaction.Commit();
                }
            }
        }

        public void Delete(Process newProcess)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(newProcess);
                    transaction.Commit();
                }
            }
        }
    }
}
