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
using LibraryKadr;

namespace Kadr
{
    public partial class Family : Form
    {
        DataGridView dgViewRelative;
        VisiblePanel pnVisible;

        // Объявление таблиц
        RELATIVE_seq relative;
        //REL_TYPE_seq rel_type;
        EMP_seq emp;

        // Объявление строки образование, табельного номера и флага добавления данных
        RELATIVE_obj r_relative;
        string per_num, per_num_rel;
        bool f_addrelative;

        // Создание формы
        public Family(string _per_num, bool _f_addrelative, int pos, RELATIVE_seq _relative,
            REL_TYPE_seq _rel_type, DataGridView _dgView, VisiblePanel _pnVisible, EMP_seq _emp)
        {            
            InitializeComponent();
            dgViewRelative = _dgView;
            pnVisible = _pnVisible;
            emp = new EMP_seq(Connect.CurConnect);
            emp.Fill(string.Format("order by {0}", EMP_seq.ColumnsName.PER_NUM));
            f_addrelative = _f_addrelative;
            relative = _relative;
            per_num = _per_num;
            per_num_rel = per_num;

            // Проверка нужно ли добавлять новую запись
            if (f_addrelative)
            {
                r_relative = relative.AddNew();
                r_relative.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[relative]).Position = ((CurrencyManager)BindingContext[relative]).Count - 1;
            }
            else
            {
                BindingContext[relative].Position = pos;
                r_relative = (RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current;
                //if (r_relative.REL_PER_NUM == "" || r_relative.REL_PER_NUM == null)
                //{
                //    cbRel_Per_Num.SelectedItem = null;
                //    tbRel_Last_Name.Enabled = true;
                //    tbRel_First_Name.Enabled = true;
                //    tbRel_Middle_Name.Enabled = true;
                //    mbBirth_Date.Enabled = true;
                //    mbBirth_Year.Enabled = true;  
                //    tbRel_Last_Name.Text = r_relative.REL_LAST_NAME;
                //    tbRel_First_Name.Text = r_relative.REL_FIRST_NAME;
                //    tbRel_Middle_Name.Text = r_relative.REL_MIDDLE_NAME;
                //    mbBirth_Date.Text = r_relative.REL_BIRTH_DATE.ToString();
                //    mbBirth_Year.Text = r_relative.REL_BIRTH_YEAR.ToString();
                //    tbRel_Last_Name.Focus();
                //}
                //else
                //{
                //    tbRel_Last_Name.Enabled = false;
                //    tbRel_First_Name.Enabled = false;
                //    tbRel_Middle_Name.Enabled = false;
                //    mbBirth_Date.Enabled = false;
                //    cbRel_Per_Num.Focus();
                //}
            }
            
            // Заполнение комбобоксов значениями из справочников
            cbRel_ID.AddBindingSource(relative, REL_TYPE_seq.ColumnsName.REL_ID, new LinkArgument(_rel_type, REL_TYPE_seq.ColumnsName.NAME_REL));

            //// Заполнение комбобокса табельными номерами для родственников
            ////cbRel_Per_Num.AddBindingSource(relative, RELATIVE_seq.ColumnsName.REL_PER_NUM, new LinkArgument(emp, RELATIVE_seq.ColumnsName.PER_NUM));
            ////bsemp_rel = new BindingSource();
            ////bsemp_rel.DataSource = emp;
            //cbRel_Per_Num.DataSource = emp;
            //cbRel_Per_Num.DisplayMember = "per_num";
            //cbRel_Per_Num.ValueMember = "per_num";
            //cbRel_Per_Num.DataBindings.Add("selectedvalue", relative, "rel_per_num", true, DataSourceUpdateMode.OnPropertyChanged, null, "");

            tbRel_Last_Name.AddBindingSource(relative, RELATIVE_seq.ColumnsName.REL_LAST_NAME);
            tbRel_First_Name.AddBindingSource(relative, RELATIVE_seq.ColumnsName.REL_FIRST_NAME);
            tbRel_Middle_Name.AddBindingSource(relative, RELATIVE_seq.ColumnsName.REL_MIDDLE_NAME);
            mbBirth_Year.AddBindingSource(relative, RELATIVE_seq.ColumnsName.REL_BIRTH_YEAR);
            deBirth_Date.AddBindingSource(relative, RELATIVE_seq.ColumnsName.REL_BIRTH_DATE);

            if (FormMain.flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }            
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            ((CurrencyManager)BindingContext[relative]).EndCurrentEdit();
            relative.Save();
            Connect.Commit();
            f_addrelative = false;
            Library.VisiblePanel(dgViewRelative, pnVisible);
            Close();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Изменение текста комбобокса табельных номеров родственников
        private void cbRel_Per_Num_TextChanged(object sender, EventArgs e)
        {
            //if (cbRel_Per_Num.Text == "") 
            //{
            //    if (f_addrelative)
            //    {
            //        ((RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current).REL_LAST_NAME = "";
            //        ((RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current).REL_FIRST_NAME = "";
            //        ((RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current).REL_MIDDLE_NAME = "";
            //        ((RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current).REL_BIRTH_YEAR = null;
            //        ((RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current).REL_BIRTH_DATE = null;
            //        tbRel_Last_Name.Text = "";
            //        tbRel_First_Name.Text = "";
            //        tbRel_Middle_Name.Text = "";
            //        mbBirth_Date.Text = "";
            //        mbBirth_Year.Text = "";
            //    }
            //    ((RELATIVE_obj)((CurrencyManager)BindingContext[relative]).Current).REL_PER_NUM = "";
            //    tbRel_Last_Name.Enabled = true;
            //    tbRel_First_Name.Enabled = true;
            //    tbRel_Middle_Name.Enabled = true;                
            //    mbBirth_Date.Enabled = true;
            //    mbBirth_Year.Enabled = true;
            //    //tbSername.Focus();
            //}
        }

        // Выбор табельного номера родственника работника
        private void cbRel_Per_Num_SelectedValueChanged(object sender, EventArgs e)
        {
            //if (cbRel_Per_Num.Text == per_num)
            //{
            //    MessageBox.Show("Вы выбрали неверный табельный номер!", "АРМ 'Кадры'", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    btSave.Enabled = false;
            //    return;
            //}
            //else
            //    btSave.Enabled = true;                
            //if (cbRel_Per_Num.Text != "")
            //{
            //    r_emp = emp.Where(s => s.PER_NUM == cbRel_Per_Num.Text).FirstOrDefault();                
            //    if (r_emp != null)
            //    {
            //        r_relative.REL_PER_NUM = r_emp.PER_NUM;
            //        r_relative.REL_BIRTH_DATE = r_emp.EMP_BIRTH_DATE;
            //        r_relative.REL_LAST_NAME = r_emp.EMP_LAST_NAME;
            //        r_relative.REL_FIRST_NAME = r_emp.EMP_FIRST_NAME;
            //        r_relative.REL_MIDDLE_NAME = r_emp.EMP_MIDDLE_NAME;
            //        r_relative.REL_BIRTH_YEAR = r_emp.EMP_BIRTH_DATE.Value.Year;
            //        tbRel_Last_Name.Enabled = false;
            //        tbRel_First_Name.Enabled = false;
            //        tbRel_Middle_Name.Enabled = false;
            //        tbRel_Last_Name.BackColor = Color.White;
            //        tbRel_First_Name.BackColor = Color.White;
            //        tbRel_Middle_Name.BackColor = Color.White;
            //        mbBirth_Date.Enabled = false;
            //        mbBirth_Date.BackColor = Color.White;
            //        mbBirth_Year.Enabled = false;
            //        mbBirth_Year.BackColor = Color.White;
            //        tbRel_Last_Name.Text = r_emp.EMP_LAST_NAME;
            //        tbRel_First_Name.Text = r_emp.EMP_FIRST_NAME;
            //        tbRel_Middle_Name.Text = r_emp.EMP_MIDDLE_NAME;
            //        mbBirth_Date.Text = r_emp.EMP_BIRTH_DATE.ToString();
            //        mbBirth_Year.Text = r_emp.EMP_BIRTH_DATE.Value.Year.ToString();
            //        cbRel_ID.Focus();
            //    }
            //}
            //if (FormMain.flagArchive)
            //{
            //    DisableControl.DisableAll(this, false, Color.White);
            //    btExit.Enabled = true;
            //}
        }

        /// <summary>
        /// Если данные не сохранены до закрытия формы происходит откат сохранения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void Family_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addrelative)
            {
                relative.CancelNew(((CurrencyManager)BindingContext[relative]).Position);
            }
            else if (relative.IsDataChanged())
            {
                relative.RollBack();
            }
            dgViewRelative.Invalidate();
        }

        /// <summary>
        /// Проверка ввода даты рождения родственника.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbBirth_Date_Validating(object sender, CancelEventArgs e)
        {
            ///// Если поле очищено, то обнуляем дату
            //if (mbBirth_Date.Text.Replace(".", "").Trim() == "")
            //{
            //    ((RELATIVE_obj)(((CurrencyManager)BindingContext[relative]).Current)).REL_BIRTH_DATE = null;
            //}
            ///// Если дата введена, проверяем ее правильность
            //else
            //{
            //    try
            //    {
            //        ((RELATIVE_obj)(((CurrencyManager)BindingContext[relative]).Current)).REL_BIRTH_DATE =
            //            Convert.ToDateTime(mbBirth_Date.Text);
            //    }
            //    catch
            //    {
            //        e.Cancel = true;
            //        MessageBox.Show("Вы ввели неверную дату!", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //if (mbBirth_Date.Text.Replace(".", "").Trim() == "")
            //{
            //    mbBirth_Date.DataBindings.Clear();
            //}
            //else
            //{
            //    mbBirth_Date.DataBindings.Add("text", relative, RELATIVE_seq.ColumnsName.REL_BIRTH_DATE.ToString(),
            //        true, DataSourceUpdateMode.OnPropertyChanged, null);
            //}
        }

        /// <summary>
        /// Перевод фамилии родственника в верхний регистр 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRel_Last_Name_Validating(object sender, CancelEventArgs e)
        {
            tbRel_Last_Name.Text = tbRel_Last_Name.Text.ToUpper();
        }

        /// <summary>
        /// Перевод имени родственника в верхний регистр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRel_First_Name_Validating(object sender, CancelEventArgs e)
        {
            tbRel_First_Name.Text = tbRel_First_Name.Text.ToUpper();
        }

        /// <summary>
        /// Перевод отчества родственника в верхний регистр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRel_Middle_Name_Validating(object sender, CancelEventArgs e)
        {
            tbRel_Middle_Name.Text = tbRel_Middle_Name.Text.ToUpper();
        }
    }
}
