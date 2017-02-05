using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfControlLibrary
{
    class Pass_Office_Commands
    {
        private static RoutedUICommand _add_Violation, _edit_Violation, _delete_Violation, _find_Violator, _save_Violation,
            _add_Stolen_Property, _delete_Stolen_Property, _save_Stolen_Property,
            _add_Punishment, _delete_Punishment, _save_Punishment, _cancel_Punishment,
            _add_List_Punishment, _delete_List_Punishment, _save_List_Punishment, _cancel_List_Punishment,
            _add_Other_Violator, _delete_Other_Violator, _selectEmp, _printViolationByPeriod, _summaryDataOfTheEmployee, _disciplineDisturbers,
            _select_Chief_Violator, _clear_Chief_Violator;
        
        static Pass_Office_Commands()
        {
            _add_Violation = new RoutedUICommand(
                "Добавить данные", "Add_Violation", typeof(Pass_Office_Commands));
            _edit_Violation = new RoutedUICommand(
                "Редактировать данные", "Edit_Violation", typeof(Pass_Office_Commands));
            _delete_Violation = new RoutedUICommand(
                "Удалить данные", "Delete_Violation", typeof(Pass_Office_Commands));
            _find_Violator = new RoutedUICommand(
                "Поиск нарушителя", "Find_Violator", typeof(Pass_Office_Commands));
            _save_Violation = new RoutedUICommand(
                "Сохранить", "Save_Violation", typeof(Pass_Office_Commands));
            _add_Stolen_Property = new RoutedUICommand(
                "Добавить похищенное ТМЦ", "Add_Stolen_Property", typeof(Pass_Office_Commands));
            _delete_Stolen_Property = new RoutedUICommand(
                "Удалить похищенное ТМЦ", "Delete_Stolen_Property", typeof(Pass_Office_Commands));
            _save_Stolen_Property = new RoutedUICommand(
                "Сохранить похищенное ТМЦ", "Save_Stolen_Property", typeof(Pass_Office_Commands));
            _add_Punishment = new RoutedUICommand("Добавить взыскание", "Add_Punishment", typeof(Pass_Office_Commands));
            _delete_Punishment = new RoutedUICommand("Удалить взыскание", "Delete_Punishment", typeof(Pass_Office_Commands));
            _save_Punishment = new RoutedUICommand("Сохранить взыскание", "Save_Punishment", typeof(Pass_Office_Commands));
            _cancel_Punishment = new RoutedUICommand("Отменить изменения взыскания", "Cancel_Punishment", typeof(Pass_Office_Commands));
            _add_List_Punishment = new RoutedUICommand("Добавить взыскание", "Add_List_Punishment", typeof(Pass_Office_Commands));
            _delete_List_Punishment = new RoutedUICommand("Удалить взыскание", "Delete_List_Punishment", typeof(Pass_Office_Commands));
            _save_List_Punishment = new RoutedUICommand("Сохранить изменения", "Save_List_Punishment", typeof(Pass_Office_Commands));
            _cancel_List_Punishment = new RoutedUICommand("Отменить изменения", "Cancel_List_Punishment", typeof(Pass_Office_Commands));
            _add_Other_Violator = new RoutedUICommand("Добавить введенные данные", "Add_Other_Violator", typeof(Pass_Office_Commands));
            _delete_Other_Violator = new RoutedUICommand("Удалить запись", "Delete_Other_Violator", typeof(Pass_Office_Commands));
            _selectEmp = new RoutedUICommand(
                "Выбрать сотрудника", "SelectEmp", typeof(Pass_Office_Commands));
            _printViolationByPeriod = new RoutedUICommand(
                "Сводка нарушителей за период", "PrintViolationByPeriod", typeof(Pass_Office_Commands));
            _summaryDataOfTheEmployee = new RoutedUICommand(
                "Сводные данные по работнику", "SummaryDataOfTheEmployee", typeof(Pass_Office_Commands));
            _disciplineDisturbers = new RoutedUICommand(
                "Нарушители трудовой дисциплины", "DisciplineDisturbers", typeof(Pass_Office_Commands));
            _select_Chief_Violator = new RoutedUICommand("Выбрать руководителя нарушителя", "Select_Chief_Violator", typeof(Pass_Office_Commands));
            _clear_Chief_Violator = new RoutedUICommand("Убрать руководителя нарушителя", "Clear_Chief_Violator", typeof(Pass_Office_Commands));
        }

        public static RoutedUICommand Add_Violation
        {
            get { return Pass_Office_Commands._add_Violation; }
            set { Pass_Office_Commands._add_Violation = value; }
        }

        public static RoutedUICommand Edit_Violation
        {
            get { return Pass_Office_Commands._edit_Violation; }
            set { Pass_Office_Commands._edit_Violation = value; }
        }

        public static RoutedUICommand Find_Violator
        {
            get { return Pass_Office_Commands._find_Violator; }
            set { Pass_Office_Commands._find_Violator = value; }
        }

        public static RoutedUICommand Save_Violation
        {
            get { return Pass_Office_Commands._save_Violation; }
            set { Pass_Office_Commands._save_Violation = value; }
        }

        public static RoutedUICommand Add_Stolen_Property
        {
            get { return Pass_Office_Commands._add_Stolen_Property; }
            set { Pass_Office_Commands._add_Stolen_Property = value; }
        }

        public static RoutedUICommand Delete_Stolen_Property
        {
            get { return Pass_Office_Commands._delete_Stolen_Property; }
            set { Pass_Office_Commands._delete_Stolen_Property = value; }
        }

        public static RoutedUICommand Save_Stolen_Property
        {
            get { return Pass_Office_Commands._save_Stolen_Property; }
            set { Pass_Office_Commands._save_Stolen_Property = value; }
        }
        
        public static RoutedUICommand Add_Punishment
        {
            get { return Pass_Office_Commands._add_Punishment; }
            set { Pass_Office_Commands._add_Punishment = value; }
        }

        public static RoutedUICommand Delete_Punishment
        {
            get { return Pass_Office_Commands._delete_Punishment; }
            set { Pass_Office_Commands._delete_Punishment = value; }
        }

        public static RoutedUICommand Save_Punishment
        {
            get { return Pass_Office_Commands._save_Punishment; }
            set { Pass_Office_Commands._save_Punishment = value; }
        }

        public static RoutedUICommand Cancel_Punishment
        {
            get { return Pass_Office_Commands._cancel_Punishment; }
            set { Pass_Office_Commands._cancel_Punishment = value; }
        }

        public static RoutedUICommand Add_List_Punishment
        {
            get { return Pass_Office_Commands._add_List_Punishment; }
            set { Pass_Office_Commands._add_List_Punishment = value; }
        }

        public static RoutedUICommand Delete_List_Punishment
        {
            get { return Pass_Office_Commands._delete_List_Punishment; }
            set { Pass_Office_Commands._delete_List_Punishment = value; }
        }

        public static RoutedUICommand Save_List_Punishment
        {
            get { return Pass_Office_Commands._save_List_Punishment; }
            set { Pass_Office_Commands._save_List_Punishment = value; }
        }

        public static RoutedUICommand Cancel_List_Punishment
        {
            get { return Pass_Office_Commands._cancel_List_Punishment; }
            set { Pass_Office_Commands._cancel_List_Punishment = value; }
        }

        public static RoutedUICommand Add_Other_Violator
        {
            get { return Pass_Office_Commands._add_Other_Violator; }
            set { Pass_Office_Commands._add_Other_Violator = value; }
        }

        public static RoutedUICommand SelectEmp
        {
            get { return Pass_Office_Commands._selectEmp; }
            set { Pass_Office_Commands._selectEmp = value; }
        }

        public static RoutedUICommand Delete_Violation
        {
            get { return Pass_Office_Commands._delete_Violation; }
            set { Pass_Office_Commands._delete_Violation = value; }
        }

        public static RoutedUICommand PrintViolationByPeriod
        {
            get { return Pass_Office_Commands._printViolationByPeriod; }
            set { Pass_Office_Commands._printViolationByPeriod = value; }
        }

        public static RoutedUICommand SummaryDataOfTheEmployee
        {
            get { return Pass_Office_Commands._summaryDataOfTheEmployee; }
            set { Pass_Office_Commands._summaryDataOfTheEmployee = value; }
        }

        public static RoutedUICommand DisciplineDisturbers
        {
            get { return Pass_Office_Commands._disciplineDisturbers; }
            set { Pass_Office_Commands._disciplineDisturbers = value; }
        }

        public static RoutedUICommand Select_Chief_Violator
        {
            get { return Pass_Office_Commands._select_Chief_Violator; }
            set { Pass_Office_Commands._select_Chief_Violator = value; }
        }

        public static RoutedUICommand Clear_Chief_Violator
        {
            get { return Pass_Office_Commands._clear_Chief_Violator; }
            set { Pass_Office_Commands._clear_Chief_Violator = value; }
        }

        public static RoutedUICommand Delete_Other_Violator
        {
            get { return Pass_Office_Commands._delete_Other_Violator; }
            set { Pass_Office_Commands._delete_Other_Violator = value; }
        }
    }
}
