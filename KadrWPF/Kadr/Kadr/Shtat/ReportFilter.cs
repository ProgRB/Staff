using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using Oracle.DataAccess.Client;
namespace Kadr.Shtat
{
    public partial class ReportFilter : Form
    {
        public ReportFilter(decimal? subdiv_id,string degree_id)
        {
            InitializeComponent();
            current_subdiv.subdiv_id = subdiv_id;
            cur_date.Value = DateTime.Now;
            DataTable t = new DataTable();
            new OracleDataAdapter(string.Format("select degree_id,degree_name,code_degree from {0}.degree", DataSourceScheme.SchemeName), LibraryKadr.Connect.CurConnect).Fill(t);
            cbCodeDegree.DataSource = t;
            cbCodeDegree.DisplayMember = "code_degree";
            cbCodeDegree.ValueMember = "degree_id";
            cbNameDegree.DataSource = t;
            cbNameDegree.DisplayMember = "degree_name";
            cbNameDegree.ValueMember = "degree_id";
            cbCodeDegree.SelectedValue = degree_id;
        }

        public decimal? DegreeId
        {
            get
            {
                if (cbCodeDegree.SelectedValue == null)
                    return null;
                else
                    return decimal.Parse(cbCodeDegree.SelectedValue.ToString());
            }
        }
        public DateTime CurrentDate
        {
            get
            {
                return cur_date.Value;
            }
        }
        public decimal? subdiv_id
        {
            get
            {
                return current_subdiv.subdiv_id;
            }
        }

        public string DegreeName
        {
            get
            {
                return cbNameDegree.Text;
            }
        }

        public string CodeSubdiv
        {
            get
            {
                return current_subdiv.CodeSubdiv;
            }
        }

        public string SubdivName
        {
            get
            {
                return current_subdiv.SubdivName;
            }
        }

        private void ReportFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult==DialogResult.OK && (this.DegreeId == null || subdiv_id!=null))
                e.Cancel = true;
        }
    }

        
}
