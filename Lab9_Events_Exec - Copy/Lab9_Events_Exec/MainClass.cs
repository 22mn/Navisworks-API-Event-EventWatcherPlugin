using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using f = System.Windows.Forms;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace Lab9_Events_Exec
{
    // Plugin attribute - name, developer
    [Plugin("Lab9_Events", "TwentyTwo")]
    
    // EventWatcherPlugin interface - OnLoaded, OnUnloading 
    public class MainClass : EventWatcherPlugin
    {
        // document field
        Document doc = null;
        // method called when application start & Plugin is loaded/created
        public override void OnLoaded()
        {
            f.MessageBox.Show("OnLoaded method called!!!", "Method Called");
            // Subscribe to Active Document Changed event
            Application.ActiveDocumentChanged += Application_ActiveDocumentChanged;

        }

        // method called when application closing & Plugin is unloading
        public override void OnUnloading()
        {
            f.MessageBox.Show("OnUnloading method called!!!", "Method Called");
            // Unsubscribe to Active Document Changed event
            Application.ActiveDocumentChanged -= Application_ActiveDocumentChanged;

        }

        // method called when Active Document Changed
        private void Application_ActiveDocumentChanged(object sender, EventArgs e)
        {
            f.MessageBox.Show("Application_ActiveDocumentChanged method called!!!", "Method Called");
            // get the current document
            doc = Application.ActiveDocument;
            if (doc != null)
            {
                // Subscribe to Current Selection Changed event
                doc.CurrentSelection.Changed += CurrentSelection_Changed;
            }
        }

        // method called when Current Selection Changed 
        private void CurrentSelection_Changed(object sender, EventArgs e)
        {
            f.MessageBox.Show("CurrentSelection_Changed method called!!!", "Method Called");

            // get current selected items
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            // selection not null and selected item(s)
            if (selectionItems != null && selectionItems.Count > 0)
            {
                // empty collection for invert items & blur items
                ModelItemCollection invertItems = new ModelItemCollection();
                invertItems.CopyFrom(selectionItems);
                // invert items 
                invertItems.Invert(doc);
                // override transparency
                doc.Models.OverridePermanentTransparency(invertItems, 0.9);
            }

            // reset only there is/are model(s) 
            // (prevent the crash when opening another file)
            else if (doc.Models.Count > 0)
            {
                // resets transparency (color)
                doc.Models.ResetAllPermanentMaterials();
            }
        }

    }
}
