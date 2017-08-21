using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Domain
{
    public class ProcessStep
    {
        public virtual Guid Id { get; set; }
        public virtual String Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Step { get; set; }
        public virtual string ElementName { get; set; }
        public virtual string Type { get; set; }
        public virtual string Program { get; set; }
        public virtual string Window { get; set; }
        public virtual string Parent { get; set; }
        private IList<Process> ProcessList;


        public ProcessStep()
        {
            ProcessList = new List<Process>();
        }

        public ProcessStep(string Name, string Description, int Step, string ElementName, string Type, string Program, string Window, string Parent)
            : this()
        {
            this.Name = Name;
            this.Description = Description;
            this.Step = Step;
            this.ElementName = ElementName;
            this.Type = Type;
            this.Program = Program;
            this.Window = Window;
            this.Parent = Parent;
        }

        public virtual IList<Process> Processes
        {
            get { return ProcessList; }
            set { ProcessList = value; }
        }

        public override string ToString()
        {
            return String.Format(this.Name);
        }
    }
}
