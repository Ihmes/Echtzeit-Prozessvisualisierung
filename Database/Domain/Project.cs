using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Domain
{
    public class Project
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        private IList<Process> ProcessList;

        public Project()
        {
            ProcessList = new List<Process>();
        }


        public Project(string Name, string Description) : this()
        {
            this.Name = Name;
            this.Description = Description;
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
