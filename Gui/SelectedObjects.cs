using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Domain;
using Database.Repository;

namespace Gui
{
    public static class SelectedObjects
    {
        public static Project Project { set; get; }
        public static Process Process { set; get; }
        public static ProcessStep ProcessStep { set; get; }
    }
}
