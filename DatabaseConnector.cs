using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Odbc; 
using System.Data.SqlClient;
namespace ETL
{
    public class DatabaseConnector
    {
        private string ConnectionStr { set; get; }
        //private string DataSource{ set; get; } 
        private string DatabaseName { set; get; }
        private string Server { set; get; }
        private string UID { set; get; }
        private string Passwrod { set; get; }
        private SqlConnection MyConnection;
        public DatabaseConnector(){

        }
        public DatabaseConnector(string server, string database, string uid, string password)
        {
            ConnectionStr ="SERVER=" + server + ";" + "DATABASE=" +database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            MyConnection = new SqlConnection(ConnectionStr);
        }
        public virtual int OpenConnection()
        {

            try
            {
                MyConnection.Open();
                return 1;
            }
            catch (SqlException ex)
            {
                // ex.Message
                Console.WriteLine("****my error"+ex.Message);
                return ex.Number;
            }
           
        }

        public int closeConnection()
        {

            MyConnection.Close();

            return 0;
        }

        public virtual List<string> executeQuerySql(string query)
        {
            List<string> table = new List<string>();
            MyConnection.Open();

            SqlCommand myCommand = new SqlCommand(query, MyConnection);
            SqlDataReader reader = myCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}",
                        reader[0]));
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }

            MyConnection.Close();
            return table;

        }

        public virtual string GetTablesSql()
        {

            string sql = "show databases;";
            return sql;
        
        }

        public virtual int TransformTable(SqlConnection sConnection, SqlConnection dConnection, string sTableName, string dTableName)
        {
            return 0;
        }
         

    }
}
