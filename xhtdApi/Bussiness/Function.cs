using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Net;

namespace xhtdApi.Bussiness
{
    public class Function
    {
        static string strConn = "Server = '45.124.94.191\\SQLEXPRESS'; uid = 'xhtd'; pwd = '1234@1234'; Database = 'XHTD'; pooling='true';connection lifetime='120';max pool size='500'; Encrypt=False";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region method checkUserNameAndPassWord
        public bool checkUserNameAndPassWord(string UserName, string PassWord)
        {
            bool tmpValue = false;
            SqlConnection sqlCon = new SqlConnection(strConn);
            try
            {
                sqlCon.Open();
                SqlCommand Cmd = sqlCon.CreateCommand();
                Cmd.CommandText = "SELECT * FROM tblAccount WHERE UserName = @UserName AND PassWord = @PassWord AND State = 1";
                Cmd.Parameters.Add("UserName", SqlDbType.NVarChar).Value = UserName;
                Cmd.Parameters.Add("PassWord", SqlDbType.NVarChar).Value = PassWord;
                SqlDataReader Rd = Cmd.ExecuteReader();
                while (Rd.Read())
                {
                    tmpValue = true;
                }
                Rd.Close();
            }
            catch (Exception Ex)
            {
                log.Info(Ex.Message);
                tmpValue = false;
            }
            finally
            {
                sqlCon.Close();
                sqlCon.Dispose();
            }
            return tmpValue;
        }
        #endregion
    }
}
