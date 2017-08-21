using System;
using ManagedWinapi.Accessibility;

namespace Technologies
{

    public class TechnologyControlEventArgs : EventArgs
    {
        public System.Windows.Point pt;

        public string Id; // currently used for SAP only

        public SystemAccessibleObject accessibleObject;

        public TechnologyControlEventArgs(System.Windows.Point point)
        {
            this.pt = point;
        }

        // only used for ACC Focus Events (in LC Pad)
        public TechnologyControlEventArgs(SystemAccessibleObject accessibleObject)
        {
            this.accessibleObject = accessibleObject;
        }

        // only used for SAP Focus Events
        public TechnologyControlEventArgs(string Id)
        {
            this.Id = Id;
        }
    }
}