using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevCore.Core.Editor
{
    /// <summary>
    /// Utility class to use UI Elements
    /// </summary>
    public static class DevCoreUIElements
    {
        /// <summary>
        /// return a split view and automatically register the input panes
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="size"></param>
        /// <param name="orientation"></param>
        /// <param name="leftPane"></param>
        /// <param name="rightPane"></param>
        /// <returns></returns>
        public static VisualElement AttachSplitViews(VisualElement parent, float size, 
            TwoPaneSplitViewOrientation orientation,
            [NotNull] VisualElement leftPane,
            [NotNull] VisualElement rightPane) {
		
            var splitView = new TwoPaneSplitView(0, size, orientation);
            parent.Add(splitView);

            splitView.Add(leftPane);
            splitView.Add(rightPane);
            return splitView;
        }
    }
}
