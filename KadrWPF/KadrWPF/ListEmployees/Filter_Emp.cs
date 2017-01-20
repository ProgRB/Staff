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
using WpfControlLibrary;

namespace Kadr
{
    public partial class Filter_Emp : System.Windows.Forms.UserControl
    {
        SUBDIV_seq subdivSource;
        //POSITION_seq position;
        INSTIT_seq instit;
        DEGREE_seq degree;
        SPECIALITY_seq speciality;
        QUAL_seq qual;
        TYPE_EDU_seq type_edu;
        TYPE_STUDY_seq type_study;
        WORK_TYPE_seq work_type;
        REWARD_NAME_seq reward_name;
        TYPE_REWARD_seq type_reward;
        REASON_DISMISS_seq reason_dismiss;
        FORM_OPERATION_seq form_operation;
        SOURCE_EMPLOYABILITY_seq source_employability;
        /// Строка фильтра
        public StringBuilder str_filter = new StringBuilder();
        System.Windows.Forms.Control _listemp;
        /// <summary>
        /// Конструктор формы фильтра
        /// </summary>
        /// <param name="_connection">Строка подключения</param>
        public Filter_Emp(System.Windows.Forms.Control listemp)
        {
            InitializeComponent();
            _listemp = listemp;
            instit = new INSTIT_seq(Connect.CurConnect);
            instit.Fill(string.Format("order by {0}", INSTIT_seq.ColumnsName.INSTIT_NAME));
            degree = new DEGREE_seq(Connect.CurConnect);
            degree.Fill(string.Format("order by {0}", DEGREE_seq.ColumnsName.DEGREE_NAME));
            speciality = new SPECIALITY_seq(Connect.CurConnect);
            speciality.Fill(string.Format("order by {0}", SPECIALITY_seq.ColumnsName.NAME_SPEC));
            qual = new QUAL_seq(Connect.CurConnect);
            qual.Fill(string.Format("order by {0}", QUAL_seq.ColumnsName.QUAL_NAME));
            type_edu = new TYPE_EDU_seq(Connect.CurConnect);
            type_edu.Fill(string.Format("order by {0}", TYPE_EDU_seq.ColumnsName.TE_NAME));
            type_study = new TYPE_STUDY_seq(Connect.CurConnect);
            type_study.Fill(string.Format("order by {0}", TYPE_STUDY_seq.ColumnsName.TS_NAME));
            work_type = new WORK_TYPE_seq(Connect.CurConnect);
            work_type.Fill(string.Format("order by {0}", WORK_TYPE_seq.ColumnsName.TYPE_NAME));
            reward_name = new REWARD_NAME_seq(Connect.CurConnect);
            reward_name.Fill(string.Format("order by {0}", REWARD_NAME_seq.ColumnsName.REWARD_NAME));
            type_reward = new TYPE_REWARD_seq(Connect.CurConnect);
            type_reward.Fill(string.Format("order by {0}", TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_NAME));
            form_operation = new FORM_OPERATION_seq(Connect.CurConnect);
            form_operation.Fill("order by NAME_FORM_OPERATION");
            source_employability = new SOURCE_EMPLOYABILITY_seq(Connect.CurConnect);
            source_employability.Fill("ORDER BY source_employability_NAME");
            if (!((ListEmp)_listemp).FlagArchive)
            {
                cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), new LinkArgument(AppDataSet.subdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                cbSubdiv_Name.SelectedItem = null;
                cbPos_Name.AddBindingSource(POSITION_seq.ColumnsName.POS_ID.ToString(), new LinkArgument(AppDataSet.position, POSITION_seq.ColumnsName.POS_NAME));
                cbPos_Name.SelectedItem = null;                
            }
            else
            {
                cbSubdiv_Name.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                    new LinkArgument(AppDataSet.allSubdiv, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
                cbSubdiv_Name.SelectedItem = null;
                cbPos_Name.AddBindingSource(POSITION_seq.ColumnsName.POS_ID.ToString(),
                    new LinkArgument(AppDataSet.allPosition, POSITION_seq.ColumnsName.POS_NAME));
                cbPos_Name.SelectedItem = null;
                reason_dismiss = new REASON_DISMISS_seq(Connect.CurConnect);
                reason_dismiss.Fill("order by REASON_NAME");
                lbReason_dismiss.Visible = true;
                cbReason_dismiss.Visible = true;
                cbReason_dismiss.AddBindingSource(REASON_DISMISS_seq.ColumnsName.REASON_ID.ToString(),
                    new LinkArgument(reason_dismiss, REASON_DISMISS_seq.ColumnsName.REASON_NAME));
                cbReason_dismiss.SelectedItem = null;
            }

            /* Настраиваем работу с подразделением-источником*/
            subdivSource = new SUBDIV_seq(Connect.CurConnect);
            subdivSource.Fill("where WORK_TYPE_ID != 7 order by SUBDIV_NAME");
            cbSubdivSource.AddBindingSource(SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                new LinkArgument(subdivSource, SUBDIV_seq.ColumnsName.SUBDIV_NAME.ToString()));
            cbSubdivSource.SelectedItem = null;

            cbInstit_ID.AddBindingSource(INSTIT_seq.ColumnsName.INSTIT_ID.ToString(), 
                new LinkArgument(instit, INSTIT_seq.ColumnsName.INSTIT_NAME));
            cbInstit_ID.SelectedItem = null;
            cbDegree_Name.AddBindingSource(DEGREE_seq.ColumnsName.DEGREE_ID.ToString(), 
                new LinkArgument(degree, DEGREE_seq.ColumnsName.DEGREE_NAME));
            cbDegree_Name.SelectedItem = null;
            cbSpec_ID.AddBindingSource(SPECIALITY_seq.ColumnsName.SPEC_ID.ToString(), 
                new LinkArgument(speciality, SPECIALITY_seq.ColumnsName.NAME_SPEC));
            cbSpec_ID.SelectedItem = null;
            cbQual_ID.AddBindingSource(QUAL_seq.ColumnsName.QUAL_ID.ToString(), 
                new LinkArgument(qual, QUAL_seq.ColumnsName.QUAL_NAME));
            cbQual_ID.SelectedItem = null;
            cbType_Edu_ID.AddBindingSource(TYPE_EDU_seq.ColumnsName.TYPE_EDU_ID.ToString(), 
                new LinkArgument(type_edu, TYPE_EDU_seq.ColumnsName.TE_NAME));
            cbType_Edu_ID.SelectedItem = null;
            cbType_Study_ID.AddBindingSource(TYPE_STUDY_seq.ColumnsName.TYPE_STUDY_ID.ToString(),
                new LinkArgument(type_study, TYPE_STUDY_seq.ColumnsName.TS_NAME));
            cbType_Study_ID.SelectedItem = null;
            cbWork_Type_Name.AddBindingSource(WORK_TYPE_seq.ColumnsName.WORK_TYPE_ID.ToString(), 
                new LinkArgument(work_type, WORK_TYPE_seq.ColumnsName.TYPE_NAME));
            cbWork_Type_Name.SelectedItem = null;
            cbReward_Name.AddBindingSource(REWARD_NAME_seq.ColumnsName.REWARD_NAME_ID.ToString(), 
                new LinkArgument(reward_name, REWARD_NAME_seq.ColumnsName.REWARD_NAME));
            cbReward_Name.SelectedItem = null;
            cbType_Reward.AddBindingSource(TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_ID.ToString(), 
                new LinkArgument(type_reward, TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_NAME));
            cbType_Reward.SelectedItem = null;
            cbForm_Operation.AddBindingSource(FORM_OPERATION_seq.ColumnsName.FORM_OPERATION_ID.ToString(),
                new LinkArgument(form_operation, FORM_OPERATION_seq.ColumnsName.NAME_FORM_OPERATION));
            cbForm_Operation.SelectedItem = null;
            cbSource_Employability.AddBindingSource(SOURCE_EMPLOYABILITY_seq.ColumnsName.SOURCE_EMPLOYABILITY_ID.ToString(),
                new LinkArgument(source_employability, SOURCE_EMPLOYABILITY_seq.ColumnsName.SOURCE_EMPLOYABILITY_NAME));
            cbSource_Employability.SelectedItem = null;

            cbSubdiv_Name.SelectedIndexChanged += new EventHandler(cbSubdiv_Name_SelectedIndexChanged);
            cbPos_Name.SelectedIndexChanged += new EventHandler(cbPos_Name_SelectedIndexChanged);
            cbDegree_Name.SelectedIndexChanged += new EventHandler(cbDegree_Name_SelectedIndexChanged);
            cbSubdivSource.SelectedIndexChanged += new EventHandler(cbSubdivSource_SelectedIndexChanged);

            /// Привязываем событие проверки ввода числа
            tbAgeFrom.Validating += new CancelEventHandler(Library.ValidatingInt);
            tbAgeOn.Validating += new CancelEventHandler(Library.ValidatingInt);
            tbClassific.Validating += new CancelEventHandler(Library.ValidatingInt);
            tbPer_Num.Validating += new CancelEventHandler(Library.ValidatingInt);
            /// Привязываем событие проверки ввода даты для всех MaskedTextBox
            foreach (Control control in this.gbFilter.Controls)
            {
                if (control is MaskedTextBox)
                {
                    ((MaskedTextBox)control).Validating += new CancelEventHandler(Library.ValidatingDate);
                }
            }

            foreach (Control control in gbFilter.Controls)
            {
                if (control.GetType() == typeof(TextBox) || control.GetType() == typeof(ComboBox) || control.GetType() == typeof(MaskedTextBox)
                    || control.GetType() == typeof(DateEditor))
                    control.KeyDown += Control_KeyDown;
                else
                    if (control.GetType() == typeof(TabControl))
                    foreach (TabPage page in ((TabControl)control).TabPages)
                    {
                        foreach (Control c1 in page.Controls)
                        {
                            if (c1.GetType() == typeof(TextBox) || c1.GetType() == typeof(ComboBox) || c1.GetType() == typeof(MaskedTextBox)
                                || c1.GetType() == typeof(DateEditor))
                                c1.KeyDown += Control_KeyDown;
                        }
                    }
            }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                btFilter_Click(null, null);
        }

        /// <summary>
        /// Событие нажатия кнопки формирования строки фильтра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFilter_Click(object sender, EventArgs e)
        {
            str_filter.Clear();
            FieldFilter[] fieldFilter = new FieldFilter[] { 
            new FieldFilter("transfer", TRANSFER_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.Name),
            new FieldFilter("transfer", TRANSFER_seq.ColumnsName.POS_ID.ToString(), cbPos_Name.Name),
            /*new FieldFilter("per_data", PER_DATA_seq.ColumnsName.SIGN_PROFUNION.ToString(), chSign_ProfUnion.Name),*/
            new FieldFilter("per_data", PER_DATA_seq.ColumnsName.RETIRER_SIGN.ToString(), chRetirer_Sign.Name),
            new FieldFilter("emp", EMP_seq.ColumnsName.EMP_BIRTH_DATE.ToString(), mbBirth_DateFrom.Name),
            new FieldFilter("transfer", TRANSFER_seq.ColumnsName.DATE_HIRE.ToString(), mbDate_HireFrom.Name)
            };
            /// Фильтрация по полям ввода из списка полей ввода fieldFilter
            foreach (Control control in this.gbFilter.Controls)
            {
                if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault() != null)
                {
                    if ((control is TextBox) && (control.Text.Trim() != ""))
                    {
                        if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName == "transfer")
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                                "{0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = '{3}')",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                control.Text.Trim(), str_filter.Length != 0 ? " and" : ""));
                        }
                        else
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = '{3}')",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                control.Text.Trim(), str_filter.Length != 0 ? " and" : ""));
                        }
                    }
                    if ((control is ComboBox) && (((ComboBox)control).SelectedValue != null))
                    {
                        if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName == "transfer")
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                                "{0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                ((ComboBox)control).SelectedValue, str_filter.Length != 0 ? " and" : ""));
                        }
                        else
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                ((ComboBox)control).SelectedValue, str_filter.Length != 0 ? " and" : ""));
                        }
                    }
                    if ((control is CheckBox) && ((CheckBox)control).Checked)
                    {
                        if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName == "transfer")
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                                "{0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = '{3}')",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName, 1,
                                str_filter.Length != 0 ? " and" : ""));
                        }
                        else
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName, 1,
                                str_filter.Length != 0 ? " and" : ""));
                        }
                    }
                    if (control is MaskedTextBox)
                    {
                        string nameControl = control.Name.Substring(0, control.Name.Length - 4) + "On";
                        if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName == "transfer")
                        {
                            if (control.Text.Replace(".", "").Trim() != "" && this.gbFilter.Controls[nameControl].Text.Replace(".", "").Trim() != "")
                            {
                                str_filter.Append(string.Format("{5} exists " +
                                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                                    " and {0}.{1}.{2} between to_date('{3}','dd.MM.yyyy') and to_date('{4}','dd.MM.yyyy'))",
                                    Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                    control.Text, this.gbFilter.Controls[nameControl].Text,
                                    str_filter.Length != 0 ? " and" : ""));
                            }
                            else if (control.Text.Replace(".", "").Trim() != "")
                            {
                                str_filter.Append(string.Format("{4} exists " +
                                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                                    " and {0}.{1}.{2} >= to_date('{3}','dd.MM.yyyy') )",
                                    Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                    control.Text,
                                    str_filter.Length != 0 ? " and" : ""));
                            }
                            else if (this.gbFilter.Controls[nameControl].Text.Replace(".", "").Trim() != "")
                            {
                                str_filter.Append(string.Format("{4} exists " +
                                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                                    " and {0}.{1}.{2} <= to_date('{3}','dd.MM.yyyy') )",
                                    Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                    this.gbFilter.Controls[nameControl].Text,
                                    str_filter.Length != 0 ? " and" : ""));
                            }
                        }
                        else
                        {
                            if (control.Text.Replace(".", "").Trim() != "" && this.gbFilter.Controls[nameControl].Text.Replace(".", "").Trim() != "")
                            {
                                str_filter.Append(string.Format("{5} exists " +
                                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                                    " and {0}.{1}.{2} between to_date('{3}','dd.MM.yyyy') and to_date('{4}','dd.MM.yyyy'))",
                                    Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                    control.Text, this.gbFilter.Controls[nameControl].Text,
                                    str_filter.Length != 0 ? " and" : ""));
                            }
                            else if (control.Text.Replace(".", "").Trim() != "")
                            {
                                str_filter.Append(string.Format("{4} exists " +
                                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                                    " and {0}.{1}.{2} >= to_date('{3}','dd.MM.yyyy') )",
                                    Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                    control.Text,
                                    str_filter.Length != 0 ? " and" : ""));
                            }
                            else if (this.gbFilter.Controls[nameControl].Text.Replace(".", "").Trim() != "")
                            {
                                str_filter.Append(string.Format("{4} exists " +
                                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                                    " and {0}.{1}.{2} <= to_date('{3}','dd.MM.yyyy') )",
                                    Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                    fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                    this.gbFilter.Controls[nameControl].Text,
                                    str_filter.Length != 0 ? " and" : ""));
                            }
                        }
                    }
                }
            }
            // Фильтрация по образованию
            // Старый вариант
            /*foreach (Control control in tpEdu.Controls)
            {
                if (fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault() != null)
                {
                    if ((control is ComboBox) && (((ComboBox)control).SelectedValue != null))
                    {
                        str_filter.Append(string.Format("{4} exists " +
                            "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
                            Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                            fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                            ((ComboBox)control).SelectedValue, str_filter.Length != 0 ? " and" : ""));
                    }
                    if (control is MaskedTextBox)
                    {
                        string nameControl = control.Name.Substring(0, control.Name.Length - 4) + "On";
                        if (control.Text.Replace(".", "").Trim() != "" &&
                            tpEdu.Controls[nameControl].Text.Replace(".", "").Trim() != "")
                        {
                            str_filter.Append(string.Format("{5} exists " +
                                "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                                " and {0}.{1}.{2} between to_date('{3}','dd.MM.yyyy') and to_date('{4}','dd.MM.yyyy'))",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                control.Text, tpEdu.Controls[nameControl].Text,
                                str_filter.Length != 0 ? " and" : ""));
                        }
                        else if (control.Text.Replace(".", "").Trim() != "")
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                                " and {0}.{1}.{2} >= to_date('{3}','dd.MM.yyyy') )",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                control.Text,
                                str_filter.Length != 0 ? " and" : ""));
                        }
                        else if (tpEdu.Controls[nameControl].Text.Replace(".", "").Trim() != "")
                        {
                            str_filter.Append(string.Format("{4} exists " +
                                "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                                " and {0}.{1}.{2} <= to_date('{3}','dd.MM.yyyy') )",
                                Connect.Schema, fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().TableName,
                                fieldFilter.Where(i => i.ControlValue == control.Name).FirstOrDefault().FieldName,
                                tpEdu.Controls[nameControl].Text,
                                str_filter.Length != 0 ? " and" : ""));
                        }
                    }
                }
            }*/
            // Новый вариант
            string _filter_by_edu = "";
            if (cbInstit_ID.SelectedValue != null)
            {
                _filter_by_edu += string.Format("{1} INSTIT_ID = {0}",
                    cbInstit_ID.SelectedValue, _filter_by_edu.Length != 0 ? " and" : "");
            }
            if (cbSpec_ID.SelectedValue != null)
            {
                _filter_by_edu += string.Format("{1} SPEC_ID = {0}",
                    cbSpec_ID.SelectedValue, _filter_by_edu.Length != 0 ? " and" : "");
            }
            if (cbQual_ID.SelectedValue != null)
            {
                _filter_by_edu += string.Format("{1} QUAL_ID = {0}",
                    cbQual_ID.SelectedValue, _filter_by_edu.Length != 0 ? " and" : "");
            }
            if (cbType_Edu_ID.SelectedValue != null)
            {
                _filter_by_edu += string.Format("{1} TYPE_EDU_ID = {0}",
                    cbType_Edu_ID.SelectedValue, _filter_by_edu.Length != 0 ? " and" : "");
            }
            if (cbType_Study_ID.SelectedValue != null)
            {
                _filter_by_edu += string.Format("{1} TYPE_STUDY_ID = {0}",
                    cbType_Study_ID.SelectedValue, _filter_by_edu.Length != 0 ? " and" : "");
            }
            if (deYear_GraduatingFrom.Date != null && deYear_GraduatingOn.Date != null)
            {
                _filter_by_edu += string.Format("{2} YEAR_GRADUATING between to_date('{0}','dd.MM.yyyy') and to_date('{1}','dd.MM.yyyy')",
                    deYear_GraduatingFrom.TextDate, deYear_GraduatingOn.TextDate, _filter_by_edu.Length != 0 ? " and" : "");
            }
            else if (deYear_GraduatingFrom.Date != null)
            {
                _filter_by_edu += string.Format("{1} YEAR_GRADUATING >= to_date('{0}','dd.MM.yyyy')",
                    deYear_GraduatingFrom.TextDate, _filter_by_edu.Length != 0 ? " and" : "");
            }
            else if (deYear_GraduatingOn.Date != null)
            {
                _filter_by_edu += string.Format("{1} YEAR_GRADUATING <= to_date('{0}','dd.MM.yyyy')",
                    deYear_GraduatingOn.TextDate, _filter_by_edu.Length != 0 ? " and" : "");
            }
            if (_filter_by_edu.Length > 0)
            {
                str_filter.Append(string.Format("{2} exists " +
                    "(select null from {0}.EDU ED where ED.per_num = CUR_EMP.per_num and {1})",
                    Connect.Schema, _filter_by_edu, str_filter.Length != 0 ? " and" : ""));
            }
            if (chOneType_Edu.CheckState != CheckState.Indeterminate)
            {
                str_filter.Append(string.Format(@"{0} 
                    (select count(distinct TYPE_EDU_ID) from {1}.EDU E where PER_NUM = CUR_EMP.per_num) {2} 1",
                    (str_filter.Length != 0 ? " and" : ""), Connect.Schema, (chOneType_Edu.Checked ? "=" : "!=")));
            }
            if (chMostOneType_Edu.CheckState != CheckState.Indeterminate)
            {
                str_filter.Append(string.Format(@"{0} 
                    (select count(distinct TYPE_EDU_ID) from {1}.EDU E where PER_NUM = CUR_EMP.per_num) {2} 2",
                    (str_filter.Length != 0 ? " and" : ""), Connect.Schema, (chMostOneType_Edu.Checked ? ">=" : "<")));
            }
            ///////////////////////// Устанавливаем признак фильтра по подразделению (необходим Каурову Ивану 
            ///////////////////////// для формирования отчета)
            //////////////////////if (tbCode_Subdiv.Text.Trim() != "" && cbSubdiv_Name.SelectedValue != null)
            //////////////////////{
            //////////////////////    FormMain.filterOnSubdiv = cbSubdiv_Name.SelectedValue.ToString();
            //////////////////////}
            //////////////////////else
            //////////////////////{
            //////////////////////    FormMain.filterOnSubdiv = "";
            //////////////////////}

            /// Фильтрация по табельному номеру
            if (tbPer_Num.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.{2} = CUR_EMP.{2} " +
                    " and {0}.{1}.{2} = '{3}')",
                    Connect.Schema, "emp", EMP_seq.ColumnsName.PER_NUM.ToString(),
                    tbPer_Num.Text.Trim().PadLeft(5, '0'),
                    str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по фамилии
            if (tbEmp_Last_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.emp where {0}.emp.per_num = CUR_EMP.PER_NUM " +
                    " and {0}.emp.{1} like upper('{2}%'))",
                    Connect.Schema, EMP_seq.ColumnsName.EMP_LAST_NAME, tbEmp_Last_Name.Text.Trim(),
                    str_filter.Length != 0 ? " and" : ""));
            }
            /// Фильтрация по имени
            if (tbEmp_First_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.emp where {0}.emp.per_num = CUR_EMP.PER_NUM " +
                    " and {0}.emp.{1} like upper('{2}%'))",
                    Connect.Schema, EMP_seq.ColumnsName.EMP_FIRST_NAME, tbEmp_First_Name.Text.Trim(),
                    str_filter.Length != 0 ? " and" : ""));
            }
            /// Фильтрация по отчеству
            if (tbEmp_Middle_Name.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.emp where {0}.emp.per_num = CUR_EMP.PER_NUM " +
                    " and {0}.emp.{1} like upper('{2}%'))",
                    Connect.Schema, EMP_seq.ColumnsName.EMP_MIDDLE_NAME, tbEmp_Middle_Name.Text.Trim(),
                    str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по разряду
            if (tbClassific.Text.Trim() != "")
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.account_data where {0}.account_data.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.account_data.{1} = {2})",
                    Connect.Schema, ACCOUNT_DATA_seq.ColumnsName.CLASSIFIC,
                    tbClassific.Text.Trim(), str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация для даты увольнения
            if (mbDate_DismissFrom.Text.Replace(".", "").Trim() != "" && mbDate_DismissOn.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{5} exists " +
                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.{1}.type_transfer_id = 3 and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                    " and trunc({0}.{1}.{2}) between to_date('{3}','dd.MM.yyyy') and to_date('{4}','dd.MM.yyyy'))",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.DATE_TRANSFER.ToString(),
                    mbDate_DismissFrom.Text, mbDate_DismissOn.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (mbDate_DismissFrom.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.{1}.type_transfer_id = 3 and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                    " and trunc({0}.{1}.{2}) >= to_date('{3}','dd.MM.yyyy') )",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.DATE_TRANSFER.ToString(),
                    mbDate_DismissFrom.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (mbDate_DismissOn.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.{1}.type_transfer_id = 3 and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                    " and trunc({0}.{1}.{2}) <= to_date('{3}','dd.MM.yyyy') )",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.DATE_TRANSFER.ToString(),
                    mbDate_DismissOn.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация для даты перевода
            if (mbDate_TransferFrom.Text.Replace(".", "").Trim() != "" && mbDate_TransferOn.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{5} exists " +
                    "(select null from {0}.{1} where {0}.{1}.type_transfer_id = 2 and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                    " and trunc({0}.{1}.{2}) between to_date('{3}','dd.MM.yyyy') and to_date('{4}','dd.MM.yyyy'))",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.DATE_TRANSFER.ToString(),
                    mbDate_TransferFrom.Text, mbDate_TransferOn.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (mbDate_TransferFrom.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.type_transfer_id = 2 and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                    " and trunc({0}.{1}.{2}) >= to_date('{3}','dd.MM.yyyy') )",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.DATE_TRANSFER.ToString(),
                    mbDate_TransferFrom.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (mbDate_TransferOn.Text.Replace(".", "").Trim() != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.type_transfer_id = 2 and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num " +
                    " and trunc({0}.{1}.{2}) <= to_date('{3}','dd.MM.yyyy') )",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.DATE_TRANSFER.ToString(),
                    mbDate_TransferOn.Text,
                    str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация по возрасту сотрудника
            if (tbAgeFrom.Text != "" && tbAgeOn.Text != "")
            {
                if (Convert.ToInt32(tbAgeFrom.Text) > Convert.ToInt32(tbAgeOn.Text))
                {
                    MessageBox.Show("Неверно задан возраст сотрудников.\nПроверьте правильность введенных данных и попробуйте заново.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbAgeFrom.Focus();
                    return;
                }
                str_filter.Append(string.Format("{5} exists " +
                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                    " and {0}.{1}.{2} between to_date('{3}','dd.MM.yyyy') and to_date('{4}','dd.MM.yyyy'))",
                    Connect.Schema, "emp",
                    EMP_seq.ColumnsName.EMP_BIRTH_DATE.ToString(),
                    DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tbAgeOn.Text)),
                    DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tbAgeFrom.Text)),
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (tbAgeFrom.Text != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                    " and {0}.{1}.{2} <= to_date('{3}','dd.MM.yyyy') )",
                    Connect.Schema, "emp", EMP_seq.ColumnsName.EMP_BIRTH_DATE.ToString(),
                    DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tbAgeFrom.Text)),
                    str_filter.Length != 0 ? " and" : ""));
            }
            else if (tbAgeOn.Text != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                    " and {0}.{1}.{2} >= to_date('{3}','dd.MM.yyyy') )",
                    Connect.Schema, "emp", EMP_seq.ColumnsName.EMP_BIRTH_DATE.ToString(),
                    DateTime.Now.Day + "." + DateTime.Now.Month + "." + (DateTime.Now.Year - Convert.ToInt32(tbAgeOn.Text)),
                    str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация по полу сотрудника
            if (cbEmp_Sex.Text != "")
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                    " and {0}.{1}.{2} = '{3}')",
                    Connect.Schema, "emp", EMP_seq.ColumnsName.EMP_SEX.ToString(), cbEmp_Sex.Text.Substring(0, 1),
                    str_filter.Length != 0 ? " and" : ""));
            }

            // Фильтрация по категории
            if (cbDegree.Text != "")
            {
                str_filter.Append(string.Format("{2} exists " +
                    "(select null from {0}.transfer where {0}.transfer.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.transfer.per_num = CUR_EMP.per_num and (select substr(code_pos, 1, 1) from {0}.position " +
                    "where {0}.transfer.pos_id = {0}.position.pos_id) " +
                    "{1})",
                    Connect.Schema, cbDegree.SelectedIndex <= 2 ? " = " + (cbDegree.SelectedIndex + 1) :
                    "not in ('1', '2', '3')",
                    str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по шифру категории
            if (cbDegree_Name.SelectedValue != null)
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.DEGREE_ID,
                    cbDegree_Name.SelectedValue, str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по типу работы подразделения
            if (cbWork_Type_Name.SelectedValue != null)
            {
                str_filter.Append(string.Format("{5} exists " +
                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num and (select work_type_id from {0}.{2} " +
                    "where {0}.{1}.{3} = {0}.{2}.{3}) = '{4}')",
                    Connect.Schema, "transfer", "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(),
                    cbWork_Type_Name.SelectedValue, str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по видам производства
            if (cbForm_Operation.SelectedValue != null)
            {
                str_filter.Append(string.Format("{2} exists " +
                    "(select null from {0}.transfer where {0}.transfer.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.transfer.per_num = CUR_EMP.per_num and {0}.transfer.FORM_OPERATION_ID = {1})",
                    Connect.Schema, cbForm_Operation.SelectedValue, str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по источнику трудоустройства
            if (cbSource_Employability.SelectedValue != null)
            {
                str_filter.Append(string.Format("{2} exists " +
                    "(select null from {0}.PER_DATA PD where " +
                    "PD.per_num = CUR_EMP.per_num and PD.SOURCE_EMPLOYABILITY_ID = {1})",
                    Connect.Schema, cbSource_Employability.SelectedValue, str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по молодым специалистам
            if (chSign_Young_Spec.CheckState != CheckState.Indeterminate)
            {
                str_filter.Append(string.Format("{1} exists " +
                    "(select null from {0}.YOUNG_SPECIALIST YS where " +
                    "YS.per_num = CUR_EMP.per_num and SYSDATE BETWEEN NVL(YS.DATE_BEGIN_ADD, DATE '1000-01-01') and NVL(YS.DATE_END_ADD, DATE '2000-01-01'))",
                    Connect.Schema, str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по работающим
            if (chNotDismiss.Checked)
            {
                str_filter.Append(string.Format("{3} exists " +
                    "(select null from {0}.{1} where {0}.{1}.transfer_id = CUR_EMP.transfer_id and " +
                    "{0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} != 3)",
                    Connect.Schema, "transfer", TRANSFER_seq.ColumnsName.TYPE_TRANSFER_ID,
                    str_filter.Length != 0 ? " and" : ""));
            }

            /// Фильтрация по совместителям
            if (chSign_Comb.Checked)
            {
                str_filter.Append(string.Format("{1} sign_comb is not null or " +
                    "exists (select null from {0}.transfer " +
                    "       where {0}.transfer.per_num = CUR_EMP.per_num and {0}.transfer.SIGN_COMB = 1 " +
                    "           and {0}.TRANSFER.SIGN_CUR_WORK = 1 /*and {0}.transfer.pos_id = CUR_EMP.pos_id*/)",
                    Connect.Schema, str_filter.Length != 0 ? " and" : ""));
            }

            /*/// Фильтрация по медицинскому полису
            if (tbSer_Med_Polus.Text != "" && tbNum_Med_Polus.Text != "" && tbSer_Med_Polus1.Text != "" &&
                tbNum_Med_Polus1.Text != "")
            {
                str_filter.Append(string.Format("{6} exists " +
                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num " +
                    " and upper({0}.{1}.SER_MED_POLUS) between upper('{2}') and upper('{3}') and " +
                    "upper({0}.{1}.NUM_MED_POLUS) between upper('{4}') and upper('{5}'))",
                    Connect.Schema, "per_data", tbSer_Med_Polus.Text.Trim(), tbSer_Med_Polus1.Text.Trim(),
                    tbNum_Med_Polus.Text.Trim(), tbNum_Med_Polus1.Text.Trim(), str_filter.Length != 0 ? " and" : ""));
            }*/

            /// Фильрация по наименованию награды
            if (cbReward_Name.SelectedValue != null)
            {
                str_filter.Append(string.Format("{4} exists " +
                    "(select null from {0}.{1} where {0}.{1}.per_num = CUR_EMP.per_num and {0}.{1}.{2} = {3})",
                    Connect.Schema, "reward", REWARD_seq.ColumnsName.REWARD_NAME_ID,
                    cbReward_Name.SelectedValue, str_filter.Length != 0 ? " and" : ""));
            }
            /// Фильрация по типу награды
            if (cbType_Reward.SelectedValue != null)
            {
                str_filter.Append(string.Format("{6} exists " +
                    "(select null from {0}.{1} r where r.per_num = CUR_EMP.per_num and " +
                    "exists(select null from {0}.{2} rn where r.{3} = rn.{3} and rn.{4} = {5}))",
                    Connect.Schema, "reward", "reward_name", REWARD_NAME_seq.ColumnsName.REWARD_NAME_ID,
                    TYPE_REWARD_seq.ColumnsName.TYPE_REWARD_ID,
                    cbType_Reward.SelectedValue, str_filter.Length != 0 ? " and" : ""));
            }
            // Фильтрация по периоду рождения
            if (chFilterBirthDate.Checked)
            {
                //str_filter.Append(string.Format(" {4} " +
                //    "extract(day from EMP_BIRTH_DATE) between {0} and {1} and " +
                //    "extract(month from EMP_BIRTH_DATE) between {2} and {3}",
                //    nudDayBegin.Value, nudDayEnd.Value, nudMonthBegin.Value, nudMonthEnd.Value,
                //    str_filter.Length != 0 ? " and" : ""));
                // 29.06.2015 - переделываю условия, т.к. старое работает некорретно если месяца разные
                str_filter.Append(string.Format(" {4} " +
                    "TO_CHAR(EMP_BIRTH_DATE,'MM.DD') BETWEEN '{0}.{1}' and '{2}.{3}'",
                    nudMonthBegin.Value.ToString("00"), nudDayBegin.Value.ToString("00"), nudMonthEnd.Value.ToString("00"), nudDayEnd.Value.ToString("00"), 
                    str_filter.Length != 0 ? " and" : ""));
            }
            // Фильтрация по наименованию профессии
            if (tbPos_name.Text.Trim() != "")
            {
                str_filter.Append(string.Format(" {1} " +
                    "REGEXP_LIKE(UPPER(POS_NAME), '{0}') ",
                    tbPos_name.Text.ToUpper(),
                    str_filter.Length != 0 ? " and" : ""));
            }
            // Фильтрация по старому ФИО
            if (tbOld_last_name.Text != "")
            {
                str_filter.Append(string.Format(" {2} " +
                    "PER_NUM in (select PER_NUM from {0}.EMP_OLD_NAME where upper(OLD_LAST_NAME) = '{1}')",
                    Connect.Schema, tbOld_last_name.Text.ToUpper(),
                    str_filter.Length != 0 ? " and" : ""));
            }
            if (tbOld_first_name.Text != "")
            {
                str_filter.Append(string.Format(" {2} " +
                    "PER_NUM in (select PER_NUM from {0}.EMP_OLD_NAME where upper(OLD_FIRST_NAME) = '{1}')",
                    Connect.Schema, tbOld_first_name.Text.ToUpper(),
                    str_filter.Length != 0 ? " and" : ""));
            }
            if (tbOld_middle_name.Text != "")
            {
                str_filter.Append(string.Format(" {2} " +
                    "PER_NUM in (select PER_NUM from {0}.EMP_OLD_NAME where upper(OLD_MIDDLE_NAME) = '{1}')",
                    Connect.Schema, tbOld_middle_name.Text.ToUpper(),
                    str_filter.Length != 0 ? " and" : ""));
            }
            // Фильтрация по переводам из подразделения - источника
            if (cbSubdivSource.SelectedValue != null)
            {
                str_filter.Append(string.Format(" {4} " +
                    "PER_NUM in (select T.PER_NUM from (select * from {0}.TRANSFER " + 
                    "where DATE_TRANSFER between to_date('{1}','DD.MM.YYYY') and to_date('{2}','DD.MM.YYYY')) T " + 
                    "join {0}.TRANSFER TF on T.FROM_POSITION = TF.TRANSFER_ID " + 
                    "where TF.SUBDIV_ID = {3} AND T.SUBDIV_ID != TF.SUBDIV_ID)",
                    Connect.Schema, deBeginTransferSource.Date != null ? deBeginTransferSource.Date.Value.ToShortDateString() : "01.01.1000",
                    deEndTransferSource.Date != null ? deEndTransferSource.Date.Value.ToShortDateString() : "01.01.3000",
                    cbSubdivSource.SelectedValue.ToString(),
                    str_filter.Length != 0 ? " and" : ""));
            }
            /// Фильтрация по причине увольнения
            if (cbReason_dismiss.SelectedValue != null)
            {
                str_filter.Append(string.Format("{1} REASON_ID = {0} ",
                    cbReason_dismiss.SelectedValue, str_filter.Length != 0 ? " and" : ""));
            }

            // 12.02.2015 Новый вариант фильтра по признаку профсоюза
            // Фильтрация по признаку профсоюза
            if (chSign_ProfUnion.CheckState != CheckState.Indeterminate)
            {
                str_filter.Append(string.Format(@"{0} 
                    {1}.GET_SIGN_PROFUNION(CUR_EMP.WORKER_ID, sysdate) = {2} ",
                    (str_filter.Length != 0 ? " and" : ""), Connect.Schema, (chSign_ProfUnion.Checked ? 1 : 0)));
            }

            if (str_filter.Length == 0)
            {
                MessageBox.Show("Вы не ввели ни одного критерия для фильтра!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                ApplyFilter(DialogResult.OK);
            }
        }

        /// <summary>
        /// Событие нажатия кнопки отмены фильтра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btNoFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter(DialogResult.Abort);
            ClearContentControls.ClearControls(this);
        }

        void ApplyFilter(DialogResult rezFilter)
        {
            string strCount = "", strTemp, textQuery = "";
            if (((ListEmp)_listemp).NameForm == "ListEmpTerm")
                if (rezFilter == DialogResult.OK)
                    strTemp = " where " + str_filter.ToString() + " ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM";
                else
                    strTemp = "ORDER BY DATE_END_CONTR, CODE_SUBDIV, PER_NUM";
            else
                if (rezFilter == DialogResult.OK)
                    strTemp = " where " + str_filter.ToString() + " order by CODE_SUBDIV, PER_NUM";
                else
                    strTemp = " order by CODE_SUBDIV, PER_NUM";
            //strFilterEmp = strTemp;
            switch (((ListEmp)_listemp).NameForm)
            {
                case "ListEmpArchive":
                    textQuery = string.Format(Queries.GetQuery("SelectListEmpArchive.sql"),
                        Connect.Schema, strTemp);
                    strCount = string.Format(Queries.GetQuery("SelectListEmpArchiveCount.sql"),
                        Connect.Schema, rezFilter == DialogResult.OK ? " where " + str_filter.ToString() : "");
                    break;
                case "ListEmpMain":
                    textQuery = string.Format(Queries.GetQuery("SelectListEmp.sql"),
                        Connect.Schema, strTemp);
                    strCount = string.Format(Queries.GetQuery("SelectListEmpCount.sql"),
                        Connect.Schema, rezFilter == DialogResult.OK ? " where " + str_filter.ToString() : "");
                    break;
                case "ListEmpTerm":
                    textQuery = string.Format(Queries.GetQuery("SelectListEmp_Term.sql"),
                        Connect.Schema, strTemp);
                    break;
                default: break;
            }
            OracleDataTable newDataEmp = new OracleDataTable(textQuery, Connect.CurConnect);
            newDataEmp.Fill();
            int count, countComb, countWomen; // Основная работа, Совместителей, Женщин (Основная работа)
            count = countComb = countWomen = 0;
            foreach (DataRow row in newDataEmp.Rows)
            {
                if (row["SIGN_COMB"].ToString() == "")
                    count++;
                else
                    countComb++;
                if (row["EMP_SEX"].ToString() == "Ж" && row["SIGN_COMB"].ToString() == "")
                    countWomen++;
            }
            MessageBox.Show("Количество работников по основной должности - "
                + count.ToString() + ",\nколичество совместителей - " + countComb.ToString()
                + ",\nколичество женщин (основная работа) - " + countWomen.ToString(),
                "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OracleDataTable dtEmpCount;
            switch (((ListEmp)_listemp).NameForm)
            {
                case "ListEmpArchive":
                    dtEmpCount = new OracleDataTable(strCount, Connect.CurConnect);
                    dtEmpCount.Fill();
                    ((ListEmp)_listemp).ssCountEmp.Visible = true;
                    ((ListEmp)_listemp).tsslMainEmp.Text = dtEmpCount.Rows[0]["COUNT_EMP"].ToString();
                    ((ListEmp)_listemp).tsslWoman.Text = dtEmpCount.Rows[0]["COUNT_WOMAN"].ToString();
                    ((ListEmp)_listemp).tsslComb.Text = dtEmpCount.Rows[0]["COUNT_COMB"].ToString();
                    ((ListEmp)_listemp).tsslCombIn.Text = dtEmpCount.Rows[0]["COUNT_IN_COMB"].ToString();
                    ((ListEmp)_listemp).tsslCombOut.Text = dtEmpCount.Rows[0]["COUNT_OUT_COMB"].ToString();

                    ((ListEmp)_listemp).dgEmp.DataSource = null;
                    ((ListEmp)_listemp).dtEmp = newDataEmp;
                    ((ListEmp)_listemp).bsEmp.PositionChanged -= ((ListEmp)_listemp).PositionChange;
                    ((ListEmp)_listemp).bsEmp.DataSource = ((ListEmp)_listemp).dtEmp;
                    ((ListEmp)_listemp).dgEmp.DataSource = ((ListEmp)_listemp).dtEmp;
                    ((ListEmp)_listemp).RefreshGridEmp();
                    ((ListEmp)_listemp).bsEmp.PositionChanged += new EventHandler(((ListEmp)_listemp).PositionChange);
                    ((ListEmp)_listemp).bsEmp.Position = 0;
                    ((ListEmp)_listemp).PositionChange(((ListEmp)_listemp).bsEmp, null);
                    ((ListEmp)_listemp).dgEmp.Focus();
                    break;
                case "ListEmpMain":
                    dtEmpCount = new OracleDataTable(strCount, Connect.CurConnect);
                    dtEmpCount.Fill();
                    ((ListEmp)_listemp).ssCountEmp.Visible = true;
                    ((ListEmp)_listemp).tsslMainEmp.Text = dtEmpCount.Rows[0]["COUNT_EMP"].ToString();
                    ((ListEmp)_listemp).tsslWoman.Text = dtEmpCount.Rows[0]["COUNT_WOMAN"].ToString();
                    ((ListEmp)_listemp).tsslComb.Text = dtEmpCount.Rows[0]["COUNT_COMB"].ToString();
                    ((ListEmp)_listemp).tsslCombIn.Text = dtEmpCount.Rows[0]["COUNT_IN_COMB"].ToString();
                    ((ListEmp)_listemp).tsslCombOut.Text = dtEmpCount.Rows[0]["COUNT_OUT_COMB"].ToString();

                    ((ListEmp)_listemp).dgEmp.DataSource = null;
                    ((ListEmp)_listemp).dtEmp = newDataEmp;
                    ((ListEmp)_listemp).bsEmp.PositionChanged -= ((ListEmp)_listemp).PositionChange;
                    ((ListEmp)_listemp).bsEmp.DataSource = ((ListEmp)_listemp).dtEmp;
                    ((ListEmp)_listemp).dgEmp.DataSource = ((ListEmp)_listemp).dtEmp;
                    ((ListEmp)_listemp).RefreshGridEmp();
                    ((ListEmp)_listemp).bsEmp.PositionChanged += new EventHandler(((ListEmp)_listemp).PositionChange);
                    ((ListEmp)_listemp).bsEmp.Position = 0;
                    ((ListEmp)_listemp).PositionChange(((ListEmp)_listemp).bsEmp, null);
                    ((ListEmp)_listemp).dgEmp.Focus();
                    break;
                case "ListEmpTerm":
                    ((ListEmp)_listemp).dgEmp.DataSource = null;
                    ((ListEmp)_listemp).dtEmp = newDataEmp;
                    ((ListEmp)_listemp).bsEmp.PositionChanged -= ((ListEmp)_listemp).PositionChange;
                    ((ListEmp)_listemp).bsEmp.DataSource = ((ListEmp)_listemp).dtEmp;
                    ((ListEmp)_listemp).dgEmp.DataSource = ((ListEmp)_listemp).dtEmp;
                    ((ListEmp)_listemp).RefreshGridEmp();
                    ((ListEmp)_listemp).bsEmp.PositionChanged += new EventHandler(((ListEmp)_listemp).PositionChange);
                    ((ListEmp)_listemp).bsEmp.Position = 0;
                    ((ListEmp)_listemp).dgEmp.Focus();
                    break;
                default: break;
            }
        }
        /// <summary>
        /// Событие нажатия чекбокса Пол сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chEmp_Sex_CheckedChanged(object sender, EventArgs e)
        {
            if (chEmp_Sex.Checked == true)
            {
                cbEmp_Sex.Enabled = true;
            }
            else
            {
                cbEmp_Sex.Enabled = false;
                cbEmp_Sex.SelectedValue = null;
                cbEmp_Sex.Text = "";
            }
        }

        /// <summary>
        /// Событие нажатия чекбокса Категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chDegree_CheckedChanged(object sender, EventArgs e)
        {
            if (chDegree.Checked == true)
            {
                cbDegree.Enabled = true;
            }
            else
            {
                cbDegree.Enabled = false;
                cbDegree.SelectedValue = null;
                cbDegree.Text = "";
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
                    Connect.Schema, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdiv_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса подразделения-источника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbSubdivSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubdivSource.SelectedValue != null)
            {
                tbCode_SubdivSource.Text = 
                    Library.CodeBySelectedValue(Connect.CurConnect, SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(),
                    Connect.Schema, "subdiv", SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), cbSubdivSource.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса должности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbPos_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPos_Name.SelectedValue != null)
            {
                tbCode_Pos.Text = 
                    Library.CodeBySelectedValue(Connect.CurConnect, POSITION_seq.ColumnsName.CODE_POS.ToString(),
                    Connect.Schema, "position", POSITION_seq.ColumnsName.POS_ID.ToString(), cbPos_Name.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Событие изменения индекса категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void cbDegree_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDegree_Name.SelectedValue != null)
            {
                tbCode_Degree.Text = Library.CodeBySelectedValue(Connect.CurConnect, DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(),
                    Connect.Schema, "degree", DEGREE_seq.ColumnsName.DEGREE_ID.ToString(), cbDegree_Name.SelectedValue.ToString());
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
                Connect.Schema, "subdiv", SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_Subdiv.Text);
        }

        /// <summary>
        /// Проверка введенного шифра подразделения и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_SubdivSource_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_SubdivSource, cbSubdivSource, 3, Connect.CurConnect, e, 
                SUBDIV_seq.ColumnsName.SUBDIV_ID.ToString(), Connect.Schema, "subdiv", 
                SUBDIV_seq.ColumnsName.CODE_SUBDIV.ToString(), tbCode_SubdivSource.Text);
        }
        /// <summary>
        /// Проверка введенного шифра должности и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Pos_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Pos, cbPos_Name, 5, Connect.CurConnect, e, 
                POSITION_seq.ColumnsName.POS_ID.ToString(),
                Connect.Schema, "position", POSITION_seq.ColumnsName.CODE_POS.ToString(), tbCode_Pos.Text);
        }

        /// <summary>
        /// Проверка введенного шифра категории и изменение позиции комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbCode_Degree_Validating(object sender, CancelEventArgs e)
        {
            Library.ValidTextBox(tbCode_Degree, cbDegree_Name, 2, Connect.CurConnect, e, 
                DEGREE_seq.ColumnsName.DEGREE_ID.ToString(),
                Connect.Schema, "degree", DEGREE_seq.ColumnsName.CODE_DEGREE.ToString(), tbCode_Degree.Text);
        }

        private void chFilterBirthDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chFilterBirthDate.Checked)
            {
                nudDayBegin.Enabled = true;
                nudDayEnd.Enabled = true;
                nudMonthBegin.Enabled = true;
                nudMonthEnd.Enabled = true;
            }
            else
            {
                nudDayBegin.Enabled = false;
                nudDayEnd.Enabled = false;
                nudMonthBegin.Enabled = false;
                nudMonthEnd.Enabled = false;
            }
        }

        private void Filter_Emp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))
            {
                btFilter_Click(null, null);
            }
        }
    }

    public class FieldFilter
    {
        private string tableName;
        private string fieldName;
        private string controlValue;

        public string TableName
        {
            get 
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }

        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;
            }
        }

        public string ControlValue
        {
            get
            {
                return controlValue;
            }
            set
            {
                controlValue = value;
            }
        }

        public FieldFilter(string _tableName, string _fieldName, string _controlValue)
        {
            tableName = _tableName;
            fieldName = _fieldName;
            controlValue = _controlValue;
        }
    }
}
