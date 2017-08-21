using System;
using System.Text.RegularExpressions;

namespace LiveContext.Utility
{
    public class StringConverter
    {
        // Converts a string of the form majorVersion.minorVersion.microVersion to three integers.
        public static void VersionStringToIntConverter(string version, out int majorVersion, out int minorVersion, out int microVersion)
        {
            majorVersion = 0;
            minorVersion = 0;
            microVersion = 0;

            var regex = new Regex(@"(?<major>\d+)(\.(?<minor>\d+))?(\.(?<micro>\d+))?");
            var match = regex.Match(version);

            var matches = regex.Match(version);
            var major = matches.Groups["major"];
            if (major.Success)
                majorVersion = int.Parse(major.Value);
            var minor = matches.Groups["minor"];
            if (minor.Success)
                minorVersion = int.Parse(minor.Value);
            var micro = matches.Groups["micro"];
            if (micro.Success)
                microVersion = int.Parse(micro.Value);
        }
        
        // Converts a string of the form majorVersion.minorVersion.microVersion to three integers.
        public static void ContentTextToTitleConverter(string contentText, out string title)
        {
            var tanga = contentText;

            try
            {
                // remove all xml-tags:
                Regex regex = new Regex(@"(\<(.*?)\>)");
                while (regex.IsMatch(tanga))
                {
                    tanga = regex.Replace(tanga, "");
                }

                // replace encoded characters (bugfix 6368)
                string str = tanga.Substring(0);
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&amp;", "&");
                tanga = str;

                // cut text to a maximal length of 20 chars
                if (tanga.Length > 20)
                    tanga = tanga.Substring(0, 17) + "...";
            }
            catch (Exception exc) { Console.WriteLine(exc); }

            title = tanga;
        }
    }
}
