using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiveContext.Utility 
{
    public class XamlUtilities
    {
        private static string richtTextPre = "<Section xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\" TextAlignment=\"Left\" LineHeight=\"Auto\" IsHyphenationEnabled=\"False\" xml:lang=\"de\" FlowDirection=\"LeftToRight\" NumberSubstitution.CultureSource=\"User\" NumberSubstitution.Substitution=\"AsCulture\" FontFamily=\"Tahoma\" FontStyle=\"Normal\" FontWeight=\"Normal\" FontStretch=\"Normal\" FontSize=\"12\" Foreground=\"#FF000000\" Typography.StandardLigatures=\"True\" Typography.ContextualLigatures=\"True\" Typography.DiscretionaryLigatures=\"False\" Typography.HistoricalLigatures=\"False\" Typography.AnnotationAlternates=\"0\" Typography.ContextualAlternates=\"True\" Typography.HistoricalForms=\"False\" Typography.Kerning=\"True\" Typography.CapitalSpacing=\"False\" Typography.CaseSensitiveForms=\"False\" Typography.StylisticSet1=\"False\" Typography.StylisticSet2=\"False\" Typography.StylisticSet3=\"False\" Typography.StylisticSet4=\"False\" Typography.StylisticSet5=\"False\" Typography.StylisticSet6=\"False\" Typography.StylisticSet7=\"False\" Typography.StylisticSet8=\"False\" Typography.StylisticSet9=\"False\" Typography.StylisticSet10=\"False\" Typography.StylisticSet11=\"False\" Typography.StylisticSet12=\"False\" Typography.StylisticSet13=\"False\" Typography.StylisticSet14=\"False\" Typography.StylisticSet15=\"False\" Typography.StylisticSet16=\"False\" Typography.StylisticSet17=\"False\" Typography.StylisticSet18=\"False\" Typography.StylisticSet19=\"False\" Typography.StylisticSet20=\"False\" Typography.Fraction=\"Normal\" Typography.SlashedZero=\"False\" Typography.MathematicalGreek=\"False\" Typography.EastAsianExpertForms=\"False\" Typography.Variants=\"Normal\" Typography.Capitals=\"Normal\" Typography.NumeralStyle=\"Normal\" Typography.NumeralAlignment=\"Normal\" Typography.EastAsianWidths=\"Normal\" Typography.EastAsianLanguage=\"Normal\" Typography.StandardSwashes=\"0\" Typography.ContextualSwashes=\"0\" Typography.StylisticAlternates=\"0\">";
        private static string richtTextPost = "</Section>";
        private static string paragraphPre = "<Paragraph><Run xml:lang=\"de-de\">";
        private static string paragraphPost = "</Run></Paragraph>";


        public static string ConvertStringToXaml(string text)
        {
            string returnXamlString = "";

            returnXamlString = richtTextPre;

            int lastLineBreakPos = 0;
            int lineBreakPos = text.IndexOf("\n");
            while (lineBreakPos != -1)
            {
                string line = text.Substring(lastLineBreakPos, (lineBreakPos - lastLineBreakPos));
                returnXamlString += (paragraphPre + line + paragraphPost);
                lastLineBreakPos = lineBreakPos + 1;
                lineBreakPos = text.IndexOf("\n", lineBreakPos+1);
            }

            string lastLine = text.Substring(lastLineBreakPos, text.Length - lastLineBreakPos);
            if (lastLine.Length > 0)
                returnXamlString += (paragraphPre + lastLine + paragraphPost);

            returnXamlString += richtTextPost;

            return returnXamlString;
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
           // Confirm parent and childName are valid. 
           if (parent == null) return null;

           T foundChild = null;

           int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
           for (int i = 0; i < childrenCount; i++)
           {
              var child = VisualTreeHelper.GetChild(parent, i);
              // If the child is not of the request child type child
              T childType = child as T;
              if (childType == null)
              {
                 // recursively drill down the tree
                 foundChild = FindChild<T>(child, childName);

                 // If the child is found, break so we do not overwrite the found child. 
                 if (foundChild != null) break;
              }
              else if (!string.IsNullOrEmpty(childName))
              {
                 var frameworkElement = child as FrameworkElement;
                 // If the child's name is set for search
                 if (frameworkElement != null && frameworkElement.Name == childName)
                 {
                    // if the child's name is of the request name
                    foundChild = (T)child;
                    break;
                 }
              }
              else
              {
                 // child element found.
                 foundChild = (T)child;
                 break;
              }
           }

           return foundChild;
        }

        private static IEnumerable<DependencyObject> GetVisuals(DependencyObject root)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
            {
                yield return child;
                foreach (var descendants in GetVisuals(child))
                    yield return descendants;
            }
        }

    }
}