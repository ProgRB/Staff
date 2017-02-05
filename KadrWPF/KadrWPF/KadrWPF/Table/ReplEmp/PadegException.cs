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
using LibraryKadr;

namespace Kadr
{
    public partial class PadegException : Form
    {
        private string[] except= new string[5]{"","","","",""};
        private void FillExceptionGrid(int cur_row, int First_disp_row)
        {
            OracleDataAdapter a = new OracleDataAdapter(string.Format("select * from {0}.padeg_except order by val,padeg_id",DataSourceScheme.SchemeName),Connect.CurConnect);
            DataTable table = new DataTable();
            a.Fill(table);
            GridExcept.DataSource = table;
            GridExcept.Columns["padeg_id"].Visible = false;
            GridExcept.Columns["val"].HeaderText = "Слово";
            GridExcept.Columns["ROD_P"].HeaderText = "Родительный падеж";
            GridExcept.Columns["DAT_P"].HeaderText = "Дательный падеж";
            GridExcept.Columns["VIN_P"].HeaderText = "Винительный падеж";
            GridExcept.Columns["TYPE_WORD"].Visible = false;
            if (cur_row < GridExcept.RowCount)
                GridExcept.Rows[cur_row].Selected = true;
            if (First_disp_row < GridExcept.RowCount)
                GridExcept.FirstDisplayedScrollingRowIndex = First_disp_row;

        }
        public PadegException()
        {
            InitializeComponent();
            FillExceptionGrid(0, 0);
        }
        
        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rbFam_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTheNoun.Checked)
                SexWord.Enabled = false;
            else
                SexWord.Enabled = true;
            Val_TextChanged(null, null);

        }

        private void btNewExceptWord_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand(string.Format("insert into {0}.padeg_except(padeg_id) values({0}.padeg_id_seq.nextval)", DataSourceScheme.SchemeName), Connect.CurConnect);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "commit";
            cmd.ExecuteNonQuery();
            FillExceptionGrid(0, 0);
        }

        private void btDropFromDict_Click(object sender, EventArgs e)
        {
            if (GridExcept.SelectedRows.Count>0 &&  MessageBox.Show(this, "Вы действительно хотите удалить слово из списка исключений?", "АРМ Штатное расписание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OracleCommand cmd = new OracleCommand(string.Format("Delete from {0}.padeg_except where padeg_id='{1}'", DataSourceScheme.SchemeName, GridExcept.SelectedRows[0].Cells["padeg_id"].Value.ToString()), Connect.CurConnect);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
                FillExceptionGrid(0, 0);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            int k;
            if (rbFam.Checked) k=(SexWord.Text=="М"?0:1);
            else
                if (rbName.Checked) k=(SexWord.Text=="М"?2:3);
            else
                    if (rbMiddle.Checked) k=(SexWord.Text=="М"?4:5);
            else
                        k=6;

            OracleCommand cmd = new OracleCommand(string.Format("update {0}.padeg_except set val='{1}', ROD_P='{2}',DAT_P='{3}', VIN_P='{4}',TYPE_WORD='{5}' where padeg_id='{6}'",
                     DataSourceScheme.SchemeName, Val.Text, Rod.Text, dat.Text, Vin.Text, k, GridExcept.SelectedRows[0].Cells["padeg_id"].Value.ToString()), Connect.CurConnect);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "commit";
            cmd.ExecuteNonQuery();
            FillExceptionGrid(GridExcept.SelectedRows[0].Index, GridExcept.FirstDisplayedScrollingRowIndex);
            btSave.Enabled = false;
        }

        private void GridExcept_SelectionChanged(object sender, EventArgs e)
        {
            if (GridExcept.SelectedRows.Count > 0)
            {
                groupBox1.Enabled = true;
                DataGridViewRow r =GridExcept.SelectedRows[0];

                except[0] = r.Cells["val"].Value.ToString();
                except[1] = r.Cells["Rod_p"].Value.ToString();
                except[2] = r.Cells["Dat_p"].Value.ToString();
                except[3] = r.Cells["Vin_p"].Value.ToString();
                except[4] = r.Cells["type_word"].Value.ToString();

                Val.Text = r.Cells["val"].Value.ToString();
                Rod.Text = r.Cells["Rod_p"].Value.ToString();
                dat.Text = r.Cells["Dat_p"].Value.ToString();
                Vin.Text = r.Cells["Vin_p"].Value.ToString();

                switch (r.Cells["type_word"].Value.ToString())
                {
                    case "0": { rbFam.Checked = true; SexWord.Text = "М"; } break;
                    case "1": { rbFam.Checked = true; SexWord.Text = "Ж"; } break;
                    case "2": { rbName.Checked = true; SexWord.Text = "М"; } break;
                    case "3": { rbName.Checked = true; SexWord.Text = "Ж"; } break;
                    case "4": { rbMiddle.Checked = true; SexWord.Text = "М"; } break;
                    case "5": { rbMiddle.Checked = true; SexWord.Text = "Ж"; } break;
                    case "6": rbTheNoun.Checked = true; break;
                }
            }
            else
                groupBox1.Enabled = false;
        }

        private void Val_TextChanged(object sender, EventArgs e)
        {
            int k;
            if (rbFam.Checked) k = (SexWord.Text == "М" ? 0 : 1);
            else
                if (rbName.Checked) k = (SexWord.Text == "М" ? 2 : 3);
                else
                    if (rbMiddle.Checked) k = (SexWord.Text == "М" ? 4 : 5);
                    else
                        k = 6;

            if (except[0].ToUpper() == Val.Text.ToUpper() &&
                except[1].ToUpper() == Rod.Text.ToUpper() &&
                except[2].ToUpper() == dat.Text.ToUpper() &&
                except[3].ToUpper() == Vin.Text.ToUpper() &&
                except[4] == k.ToString())
                btSave.Enabled = false;
            else
                btSave.Enabled = true;
        }


    }
}
