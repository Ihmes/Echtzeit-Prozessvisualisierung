using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;


namespace Technologies
{
    public abstract class ITechnology
    {
        public const String ERROR_IDENTIFICATION_FAILED = null;
        public const String NO_NAME_FOUND = "no name";
        public const int TYPE_UNKNOWN = -1;

        public abstract String GetNameByPosition(System.Windows.Point point);
        public abstract int GetTypeByPosition(System.Windows.Point point);
        public abstract String GetProcessNameByPosition(System.Windows.Point point);
        public abstract String GetWindowNameByPosition(System.Windows.Point point);

    }
}
