using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Database.Repository;

namespace Database.Test
{
    [TestFixture]
    class TestProcessRepository
    {
        private ProcessRepository repoProcess;
        private ProjectRepository repoProject;

        public TestProcessRepository()
        {
            repoProcess = new ProcessRepository();
            repoProject = new ProjectRepository();
        }

    }
}
