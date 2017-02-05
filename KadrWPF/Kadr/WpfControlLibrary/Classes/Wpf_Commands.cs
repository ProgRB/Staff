using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfControlLibrary
{
    class Wpf_Commands
    {
        private static RoutedUICommand _addOutside_Emp, _editOutside_Emp, _deleteOutside_Emp, _saveOutside_Emp,
            _addOutside_Transfer, _deleteOutside_Transfer, _saveOutside_Transfer, _cancelOutside_Transfer,
            _addResume, _editResume, _deleteResume, _saveResume, _hireEmp,
            _addEdu, _editEdu, _deleteEdu, _saveEdu,
            _addPrev_Work, _editPrev_Work, _deletePrev_Work, _savePrev_Work,
            _edit_Address_None_Kladr,
            _make_Hire, _make_Transfer, _make_Dismiss, _annul_Project, _project_Order_Dismiss, 
            _find_Old_Emp, _hire_New_Emp, _hire_Old_Emp, _hire_Resume_Emp, _saveEmp_Project, _saveTransfer_Project, _saveDismiss_Project,
            _edit_Project, _delete_Project,
            _add_List_Repl_Emp, _edit_List_Repl_Emp, _delete_List_Repl_Emp,
            _add_List_Repl_Contr, _edit_List_Repl_Contr, _delete_List_Repl_Contr,
            _add_Type_Condition, _delete_Type_Condition, _save_Type_Condition, _cancel_Type_Condition,
            _save_Project_Approval,
            _form_Order_Project, _project_To_Transfer, _form_Contract, _registration_Project, _edit_From_Position_By_Project,
            _saveDismissed_Emp, _cancelDismissed_Emp,
            _addProject_Appendix, _editProject_Appendix, _deleteProject_Appendix,
            _saveProject_Appendix, _searchFileAppendix, _viewProject_Appendix,
            _saveTransfer_Overtime, _saveExecuted_Transfer_Emp, _cancelExecuted_Transfer_Emp,
            _unloadTable_To_Salary, _editList_Subdiv_Table, _setSign_Processing, _clearSing_Processing, _editTable_Closing,
            _add_Project_Statement, _edit_Project_Statement, _delete_Project_Statement,
            _addProject_Statement_Appendix, _editProject_Statement_Appendix, _deleteProject_Statement_Appendix,
            _matching_List;

        private static RoutedUICommand _saveAccess_Template, _cancelAccess_Template, _unloadTemplate_From_Perco,
            _editAccess_Templ_By_Subdiv,
            _addAccess_Templ_By_Emp, _editAccess_Templ_By_Emp, _deleteAccess_Templ_By_Emp, _saveAccess_Templ_By_Emp, _cancelAccess_Templ_By_Emp,
            _setAccess_Template;
                
        static Wpf_Commands()
        {
            InputGestureCollection g = new InputGestureCollection();
            g.Add(new KeyGesture(Key.N, ModifierKeys.Control, "Ctrl+N"));
            _addOutside_Emp = new RoutedUICommand("Добавить сотрудника", "AddOutside_Emp", typeof(Wpf_Commands), g);
            _addOutside_Transfer = new RoutedUICommand("Добавить работу", "AddOutside_Transfer", typeof(Wpf_Commands), g);
            _addResume = new RoutedUICommand("Добавить резюме", "AddResume", typeof(Wpf_Commands), g);
            _add_Project_Statement = new RoutedUICommand("Добавить заявление", "Add_Project_Statement", typeof(Wpf_Commands), g);
            _addAccess_Templ_By_Emp = new RoutedUICommand("Добавить доступ", "AddAccess_Templ_By_Emp", typeof(Wpf_Commands), g);

            g = new InputGestureCollection();
            g.Add(new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+E"));
            _editResume = new RoutedUICommand("Редактировать резюме", "EditResume", typeof(Wpf_Commands), g);
            _editOutside_Emp = new RoutedUICommand("Редактировать сотрудника", "EditOutside_Emp", typeof(Wpf_Commands), g);
            _editAccess_Templ_By_Subdiv = new RoutedUICommand("Редактировать список подразделений для шаблона", "EditAccess_Templ_By_Subdiv", typeof(Wpf_Commands), g);
            _editAccess_Templ_By_Emp = new RoutedUICommand("Редактировать доступ", "EditAccess_Templ_By_Emp", typeof(Wpf_Commands), g);
            _edit_Project_Statement = new RoutedUICommand("Редактировать заявление", "Edit_Project_Statement", typeof(Wpf_Commands), g);

            g = new InputGestureCollection();
            g.Add(new KeyGesture(Key.Delete, ModifierKeys.Control, "Delete"));
            _deleteOutside_Emp = new RoutedUICommand("Удалить сотрудника", "DeleteOutside_Emp", typeof(Wpf_Commands), g);
            _deleteOutside_Transfer = new RoutedUICommand("Удалить работу", "DeleteOutside_Transfer", typeof(Wpf_Commands), g);
            _deleteResume = new RoutedUICommand("Удалить резюме", "DeleteResume", typeof(Wpf_Commands), g);
            _delete_Project_Statement = new RoutedUICommand("Удалить заявление", "Delete_Project_Statement", typeof(Wpf_Commands), g);
            _deleteAccess_Templ_By_Emp = new RoutedUICommand("Удалить доступ", "DeleteAccess_Templ_By_Emp", typeof(Wpf_Commands), g);

            g = new InputGestureCollection();
            g.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S"));
            _saveOutside_Emp = new RoutedUICommand("Сохранить данные", "SaveOutside_Emp", typeof(Wpf_Commands), g);
            _saveOutside_Transfer = new RoutedUICommand("Сохранить данные", "SaveOutside_Transfer", typeof(Wpf_Commands), g);
            _saveResume = new RoutedUICommand("Сохранить данные", "SaveResume", typeof(Wpf_Commands), g);
            _saveDismissed_Emp = new RoutedUICommand("Сохранить изменения", "SaveDismissed_Emp", typeof(Wpf_Commands), g);
            _saveAccess_Template = new RoutedUICommand("Сохранить изменения", "SaveAccess_Template", typeof(Wpf_Commands), g);
            _saveAccess_Templ_By_Emp = new RoutedUICommand("Сохранить изменения", "SaveAccess_Templ_By_Emp", typeof(Wpf_Commands), g);

            g = new InputGestureCollection();
            g.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+Z"));
            _cancelOutside_Transfer = new RoutedUICommand("Отменить изменения", "CancelOutside_Transfer", typeof(Wpf_Commands), g);
            _cancelDismissed_Emp = new RoutedUICommand("Отменить изменения", "CancelDismissed_Emp", typeof(Wpf_Commands), g);
            _cancelAccess_Template = new RoutedUICommand("Отменить изменения", "CancelAccess_Template", typeof(Wpf_Commands), g);
            _cancelAccess_Templ_By_Emp = new RoutedUICommand("Отменить изменения", "CancelAccess_Templ_By_Emp", typeof(Wpf_Commands), g);

            _edit_Address_None_Kladr = new RoutedUICommand("Редактировать адрес отсутствующий в КЛАДР", "Edit_Address_None_Kladr", typeof(Wpf_Commands));
            _addEdu = new RoutedUICommand("Добавить образование", "AddEdu", typeof(Wpf_Commands));
            _editEdu = new RoutedUICommand("Редактировать образование", "EditEdu", typeof(Wpf_Commands));
            _deleteEdu = new RoutedUICommand("Удалить образование", "DeleteEdu", typeof(Wpf_Commands));
            _saveEdu = new RoutedUICommand("Сохранить данные", "SaveEdu", typeof(Wpf_Commands));
            _addPrev_Work = new RoutedUICommand("Добавить работу", "AddPrev_Work", typeof(Wpf_Commands));
            _editPrev_Work = new RoutedUICommand("Редактировать работу", "EditPrev_Work", typeof(Wpf_Commands));
            _deletePrev_Work = new RoutedUICommand("Удалить работу", "DeletePrev_Work", typeof(Wpf_Commands));
            _savePrev_Work = new RoutedUICommand("Сохранить данные", "SavePrev_Work", typeof(Wpf_Commands));

            _hireEmp = new RoutedUICommand("Принять работника", "HireEmp", typeof(Wpf_Commands));

            _make_Hire = new RoutedUICommand("Оформить прием", "Make_Hire", typeof(Wpf_Commands));
            _make_Transfer = new RoutedUICommand("Оформить перевод", "Make_Transfer", typeof(Wpf_Commands));
            _make_Dismiss = new RoutedUICommand("Оформить увольнение", "Make_Dismiss", typeof(Wpf_Commands));

            _find_Old_Emp = new RoutedUICommand("Поиск данных", "Find_Old_Emp", typeof(Wpf_Commands));
            _hire_New_Emp = new RoutedUICommand("Новый работник", "Hire_New_Emp", typeof(Wpf_Commands));
            _hire_Old_Emp = new RoutedUICommand("Принять работника", "Hire_Old_Emp", typeof(Wpf_Commands));
            _hire_Resume_Emp = new RoutedUICommand("Принять по резюме", "Hire_Resume_Emp", typeof(Wpf_Commands));
            _saveEmp_Project = new RoutedUICommand("Сохранить данные работника", "SaveEmp_Project", typeof(Wpf_Commands));
            _saveTransfer_Project = new RoutedUICommand("Сохранить данные проекта", "SaveTransfer_Project", typeof(Wpf_Commands));
            _saveDismiss_Project = new RoutedUICommand("Сохранить данные увольнения", "SaveDismiss_Project", typeof(Wpf_Commands));
            _edit_Project = new RoutedUICommand("Редактировать проект", "Edit_Project", typeof(Wpf_Commands));
            _delete_Project = new RoutedUICommand("Удалить проект", "Delete_Project", typeof(Wpf_Commands));

            _add_List_Repl_Emp = new RoutedUICommand("Добавить замещаемого сотрудника", "Add_List_Repl_Emp", typeof(Wpf_Commands));
            _add_List_Repl_Contr = new RoutedUICommand("Добавить замещаемого сотрудника", "Add_List_Repl_Contr", typeof(Wpf_Commands));
            _edit_List_Repl_Emp = new RoutedUICommand("Редактировать замещаемого сотрудника", "Edit_List_Repl_Emp", typeof(Wpf_Commands));
            _edit_List_Repl_Contr = new RoutedUICommand("Редактировать замещаемого сотрудника", "Edit_List_Repl_Contr", typeof(Wpf_Commands));
            _delete_List_Repl_Emp = new RoutedUICommand("Удалить замещаемого сотрудника", "Delete_List_Repl_Emp", typeof(Wpf_Commands));
            _delete_List_Repl_Contr = new RoutedUICommand("Удалить замещаемого сотрудника", "Delete_List_Repl_Contr", typeof(Wpf_Commands));

            _add_Type_Condition = new RoutedUICommand("Добавить", "Add_Type_Condition", typeof(Wpf_Commands));
            _delete_Type_Condition = new RoutedUICommand("Удалить", "Delete_Type_Condition", typeof(Wpf_Commands));
            _save_Type_Condition = new RoutedUICommand("Сохранить данные", "Save_Type_Condition", typeof(Wpf_Commands));
            _cancel_Type_Condition = new RoutedUICommand("Отменить изменения", "Cancel_Type_Condition", typeof(Wpf_Commands));

            _save_Project_Approval = new RoutedUICommand("Сохранить решение", "Save_Project_Approval", typeof(Wpf_Commands));

            _form_Order_Project = new RoutedUICommand("Сформировать приказ", "Form_Order_Project", typeof(Wpf_Commands));
            _project_To_Transfer = new RoutedUICommand("Провести в Основную БД", "Project_To_Transfer", typeof(Wpf_Commands));
            _registration_Project = new RoutedUICommand("Зарегистрировано в ШР", "Registration_Project", typeof(Wpf_Commands));
            _form_Contract = new RoutedUICommand("Договор (Соглашение)", "Form_Contract", typeof(Wpf_Commands));
            _edit_From_Position_By_Project = new RoutedUICommand("Редактировать родительский перевод для проекта", "Edit_From_Position_By_Project", typeof(Wpf_Commands));

            _addProject_Appendix = new RoutedUICommand("Добавить документ", "AddProject_Appendix", typeof(Wpf_Commands));
            _editProject_Appendix = new RoutedUICommand("Редактировать документ", "EditProject_Appendix", typeof(Wpf_Commands));
            _deleteProject_Appendix = new RoutedUICommand("Удалить документ", "DeleteProject_Appendix", typeof(Wpf_Commands));
            _saveProject_Appendix = new RoutedUICommand("Сохранить документ", "SaveProject_Appendix", typeof(Wpf_Commands));
            _searchFileAppendix = new RoutedUICommand("Выбрать файл приложения", "SearchFileAppendix", typeof(Wpf_Commands));
            _viewProject_Appendix = new RoutedUICommand("Просмотр приложения", "ViewProject_Appendix", typeof(Wpf_Commands));

            _annul_Project = new RoutedUICommand("Аннулировать проект", "Annul_Project", typeof(Wpf_Commands));
            _project_Order_Dismiss = new RoutedUICommand("Проект приказа", "Project_Order_Dismiss", typeof(Wpf_Commands));

            _saveTransfer_Overtime = new RoutedUICommand("Сохранить изменения", "SaveTransfer_Overtime", typeof(Wpf_Commands));
            _saveExecuted_Transfer_Emp = new RoutedUICommand("Сохранить изменения", "SaveExecuted_Transfer_Emp", typeof(Wpf_Commands));
            _cancelExecuted_Transfer_Emp = new RoutedUICommand("Отменить изменения", "CancelExecuted_Transfer_Emp", typeof(Wpf_Commands));

            _unloadTable_To_Salary = new RoutedUICommand("Выгрузить данные табеля для расчета зарплаты", "UnloadTable_To_Salary", typeof(Wpf_Commands));
            _editList_Subdiv_Table = new RoutedUICommand("Редактировать список подразделений", "EditList_Subdiv_Table", typeof(Wpf_Commands));
            _setSign_Processing = new RoutedUICommand("Установить признак обработки", "SetSign_Processing", typeof(Wpf_Commands));
            _clearSing_Processing = new RoutedUICommand("Очистить признак обработки", "ClearSing_Processing", typeof(Wpf_Commands));
            _editTable_Closing = new RoutedUICommand("Редактировать данные по подразделению", "EditTable_Closing", typeof(Wpf_Commands));

            //Команды табеля
            SaveRegDoc = new RoutedUICommand("Сохранить документ", "EditRegDocTable", typeof(Wpf_Commands));

            _unloadTemplate_From_Perco = new RoutedUICommand("Обновить шаблоны из Perco-S-20", "UnloadTemplate_From_Perco", typeof(Wpf_Commands));

            _addProject_Statement_Appendix = new RoutedUICommand("Добавить документ", "AddProject_Statement_Appendix", typeof(Wpf_Commands));
            _editProject_Statement_Appendix = new RoutedUICommand("Редактировать документ", "EditProject_Statement_Appendix", typeof(Wpf_Commands));
            _deleteProject_Statement_Appendix = new RoutedUICommand("Удалить документ", "DeleteProject_Statement_Appendix", typeof(Wpf_Commands));

            _matching_List = new RoutedUICommand("Лист согласования", "Matching_List", typeof(Wpf_Commands));

            _setAccess_Template = new RoutedUICommand("Установить доступ", "SetAccess_Template", typeof(Wpf_Commands));
        }

        public static RoutedUICommand AddOutside_Emp
        {
            get { return Wpf_Commands._addOutside_Emp; }
            set { Wpf_Commands._addOutside_Emp = value; }
        }

        public static RoutedUICommand EditOutside_Emp
        {
            get { return Wpf_Commands._editOutside_Emp; }
            set { Wpf_Commands._editOutside_Emp = value; }
        }

        public static RoutedUICommand DeleteOutside_Emp
        {
            get { return Wpf_Commands._deleteOutside_Emp; }
            set { Wpf_Commands._deleteOutside_Emp = value; }
        }

        public static RoutedUICommand SaveOutside_Emp
        {
            get { return Wpf_Commands._saveOutside_Emp; }
            set { Wpf_Commands._saveOutside_Emp = value; }
        }

        public static RoutedUICommand AddOutside_Transfer
        {
            get { return Wpf_Commands._addOutside_Transfer; }
            set { Wpf_Commands._addOutside_Transfer = value; }
        }

        public static RoutedUICommand DeleteOutside_Transfer
        {
            get { return Wpf_Commands._deleteOutside_Transfer; }
            set { Wpf_Commands._deleteOutside_Transfer = value; }
        }

        public static RoutedUICommand SaveOutside_Transfer
        {
            get { return Wpf_Commands._saveOutside_Transfer; }
            set { Wpf_Commands._saveOutside_Transfer = value; }
        }

        public static RoutedUICommand CancelOutside_Transfer
        {
            get { return Wpf_Commands._cancelOutside_Transfer; }
            set { Wpf_Commands._cancelOutside_Transfer = value; }
        }

        public static RoutedUICommand AddResume
        {
            get { return Wpf_Commands._addResume; }
            set { Wpf_Commands._addResume = value; }
        }

        public static RoutedUICommand EditResume
        {
            get { return Wpf_Commands._editResume; }
            set { Wpf_Commands._editResume = value; }
        }

        public static RoutedUICommand DeleteResume
        {
            get { return Wpf_Commands._deleteResume; }
            set { Wpf_Commands._deleteResume = value; }
        }

        public static RoutedUICommand SaveResume
        {
            get { return Wpf_Commands._saveResume; }
            set { Wpf_Commands._saveResume = value; }
        }

        public static RoutedUICommand AddEdu
        {
            get { return Wpf_Commands._addEdu; }
            set { Wpf_Commands._addEdu = value; }
        }

        public static RoutedUICommand EditEdu
        {
            get { return Wpf_Commands._editEdu; }
            set { Wpf_Commands._editEdu = value; }
        }

        public static RoutedUICommand DeleteEdu
        {
            get { return Wpf_Commands._deleteEdu; }
            set { Wpf_Commands._deleteEdu = value; }
        }

        public static RoutedUICommand SaveEdu
        {
            get { return Wpf_Commands._saveEdu; }
            set { Wpf_Commands._saveEdu = value; }
        }

        public static RoutedUICommand AddPrev_Work
        {
            get { return Wpf_Commands._addPrev_Work; }
            set { Wpf_Commands._addPrev_Work = value; }
        }

        public static RoutedUICommand EditPrev_Work
        {
            get { return Wpf_Commands._editPrev_Work; }
            set { Wpf_Commands._editPrev_Work = value; }
        }

        public static RoutedUICommand DeletePrev_Work
        {
            get { return Wpf_Commands._deletePrev_Work; }
            set { Wpf_Commands._deletePrev_Work = value; }
        }

        public static RoutedUICommand SavePrev_Work
        {
            get { return Wpf_Commands._savePrev_Work; }
            set { Wpf_Commands._savePrev_Work = value; }
        }

        public static RoutedUICommand HireEmp
        {
            get { return Wpf_Commands._hireEmp; }
            set { Wpf_Commands._hireEmp = value; }
        }

        public static RoutedUICommand Make_Hire
        {
            get { return Wpf_Commands._make_Hire; }
            set { Wpf_Commands._make_Hire = value; }
        }

        public static RoutedUICommand Make_Transfer
        {
            get { return Wpf_Commands._make_Transfer; }
            set { Wpf_Commands._make_Transfer = value; }
        }

        public static RoutedUICommand Find_Old_Emp
        {
            get { return Wpf_Commands._find_Old_Emp; }
            set { Wpf_Commands._find_Old_Emp = value; }
        }

        public static RoutedUICommand Hire_New_Emp
        {
            get { return Wpf_Commands._hire_New_Emp; }
            set { Wpf_Commands._hire_New_Emp = value; }
        }

        public static RoutedUICommand Hire_Old_Emp
        {
            get { return Wpf_Commands._hire_Old_Emp; }
            set { Wpf_Commands._hire_Old_Emp = value; }
        }

        public static RoutedUICommand Hire_Resume_Emp
        {
            get { return Wpf_Commands._hire_Resume_Emp; }
            set { Wpf_Commands._hire_Resume_Emp = value; }
        }

        public static RoutedUICommand SaveTransfer_Project
        {
            get { return Wpf_Commands._saveTransfer_Project; }
            set { Wpf_Commands._saveTransfer_Project = value; }
        }

        public static RoutedUICommand Edit_Project
        {
            get { return Wpf_Commands._edit_Project; }
            set { Wpf_Commands._edit_Project = value; }
        }

        public static RoutedUICommand Delete_Project
        {
            get { return Wpf_Commands._delete_Project; }
            set { Wpf_Commands._delete_Project = value; }
        }

        public static RoutedUICommand SaveEmp_Project
        {
            get { return Wpf_Commands._saveEmp_Project; }
            set { Wpf_Commands._saveEmp_Project = value; }
        }

        public static RoutedUICommand Add_List_Repl_Emp
        {
            get { return Wpf_Commands._add_List_Repl_Emp; }
            set { Wpf_Commands._add_List_Repl_Emp = value; }
        }

        public static RoutedUICommand Edit_List_Repl_Emp
        {
            get { return Wpf_Commands._edit_List_Repl_Emp; }
            set { Wpf_Commands._edit_List_Repl_Emp = value; }
        }

        public static RoutedUICommand Delete_List_Repl_Emp
        {
            get { return Wpf_Commands._delete_List_Repl_Emp; }
            set { Wpf_Commands._delete_List_Repl_Emp = value; }
        }

        public static RoutedUICommand Add_List_Repl_Contr
        {
            get { return Wpf_Commands._add_List_Repl_Contr; }
            set { Wpf_Commands._add_List_Repl_Contr = value; }
        }

        public static RoutedUICommand Edit_List_Repl_Contr
        {
            get { return Wpf_Commands._edit_List_Repl_Contr; }
            set { Wpf_Commands._edit_List_Repl_Contr = value; }
        }

        public static RoutedUICommand Delete_List_Repl_Contr
        {
            get { return Wpf_Commands._delete_List_Repl_Contr; }
            set { Wpf_Commands._delete_List_Repl_Contr = value; }
        }

        public static RoutedUICommand Add_Type_Condition
        {
            get { return Wpf_Commands._add_Type_Condition; }
            set { Wpf_Commands._add_Type_Condition = value; }
        }

        public static RoutedUICommand Delete_Type_Condition
        {
            get { return Wpf_Commands._delete_Type_Condition; }
            set { Wpf_Commands._delete_Type_Condition = value; }
        }

        public static RoutedUICommand Save_Type_Condition
        {
            get { return Wpf_Commands._save_Type_Condition; }
            set { Wpf_Commands._save_Type_Condition = value; }
        }

        public static RoutedUICommand Cancel_Type_Condition
        {
            get { return Wpf_Commands._cancel_Type_Condition; }
            set { Wpf_Commands._cancel_Type_Condition = value; }
        }

        public static RoutedUICommand Save_Project_Approval
        {
            get { return Wpf_Commands._save_Project_Approval; }
            set { Wpf_Commands._save_Project_Approval = value; }
        }

        public static RoutedUICommand Form_Order_Project
        {
            get { return Wpf_Commands._form_Order_Project; }
            set { Wpf_Commands._form_Order_Project = value; }
        }

        public static RoutedUICommand Project_To_Transfer
        {
            get { return Wpf_Commands._project_To_Transfer; }
            set { Wpf_Commands._project_To_Transfer = value; }
        }

        public static RoutedUICommand Form_Contract
        {
            get { return Wpf_Commands._form_Contract; }
            set { Wpf_Commands._form_Contract = value; }
        }

        public static RoutedUICommand SaveDismissed_Emp
        {
            get { return Wpf_Commands._saveDismissed_Emp; }
            set { Wpf_Commands._saveDismissed_Emp = value; }
        }

        public static RoutedUICommand CancelDismissed_Emp
        {
            get { return Wpf_Commands._cancelDismissed_Emp; }
            set { Wpf_Commands._cancelDismissed_Emp = value; }
        }

        public static RoutedUICommand AddProject_Appendix
        {
            get { return Wpf_Commands._addProject_Appendix; }
            set { Wpf_Commands._addProject_Appendix = value; }
        }

        public static RoutedUICommand EditProject_Appendix
        {
            get { return Wpf_Commands._editProject_Appendix; }
            set { Wpf_Commands._editProject_Appendix = value; }
        }

        public static RoutedUICommand DeleteProject_Appendix
        {
            get { return Wpf_Commands._deleteProject_Appendix; }
            set { Wpf_Commands._deleteProject_Appendix = value; }
        }

        public static RoutedUICommand SaveProject_Appendix
        {
            get { return Wpf_Commands._saveProject_Appendix; }
            set { Wpf_Commands._saveProject_Appendix = value; }
        }

        public static RoutedUICommand SearchFileAppendix
        {
            get { return Wpf_Commands._searchFileAppendix; }
            set { Wpf_Commands._searchFileAppendix = value; }
        }

        public static RoutedUICommand Registration_Project
        {
            get { return Wpf_Commands._registration_Project; }
            set { Wpf_Commands._registration_Project = value; }
        }

        public static RoutedUICommand Make_Dismiss
        {
            get { return Wpf_Commands._make_Dismiss; }
            set { Wpf_Commands._make_Dismiss = value; }
        }

        public static RoutedUICommand SaveDismiss_Project
        {
            get { return Wpf_Commands._saveDismiss_Project; }
            set { Wpf_Commands._saveDismiss_Project = value; }
        }

        public static RoutedUICommand Annul_Project
        {
            get { return Wpf_Commands._annul_Project; }
            set { Wpf_Commands._annul_Project = value; }
        }

        public static RoutedUICommand Edit_Address_None_Kladr
        {
            get { return Wpf_Commands._edit_Address_None_Kladr; }
            set { Wpf_Commands._edit_Address_None_Kladr = value; }
        }

        public static RoutedUICommand Project_Order_Dismiss
        {
            get { return Wpf_Commands._project_Order_Dismiss; }
            set { Wpf_Commands._project_Order_Dismiss = value; }
        }

        public static RoutedUICommand Edit_From_Position_By_Project
        {
            get { return Wpf_Commands._edit_From_Position_By_Project; }
            set { Wpf_Commands._edit_From_Position_By_Project = value; }
        }

        public static RoutedUICommand SaveTransfer_Overtime
        {
            get { return Wpf_Commands._saveTransfer_Overtime; }
            set { Wpf_Commands._saveTransfer_Overtime = value; }
        }

        public static RoutedUICommand SaveExecuted_Transfer_Emp
        {
            get { return Wpf_Commands._saveExecuted_Transfer_Emp; }
            set { Wpf_Commands._saveExecuted_Transfer_Emp = value; }
        }

        public static RoutedUICommand CancelExecuted_Transfer_Emp
        {
            get { return Wpf_Commands._cancelExecuted_Transfer_Emp; }
            set { Wpf_Commands._cancelExecuted_Transfer_Emp = value; }
        }

        public static RoutedUICommand ViewProject_Appendix
        {
            get { return Wpf_Commands._viewProject_Appendix; }
            set { Wpf_Commands._viewProject_Appendix = value; }
        }

        public static RoutedUICommand UnloadTable_To_Salary
        {
            get { return Wpf_Commands._unloadTable_To_Salary; }
            set { Wpf_Commands._unloadTable_To_Salary = value; }
        }

        public static RoutedUICommand EditList_Subdiv_Table
        {
            get { return Wpf_Commands._editList_Subdiv_Table; }
            set { Wpf_Commands._editList_Subdiv_Table = value; }
        }

        public static RoutedUICommand SetSign_Processing
        {
            get { return Wpf_Commands._setSign_Processing; }
            set { Wpf_Commands._setSign_Processing = value; }
        }

        public static RoutedUICommand ClearSing_Processing
        {
            get { return Wpf_Commands._clearSing_Processing; }
            set { Wpf_Commands._clearSing_Processing = value; }
        }

        public static RoutedUICommand EditTable_Closing
        {
            get { return Wpf_Commands._editTable_Closing; }
            set { Wpf_Commands._editTable_Closing = value; }
        }

        public static RoutedUICommand SaveRegDoc { get; set; }
        
        public static RoutedUICommand SaveAccess_Template
        {
            get { return Wpf_Commands._saveAccess_Template; }
            set { Wpf_Commands._saveAccess_Template = value; }
        }

        public static RoutedUICommand CancelAccess_Template
        {
            get { return Wpf_Commands._cancelAccess_Template; }
            set { Wpf_Commands._cancelAccess_Template = value; }
        }

        public static RoutedUICommand UnloadTemplate_From_Perco
        {
            get { return Wpf_Commands._unloadTemplate_From_Perco; }
            set { Wpf_Commands._unloadTemplate_From_Perco = value; }
        }

        public static RoutedUICommand EditAccess_Templ_By_Subdiv
        {
            get { return Wpf_Commands._editAccess_Templ_By_Subdiv; }
            set { Wpf_Commands._editAccess_Templ_By_Subdiv = value; }
        }

        public static RoutedUICommand Add_Project_Statement
        {
            get { return Wpf_Commands._add_Project_Statement; }
            set { Wpf_Commands._add_Project_Statement = value; }
        }

        public static RoutedUICommand Edit_Project_Statement
        {
            get { return Wpf_Commands._edit_Project_Statement; }
            set { Wpf_Commands._edit_Project_Statement = value; }
        }

        public static RoutedUICommand Delete_Project_Statement
        {
            get { return Wpf_Commands._delete_Project_Statement; }
            set { Wpf_Commands._delete_Project_Statement = value; }
        }

        public static RoutedUICommand AddProject_Statement_Appendix
        {
            get { return Wpf_Commands._addProject_Statement_Appendix; }
            set { Wpf_Commands._addProject_Statement_Appendix = value; }
        }

        public static RoutedUICommand EditProject_Statement_Appendix
        {
            get { return Wpf_Commands._editProject_Statement_Appendix; }
            set { Wpf_Commands._editProject_Statement_Appendix = value; }
        }

        public static RoutedUICommand DeleteProject_Statement_Appendix
        {
            get { return Wpf_Commands._deleteProject_Statement_Appendix; }
            set { Wpf_Commands._deleteProject_Statement_Appendix = value; }
        }

        public static RoutedUICommand Matching_List
        {
            get { return Wpf_Commands._matching_List; }
            set { Wpf_Commands._matching_List = value; }
        }

        public static RoutedUICommand EditAccess_Templ_By_Emp
        {
            get { return Wpf_Commands._editAccess_Templ_By_Emp; }
            set { Wpf_Commands._editAccess_Templ_By_Emp = value; }
        }

        public static RoutedUICommand AddAccess_Templ_By_Emp
        {
            get { return Wpf_Commands._addAccess_Templ_By_Emp; }
            set { Wpf_Commands._addAccess_Templ_By_Emp = value; }
        }

        public static RoutedUICommand DeleteAccess_Templ_By_Emp
        {
            get { return Wpf_Commands._deleteAccess_Templ_By_Emp; }
            set { Wpf_Commands._deleteAccess_Templ_By_Emp = value; }
        }

        public static RoutedUICommand SaveAccess_Templ_By_Emp
        {
            get { return Wpf_Commands._saveAccess_Templ_By_Emp; }
            set { Wpf_Commands._saveAccess_Templ_By_Emp = value; }
        }

        public static RoutedUICommand CancelAccess_Templ_By_Emp
        {
            get { return Wpf_Commands._cancelAccess_Templ_By_Emp; }
            set { Wpf_Commands._cancelAccess_Templ_By_Emp = value; }
        }

        public static RoutedUICommand SetAccess_Template
        {
            get { return Wpf_Commands._setAccess_Template; }
            set { Wpf_Commands._setAccess_Template = value; }
        }
    }
}
