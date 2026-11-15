using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Opens case windows without blocking the rest of the app: several cases can be open side by side,
    /// and opening a case that is already open brings its window to the front instead of a second copy.
    /// </summary>
    internal static class CaseWindows
    {
        private static readonly Dictionary<int, CaseForm> open = new Dictionary<int, CaseForm>();

        /// <summary>Opens an existing case. <paramref name="onChanged"/> runs after the window closes if anything was saved.</summary>
        public static void OpenCase(IWin32Window owner, int id, Action onChanged)
        {
            // Look by the case shown, not by the key: a new case keeps its temporary key after its first save.
            CaseForm existing = null;
            foreach (CaseForm form in open.Values)
                if (!form.IsDisposed && form.CaseId == id) existing = form;
            if (existing != null)
            {
                if (existing.WindowState == FormWindowState.Minimized) existing.WindowState = FormWindowState.Normal;
                existing.Activate();
                return;
            }
            Show(owner, () => new CaseForm(id), id, onChanged);
        }

        /// <summary>Opens a new court case or property deal.</summary>
        public static void NewCase(IWin32Window owner, bool isDeal, Action onChanged)
        {
            Show(owner, () => new CaseForm(isDeal), 0, onChanged);
        }

        /// <summary>Number of open case windows with changes that were not saved.</summary>
        public static int UnsavedCount
        {
            get
            {
                int count = 0;
                foreach (CaseForm form in open.Values)
                    if (!form.IsDisposed && form.HasUnsavedChanges) count++;
                return count;
            }
        }

        /// <summary>Closes every case window without asking again (the user already confirmed on logout).</summary>
        public static void CloseAll()
        {
            foreach (var form in new List<CaseForm>(open.Values))
            {
                if (form.IsDisposed) continue;
                form.DiscardChangesOnClose = true;
                form.Close();
            }
        }

        private static void Show(IWin32Window owner, Func<CaseForm> create, int id, Action onChanged)
        {
            CaseForm form;
            try
            {
                form = create();
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן לפתוח את התיק.", ex);
                return;
            }
            int key = id != 0 ? id : -Environment.TickCount; // new cases get a temporary key
            open[key] = form;
            form.FormClosed += (s, e) =>
            {
                open.Remove(key);
                if (form.Changed && onChanged != null) onChanged();
            };
            form.Show(owner);
        }
    }
}
