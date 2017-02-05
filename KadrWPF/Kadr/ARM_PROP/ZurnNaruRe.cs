using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using LibraryKadr;
using Kadr;
using StaffReports;

namespace ARM_PROP
{
    public partial class ZurnNaruRe : Form
    {        
        EMP_seq emp;
        LIST_VIOLATOR_seq list_violator;
        VIOLATION_LOG_seq violation_log;
        TYPE_VIOLATION_seq types_violation;
        string per_num;
        decimal perco_sync_id;
        decimal other_violator_id;
        public OracleDataTable dtEmp, dtTransfer;
        public BindingSource bsEmp, bsTransfer;
        public ZurnNaruRe jurnal;
        public Group group;
        //StringBuilder sort;
        string textBlock;
        Narushiteli narushiteli;
        /// <summary>
        /// Конструктор формы списка нарушителей
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        /// <param name="_dtEmp">Таблица с данными нарушителей</param>
        /// <param name="FormMain">Родительская форма</param>
        public ZurnNaruRe(OracleDataTable _dtEmp, EMP_seq _emp)
        {
            InitializeComponent();        
            dtEmp = _dtEmp;
            emp = _emp;
          
            bsEmp = new BindingSource();
            bsEmp.DataSource = dtEmp;
            dgvGurnal.DataSource = bsEmp;

            bsEmp.PositionChanged += new EventHandler(PositionChange);
            PositionChange(bsEmp, null);

            types_violation = new TYPE_VIOLATION_seq(Connect.CurConnect);
            types_violation.Fill();
            violation_log = new VIOLATION_LOG_seq(Connect.CurConnect);
            violation_log.Fill();
            list_violator = new LIST_VIOLATOR_seq(Connect.CurConnect);
            list_violator.Fill();

            tbPrichzade.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.CAUSE_ARREST);
            tbMeri.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TAKE_MEASURES);
            tbPrim.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.NOTE);
            comboBox1.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.TYPE_VIOLATION_ID, new LinkArgument(types_violation, TYPE_VIOLATION_seq.ColumnsName.TYPE_VIOLATION_NAME));
            mtbZader.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.ARREST_DATE);
            mtbPodra.AddBindingSource(violation_log, VIOLATION_LOG_seq.ColumnsName.INFORM_SUBD_DATE);
            dgvDanNarush.AddBindingSource(violation_log, new LinkArgument(list_violator, LIST_VIOLATOR_seq.ColumnsName.VIOLATION_LOG_ID));
            dgvDanNarush.Columns["TYPE_STOLEN_TMC"].DisplayIndex = 10;
            dgvDanNarush.Visible = false;
            RefreshGridNarush();
            dgvGurnal_SelectionChanged(null, null);
        }

        /// <summary>
        /// Событие нажатия кнопки выхода из формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Событие нажатия кнопки добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btAdd_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() != 0)
            {
                MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }          
            narushiteli = new Narushiteli(per_num, true, perco_sync_id, other_violator_id, this);
            narushiteli.ShowDialog();
        }

        /// <summary>
        /// Событие нажатия кнопки редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btInsert_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() != 0)
            {
                MessageBox.Show("Вы не закончили работу в предыдущем окне!\nЗакройте окно и попытайтесь снова.", "АРМ Кадры", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dgvGurnal.CurrentRow != null)
            {
                string per_num = dgvGurnal.CurrentRow.Cells["per_num"].Value.ToString();
                string other_violation_id = dgvGurnal.CurrentRow.Cells["OTHER_VIOLATOR_ID"].Value.ToString();
                string perco_sync_id = dgvGurnal.CurrentRow.Cells["perco_sync_id"].Value.ToString();
                string last_name = dgvGurnal.CurrentRow.Cells["LAST_NAME"].Value.ToString();
                decimal violation_log_id = (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).VIOLATION_LOG_ID;//dgvDanNarush.CurrentRow.Cells["VIOLATION_LOG_ID"].Value.ToString();
                string laste_name = dgvGurnal.CurrentRow.Cells["LAST_NAME"].Value.ToString();
                string first_name = dgvGurnal.CurrentRow.Cells["FIRST_NAME"].Value.ToString();
                string middle_name = dgvGurnal.CurrentRow.Cells["MIDDLE_NAME"].Value.ToString();
                string cause_arrest = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).CAUSE_ARREST;//dgvDanNarush.CurrentRow.Cells["VIOLATION_LOG_ID"].Value.ToString();
                decimal types_exact_id = (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_EXACT_ID;
                string unit = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).UNIT;
                decimal kolvo = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).QUANTITY_GOODS != null ?
                (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).QUANTITY_GOODS : 0;
                decimal summ = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TOTAL_STOLEN != null ?
                (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TOTAL_STOLEN : 0;
                decimal type_violation_id = (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_VIOLATION_ID;//(decimal)dgvDanNarush.CurrentRow.Cells["type_violation_id"].Value;                

                InsertNarush ins = new InsertNarush(per_num, other_violation_id, perco_sync_id, last_name, first_name, middle_name, violation_log_id, cause_arrest, types_exact_id, unit, kolvo, summ, type_violation_id);
                ins.ShowDialog();
            }
        }

        private void dgvGurnal_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGurnal.CurrentRow != null)
            {
                violation_log.Fill(string.Format("where {0} = '{1}'", VIOLATION_LOG_seq.ColumnsName.VIOLATION_LOG_ID, dgvGurnal.CurrentRow.Cells["violation_log_id"].Value.ToString()));

                if (((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_VIOLATION_ID == 1 || 
                    ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_VIOLATION_ID == 3)
                {
                    dgvDanNarush.Columns["TYPE_STOLEN_TMC"].Visible = false;
                    dgvDanNarush.Columns["UNIT"].Visible = false;
                    dgvDanNarush.Columns["QUANTITY_GOODS"].Visible = false;
                    dgvDanNarush.Columns["TOTAL_STOLEN"].Visible = false;
                }
                if (((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_VIOLATION_ID == 2)
                {
                    dgvDanNarush.Columns["TYPE_STOLEN_TMC"].Visible = true;
                    dgvDanNarush.Columns["UNIT"].Visible = true;
                    dgvDanNarush.Columns["QUANTITY_GOODS"].Visible = true;
                    dgvDanNarush.Columns["TOTAL_STOLEN"].Visible = true;
                }
                per_num = dgvGurnal.CurrentRow.Cells["per_num"].Value.ToString();
            }
        }

        /// <summary>
        /// Событие выполняемые при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZurnNaruRe_Load(object sender, EventArgs e)
        {
            dgvDanNarush.Columns["TYPE_EXACT_ID"].Visible = false;
            dgvDanNarush.Columns["TYPE_VIOLATION_ID"].Visible = false;
            //btKolNar.Enabled = false;
        }

        /// <summary>
        /// Событие нажатия кнопки удаления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btDelete_Click(object sender, EventArgs e)
        {
            if (dgvGurnal.RowCount != 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить нарушителя", "АСУ \"Кадры\"", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //string vlid = ((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current).VIOLATION_LOG_ID.ToString();
                    list_violator.Clear();
                    string list_violation_id = dgvGurnal.CurrentRow.Cells["LIST_VIOLATOR_ID"].Value.ToString();
                    list_violator.Fill("where list_violator_id = " + list_violation_id);
                    dgvGurnal.Rows.Remove(dgvGurnal.CurrentRow);
                    list_violator.Remove((LIST_VIOLATOR_obj)((CurrencyManager)BindingContext[list_violator]).Current);                                        
                    list_violator.Save();
                    Connect.Commit();
                }
            }
        }

        /// <summary>
        /// Событие нажатия кнопки просмотра всех нарушений по человеку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btNarushChel_Click(object sender, EventArgs e)
        {
            if (dgvGurnal.CurrentRow != null)
            {
                string per_num = dgvGurnal.CurrentRow.Cells["per_num"].Value.ToString();
                string other_violation_id = dgvGurnal.CurrentRow.Cells["OTHER_VIOLATOR_ID"].Value.ToString();
                string perco_sync_id = dgvGurnal.CurrentRow.Cells["perco_sync_id"].Value.ToString();
                decimal violation_log_id = (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).VIOLATION_LOG_ID;//dgvDanNarush.CurrentRow.Cells["VIOLATION_LOG_ID"].Value.ToString();
                string laste_name = dgvGurnal.CurrentRow.Cells["LAST_NAME"].Value.ToString();
                string first_name = dgvGurnal.CurrentRow.Cells["FIRST_NAME"].Value.ToString();
                string middle_name = dgvGurnal.CurrentRow.Cells["MIDDLE_NAME"].Value.ToString();
                string cause_arrest = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).CAUSE_ARREST;
                decimal types_exact_id = (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_EXACT_ID;
                string unit = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).UNIT;
                decimal kolvo = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).QUANTITY_GOODS != null ?
                (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).QUANTITY_GOODS : 0;
                decimal summ = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TOTAL_STOLEN != null ?
                (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TOTAL_STOLEN : 0;

                OracleDataTable dtEmp = new OracleDataTable("", Connect.CurConnect); ;

                if (per_num != "")
                {
                    emp = new EMP_seq(Connect.CurConnect);
                    textBlock = string.Format(Queries.GetQuery("SelectKolNarush.sql"), 
                        Connect.Schema, per_num);
                    dtEmp.SelectCommand.CommandText = textBlock;
                    dtEmp.Fill();
                }
                if (other_violation_id != "")
                {
                    emp = new EMP_seq(Connect.CurConnect);
                    textBlock = string.Format(Queries.GetQuery("SelectKolNarush2.sql"), other_violation_id);
                    dtEmp.SelectCommand.CommandText = textBlock;
                    dtEmp.Fill();
                }
                if (perco_sync_id != "")
                {
                    emp = new EMP_seq(Connect.CurConnect);
                    textBlock = string.Format(Queries.GetQuery("SelectKolNarush3.sql"), perco_sync_id);
                    dtEmp.SelectCommand.CommandText = textBlock;
                    dtEmp.Fill();
                }

                KolvoNarush kl = new KolvoNarush(dtEmp, per_num, other_violation_id, perco_sync_id, violation_log_id, cause_arrest, types_exact_id, laste_name, first_name, middle_name, unit, kolvo, summ);
                kl.ShowDialog();
            }
        }

        /// <summary>
        /// Метод обновляет грид нарушителей
        /// </summary>
        public void RefreshGridNarush()
        {
            dgvGurnal.DataSource = bsEmp;
            dgvGurnal.Columns["SUBDIV_NAME"].HeaderText = "Подр.";
            dgvGurnal.Columns["SUBDIV_NAME"].Width = 80;            
            dgvGurnal.Columns["SUBDIV_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvGurnal.Columns["SUBDIV_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGurnal.Columns["LAST_NAME"].HeaderText = "Фамилия";
            dgvGurnal.Columns["LAST_NAME"].Width = 180;
            dgvGurnal.Columns["LAST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvGurnal.Columns["LAST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGurnal.Columns["FIRST_NAME"].HeaderText = "Имя";
            dgvGurnal.Columns["FIRST_NAME"].Width = 140;
            dgvGurnal.Columns["FIRST_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvGurnal.Columns["FIRST_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGurnal.Columns["MIDDLE_NAME"].HeaderText = "Отчество";
            dgvGurnal.Columns["MIDDLE_NAME"].Width = 170;
            dgvGurnal.Columns["MIDDLE_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvGurnal.Columns["MIDDLE_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGurnal.Columns["POS_NAME"].HeaderText = "Должность";
            dgvGurnal.Columns["POS_NAME"].Width = 300;
            dgvGurnal.Columns["POS_NAME"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvGurnal.Columns["POS_NAME"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGurnal.Columns["PER_NUM"].HeaderText = "Табельный номер";
            dgvGurnal.Columns["PER_NUM"].Width = 90;
            dgvGurnal.Columns["PER_NUM"].DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvGurnal.Columns["PER_NUM"].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvGurnal.Columns["VIOLATION_LOG_ID"].Visible = false;
            dgvGurnal.Columns["LIST_VIOLATOR_ID"].Visible = false;
            dgvGurnal.Columns["PERCO_SYNC_ID"].Visible = false;
            dgvGurnal.Columns["OTHER_VIOLATOR_ID"].Visible = false;
            dgvGurnal.Columns["TRANSFER_ID"].Visible = false;            
            ColumnWidthSaver.FillWidthOfColumn(dgvGurnal);
        }

        private void btKolNar_Click(object sender, EventArgs e)
        {
            PrintNarush prn = new PrintNarush();
            prn.ShowDialog();
        }

        int pos = 0;
        /// <summary>
        /// Метод обновляет данные переводов сотрудника при изменении позиции в списке сотрудников
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void PositionChange(object sender, EventArgs e)
        {
            /// Если данные есть, то продолжаем работу. 
            /// Если нет, то скрываем отображение таблиц.
            if (bsEmp.Count != 0)
            {
                /// Формируем строку запроса для переводов из файла
                string str = string.Format(Queries.GetQuery("SelectTransferByPerNum.sql"),
                    Staff.DataSourceScheme.SchemeName, per_num, 0);               
            }
        }

        private void dgvGurnal_DoubleClick(object sender, EventArgs e)
        {
            if (dgvGurnal.CurrentRow != null)
            {
                string per_num = dgvGurnal.CurrentRow.Cells["per_num"].Value.ToString();
                string other_violation_id = dgvGurnal.CurrentRow.Cells["OTHER_VIOLATOR_ID"].Value.ToString();
                string perco_sync_id = dgvGurnal.CurrentRow.Cells["perco_sync_id"].Value.ToString();
                decimal violation_log_id = (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).VIOLATION_LOG_ID;//dgvDanNarush.CurrentRow.Cells["VIOLATION_LOG_ID"].Value.ToString();
                string laste_name = dgvGurnal.CurrentRow.Cells["LAST_NAME"].Value.ToString();
                string first_name = dgvGurnal.CurrentRow.Cells["FIRST_NAME"].Value.ToString();
                string middle_name = dgvGurnal.CurrentRow.Cells["MIDDLE_NAME"].Value.ToString();
                string cause_arrest = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).CAUSE_ARREST;
                decimal types_exact_id = (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TYPE_EXACT_ID;
                string unit = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).UNIT;
                decimal kolvo = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).QUANTITY_GOODS != null ?
                (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).QUANTITY_GOODS : 0;
                decimal summ = ((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TOTAL_STOLEN != null ?
                (decimal)((VIOLATION_LOG_obj)((CurrencyManager)BindingContext[violation_log]).Current).TOTAL_STOLEN : 0;

                OracleDataTable dtEmp = new OracleDataTable("", Connect.CurConnect); ;

                if (per_num != "")
                {
                    emp = new EMP_seq(Connect.CurConnect);
                    textBlock = string.Format(Queries.GetQuery("SelectKolNarush.sql"), per_num);
                    dtEmp.SelectCommand.CommandText = textBlock;
                    dtEmp.Fill();
                }
                if (other_violation_id != "")
                {
                    emp = new EMP_seq(Connect.CurConnect);
                    textBlock = string.Format(Queries.GetQuery("SelectKolNarush2.sql"), other_violation_id);
                    dtEmp.SelectCommand.CommandText = textBlock;
                    dtEmp.Fill();
                }
                if (perco_sync_id != "")
                {
                    emp = new EMP_seq(Connect.CurConnect);
                    textBlock = string.Format(Queries.GetQuery("SelectKolNarush3.sql"), perco_sync_id);
                    dtEmp.SelectCommand.CommandText = textBlock;
                    dtEmp.Fill();
                }

                KolvoNarush kol = new KolvoNarush(dtEmp, per_num, other_violation_id, perco_sync_id, violation_log_id, cause_arrest, types_exact_id, laste_name, first_name, middle_name, unit, kolvo, summ);
                kol.ShowDialog();
            }
        }

        private void ZurnNaruRe_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

   }
}
