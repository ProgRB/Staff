using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Kadr;
using Oracle.DataAccess.Client;
namespace Kadr.Vacation_schedule
{
    public partial class Reminder : Form
    {
        private bool RefreshGrid()
        {
            DataTable t = new DataTable();
            OracleCommand cmd = new OracleCommand(string.Format(Queries.GetQuery("Go/alarm_vac.sql"), Connect.Schema),Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_subdiv_id",FilterVS.subdiv_id);
            new OracleDataAdapter(cmd).Fill(t);
            grid_alarm.Columns.Clear();
            grid_alarm.AutoGenerateColumns = false;
            grid_alarm.DataSource=t;
            DataGridViewButtonColumn c = new DataGridViewButtonColumn();
            c.DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Control);
            c.DefaultCellStyle.ForeColor = Color.Blue;
            c.DefaultCellStyle.SelectionForeColor = Color.Blue;
            c.DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Control);
            c.Name = "fl";
            c.UseColumnTextForButtonValue = true;
            c.Text = "Перейти к личной карточке";
            c.Width = 200;
            c.FlatStyle = FlatStyle.Popup;
            c.HeaderText = "";
            grid_alarm.Columns.Add(c);
            for (int i = 0; i < t.Columns.Count; ++i)
            {
                grid_alarm.Columns.Add(t.Columns[i].ColumnName, t.Columns[i].ColumnName);
                grid_alarm.Columns[i+1].DataPropertyName = t.Columns[i].ColumnName;
            }
            grid_alarm.Columns["transfer_id"].Visible=false;
            ColumnWidthSaver.FillWidthOfColumn(grid_alarm);
            Settings.SetDataGridCaption(ref grid_alarm);
            if (t.Rows.Count == 0) return false;
            else return true;
        }
        public static bool ShowForm(Form owner)
        {
            bool fl = false;
            foreach (Form fm in Application.OpenForms)
                if (fm.Name == "Reminder")
                {
                    fl = true;
                    break;
                }
            Reminder f;
            if (fl)
            {
                f=(Reminder)Application.OpenForms["Reminder"];
                f.Activate();
                f.WindowState = FormWindowState.Normal;
            }
            else
             f = new Reminder();
            if (f.RefreshGrid())
            {
                try
                {
                    f.Owner = owner;
                    f.Show();
                    return true;
                }
                catch
                {
                    return true;
                }
                
            }
            else
                return false;
        }
        private Reminder()
        {
            InitializeComponent();
            RefreshGrid();
        }

        private void Reminder_Activated(object sender, EventArgs e)
        {
            this.AllowTransparency = false;
        }

        private void Reminder_Deactivate(object sender, EventArgs e)
        {
            this.AllowTransparency = true;
            Opacity = 0.75;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_alarm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grid_alarm.Columns["fl"].Index && e.RowIndex > -1)
            {
                ViewCard frm = new ViewCard(grid_alarm["transfer_id", e.RowIndex].Value.ToString(),"");
                frm.ShowDialog();
            }
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void grid_alarm_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthSaver.SaveWidthOfColumn(sender, e);
        }
        
    }
}
