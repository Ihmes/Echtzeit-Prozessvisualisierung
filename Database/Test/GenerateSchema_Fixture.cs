using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Database.Domain;
using Database.Repository;

namespace Database.Test
{
    [TestFixture]
    public class GenerateSchema_Fixture
    {

        private ProcessRepository repoProcess;
        private ProjectRepository repoProject;
        private ProcessStepRepository repoProcessStep;

        public GenerateSchema_Fixture()
        {
            repoProcess = new ProcessRepository();
            repoProject = new ProjectRepository();
            repoProcessStep = new ProcessStepRepository();
        }


        [Test]
        public void Can_generate_schema()
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Project).Assembly);

            new SchemaExport(cfg).Execute(true, true, false);
        }

        [Test]
        public void initialize_TestData()
        {
            Project Project1 = new Project("Word", "Erstes Projekt");
            Project Project2 = new Project("Excel", "Zweites Projekt");
            Project Project3 = new Project("Access", "Drittes Projekt");

            Process Process1 = new Process("Serienbrief erstellen", "Erster Prozess", Project1);
            Process Process2 = new Process("Drucken", "Zweiter Prozess", Project1);
            Process Process3 = new Process("Neue Datei", "Dritter Prozess", Project2);
            Process Process4 = new Process("Formel eingeben erstellen", "Vierter Prozess", Project2);
            Process Process5 = new Process("Diagramm erstellen", "Fünfter Prozess", Project2);

            ProcessStep ProcessStep1 = new ProcessStep("bla", "blabla", 1, "bla", "bla", "bla", "bla","parent");
            ProcessStep ProcessStep2 = new ProcessStep("bla", "blabla", 2, "bla", "bla", "bla", "bla","parent");
            ProcessStep ProcessStep3 = new ProcessStep("bla", "blabla", 3, "bla", "bla", "bla", "bla", "parent");
            ProcessStep ProcessStep4 = new ProcessStep("bla", "blabla", 1, "bla", "bla", "bla", "bla", "parent");
            ProcessStep ProcessStep5 = new ProcessStep("bla", "blabla", 2, "bla", "bla", "bla", "bla", "parent");
            ProcessStep ProcessStep6 = new ProcessStep("bla", "blabla", 3, "bla", "bla", "bla", "bla", "parent");
            ProcessStep ProcessStep7 = new ProcessStep("bla", "blabla", 4, "bla", "bla", "bla", "bla", "parent");
            ProcessStep ProcessStep8 = new ProcessStep("bla", "blabla", 1, "bla", "bla", "bla", "bla", "parent");
            ProcessStep ProcessStep9 = new ProcessStep("bla", "blabla", 2, "bla", "bla", "bla", "bla", "parent");
            ProcessStep ProcessStep10 = new ProcessStep("bla", "blabla", 3, "bla", "bla", "bla", "bla", "parent");

            IList<ProcessStep> List1 = new List<ProcessStep>();
            List1.Add(ProcessStep1);
            List1.Add(ProcessStep2);
            List1.Add(ProcessStep3);
            IList<ProcessStep> List2 = new List<ProcessStep>();
            List2.Add(ProcessStep4);
            List2.Add(ProcessStep5);
            List2.Add(ProcessStep6);
            List2.Add(ProcessStep7);
            IList<ProcessStep> List3 = new List<ProcessStep>();
            List3.Add(ProcessStep8);
            List3.Add(ProcessStep9);
            List3.Add(ProcessStep10);

            repoProject.Add(Project1);
            repoProject.Add(Project2);
            repoProject.Add(Project3);

            repoProcess.Add(Process1);
            repoProcess.Add(Process2);
            repoProcess.Add(Process3);
            repoProcess.Add(Process4);
            repoProcess.Add(Process5);

            repoProcessStep.Add(ProcessStep1);
            repoProcessStep.Add(ProcessStep2);
            repoProcessStep.Add(ProcessStep3);
            repoProcessStep.Add(ProcessStep4);
            repoProcessStep.Add(ProcessStep5);
            repoProcessStep.Add(ProcessStep6);
            repoProcessStep.Add(ProcessStep7);
            repoProcessStep.Add(ProcessStep8);
            repoProcessStep.Add(ProcessStep9);
            repoProcessStep.Add(ProcessStep10);

            Process1.ProcessSteps = List1;
            Process2.ProcessSteps = List2;
            Process3.ProcessSteps = List3;

            repoProcess.Update(Process1);
            repoProcess.Update(Process2);
            repoProcess.Update(Process3);
        }
    }
}
