using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Repository;
using Database.Domain;
using NUnit.Framework;

namespace Database.Test
{
    [TestFixture]
    public class TestProjectRepository
    {
        private ProjectRepository repo;

        public TestProjectRepository()
        {
            repo = new ProjectRepository();        
        }

        [Test]
        public void Can_saveproject()
        {
            try
            {
                var newProject = new Project("Word", "Some Word-Processes are defined in this project");
                repo.Add(newProject);
            }
            catch (Exception e)
            {
                Console.WriteLine("Project still in DB");
            }
        }

        [Test]
        public void Can_getProjectByName()
        {
            var project = repo.GetProjectByName("Word");
            Console.WriteLine(project.ToString());
        }

        [Test]
        public void Can_getProcessesForProject()
        {
            var project = repo.GetProjectByName("Word");
            foreach (Process process in project.Processes)
            {
                Console.WriteLine(process.Name);
            }
        }

        [Test]
        public void can_getAllProjects()
        {
            var projects = repo.GetAllProjects();
            foreach (Project p in projects)
            {
                Console.WriteLine(p.ToString());
            }
        }

        [Test]
        public void Can_UpdateProject()
        {
            var project = repo.GetProjectByName("Word");
            project.Name = "Excel";
            repo.Update(project);
        }

        [Test]
        public void Can_DeleteProject()
        {
            var project = repo.GetProjectByName("Excel");
            repo.Delete(project);
        }
    }
}
