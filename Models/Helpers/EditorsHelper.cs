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

        internal static List<EditorPageView> GetEditors()
        {
            return editorPageViews;
        }

        internal static int IndexOf(EditorPageViewModel vm)
        {
            return editorPageViews.Select(x => x.DataContext as EditorPageViewModel).ToList().IndexOf(vm);
        }

        internal static void RemoveAt(int index)
        {
            var editor = editorPageViews[index];
            var dc = editor.DataContext as EditorPageViewModel;
            dc.OnRemove();
            editorPageViews.RemoveAt(index);
        }

        internal static EditorPageView AddNewEditorPage(SelectEditorResult parameter)
        {
            var editorView = new EditorPageView();
            var vm = new EditorPageViewModel(parameter);
            editorView.DataContext = vm;
            editorPageViews.Add(editorView);
            return editorView;
        }

        internal static EditorPageView GetPageView(int index)
        {
            return editorPageViews[index];
        }
    }
}
