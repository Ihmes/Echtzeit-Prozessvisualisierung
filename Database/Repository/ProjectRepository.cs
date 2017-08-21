                                                                        using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Domain;
using NHibernate;

namespace Database.Repository
{
    public class ProjectRepository
    {
        public void Add(Project newProject)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newProject);
                    transaction.Commit();
                }
            }
        }

        public IList<Project> GetAllProjects()
        {

            try
            {
                using (ISession session = NHibernateHelper.openSession())
                {
                    var projects = session.CreateQuery("from Project").List<Project>();
                    return projects;
                }
            }
            catch(ADOException e) {
                
                Console.WriteLine("ADOException: " + e);
                return null;
                
            }
        }

        public Project GetProjectByName(string name)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                var project = session.QueryOver<Project>().Where(x => x.Name == name).SingleOrDefault();
                return project;
            }

        }

        public void Update(Project newProject)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(newProject);
                    transaction.Commit();
                }
            }
        }

        public void Delete(Project newProject)
        {
            using (ISession session = NHibernateHelper.openSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(newProject);
                    transaction.Commit();
                }
            }
        }
    }
}
