using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Domain;
using NHibernate;

namespace Database.Repository
{
    public class ProcessStepRepository
    {
        public void Add(ProcessStep newProcessStep)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newProcessStep);
                    transaction.Commit();
                }
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

        public void Delete(ProcessStep newProcessStep)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete(newProcessStep);
                        transaction.Commit();
                    }
                    catch(Exception e){

                    }
                }
            }
        }

        public IList<ProcessStep> GetAllProcessSteps()
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                var ProcessSteps = session.CreateQuery("from ProcessStep").List<ProcessStep>();
                return ProcessSteps;
            }

        }

      
    }
}
