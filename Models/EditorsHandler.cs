﻿using cumcad.ViewModels;
using cumcad.ViewModels.Base;
using cumcad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.Models.Helpers
{
    internal class EditorsHandler
    {
        private static List<EditorPageView> editorPageViews = new List<EditorPageView>();

        internal static int GetListCount()
        {
            return editorPageViews.Count;
        }

        internal static List<EditorPageModel> GetEditorModels()
        {
            return editorPageViews.Select(x => (x.DataContext as EditorPageViewModel).editorModel).ToList();
        }

        internal static List<EditorPageModel> GetIndependentEditorModels(EditorPageModel currentEditor, IHandler element)
        {
            List<EditorPageModel> result = new List<EditorPageModel>();
            foreach (var model in GetEditorModels())
            {
                if (model == currentEditor)
                    continue;
                if (IsIndependentParent(model, currentEditor, element))
                {
                    result.Add(model);
                }
            }
            return result;
        }

        private static bool IsIndependentParent(EditorPageModel check, EditorPageModel current, IHandler element)
        {
            var parent = check.ParentEditorModel;
            var lastEditor = check;
            while (parent != null)
            {
                if (parent == current)
                {
                    int indexCurrent = current.IndexOf(element);
                    int indexCheck = current.IndexOf(lastEditor.EditorResult.ParentEditorItem);
                    return indexCheck < indexCurrent; // check if the 'element' is upper than 'parentEditorItem'

                }
                lastEditor = parent;
                parent = parent.ParentEditorModel;
            }
            return true;
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