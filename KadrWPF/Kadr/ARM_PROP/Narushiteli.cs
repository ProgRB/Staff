using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Staff;
using EditorsLibrary;
using System.IO;
using System.Reflection;
using Kadr;

namespace ARM_PROP
{
    public partial class Narushiteli : Form
    {
        EMP_seq emp;
        LIST_VIOLATOR_seq list_violator;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq types_violation;
        TYPE_EXACT_seq types_exact;
        ZurnNaruRe jurnal;
        //string last_name;
        string per_num;
        decimal perco_sync_id;
        decimal other_violator_id;
        OracleDataTable dtEmp;
        StringBuilder sort;
        string textBlock;
        SotZavod sotzavod;
        StrSotr strsotr;
        AddForm addform;
        Narushiteli narush;
        //FormMenu formMenu;
        bool flagAdd;

        /// <summary>
        /// Конструктор формы добавления нарушителя
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public Narushiteli(string perNum, bool _flagAdd, 
            decimal _perco_sync_id, decimal _other_violator_id, ZurnNaruRe _jurnal)
        {
            InitializeComponent();
            perNum = per_num;
            perco_sync_id = _perco_sync_id;
            other_violator_id = _other_violator_id;
            jurnal = _jurnal;
            // Создание и заполнение таблиц данными
            types_violation = new TYPE_VIOLATION_seq(Connect.CurConnect);
            types_violation.Fill();
            flagAdd = _flagAdd;
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            list_violator = new LIST_VIOLATOR_seq(Connect.CurConnect);
            types_exact = new TYPE_EXACT_seq(Connect.CurConnect);
            types_exact.Fill();
            if (flagAdd)
            {
                violation_log.AddNew();
                ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_EXACT_ID =
                        ((TYPE_EXACT_obj)((CurrencyManager)BindingContext[types_exact]).Current).TYPE_EXACT_ID;
            }
            else
            {
                violation_log.Fill();
            }

            cbPriznak.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TYPE_VIOLATION_ID, new LinkArgument(types_violation, TYPE_VIOLATION_seq.ColumnsName.TYPE_VIOLATION_NAME));
            cbVzisk.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TYPE_EXACT_ID, new LinkArgument(types_exact, TYPE_EXACT_seq.ColumnsName.TYPE_EXACT_NAME));
            tbPriZader.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.CAUSE_ARREST);
            tbEdinIzm.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.UNIT);
            mtbDatevzisk.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.EXACT_DATE);
            mtbZader.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.ARREST_DATE);
            mtbPodra.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.INFORM_SUBD_DATE);
            tbMesuri.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TAKE_MEASURES);
            tbNote.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.NOTE);            
            tbKolvo.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.QUANTITY_GOODS);
            tbSumm.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TOTAL_STOLEN);
            checkBox1.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.SIGN_GROUP);
            checkBox2.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.SIGN_CRIMINAL);
            chbprizKrag.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.SIGN_STEAL);
            mtbDatevzisk.Enabled = true;
            ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).ARREST_DATE = DateTime.Now;
            
        }

        /// <summary>
        /// Событие нажатия кнопки добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAdd_Click(object sender, EventArgs e)
        {
            if (cbPriznak.Text.Trim() == "")
            {
                MessageBox.Show("Вы не выбрали признак нарушения!", "АРМ Нарушения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.cbPriznak.Focus();                
                return;
            }
            //if (cbVzisk.Text.Trim() == "")
            //{
            //    MessageBox.Show("Вы не выбрали тип взыскания!", "АРМ Нарушения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    this.cbVzisk.Focus(); 
            //    return;
            //}
            if (mtbZader.Text == "")
            {
                MessageBox.Show("Вы не ввели дату задержания!", "АРМ Нарушения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.mtbZader.Focus(); 
                return;
            }
            if (tbPriZader.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели причину задержания!", "АРМ Нарушения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.tbPriZader.Focus();
                return;
            }
            if (tbMesuri.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели принятые меры!", "АРМ Нарушения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.tbMesuri.Focus(); 
                return;
            }
            else
            {
                mtbZader.Enabled = true;
                mtbDatevzisk.Enabled = true;
                mtbPodra.Enabled = true;
                tbPriZader.Enabled = true;
                tbMesuri.Enabled = true;
                tbNote.Enabled = true;
                DialogResult rez;
                if (rbStNarush.Checked == true)
                {
                    addform = new AddForm(other_violator_id);
                    rez = addform.ShowDialog();
                    if (rez == DialogResult.OK)
                    {
                        list_violator.AddNew();
                        ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).OTHER_VIOLATOR_ID =
                            addform.other_violator_id;
                        ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).VIOLATION_LOG_ID =
                            ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).VIOLATION_LOG_ID;
                    }
                }
                if (rbStZavod.Checked == true)
                {
                    FiltrJurn filter = new FiltrJurn();
                    DialogResult rezFilter = filter.ShowDialog();
                    sort = filter.sort;
                    if (rezFilter == DialogResult.OK)
                    {
                        emp = new EMP_seq(Connect.CurConnect);
                        string strTemp = rezFilter == DialogResult.OK ? " where " + filter.str_filter.ToString() : "";
                        textBlock = string.Format(Queries.GetQuery("SelectListTr.sql"), Staff.DataSourceScheme.SchemeName/*, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                            SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), EMP_seq.ColumnsName.PER_NUM.ToString(),
                            EMP_seq.ColumnsName.EMP_LAST_NAME.ToString(), EMP_seq.ColumnsName.EMP_FIRST_NAME.ToString(),
                            EMP_seq.ColumnsName.EMP_MIDDLE_NAME.ToString(), "position", POSITION_seq.ColumnsName.CODE_POS.ToString(),
                            POSITION_seq.ColumnsName.POS_ID.ToString(), POSITION_seq.ColumnsName.POS_NAME.ToString(),
                            "emp", "transfer", TRANSFER_seq.ColumnsName.SIGN_CUR_WORK.ToString(),
                            TRANSFER_seq.ColumnsName.TRANSFER_ID.ToString()*/, strTemp);
                        OracleDataTable newDataEmp = new OracleDataTable(textBlock, Connect.CurConnect);
                        newDataEmp.Fill();
                        sotzavod = new SotZavod(newDataEmp, emp, per_num);
                        rez = sotzavod.ShowDialog();
                        if (rez == DialogResult.OK)
                        {
                            list_violator.AddNew();
                            ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).PER_NUM =
                                sotzavod.per_num;
                            ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).VIOLATION_LOG_ID =
                                ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).VIOLATION_LOG_ID;
                        }
                    }
                }
                if (rbStSotrudnik.Checked == true)
                {
                    strsotr = new StrSotr(perco_sync_id);
                    rez = strsotr.ShowDialog();
                    if (rez == DialogResult.OK)
                    {
                        list_violator.AddNew();
                        ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).PERCO_SYNC_ID =
                            strsotr.perco_sync_id;
                        ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).VIOLATION_LOG_ID =
                            ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).VIOLATION_LOG_ID;
                    }
                }             
                               
            }
            violation_log.Save();
            list_violator.Save();
            Connect.Commit();
            list_violator.Clear();

            emp = new EMP_seq(Connect.CurConnect);
            int vlog = Convert.ToInt32(((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).VIOLATION_LOG_ID);
            textBlock = string.Format(Queries.GetQuery("SelectNarush.sql"), Connect.Schema, vlog);
            OracleDataTable dtEmp = new OracleDataTable(textBlock, Connect.CurConnect);
            dtEmp.Fill();

            dgvNarush.DataSource = dtEmp;

            dgvNarush.Columns["SUBDIV_NAME"].HeaderText = "Подразд.";
            dgvNarush.Columns["SUBDIV_NAME"].Width = 70;
            dgvNarush.Columns["SUBDIV_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvNarush.Columns["SUBDIV_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNarush.Columns["PER_NUM"].HeaderText = "Табельный номер";
            dgvNarush.Columns["PER_NUM"].Width = 80;
            dgvNarush.Columns["PER_NUM"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvNarush.Columns["PER_NUM"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNarush.Columns["LAST_NAME"].HeaderText = "Фамилия";
            dgvNarush.Columns["LAST_NAME"].Width = 110;
            dgvNarush.Columns["LAST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvNarush.Columns["LAST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNarush.Columns["FIRST_NAME"].HeaderText = "Имя";
            dgvNarush.Columns["FIRST_NAME"].Width = 100;
            dgvNarush.Columns["FIRST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvNarush.Columns["FIRST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNarush.Columns["MIDDLE_NAME"].HeaderText = "Отчество";
            dgvNarush.Columns["MIDDLE_NAME"].Width = 100;
            dgvNarush.Columns["MIDDLE_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvNarush.Columns["MIDDLE_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNarush.Columns["POS_NAME"].HeaderText = "Должность";
            dgvNarush.Columns["POS_NAME"].Width = 250;
            dgvNarush.Columns["POS_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvNarush.Columns["POS_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvNarush.Columns["VIOLATION_LOG_ID"].Visible = false;
            dgvNarush.Columns["LIST_VIOLATOR_ID"].Visible = false;
            dgvNarush.Columns["PERCO_SYNC_ID"].Visible = false;
            dgvNarush.Columns["OTHER_VIOLATOR_ID"].Visible = false;

            if (dgvNarush.RowCount != 0)
            {
                btInsert.Enabled = true;
                btDelet.Enabled = true;
                mtbZader.Enabled = false;
                mtbDatevzisk.Enabled = false;
                mtbPodra.Enabled = false;
                tbPriZader.ReadOnly = true;
                tbMesuri.ReadOnly = true;
                tbNote.ReadOnly = true;
                cbPriznak.Enabled = false;
                cbVzisk.Enabled = false;
                btDelet.Enabled = true;                
            }
            if (dgvNarush.RowCount == 2)
            {
                checkBox1.Checked = true;
                ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).SIGN_GROUP = true;
            }
        }

        /// <summary>
        /// Событие изменения признака нарушения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbPriznak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPriznak.Text == "Кража")
            {                
                if (radioButton1.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "1";
                }
                if (radioButton2.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "2";
                }
                if (radioButton3.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "3";
                }
                if (radioButton4.Checked == true)
                {
                    ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC = "4";
                }

                if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "4")
                {
                    radioButton4.Checked = true;
                }
                if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "3")
                {
                    radioButton3.Checked = true;
                }
                if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "2")
                {
                    radioButton2.Checked = true;
                }
                if (((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).TYPE_STOLEN_TMC == "1")
                {
                    radioButton1.Checked = true;
                }
                chbprizKrag.Checked = true;
                ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).SIGN_STEAL = true;
               label13.Visible = true;
               panel2.Visible = true;
               panel3.Visible = true;
            }
            if (cbPriznak.Text != "Кража")
            {
                chbprizKrag.Checked = false;
                ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).SIGN_STEAL = false;
                label13.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
            }

        }

        /// <summary>
        /// Событие выполняемые при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Narushiteli_Load(object sender, EventArgs e)
        {
            label13.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            btDelet.Enabled = false;
            btInsert.Enabled = false;
            btSaveInsert.Visible = false;          
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btInsert_Click(object sender, EventArgs e)
        {
            btDelet.Enabled = true;
            mtbZader.Enabled = true;
            mtbDatevzisk.Enabled = true;
            mtbPodra.Enabled = true;
            tbPriZader.Enabled = true;
            tbMesuri.Enabled = true;
            tbNote.Enabled = true;
            cbPriznak.Enabled = true;
            cbVzisk.Enabled = true;
            btSaveInsert.Visible = true;            
        }

        /// <summary>
        /// Событие нажатия кнопки удалить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDelet_Click(object sender, EventArgs e)
        {
            if (dgvNarush.RowCount != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить нарушителя", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //list_violator.Remove((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current);
                    //list_violator.Save();
                    //violation_log.Remove((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current);
                    //violation_log.Save();
                    //connection.Commit();
                    //string vlid = ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).VIOLATION_LOG_ID.ToString();
                  
                    //string vlid = ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).VIOLATION_LOG_ID.ToString();
                    list_violator.Clear();
                    string list_violation_id = dgvNarush.CurrentRow.Cells["LIST_VIOLATOR_ID"].Value.ToString();
                    list_violator.Fill("where list_violator_id = " + list_violation_id);
                    dgvNarush.Rows.Remove(dgvNarush.CurrentRow);
                    list_violator.Remove((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current);                                        
                    list_violator.Save();
                    Connect.Commit();

                    if (dgvNarush.RowCount == 1)
                    {                        
                        checkBox1.Checked = false;
                        ((VIOLATION_LOG_obj)(((CurrencyManager)BindingContext[violation_log]).Current)).SIGN_GROUP = false;
                    }

                    //dgvNarush.Rows.Remove(dgvNarush.CurrentRow);
                    //list_violator.Remove((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current);
                    //list_violator.Save();
                    //violation_log.Save();
                    //connection.Commit();
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки сохранения изменения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveInsert_Click_1(object sender, EventArgs e)
        {
            list_violator.Save();
            violation_log.Save();
            Connect.Commit();
            list_violator.Clear();
            mtbZader.Enabled = false;
            mtbDatevzisk.Enabled = false;
            mtbPodra.Enabled = false;
            tbPriZader.ReadOnly = true;
            tbMesuri.ReadOnly = true;
            tbNote.ReadOnly = true;
            cbPriznak.Enabled = false;
            cbVzisk.Enabled = false;
            btSaveInsert.Visible = false;            
        }

        /// <summary>
        /// Событие нажатия кнопки выход из формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            list_violator.RollBack();
            Connect.Rollback();

            this.Close();
            jurnal.bsEmp.DataSource = jurnal.dtEmp;
            jurnal.dgvGurnal.DataSource = jurnal.dtEmp;
            jurnal.RefreshGridNarush();
        }

        /// <summary>
        /// Событие нажатия checkBox признака кражи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbprizKrag_CheckedChanged(object sender, EventArgs e)
        {
            if (chbprizKrag.Checked == false)
            {
                cbPriznak.Text = "Прочее";
            }
            if (chbprizKrag.Checked == true)
            {
                cbPriznak.Text = "Кража";
            }
        }
    }
}
