using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Staff;
using System.IO;
using LibraryKadr;
using PercoXML;

namespace Kadr
{
    public partial class FR_EmpAdd : Form
    {
        FR_EMP_seq fr_Emp;
        SUBDIV_seq subdivFR;
        POSITION_seq positionFR;
        string filePath = "";
        bool flagAdd;
        string pathTemp = "";
        public FR_EmpAdd(SUBDIV_seq _subdivFR, POSITION_seq _positionFR, bool _flagAdd, 
            int _perco_sync_id)
        {
            InitializeComponent();
            pnButton.EnableByRules();
            gbPhoto.EnableByRules();
            fr_Emp = new FR_EMP_seq(Connect.CurConnect);
            flagAdd = _flagAdd;
            subdivFR = _subdivFR;
            positionFR = _positionFR;
            /// Проверка нужно ли добавлять новую запись
            if (flagAdd)
            {
                fr_Emp.AddNew();
            }
            else
            {
                fr_Emp.Fill(string.Format("where {0} = {1}", FR_EMP_seq.ColumnsName.PERCO_SYNC_ID, _perco_sync_id ));
                pbPhoto.Image = EmployeePhoto.GetForeignEmpPhoto(_perco_sync_id, Connect.CurConnect);
            }

            /// Привязка компонентов
            cbPos_Name.AddBindingSource(fr_Emp, POSITION_seq.ColumnsName.POS_ID,
                new LinkArgument(positionFR, POSITION_seq.ColumnsName.POS_NAME));
            cbSubdiv_Name.AddBindingSource(fr_Emp, SUBDIV_seq.ColumnsName.SUBDIV_ID,
                new LinkArgument(subdivFR, SUBDIV_seq.ColumnsName.SUBDIV_NAME));
            tbFR_Last_Name.AddBindingSource(fr_Emp, FR_EMP_seq.ColumnsName.FR_LAST_NAME);
            tbFR_First_Name.AddBindingSource(fr_Emp, FR_EMP_seq.ColumnsName.FR_FIRST_NAME);
            tbFR_Middle_Name.AddBindingSource(fr_Emp, FR_EMP_seq.ColumnsName.FR_MIDDLE_NAME);
            //mbFR_Date_Start.AddBindingSource(fr_Emp, FR_EMP_seq.ColumnsName.FR_DATE_START);
            deFR_Date_Start.AddBindingSource(fr_Emp, FR_EMP_seq.ColumnsName.FR_DATE_START);
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (tbFR_Last_Name.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели фамилию стороннего сотрудника.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbFR_Last_Name.Focus();
                return;
            }
            if (tbFR_First_Name.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели имя стороннего сотрудника.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbFR_First_Name.Focus();
                return;
            }
            if (cbSubdiv_Name.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали организацию.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbSubdiv_Name.Focus();
                return;
            }
            if (cbPos_Name.SelectedValue == null)
            {
                MessageBox.Show("Вы не выбрали должность.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbPos_Name.Focus();
                return;
            }
                      
            FR_EMP_obj r_fr_Emp = (FR_EMP_obj)((CurrencyManager)BindingContext[fr_Emp]).Current;            
            /// Если мы добавляли данные, то в перко вставляем новую запись.
            /// Если мы редактировали данные, то в перко обновляем имеющуюся запись.
            if (flagAdd)
            {
                /// Проверка добавления в перко. Если прошло успешно, то сохраняем в нашей базе.
                if (DictionaryPerco.employees.InsertEmployee(new PercoXML.Employee(r_fr_Emp.PERCO_SYNC_ID.ToString(), " ",
                    r_fr_Emp.FR_LAST_NAME, r_fr_Emp.FR_FIRST_NAME, r_fr_Emp.FR_MIDDLE_NAME,
                    r_fr_Emp.SUBDIV_ID.ToString(), r_fr_Emp.POS_ID.ToString()) { Photo = pathTemp, 
                        DateBegin = r_fr_Emp.FR_DATE_START.ToString() }, false))
                {
                    fr_Emp.Save();
                    if (filePath != "")
                    {
                        EmployeePhoto.SetForeignPhoto((decimal)r_fr_Emp.PERCO_SYNC_ID, Connect.CurConnect, filePath);
                    }
                    Connect.Commit();
                    ((CurrencyManager)BindingContext[fr_Emp]).Refresh();
                }
                else
                {
                    MessageBox.Show("Вставка данных сотрудника в PERCo не удалась.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                /// Проверка редактирования в перко. Если прошло успешно, то сохраняем данные в нашей базе.
                if (DictionaryPerco.employees.UpdateEmployee(new PercoXML.Employee(r_fr_Emp.PERCO_SYNC_ID.ToString(), " ",
                    r_fr_Emp.FR_LAST_NAME, r_fr_Emp.FR_FIRST_NAME, r_fr_Emp.FR_MIDDLE_NAME,
                    r_fr_Emp.SUBDIV_ID.ToString(), r_fr_Emp.POS_ID.ToString()) { Photo = pathTemp, 
                        DateBegin = r_fr_Emp.FR_DATE_START.ToString() }, false))
                {
                    fr_Emp.Save();
                    if (filePath != "")
                    {
                        EmployeePhoto.SetForeignPhoto((decimal)r_fr_Emp.PERCO_SYNC_ID, Connect.CurConnect, filePath);
                    }
                    Connect.Commit();
                    ((CurrencyManager)BindingContext[fr_Emp]).Refresh();
                }
                else
                {
                    MessageBox.Show("Редактирование данных сотрудника не удалось.", "АСУ \"Кадры\"", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }            
            Close();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            fr_Emp.RollBack();
            Connect.Rollback();
            Close();
        }
                
        private void btEditPhoto_Click(object sender, EventArgs e)
        {
            if (ofdAddPhoto.ShowDialog() == DialogResult.OK)
            {
                int perco = (int)((FR_EMP_obj)((CurrencyManager)BindingContext[fr_Emp]).Current).PERCO_SYNC_ID;
                FileInfo file = new FileInfo(ofdAddPhoto.FileName);
                /// Копируем фото сотдудника перед тем, как добавлять.
                /// Перко удаляет фото при добавлении в базу.
                filePath = file.FullName;
                pathTemp = "C:\\work\\tmp\\" + file.Name;
                file.CopyTo(pathTemp, true);
                pbPhoto.Load(pathTemp);
            }         
        }
    }
}
