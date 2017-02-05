using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryKadr;
using Oracle.DataAccess.Client;

namespace Kadr.Ras
{
    public partial class AccountSertificate : Form
    {
        public AccountSertificate()
        {
            InitializeComponent();
        }
        public static void PrintMiddleWaterProcData(object subdiv_id,string CodeSubdiv, DateTime calc_date)
        {
            OracleCommand cmd = new OracleCommand(string.Format("begin {0}.PAYMENT.DATA_MIDDLEWATER_BY_SUB(:p_subdiv_id,:p_date_calc,:cur);end;", Connect.Schema), Connect.CurConnect);
            cmd.BindByName = true;
            cmd.Parameters.Add("p_subdiv_id", subdiv_id);
            cmd.Parameters.Add("p_date_calc", calc_date);
            cmd.Parameters.Add("cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            DataSet ds = new DataSet();
            new OracleDataAdapter(cmd).Fill(ds);
            Excel.PrintWithBorder(true, "MiddleWaterData.xlt", "A5", new DataTable[] { ds.Tables[0] },
                new ExcelParameter[]{ new ExcelParameter("E2",CodeSubdiv),new ExcelParameter("G2",calc_date.ToString("MMMM yyyy"))});
        }
    }
}
