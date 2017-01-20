using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Windows;
using Salary.Helpers;
using System.Reflection;
using System.Data.Common;
using System.Data;

namespace Salary.Helpers
{
    public static class OracleCommandHelper
    {
        public static bool TryExecuteNonQuerWithTransaction(this OracleCommand sender, OracleConnection connect, Window owner = null, bool useWaitDialog=false)
        {
            if (useWaitDialog)
            {
                bool fl = false;
                OracleTransaction tr = connect.BeginTransaction();
                AbortableBackgroundWorker.RunAsyncWithWaitDialog(owner, "Выполнение операции",
                    (p, pw) =>
                    {
                        (pw.Argument as OracleCommand).ExecuteNonQuery();
                        fl = true;
                    },
                    sender, sender,
                    (p, pw) =>
                    {
                        if (pw.Cancelled) tr.Rollback();
                        else if (pw.Error != null)
                        {
                            tr.Rollback();
                            MessageBox.Show(owner, string.Format("Ошибка выполнения операции.\n{0}", pw.Error.GetFormattedException()), "Зарплата предприятия");
                        }
                        else
                        {
                            tr.Commit();
                            fl = true;
                        }
                    });
                
                return fl;
            }
            else
            {

                OracleTransaction tr = connect.BeginTransaction();
                try
                {
                    sender.ExecuteNonQuery();
                    tr.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    MessageBox.Show(ex.GetFormattedException(), "Ошибка выполнения операции", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
        }

        /// <summary>
        /// Данное расширение автоматически проставляет команде значения из класса согласно мэппингу атрибутов
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sourceValue">Источник значений, каждое значение для параметра должно быть отмечено атрибутом [OracleParameterMapping]</param>
        public static void SetParameters(this OracleCommand cmd, object sourceValue)
        {
            if (cmd == null)
                return;
            foreach (OracleParameter c in cmd.Parameters.OfType<OracleParameter>().Where(r => r.Direction != System.Data.ParameterDirection.Output))
            {
                PropertyInfo p = sourceValue.GetType().GetProperties()
                    .Where(r => r.GetCustomAttributes(typeof(OracleParameterMapping), true)
                                .Any(r1 => (r1 as OracleParameterMapping).ParameterName.Equals(c.ParameterName, StringComparison.OrdinalIgnoreCase))
                                ).FirstOrDefault();
                if (p != null)
                {
                    c.Value = p.GetValue(sourceValue, null);
                    continue;
                }
                FieldInfo p1 = sourceValue.GetType().GetFields()
                    .Where(r => r.GetCustomAttributes(typeof(OracleParameterMapping), true)
                                .Any(r1 => (r1 as OracleParameterMapping).ParameterName.Equals(c.ParameterName, StringComparison.OrdinalIgnoreCase))
                                ).FirstOrDefault();
                if (p1 != null)
                {
                    c.Value = p1.GetValue(sourceValue);
                    continue;
                }

                MethodInfo p2 = sourceValue.GetType().GetMethods()
                    .Where(r => r.GetCustomAttributes(typeof(OracleParameterMapping), true)
                                .Any(r1 => (r1 as OracleParameterMapping).ParameterName.Equals(c.ParameterName, StringComparison.OrdinalIgnoreCase))
                                ).FirstOrDefault();
                if (p2 != null)
                {
                    c.Value = p2.Invoke(sourceValue, null);
                    continue;
                }
            }
        }
    }
    /// <summary>
    /// Атрибут для класса, помогающий установить занчения для команды Оракл
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
    public class OracleParameterMapping : Attribute
    {
        /// <summary>
        /// Имя параметра, которому требуется присвоить искомое значение
        /// </summary>
        public string ParameterName
        {
            get;
            set;
        }
    }

    public static class OracleAdapterHelper
    {
        /// <summary>
        /// Попытка обновить данные
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ds">DataSet для обновления данных</param>
        /// <param name="classWithParameters">Класс содержащий информацию о параметрамх ([OracleParameterMapping] attribute)</param>
        /// <returns></returns>
        public static Exception TryFillWithClear(this OracleDataAdapter sender, DataSet ds, object classWithParameters)
        {
            if (ds == null) ds = new DataSet();
            foreach (DataTableMapping p in sender.TableMappings)
            {
                if (ds.Tables.Contains(p.DataSetTable))
                {
                    ds.Tables[p.DataSetTable].Rows.Clear();
                }
            }
            sender.SelectCommand.SetParameters(classWithParameters);
            try
            {
                sender.Fill(ds);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
