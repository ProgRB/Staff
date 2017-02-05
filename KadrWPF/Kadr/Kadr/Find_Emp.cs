using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

using Staff;
using System.Reflection;
using LibraryKadr;

namespace Kadr
{
    public partial class Find_Emp : Form
    {
        public StringBuilder str_find = new StringBuilder();
        public StringBuilder sort = new StringBuilder();
        //SUBDIV_seq subdiv;
        TYPE_PER_DOC_seq type_per_doc;
        string textFilter;
        /// <summary>
        /// Конструктор формы поиска
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public Find_Emp(string _textFilter)
        {
            InitializeComponent();
            textFilter = _textFilter;
            //subdiv = new SUBDIV_seq(connection);
            //subdiv.Fill(string.Format("where {0} = 1 order by {1}", SUBDIV_seq.ColumnsName.SUB_ACTUAL_SIGN.ToString(),
            //    SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));
            type_per_doc = new TYPE_PER_DOC_seq(Connect.CurConnect);
            type_per_doc.Fill(string.Format("order by {0}", TYPE_PER_DOC_seq.ColumnsName.NAME_DOC.ToString()));

            if (!FormMain.flagArchive)
            {
                cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), 
                    new LinkArgument(FormMain.subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                cbSubdiv_Name.SelectedItem = null;
            }
            else
            {
                cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                    new LinkArgument(FormMain.allSubdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                cbSubdiv_Name.SelectedItem = null;
            }
            cbType_Per_Doc_ID.AddBindingSource(TYPE_PER_DOC_seq.ColumnsName.TYPE_PER_DOC_ID.ToString(), 
                new LinkArgument(type_per_doc, TYPE_PER_DOC_seq.ColumnsName.NAME_DOC));
            cbType_Per_Doc_ID.SelectedItem = null;

            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbType_Per_Doc_ID.SelectedIndexChanged += new EventHandler(cbType_Per_Doc_ID_SelectedIndexChanged);

            tbPer_Num.Validating += new CancelEventHandler(Library.ValidatingInt);
        }

        OracleDataTable oracleTable ;
        /// <summary>
        /// Свойство возвращает таблицу, заполненную искомыми данными
        /// </summary>
        public OracleDataTable OracleDataTable
        {
            get
            {
                return oracleTable;
            }
        }

        /// <summary>
        /// Событие нажатия кнопки поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFind_Click(object sender, EventArgs e)
        {
            if (tbPer_Num.Text.Trim() != "")
            {
                str_find.Append(string.Format("CUR_EMP.per_num = '{0}'", tbPer_Num.Text.Trim().PadLeft(5, '0')));
            }
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper(CUR_EMP.{1}) like upper('{2}%')", str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.EMP_LAST_NAME.ToString(), tbEmp_Last_Name.Text.Trim()));
            }
            if (tbEmp_First_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper(CUR_EMP.{1}) like upper('{2}%')", str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.EMP_FIRST_NAME.ToString(), tbEmp_First_Name.Text.Trim()));
            }
            if (tbEmp_Middle_Name.Text.Trim() != "")
            {
                str_find.Append(string.Format("{0} upper(CUR_EMP.{1}) like upper('{2}%')", str_find.Length != 0 ? " and" : "",
                    EMP_seq.ColumnsName.EMP_MIDDLE_NAME.ToString(), tbEmp_Middle_Name.Text.Trim()));
            }
            //FieldFilter[] fieldFilter = new FieldFilter[] {
            //new FieldFilter("transfer", TRANSFER_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.Name),
            //new FieldFilter("per_data", PER_DATA_seq.ColumnsName.INN.ToString(), mbInn.Name),
            //new FieldFilter("per_data", PER_DATA_seq.ColumnsName.INSURANCE_NUM.ToString(), mbInsurance_Num.Name),
            //new FieldFilter("per_data", PER_DATA_seq.ColumnsName.SER_MED_POLUS.ToString(), tbSer_Med_Polus.Name),
            //new FieldFilter("per_data", PER_DATA_seq.ColumnsName.NUM_MED_POLUS.ToString(), tbNum_Med_Polus.Name),
            //new FieldFilter("passport", PASSPORT_seq.ColumnsName.TYPE_PER_DOC_ID.ToString(), cbType_Per_Doc_ID.Name),
            //new FieldFilter("passport", PASSPORT_seq.ColumnsName.SERIA_PASSPORT.ToString(), mbSeria_Passport.Name),
            //new FieldFilter("passport", PASSPORT_seq.ColumnsName.NUM_PASSPORT.ToString(), mbNum_Passport.Name)
            //};
            //foreach (Control control in this.gbFind.Controls)
            //{
            //    if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault() != null)
            //    {
            //        if (((control is TextBox) || (control is MaskedTextBox)) && (control.Text.Trim() != ""))
            //        {
            //            if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName == "transfer")
            //            {
            //                str_find.Append(string.Format("{4} exists " +
            //                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
            //                    "{0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = '{3}')",
            //                    Staff.DataSourceScheme.SchemeName, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
            //                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
            //                    control.Text.Trim(), str_find.Length != 0 ? " and" : ""));                            
            //            }
            //            else
            //            {
            //                str_find.Append(string.Format("{4} exists " +
            //                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = '{3}')",
            //                    Staff.DataSourceScheme.SchemeName, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
            //                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
            //                    control.Text.Trim(), str_find.Length != 0 ? " and" : ""));
            //            }
            //        }
            //        if ((control is ComboBox) && (((ComboBox)control).SelectedValue != null))
            //        {
            //            if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName == "transfer")
            //            {
            //                str_find.Append(string.Format("{4} exists " +
            //                    "(select null from {0}.{1} where {0}.{1}.type_transfer_id != 3 and {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
            //                    "{0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = '{3}')",
            //                    Staff.DataSourceScheme.SchemeName, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
            //                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
            //                    ((ComboBox)control).SelectedValue, str_find.Length != 0 ? " and" : ""));
            //            }
            //            else
            //            {
            //                str_find.Append(string.Format("{4} exists " +
            //                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
            //                    Staff.DataSourceScheme.SchemeName, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
            //                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
            //                    ((ComboBox)control).SelectedValue, str_find.Length != 0 ? " and" : ""));
            //            }
            //        }                    
            //    }
            //}
            FieldFilter[] fieldFilter = new FieldFilter[] {
            new FieldFilter("per_data", PER_DATA_seq.ColumnsName.INN.ToString(), mbInn.Name),
            new FieldFilter("per_data", PER_DATA_seq.ColumnsName.INSURANCE_NUM.ToString(), mbInsurance_Num.Name),
            new FieldFilter("per_data", PER_DATA_seq.ColumnsName.SER_MED_POLUS.ToString(), tbSer_Med_Polus.Name),
            new FieldFilter("per_data", PER_DATA_seq.ColumnsName.NUM_MED_POLUS.ToString(), tbNum_Med_Polus.Name),
            new FieldFilter("passport", PASSPORT_seq.ColumnsName.TYPE_PER_DOC_ID.ToString(), cbType_Per_Doc_ID.Name),
            new FieldFilter("passport", PASSPORT_seq.ColumnsName.SERIA_PASSPORT.ToString(), mbSeria_Passport.Name),
            new FieldFilter("passport", PASSPORT_seq.ColumnsName.NUM_PASSPORT.ToString(), mbNum_Passport.Name)
            };
            foreach (Control control in this.gbFind.Controls)
            {
                if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault() != null)
                {
                    if (((control is TextBox) || (control is MaskedTextBox)) && (control.Text.Trim() != ""))
                    {
                        str_find.Append(string.Format("{4} exists " +
                            "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = '{3}')",
                            Staff.DataSourceScheme.SchemeName, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                            fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                            control.Text.Trim(), str_find.Length != 0 ? " and" : ""));
                    }
                    if ((control is ComboBox) && (((ComboBox)control).SelectedValue != null))
                    {
                        str_find.Append(string.Format("{4} exists " +
                            "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
                            Staff.DataSourceScheme.SchemeName, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                            fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                            ((ComboBox)control).SelectedValue, str_find.Length != 0 ? " and" : ""));
                    }
                }
            }

            if (cbSubdiv_Name.SelectedValue != null)
            {
                str_find.Append(string.Format("{3} exists " +
                    "(select null from {0}.TRANSFER where {0}.TRANSFER.TRANSFER_ID = CUR_EMP.TRANSFER_ID and {0}.TRANSFER.{1} = '{2}')",
                    Staff.DataSourceScheme.SchemeName, TRANSFER_seq.ColumnsName.SUBDIV_ID, cbSubdiv_Name.SelectedValue, 
                    str_find.Length != 0 ? " and" : ""));
            }

            if (str_find.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для поиска!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                //str_find.Insert(0, " where ");
                sort = new StringBuilder();
                if (tbCode_Subdiv.Text.Trim() != "")
                {
                    sort.Append("code_subdiv");
                    if (tbPer_Num.Text.Trim() == "")
                    {
                        sort.Append(",per_num");
                    }
                }
                if (tbPer_Num.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",per_num");
                    }
                    else
                    {
                        sort.Append("per_num");
                    }
                }
                if (tbEmp_Last_Name.Text.Trim() != "")
                {
                    if (sort.Length != 0)
                    {
                        sort.Append(",emp_last_name,emp_first_name,emp_middle_name");
                    }
                    else
                    {
                        sort.Append("emp_last_name,emp_first_name,emp_middle_name");
                    }
                }
                if (sort.Length != 0)
                {
                    sort.Insert(0, " order by ");
                }
                else
                {
                    sort.Append("order by code_subdiv, per_num");
                }
                //string sql = string.Format(Queries.GetQuery("SelectFind.sql"), 
                //    Staff.DataSourceScheme.SchemeName, " where " + str_find.ToString() + sort.ToString());
                string sql = "";
                switch (((FormMain)this.Tag).ActiveMdiChild.Name.ToUpper())
                {
                    case "LISTEMPARCH":
                        sql = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"),
                            Connect.Schema, " where " + str_find.ToString() + sort.ToString());
                        break;
                    case "LISTEMP":
                        sql = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                            Connect.Schema, " where " + str_find.ToString() + sort.ToString());
                        break;                        
                    case "LISTEMP_TERM":
                        sql = string.Format(Queries.GetQuery("SelectListEmp_Term.sql"),
                            Connect.Schema, " where " + str_find.ToString() + " ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM");
                        break;
                    default: break;
                }  
                oracleTable = new OracleDataTable(sql, Connect.CurConnect);
                oracleTable.Fill();
                if (oracleTable.Rows.Count == 0)
                {
                    MessageBox.Show("В базе данных не найдена введенная информация!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    str_find.Remove(0, str_find.Length);
                    return;
                }
                int pos = textFilter.ToUpper().IndexOf(" ORDER BY ");
                int pos2 = textFilter.IndexOf("CUR_EMP where");
                string str;
                if (pos2 != -1)
                    str = textFilter.Substring(0, pos) + " and " + str_find + sort;
                else
                    str = textFilter.Substring(0, pos) + " where " + str_find + sort;
                oracleTable.Clear();
                oracleTable.SelectCommand.CommandText = str;
                oracleTable.Fill();
                this.DialogResult = DialogResult.OK;
                Close();
            }  
        }

        /// <summary>
        /// Событие изменения индекса подразделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbSubdiv_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdiv_Name.SelectedValue != null)
            {
                tbCode_Subdiv.Text = Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Subdiv_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Subdiv, cbSubdiv_Name, 3, Connect.CurConnect, e, SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                Staff.DataSourceScheme.SchemeName, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Событие изменения индеска типа документа личности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbType_Per_Doc_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType_Per_Doc_ID.SelectedValue != null)
            {
                TYPE_PER_DOC_obj r_type_per_doc = type_per_doc.Where(s => s.TYPE_PER_DOC_ID == (decimal)cbType_Per_Doc_ID.SelectedValue).FirstOrDefault();
                mbSeria_Passport.Mask = r_type_per_doc.TEMPL_SER;
                mbNum_Passport.Mask = r_type_per_doc.TEMPL_NUM;                
            }
        }
    }
}
