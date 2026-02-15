using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    internal static class Geometry
    {
        internal static void LimitContainerPanel1MaxSize(SplitContainer splitContainer, int maxPanelSize)
        {
            if (splitContainer.Panel1.Width > maxPanelSize)
                splitContainer.SplitterDistance = maxPanelSize;
        }

        internal static void LimitContainerPanel2MaxSize(SplitContainer splitContainer, int maxPanelSize)
        {
            if (splitContainer.Orientation == Orientation.Vertical)
            {
                int panel2Width = splitContainer.Width - splitContainer.SplitterDistance - splitContainer.SplitterWidth;

                if (panel2Width > maxPanelSize)
                {
                    splitContainer.SplitterDistance = splitContainer.Width - maxPanelSize - splitContainer.SplitterWidth;
                }
            }
            else
            {
                int panel2Height = splitContainer.Height - splitContainer.SplitterDistance - splitContainer.SplitterWidth;

                if (panel2Height > maxPanelSize)
                {
                    splitContainer.SplitterDistance = splitContainer.Height - maxPanelSize - splitContainer.SplitterWidth;
                }
            }
        }
    }
}
