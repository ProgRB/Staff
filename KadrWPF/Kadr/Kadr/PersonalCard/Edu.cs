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
    public partial class Edu : Form
    {
        DataGridView dgViewEdu;
        VisiblePanel pnVisible;

        // Объявление таблиц
        EDU_seq edu;

        // Объявление строки образование, табельного номера и флага добавления данных
        EDU_obj r_edu;
        string per_num;
        bool f_addedu;

        // Создание формы
        public Edu(string _per_num, bool _f_addedu, int pos, EDU_seq _edu, SPECIALITY_seq _speciality, 
            INSTIT_seq _instit, TYPE_STUDY_seq _type_study, TYPE_EDU_seq _type_edu, QUAL_seq _qual, GROUP_SPEC_seq _group_spec, 
            DataGridView _dgView, VisiblePanel _pnVisible)
        {
            InitializeComponent();
            dgViewEdu = _dgView;
            pnVisible = _pnVisible;
            f_addedu = _f_addedu;
            edu = _edu;
            per_num = _per_num;
            // Проверка нужно ли добавлять новую запись
            if (f_addedu)
            {
                r_edu = edu.AddNew();
                r_edu.PER_NUM = per_num;
                ((CurrencyManager)BindingContext[edu]).Position = ((CurrencyManager)BindingContext[edu]).Count - 1;
            }
            else
            {
                BindingContext[edu].Position = pos;
                r_edu = (EDU_obj)((CurrencyManager)BindingContext[edu]).Current;
            }     
            
            // Привязка компонентов
            cbSpec_ID.AddBindingSource(edu, SPECIALITY_seq.ColumnsName.SPEC_ID, new LinkArgument(_speciality, SPECIALITY_seq.ColumnsName.NAME_SPEC));
            cbInstit_ID.AddBindingSource(edu, INSTIT_seq.ColumnsName.INSTIT_ID, new LinkArgument(_instit, INSTIT_seq.ColumnsName.INSTIT_NAME));
            cbType_Study_ID.AddBindingSource(edu, TYPE_STUDY_seq.ColumnsName.TYPE_STUDY_ID, new LinkArgument(_type_study, TYPE_STUDY_seq.ColumnsName.TS_NAME));
            cbType_Edu_ID.AddBindingSource(edu, TYPE_EDU_seq.ColumnsName.TYPE_EDU_ID, new LinkArgument(_type_edu, TYPE_EDU_seq.ColumnsName.TE_NAME));
            cbQual_ID.AddBindingSource(edu, QUAL_seq.ColumnsName.QUAL_ID, new LinkArgument(_qual, QUAL_seq.ColumnsName.QUAL_NAME));
            cbGroup_Spec_ID.AddBindingSource(edu, GROUP_SPEC_seq.ColumnsName.GR_SPEC_ID, new LinkArgument(_group_spec, GROUP_SPEC_seq.ColumnsName.GS_NAME));
            tbSeria_Diploma.AddBindingSource(edu, EDU_seq.ColumnsName.SERIA_DIPLOMA);
            tbNum_Diploma.AddBindingSource(edu, EDU_seq.ColumnsName.NUM_DIPLOMA);
            tbSpecialization.AddBindingSource(edu, EDU_seq.ColumnsName.SPECIALIZATION);
            chMain_Proff.AddBindingSource(edu, EDU_seq.ColumnsName.MAIN_PROF);
            chFrom_Fact.AddBindingSource(edu, EDU_seq.ColumnsName.FROM_FACT);
            //mbYear_Graduating.AddBindingSource(edu, EDU_seq.ColumnsName.YEAR_GRADUATING);
            deYear_Graduating.AddBindingSource(edu, EDU_seq.ColumnsName.YEAR_GRADUATING);
            if (FormMain.flagArchive)
            {
                DisableControl.DisableAll(this, false, Color.White);
                btExit.Enabled = true;
            }
            //btSave.Enabled = true;
        }        

        // Сохранение данных и закрытие формы
        private void btSave_Click(object sender, EventArgs e)
        {
            edu.Save();
            Connect.Commit();
            f_addedu = false;
            Library.VisiblePanel(dgViewEdu, pnVisible);
            Close();
        }

        // Если данные не сохранены до закрытия формы происходит откат сохранения
        private void Edu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (f_addedu)
            {
                edu.CancelNew(((CurrencyManager)BindingContext[edu]).Position);
            }
            else if (edu.IsDataChanged())
            {
                edu.RollBack();
            }
            dgViewEdu.Invalidate();
        }

        // Закрытие формы
        private void btExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
