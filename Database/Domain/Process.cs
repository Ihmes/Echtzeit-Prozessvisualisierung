using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Domain;

namespace Database.Domain
{
    public class Process
    {
        public virtual Guid Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual Project Project { get; set; }
        private IList<ProcessStep> ProcessStepList;


        public Process()
        {
            ProcessStepList = new List<ProcessStep>();
        }

        public Process(string Name, string Description, Project Project) : this()
        {
            this.Name = Name;
            this.Description = Description;
            this.Project = Project;
        }

        public virtual IList<ProcessStep> ProcessSteps
        {
            get 
            {
                IEnumerable<ProcessStep> sortedEnum = ProcessStepList.OrderBy(p => p.Step);
                ProcessStepList = sortedEnum.ToList();
                return ProcessStepList; 
            }
            set { ProcessStepList = value; }
        }

        public override string ToString()
        {
            return String.Format(this.Name);
        }
    }
}
