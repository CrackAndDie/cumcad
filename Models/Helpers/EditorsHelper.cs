using cumcad.ViewModels;
using cumcad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.Models.Helpers
{
    internal class EditorsHelper
    {
        private static List<EditorPageView> editorPageViews = new List<EditorPageView>();

        internal static int GetListCount()
        {
            return editorPageViews.Count;
        }

        internal static EditorPageView AddNewEditorPage()
        {
            var editorView = new EditorPageView();
            editorPageViews.Add(editorView);
            return editorView;
        }

        internal static EditorPageView GetPageView(int index)
        {
            return editorPageViews[index];
        }
    }
}
