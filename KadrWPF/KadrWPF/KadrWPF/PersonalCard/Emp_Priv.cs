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
    public partial class Emp_Priv : Form
    {
        DataGridView dgViewEmp_Priv;
        VisiblePanel pnVisible;

        // Объявление таблиц
        EMP_PRIV_seq emp_priv;
        TYPE_PRIV_seq type_priv;

        // Объявление строки образование, табельного номера и флага добавления данных
        EMP_PRIV_obj r_emp_priv;
        string per_num;
        bool f_addemp_priv;

        // Создание формы
        public Emp_Priv(string _per_num, bool _f_addemp_priv, int pos, EMP_PRIV_seq _emp_priv,
            TYPE_PRIV_seq _type_priv, BASE_DOC_seq _base_doc, DataGridView _dgView, VisiblePanel _pnVisible, bool flagArchive)
        {
            InitializeComponent();
            dgViewEmp_Priv = _dgView;
            pnVisible = _pnVisible;
            f_addemp_priv = _f_addemp_priv;
            emp_priv = _emp_priv;
            type_priv = _type_priv;
            per_num = _per_num;

            // Проверка нужно ли добавлять новую запись
            if (f_addemp_priv)
            {
                r_emp_priv = emp_priv.AddNew();
                r_emp_priv.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[emp_priv]).Position = ((CurrencyManager)BindingContext[emp_priv]).Count - 1;
            }
            else
            {
                BindingContext[emp_priv].Position = pos;
                r_emp_priv = (EMP_PRIV_obj)((CurrencyManager)BindingContext[emp_priv]).Current;
            }

            // Привязка компонентов
            cbType_Priv_ID.AddBindingSource(emp_priv, TYPE_PRIV_seq.ColumnsName.TYPE_PRIV_ID, new LinkArgument(type_priv, TYPE_PRIV_seq.ColumnsName.NAME_PRIV));
            cbBase_Doc_ID.AddBindingSource(emp_priv, BASE_DOC_seq.ColumnsName.BASE_DOC_ID, new LinkArgument(_base_doc, BASE_DOC_seq.ColumnsName.BASE_DOC_NAME));
            tbNum_Priv.AddBindingSource(emp_priv, EMP_PRIV_seq.ColumnsName.PRIV_NUM_DOC);
            deDate_Give.AddBindingSource(emp_priv, EMP_PRIV_seq.ColumnsName.DATE_GIVE);
            deDate_Start_Priv.AddBindingSource(emp_priv, EMP_PRIV_seq.ColumnsName.DATE_START_PRIV);
            deDate_End_Priv.AddBindingSource(emp_priv, EMP_PRIV_seq.ColumnsName.DATE_END_PRIV);
            chIRP.AddBindingSource(emp_priv, EMP_PRIV_seq.ColumnsName.INDIVIDUAL_REHABILITATION);
            chMSE.AddBindingSource(emp_priv, EMP_PRIV_seq.ColumnsName.MEDICAL_SOCIAL_EXPERTISE);
            if (flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
        }

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            if (cbType_Priv_ID.SelectedValue == null)
            {
                MessageBox.Show("Не определено наименование льготы!", "АРМ 'Кадры'", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                OracleCommand _signSave = new OracleCommand(string.Format(
                    @"SELECT {0}.GET_ACCESS_SAVE_EMP_PRIV(:p_TYPE_PRIV_ID, :p_F_ADDEMP_PRIV, :p_EMP_PRIV_ID, :p_DATE_START_PRIV, :p_DATE_END_PRIV) FROM DUAL", Connect.Schema), Connect.CurConnect);
                _signSave.Parameters.Add("p_TYPE_PRIV_ID", OracleDbType.Decimal, ((TYPE_PRIV_obj)((CurrencyManager)BindingContext[type_priv]).Current).TYPE_PRIV_ID, ParameterDirection.Input);
                _signSave.Parameters.Add("p_F_ADDEMP_PRIV", OracleDbType.Decimal, f_addemp_priv, ParameterDirection.Input);
                _signSave.Parameters.Add("p_EMP_PRIV_ID", OracleDbType.Decimal, ((EMP_PRIV_obj)((CurrencyManager)BindingContext[emp_priv]).Current).EMP_PRIV_ID, ParameterDirection.Input);
                _signSave.Parameters.Add("p_DATE_START_PRIV", OracleDbType.Date, ((EMP_PRIV_obj)((CurrencyManager)BindingContext[emp_priv]).Current).DATE_START_PRIV, ParameterDirection.Input);
                _signSave.Parameters.Add("p_DATE_END_PRIV", OracleDbType.Date, ((EMP_PRIV_obj)((CurrencyManager)BindingContext[emp_priv]).Current).DATE_END_PRIV, ParameterDirection.Input);
                try
                {
                    var k = _signSave.ExecuteScalar();
                    if (Convert.ToInt16(k) != 1)
                    {
                        MessageBox.Show("Ошибка сохранения данных.\n",
                                "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Ошибка сохранения данных:\n" + ex.Message,
                            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //if (type_priv.Where(i => i.TYPE_PRIV_ID ==
                //    ((TYPE_PRIV_obj)((CurrencyManager)BindingContext[type_priv]).Current).TYPE_PRIV_ID).First().SIGN_INVALID)
                //{
                //    OracleCommand com = new OracleCommand("", Connect.CurConnect);
                //    com.BindByName = true;
                //    com.CommandText = string.Format(
                //        "select count(*) from user_role_privs where granted_role = 'ACCOUNTANT_EDIT'");
                //    int p_sign1 = Convert.ToInt32(com.ExecuteScalar());
                //    if (p_sign1 == 0)
                //    {
                //        MessageBox.Show("Нельзя вводить данную привилегию!" +
                //            "\nДоступ разрешен только бухгалтерии.",
                //            "АРМ \"Учет рабочего времени\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        return;
                //    }
                //}
            }
            emp_priv.Save();
            Connect.Commit();
            f_addemp_priv = false;
            Library.VisiblePanel(dgViewEmp_Priv, pnVisible);
            Close();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Emp_Priv_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addemp_priv)
            {
                emp_priv.CancelNew(((CurrencyManager)BindingContext[emp_priv]).Position);
            }
            else if (emp_priv.IsDataChanged())
            {
                emp_priv.RollBack();
            }
            dgViewEmp_Priv.Invalidate();
        }
    }
}
