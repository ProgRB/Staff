using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess;
using Staff;
using System.Reflection;
using Elegant.Ui;
using LibraryKadr;
using StaffReports;
using System.IO;
using EditorsLibrary;
using System.Collections;
namespace Kadr
{
    public static class ControlAccess
    {
        private static HashSet<string> componentName ;
        /// <summary>
        /// Функция, которая делает элементы 
        /// управления доступными
        /// </summary>
        /// <param name="control"></param>
        /// 
        public static void EnableByRules(this System.Windows.Forms.Control control)
        {
            //Если не заполнино то заполняем
            if (componentName == null)
            {
                componentName = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
                USER_ROLES_seq userRoles = new USER_ROLES_seq(Connect.CurConnect);
                string sql = string.Format(@" where APP_NAME = 'KADR' 
                        and (role_name in (select granted_role from user_role_privs) 
                            or role_name in (select granted_role from role_role_privs) or role_name = upper(trim(ora_login_user)))
                    order by COMPONENT_NAME");
                userRoles.Fill(sql);
                foreach (USER_ROLES_obj role in userRoles)
                {
                    componentName.Add(role.COMPONENT_NAME);
                }
            }
            //начинаем обход
            if (componentName != null && componentName.Count != 0) //Проверяем наличее прав
                EnableByRule(control, componentName);
            else
                MessageBox.Show("У вас нет прав доступа в \"АСУ Кадры\", обратитесь к администраторам");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="connection"></param>
        /// <param name="ChildControls">Проверять ли дочерние элементы функцией EnabledByRoles</param>
        public static void EnableByRules(this System.Windows.Forms.Control control, bool Lock_NON_USE_ChildControls)
        {
            //Если не заполнино то заполняем
            if (componentName == null)
            {
                componentName = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase) { };
                USER_ROLES_seq userRoles = new USER_ROLES_seq(Connect.CurConnect);
                string sql = string.Format(" where role_name in (select granted_role from user_role_privs) or role_name in (select granted_role from role_role_privs) or upper(trim(role_name)) = upper(trim(ora_login_user))");
                userRoles.Fill(sql);
                foreach (USER_ROLES_obj role in userRoles)
                {
                    componentName.Add(role.COMPONENT_NAME);
                }
            }
            //начинаем обход
            if (componentName.Count != 0) //Проверяем наличее прав
                EnableByRule(control,Lock_NON_USE_ChildControls, componentName);
            else
                MessageBox.Show("У вас нет прав доступа в \"АСУ Кадры\", обратитесь к администраторам");
        }
        
        private static void EnableByRule(System.Windows.Forms.Control control,HashSet<string> namesOfControls)
        {
            if (control != null)
            {
                //Если это один из перечисленных типов, то делаем его активным
                if (control is RibbonGroup || control is Ribbon || control is System.Windows.Forms.Panel || control is Elegant.Ui.Panel || control is GroupBox || control.Name == string.Empty || control is System.Windows.Forms.TabPage || control is System.Windows.Forms.Label || control is Form ||
                    control is DataGridView || control is ToolStrip)
                {
                    control.Enabled = true;
                }
                else //Если есть в массиве, то делаем активным
                {
                    if (namesOfControls.Contains(control.Name))
                    {
                        control.Enabled = false;
                        control.Enabled = true;
                    }
                    else//Если нет, то делаем неактивным
                    {
                        control.Enabled = false;
                        if (control is System.Windows.Forms.ComboBox) //если это ComboBox, то делаем фон белым
                        {
                            control.BackColor = Color.White;
                            ((System.Windows.Forms.ComboBox)control).DropDownStyle = ComboBoxStyle.DropDownList;
                        }
                    }
                }
                //Крутим дерево
                //Если есть элементы управления в элементе управления
                if (control.Controls.Count != 0 && !(control is DateEditor) && !(control is DataGridView))
                {
                    foreach (System.Windows.Forms.Control component in control.Controls)
                    {
                        EnableByRule(component, namesOfControls);
                    }
                }//если это DropDown
                else if (control is Elegant.Ui.DropDown)
                {
                    Elegant.Ui.DropDown dropDown = (Elegant.Ui.DropDown)control;
                    //Если это Elegant.Ui.PopupMenu, то продолжаем
                    if (dropDown.Popup is Elegant.Ui.PopupMenu)
                    {
                        Elegant.Ui.PopupMenu popupMenu = (Elegant.Ui.PopupMenu)dropDown.Popup;
                        foreach (System.Windows.Forms.Control popControl in popupMenu.Items)
                        {
                            EnableByRule(popControl, namesOfControls);
                        }
                    }
                }
                else if (control is ToolStrip)
                {
                    ToolStrip toolStrip = (ToolStrip)control;
                    foreach (ToolStripItem button in toolStrip.Items)
                    {
                        EnableByRule(button, namesOfControls);
                    }
                }
            }
        }

        private static bool EnableByRule(object control,bool ChildControls, HashSet<string> namesOfControls)
        {
            if (!(bool)control.GetType().GetProperty("Enabled").GetValue(control,null)) return false;
            else
            {
                bool fl=false;
                fl = namesOfControls.Contains( (string)control.GetType().GetProperty("Name").GetValue(control,null));
                HashSet<string> l = new HashSet<string>();
                string cur_fields="";
                object control1 = control;
                if (control is ToolStrip) cur_fields = "Items";
                else
                    if (control is Elegant.Ui.DropDown && ((DropDown)control).Popup is PopupMenu) {cur_fields = "Items";control1=((DropDown)control).Popup;}
                    else
                        if (control is System.Windows.Forms.Control) cur_fields = "Controls";
                        else
                            if (control is System.Windows.Forms.ToolStripDropDownItem)
                                cur_fields = "DropDownItems";
                            else cur_fields = "Items";
                if (control1.GetType().GetProperty(cur_fields)!=null)
                    foreach (object c in (IEnumerable)control1.GetType().GetProperty(cur_fields).GetValue(control1, null))
                    {
                        if (!(bool)c.GetType().GetProperty("Enabled").GetValue(c,null)) l.Add(c.GetType().GetProperty("Name").GetValue(c,null).ToString());
                        if (EnableByRule(c, ChildControls, namesOfControls)) 
                            fl = true;
                    }
                if (!fl && control1.GetType().GetProperty(cur_fields)!=null)
                {
                    foreach (object c in (IEnumerable)control1.GetType().GetProperty(cur_fields).GetValue(control1, null))
                        c.GetType().GetProperty("Enabled").SetValue(c,!l.Contains(c.GetType().GetProperty("Name").GetValue(c,null).ToString()),null);  
                }
                control.GetType().GetProperty("Enabled").SetValue(control, fl, null);
                return fl;
            }
        }

        private static void EnableByRule(System.Windows.Forms.ToolStripItem control, HashSet<string> namesOfControls)
        {
            if (control != null)
            {
                control.Enabled = namesOfControls.Contains(control.Name);
                //Если это один из перечисленных типов, то делаем его активным
                /*Array array = namesOfControls.Where(i => i.Trim() == control.Name).ToArray();
                if (array.Length > 0)
                {
                    control.Enabled = true;
                }
                else //Если есть в массиве, то делаем активным
                {
                    control.Enabled = false;
                }*/
            }
        }

        public static bool GetAccess(object e)
        {
            PropertyInfo p;
            return (p = e.GetType().GetProperty("Name")) != null && componentName.Contains(p.GetValue(e, null).ToString());
        }
        
        public static bool GetState(string ControlName)
        {
            return componentName.Contains(ControlName.ToUpper());
        }
    }
}
