using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LiveContext.Utility
{
    public class HyperlinkUtilities
    {
        public static Hyperlink ClearHyperlinkImageSources(Hyperlink hyperlink)
        {
            foreach (var entry in hyperlink.Inlines)
            {
                if (entry.GetType() == typeof(InlineUIContainer) &&
                    ((InlineUIContainer)entry).Child.GetType() == typeof(System.Windows.Controls.Image))
                {
                    System.Windows.Controls.Image image = ((InlineUIContainer)entry).Child as System.Windows.Controls.Image;
                    image.Source = null;
                }
            }

            return hyperlink;
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

        public static List<Hyperlink> GetHyperlinks(FlowDocument flowDocument)
        {
            List<Hyperlink> hyperlinks = GetVisuals(flowDocument).OfType<Hyperlink>().ToList<Hyperlink>();

            return hyperlinks;
        }
        public static bool IsMimeTypeImageContainer(InlineUIContainer container)
        {
            Image mimeTypeImage;

            return IsMimeTypeImageContainer(container, out mimeTypeImage);
        }

        public static bool IsMimeTypeImageContainer(InlineUIContainer container, out Image mimeTypeImage)
        {
            mimeTypeImage = null;

            try
            {
                if (container.GetType() == typeof(InlineUIContainer))
                {
                    if (container.Child != null && container.Child.GetType() == typeof(Image))
                    {
                        Image image = container.Child as Image;
                        if (image != null && String.IsNullOrEmpty(image.Uid) && image.Tag != null)
                        {
                            mimeTypeImage = image;
                            return true;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                System.Console.WriteLine(exc.ToString());
            }

            return false;
        }

        public static bool ContainsMimeTypeImage(Hyperlink hyperlink, out InlineUIContainer mimeTypeImageContainer, out Image mimeTypeImage)
        {
            mimeTypeImageContainer = null;
            mimeTypeImage = null;

            try
            {
                if (hyperlink != null)
                {
                    if (hyperlink.Inlines.Count > 0)
                    {
                        foreach (var entry in hyperlink.Inlines)
                        {
                            if (entry.GetType() == typeof(InlineUIContainer))
                            {
                                InlineUIContainer container = entry as InlineUIContainer;
                                if (IsMimeTypeImageContainer(container, out mimeTypeImage))
                                {
                                    mimeTypeImageContainer = container;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                System.Console.WriteLine(exc.ToString());
            }

            return false;
        }

    }
}
