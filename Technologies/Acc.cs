using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LiveContext.Utility;
using ManagedWinapi.Accessibility;
using LiveContext.Configuration;

namespace Technologies
{
    public class Acc : Technology
    {


       
        /*
        public override String[] GetIdsOfFocussed(TechnologyControlEventArgs tecArgs, List<Rectangle> bounds)
        {
            if (tecArgs.accessibleObject != null)
            {
                // use known acc object
                // TODO: using sequentialSao here is a hack, but ok because it should never be set when calling this
                sequentialSao = tecArgs.accessibleObject;
                var ids = GetIdsByPosition(new System.Windows.Point(), bounds);
                sequentialSao = null;
                return ids;
            }
            else
            {
                // TODO: this case is probably no longer required and may be removed
                // this is just a wrapper because acc doesn't seem to provide access to focussed elements..
                return GetIdsByPosition(tecArgs.pt, bounds);
            }
        }
         */
        public override String GetNameByPosition(System.Windows.Point point)
        {
            string name = NO_NAME_FOUND;

            try
            {
                SystemAccessibleObject sao;
                if (sequentialSao != null) sao = sequentialSao;
                else sao = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);

                // BLCD-812: receive bounds of optimized element
                // TODO: using "ref" to receive the object that correspond to the optimized ID is rather a hack here
                var id = GetOptimizedId(ref sao);

                name = sao.Name;

                if (String.IsNullOrEmpty(name))
                    name = id;
            }
            catch { }

            if (String.IsNullOrEmpty(name)) name = NO_NAME_FOUND;

            // BUGFIX 6937: remove line breaks and tab from name
            name = name.Replace("\r\n", " ");
            name = name.Replace('\n', ' ');
            name = name.Replace('\r', ' ');
            name = name.Replace('\t', ' ');

            return name.Trim();
        }

        public static String GetId(string role, string name)
        {
            string id = ERROR_IDENTIFICATION_FAILED;

            try
            {
                // NOTE: role should be a number (according to ACC), but this is not always the case
                // e.g. Firefox (3.6.8) returns html tags such as "div", "pre", "h4", ...

                // note: an empty name is not very useful here because it would result in id only e.g. "42-" for all unnamed text fields
                // note: double check required to handle names that use special characters only, e.g. buttons with names "<" and ">"
                if (!String.IsNullOrEmpty(name))
                    name = BaseUtils.sanitizeString(name);
                if (!String.IsNullOrEmpty(name))
                    id = BaseUtils.sanitizeString(role + "-" + name);
            }
            catch (Exception) { }

            return id;
        }

        public static String GetId(SystemAccessibleObject sao)
        {
            string id = ERROR_IDENTIFICATION_FAILED;

            try
            {
                if (sao != null)
                {
                    //string name = sao.Name.Trim(); // causes exception if null and is not required due to use of BaseUtils.sanitizeString() below
                    string name = sao.Name;
                    string role = sao.Role.ToString();

                    // TODO: IE & FF report static text as RoleSystemText=42 instead of RoleSystemStatictext=41
                    //       but value can only be read from Text field but not from static text
                    //       also the state "read-only" might be used to distinguish Text and Statictext

                    if (string.IsNullOrEmpty(name) && ConfigFile.Instance.analyzerMode)
                        name = "-";
                    id = GetId(role, name);
                    if (ConfigFile.Instance.analyzerMode)
                        id += " [" + sao.RoleString + "]";

                    /*
                    try
                    {
                        // debug info
                        id += " [" + ((AccRoles)sao.RoleIndex).ToString().Replace("RoleSystem", "") + "]";
                    }
                    catch (Exception ex) { Console.WriteLine(ex); }
                    */

                    /* VIE optimization
                    try
                    {
                        if (sao.RoleIndex == (int)AccRoles.RoleSystemStatictext || sao.RoleIndex == (int)AccRoles.RoleSystemText)
                        {
                            var parent = sao.Parent;
                            switch (parent.RoleIndex)
                            {
                                case (int)AccRoles.RoleSystemLink:
                                case (int)AccRoles.RoleSystemPushbutton:
                                // TODO: with region or not??
                                case (int)Acc.AccRoles.RoleSystemPane:
                                case (int)Acc.AccRoles.RoleSystemGrouping:
                                    // create acc id using parent's name
                                    var idOverwrite = GetId(parent.Role.ToString(), parent.Name);
                                    if (!string.IsNullOrEmpty(idOverwrite))
                                        id = idOverwrite;
                                    break;
                            }
                        }
                    }
                    catch { }
                    */

                    //
                    // BLCP-520: legacy code to ensure backward compatibility with DB before v3.6
                    //
                    if (String.IsNullOrEmpty(id))
                    {

                        try
                        {
                            var parent = sao.Parent;

                            if (sao.RoleIndex == (int)AccRoles.RoleSystemGraphic)
                            {
                                // fix images without names:
                                // use parent's name if parent is a rather small control elements
                                // note: list of controls may be adjusted on demand
                                switch (parent.RoleIndex)
                                {
                                    case (int)AccRoles.RoleSystemTitlebar:
                                    case (int)AccRoles.RoleSystemMenubar:
                                    case (int)AccRoles.RoleSystemMenupopup:
                                    case (int)AccRoles.RoleSystemMenuitem:
                                    case (int)AccRoles.RoleSystemTooltip:
                                    case (int)AccRoles.RoleSystemToolbar:
                                    case (int)AccRoles.RoleSystemStatusbar:
                                    case (int)AccRoles.RoleSystemCell:
                                    case (int)AccRoles.RoleSystemLink:
                                    case (int)AccRoles.RoleSystemListitem:
                                    case (int)AccRoles.RoleSystemOutlineitem:
                                    case (int)AccRoles.RoleSystemStatictext:
                                    case (int)AccRoles.RoleSystemText:
                                    case (int)AccRoles.RoleSystemPushbutton: // fixes button image detection in Visual Studio 2010
                                    case (int)AccRoles.RoleSystemCheckbutton:
                                    case (int)AccRoles.RoleSystemRadiobutton:
                                    case (int)AccRoles.RoleSystemCombobox:
                                    case (int)AccRoles.RoleSystemDroplist:
                                    case (int)AccRoles.RoleSystemProgressbar:
                                    case (int)AccRoles.RoleSystemSpinbutton:
                                    case (int)AccRoles.RoleSystemSplitbutton:
                                    case (int)AccRoles.RoleSystemButtondropdown:
                                    case (int)AccRoles.RoleSystemButtonmenu:
                                    case (int)AccRoles.RoleSystemButtondropdowngrid:

                                        // create acc id using parent's name
                                        // TODO: should use parent.Role but this would break backward compatibility
                                        id = GetId(sao.Role.ToString(), parent.Name);
                                        break;
                                }
                            }
                            else if (sao.RoleIndex == (int)AccRoles.RoleSystemTitlebar)
                            {
                                // TODO: fails for Windows Ribbon style, which has titlebar in ribbon (type "RoleSystemPropertypage" = 38), which has name "ribbon"

                                // fix titlebar without name:
                                // use parent's name / window name if title bar is unnamed
                                switch (parent.RoleIndex)
                                {
                                    case (int)AccRoles.RoleSystemWindow:
                                        // create acc id using parent's name
                                        id = GetId(sao.Role.ToString(), parent.Name);
                                        break;
                                }
                            }

                            /*
                            // replaced by inheritance concept
                            if (String.IsNullOrEmpty(id))
                            {
                                // BLCP-472: use parent id
                                // especially solves problems with <span> and <div> tags in Firefox
                                id = GetId(parent.Role.ToString(), parent.Name);
                            }
                            */
                        }
                        catch { }
                    }
                }
            }
            catch (Exception) { }

            if (String.IsNullOrEmpty(id))
                id = ERROR_IDENTIFICATION_FAILED;
            return id;
        }

        // returns meaningful ID, i.e.
        // 1.) search path of parents for first meaningful ID
        // 2.) merge all childdren of major control (e.g. button) to ID of that major control
        public static String GetOptimizedId(ref SystemAccessibleObject saoRef)
        {
            string id = ERROR_IDENTIFICATION_FAILED;
            SystemAccessibleObject sao = saoRef;
            Rectangle bounds;
            try { bounds = sao.Location; }
            catch { bounds = Rectangle.Empty; }

            var depth = ConfigFile.Instance.searchDepth;

            for (int i = 0; sao != null && i < depth; i++)
            {
                try
                {
                    // check whether parent includes top level children or not
                    // notes: filter elements with wrong bounds such as tab headers
                    if (!bounds.IsEmpty)
                    {
                        Rectangle parentBounds = sao.Location;
                        parentBounds.Intersect(bounds);
                        if (!parentBounds.Equals(bounds))
                        {
                            // skip parent that does not include top level element
                            sao = sao.Parent;
                            continue;
                        }
                    }

                    string name = sao.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        // continue with parent
                        sao = sao.Parent;
                        continue;
                    }

                    // note: 
                    // - Role is typically the number of the role, but maybe a string (especially for web browsers), which relates to RoleIndex -1
                    // - RoleIndex is the number of the role or -1
                    // - RoleString is a language dependent name of the role                   
                    string role = sao.Role.ToString();
                    int type = sao.RoleIndex;

                    switch (type)
                    {
                        case (int)AccRoles.RoleSystemMenupopup:
                        case (int)AccRoles.RoleSystemMenuitem:
                        case (int)AccRoles.RoleSystemToolbar:
                        case (int)AccRoles.RoleSystemTooltip:
                        case (int)AccRoles.RoleSystemLink:
                        case (int)AccRoles.RoleSystemListitem:
                        case (int)AccRoles.RoleSystemOutlineitem:
                        case (int)AccRoles.RoleSystemPagetab:
                        case (int)AccRoles.RoleSystemPushbutton:
                        case (int)AccRoles.RoleSystemCheckbutton:
                        case (int)AccRoles.RoleSystemRadiobutton:
                        case (int)AccRoles.RoleSystemCombobox:
                        case (int)AccRoles.RoleSystemDroplist:
                        case (int)AccRoles.RoleSystemSpinbutton:
                        case (int)AccRoles.RoleSystemSplitbutton:
                        case (int)AccRoles.RoleSystemButtondropdown:
                        case (int)AccRoles.RoleSystemButtonmenu:
                        case (int)AccRoles.RoleSystemButtondropdowngrid:
                            // major control found

                            // overwrite existing ID with major control element
                            id = GetId(role, name);

                            // keep ref
                            saoRef = sao;

                            // do not continue with parents
                            sao = null;
                            break;

                        default:
                            // no major control

                            // use ID if nothing found so far
                            if (string.IsNullOrEmpty(id))
                            {
                                id = GetId(role, name);
                                // keep ref
                                saoRef = sao;
                            }

                            // continue with parent
                            sao = sao.Parent;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("- GetOptimizedId - " + ex);

                    // continue with parent
                    if (sao != null)
                        sao = sao.Parent;
                }
            }

            if (String.IsNullOrEmpty(id))
                id = ERROR_IDENTIFICATION_FAILED;
            return id;
        }
    }
}
