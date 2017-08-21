using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gui
{
    public class FormularEventArgs : EventArgs
    {
        public readonly string Name;
        public readonly string Description;

        public FormularEventArgs(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
