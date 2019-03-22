using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI
{
    public partial class Excel_to_SQL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }


        private static string constr = ConfigurationManager.ConnectionStrings["ConnLocal"].ConnectionString;

        protected void Upload(object sender, EventArgs e)
        {
            //TESTANDO CONEXÂO .... 
            //SqlConnection connect = null;
            //connect = new SqlConnection(constr);


            //// vamos criar a conexão
            //connect = new SqlConnection(constr);


            //// a conexão foi feita com sucesso?
            //try
            //{
            //    // abre a conexão e a devolve ao chamador do método
            //    connect.Open();
            //}

            //catch (Exception ee)
            //{
            //    ee.Message.ToString();
            //    throw;
            //}
            //finally
            //{
            //    connect.Close();
            //}


            //Solucion Diego
            //String _Arquivo = caminho;
            //String _StringConexao = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + caminho + ";Extended Properties='Excel 12.0 Xml;HDR=YES'";
            //System.Data.DataTable dt = null;
            //OleDbConnection olecon = new OleDbConnection(_StringConexao);
            //olecon.Open();
            //OleDbCommand oleCmd = new OleDbCommand();
            //oleCmd.Connection = olecon;
            //oleCmd.CommandType = CommandType.Text;

            //dt = olecon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //String[] excelSheets = new String[dt.Rows.Count];
            //int i = 0;
            //// Add the sheet name to the string array.
            //foreach (DataRow row in dt.Rows)
            //{
            //    excelSheets[i] = row["TABLE_NAME"].ToString();
            //    i++;
            //}
            

            try
            {
                //Upload and save the file
                string excelPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(excelPath);

                string conString = string.Empty;
                string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                        break;

                }
                conString = string.Format(conString, excelPath);
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    DataTable dtExcelData = new DataTable();

                   
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                    //string consString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name
                            sqlBulkCopy.DestinationTableName = "dbo.EFV_SAIDA";                         
                                                   
                            con.Open();
                            sqlBulkCopy.WriteToServer(dtExcelData);
                            con.Close();
                        }
                    }
                }
            }

            catch (Exception ee)
            {
                ee.Message.ToString();
                throw;
            }
            finally
            {
                //dCmd2.Dispose();
                //conn2.Close();
                //conn2.Dispose();
            }


        }

    }






}





