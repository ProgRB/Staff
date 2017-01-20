using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.IO;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Salary;
using LibraryKadr;
namespace Staff
{
    /// <summary>
    /// ����� "���������� ����������"
    /// </summary>
    public class EmployeePhoto
    {
        /// <summary>
        /// �������, ������� ���������� ����������
        /// ����������, � ������ ���� � ���������� ��� 
        /// ���������� ������������ �������� null
        /// </summary>
        /// <param name="per_num">��������� ����� ����������</param>
        /// 
        /// <returns>���������� ����������</returns>
        public static Image GetPhoto(string per_num)
        {
            string nameOfScheme = DataSourceScheme.SchemeName;
            string textBlock = string.Format(
            @"declare 
               hasPhoto number; 
              begin 
                :photo := null;
                select count(*) into hasPhoto from {0}.emp where per_num = :p_per_num; 
                if hasPhoto = 1 then
                 select photo into :photo from {0}.emp where per_num = :p_per_num;
                end if;
              end;", nameOfScheme);
            OracleCommand command = new OracleCommand(textBlock, Connect.CurConnect);
            command.BindByName = true;
            command.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            OracleParameter paremeter = new OracleParameter();
            paremeter.ParameterName = "photo";
            paremeter.OracleDbType = OracleDbType.Blob;
            paremeter.Direction = System.Data.ParameterDirection.Output;
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.Add(paremeter);
            command.ExecuteNonQuery();
            if (!(paremeter.Value as OracleBlob).IsNull)
                return Image.FromStream(new MemoryStream((byte[])(paremeter.Value as OracleBlob).Value));
            else
                return null;
        }
        /// <summary>
        /// ������� ��������� ����������
        /// </summary>
        /// <param name="per_num">��������� ����� ����������</param>
        /// <param name="connection">����������</param>
        /// <param name="imagePath">���� � �����</param>
        public static void SetPhoto(string per_num, OracleConnection connection, string imagePath)
        {
            string nameOfScheme = DataSourceScheme.SchemeName;
            string textBlock = string.Format("begin {0}.Set_Emp_Photo(:p_per_num,:photo); end;", nameOfScheme);
            OracleCommand command = new OracleCommand(textBlock, connection);
            command.BindByName = true;
            command.Parameters.Add("p_per_num", OracleDbType.Varchar2).Value = per_num;
            command.CommandType = System.Data.CommandType.Text;
            OracleParameter paremeter = new OracleParameter();
            paremeter.ParameterName = "photo";
            paremeter.Value = File.ReadAllBytes(imagePath);
            paremeter.OracleDbType = OracleDbType.Blob;
            paremeter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(paremeter);
            command.ExecuteNonQuery();
        }
        /// <summary>
        /// ������� ��������� ���������� ���������� ���������
        /// </summary>
        /// <param name="perco_sync_id">���������� �������������</param>
        /// <param name="connection">���������� � oracle</param>
        /// <returns>���������� ����������</returns>
        public static Image GetForeignEmpPhoto(decimal perco_sync_id, OracleConnection connection)
        {
            string nameOfScheme = DataSourceScheme.SchemeName;
            string textBlock = string.Format(
            " declare " +
            " hasPhoto number; " +
            " begin " +
            " select count(*) into hasPhoto from {0}.fr_emp where perco_sync_id = :p_perco_sync_id; " +
            "   if hasPhoto = 1 then" +
            "       select fr_photo into :fr_photo from {0}.fr_emp where perco_sync_id = :p_perco_sync_id; " +
            "   end if;" +
            " end;", nameOfScheme);
            OracleCommand command = new OracleCommand(textBlock, connection);
            command.BindByName = true;
            command.Parameters.Add("p_perco_sync_id", OracleDbType.Decimal).Value = perco_sync_id;
            OracleParameter paremeter = new OracleParameter();
            paremeter.ParameterName = "fr_photo";
            paremeter.OracleDbType = OracleDbType.Blob;
            paremeter.Direction = System.Data.ParameterDirection.Output;
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.Add(paremeter);
            command.ExecuteNonQuery();
            if (!(paremeter.Value as OracleBlob).IsNull)
                return Image.FromStream(new MemoryStream((byte[])(paremeter.Value as OracleBlob).Value));
            else
                return null;
            //if (!(paremeter.Value as OracleBlob).IsNull )
            //    return Image.FromStream(new MemoryStream((byte[])paremeter.Value));
            //else
            //    return null;
        }
        /// <summary>
        /// ��������� ���������� ���������� �������� ���������
        /// </summary>
        /// <param name="perco_sync_id">���������� ������������� �������� ����������</param>
        /// <param name="connection">���������� � Oracle</param>
        /// <param name="imagePath">���� � ����������</param>
        public static void SetForeignPhoto(decimal perco_sync_id, OracleConnection connection, string imagePath)
        {
            string nameOfScheme = DataSourceScheme.SchemeName;
            string textBlock = string.Format("begin {0}.Set_FR_Emp_Photo(:p_perco_sync_id,:fr_photo); end;", 
                nameOfScheme);
            OracleCommand command = new OracleCommand(textBlock, connection);
            command.BindByName = true;
            command.Parameters.Add("p_perco_sync_id", OracleDbType.Decimal).Value = perco_sync_id;
            command.CommandType = System.Data.CommandType.Text;
            OracleParameter paremeter = new OracleParameter();
            paremeter.ParameterName = "fr_photo";
            paremeter.Value = File.ReadAllBytes(imagePath);
            paremeter.OracleDbType = OracleDbType.Blob;
            paremeter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(paremeter);
            command.ExecuteNonQuery();
        }

    }
}