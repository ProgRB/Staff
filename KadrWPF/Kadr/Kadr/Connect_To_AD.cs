using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
namespace Kadr
{
    public partial class Connect_To_AD : Form
    {
        public Connect_To_AD()
        {
            InitializeComponent();
        }

        private void btLoadFromAD_Click(object sender, EventArgs e)
        {
            DirectoryContext context = new DirectoryContext(DirectoryContextType.DirectoryServer, "ds00.uuap.com");
            DomainController dc = DomainController.GetDomainController(context);
            DirectorySearcher dirSearcher = dc.GetDirectorySearcher();
            dirSearcher.Filter = "(objectClass=user)";
            var searchResults = dirSearcher.FindAll();
            /*var domainPath = "ds00.uuap.com";
            var directoryEntry = new DirectoryEntry(domainPath);
            var dirSearcher = new DirectorySearcher(directoryEntry);
            dirSearcher.SearchScope = SearchScope.Subtree;
            dirSearcher.Filter = string.Format("(&(objectClass=user)(|(cn={0})(sn={0})(givenName={0})(sAMAccountName={0})))", "bmv12714");
            var searchResults = dirSearcher.FindAll();*/
            foreach (SearchResult sr in searchResults)
            {
                var de = sr.GetDirectoryEntry();
                //...do smth
            }

            
        }
    }
}
