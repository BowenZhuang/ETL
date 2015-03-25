using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data; 

namespace ETL
{
    class MySqlDatabaseConnector : DatabaseConnector
    {
        private string ConnectionStr { set; get; }
        //private string DataSource{ set; get; } 
        private string DatabaseName { set; get; }
        private string Server { set; get; }
        private string UID { set; get; }
        private string Passwrod { set; get; }
        private MySqlConnection MyConnection;
        private TableForm tableForm;
        private string DConnectionStr { set; get; }
        public MySqlDatabaseConnector()
        {

        }
        public MySqlDatabaseConnector(string server, string database, string uid, string password)
        {
            this.ConnectionStr ="SERVER=" + server + ";" + "DATABASE=" +database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            this.DatabaseName = database;
            this.Server = server;
            this.UID = uid;
            this.Passwrod = password;
            this.MyConnection = new MySqlConnection(ConnectionStr);
        }
        public override int OpenConnection()
        {

            try
            {
                MyConnection.Open();
                return 1;
            }
            catch (MySqlException ex)
            {
                // ex.Message
                Console.WriteLine("****my error"+ex.Message);
                return ex.Number;
            }
           
        }

        public override List<string> executeQuerySql(string query)
        {
            MyConnection.Open();
            MySqlCommand myCommand = new MySqlCommand(query, MyConnection);
            MySqlDataReader reader = myCommand.ExecuteReader();
            List<string> tableName = new List<string>();
            try
            {
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}",
                        reader[0]));
                    string tablename = reader[0].ToString();
                    tableName.Add(tablename);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
            MyConnection.Close();
            return tableName;

        }

        public override string GetTablesSql()
        {

            string sql = "show databases;";
            return sql;
        
        }

         public void getSchema()
        {
            this.MyConnection.Open();
           DataTable table =  MyConnection.GetSchema("categories");

           MySqlBulkLoader bulkCopy = new MySqlBulkLoader(MyConnection);
            foreach (DataColumn col in table.Columns)
            {
                string colName = col.ColumnName;
                Type type = col.DataType;
                
            }
            //bulkCopy.BulkCopyTimeout = 600;
            
        }

         public  int TransformTable1(MySqlConnection sConnection, MySqlConnection dConnection, string sTableName, string dTableName,string dDataBase)
         {
             sTableName = "categories";
             sConnection = new MySqlConnection("SERVER=127.0.0.1;" + "DATABASE=northwind;UID=root;" + "PASSWORD=1234;");
             sConnection.Open();
             String selectsql1 = "select * from " + sTableName + ";";
             MySqlDataAdapter sdataAdapter = new MySqlDataAdapter(selectsql1, sConnection);
             DataSet stable = new DataSet();
                 sdataAdapter.Fill(stable, sTableName);
                 //stable.AcceptChanges();
                 foreach (DataRow pRow in stable.Tables[sTableName].Rows)
                 {
                    
                     pRow.SetAdded();
                 }
             string query = "Desc " + sTableName;
             MySqlCommand myCommand = new MySqlCommand(query, sConnection);
             MySqlDataReader reader = myCommand.ExecuteReader();
             List<string> field = new List<string>();
             List<string> type = new List<string>();
             List<string> key = new List<string>();

             try
             {
                 while (reader.Read())
                 {
                     Console.WriteLine(String.Format("{0}",
                         reader[0]));
                     string colname = reader[0].ToString();
                     string coltype = reader[1].ToString();
                     string pri = reader[3].ToString();
                     field.Add(colname);
                     type.Add(coltype);
                     key.Add(pri);
                 }
             }
             finally
             {
                 // Always call Close when done reading.
                 reader.Close();
             }
             sConnection.Close();
             dConnection = new MySqlConnection("SERVER=10.113.21.23;" + "DATABASE=test;UID=root;" + "PASSWORD=Conestoga1;");
             dConnection.Open();
             String selectsql = "SELECT * FROM INFORMATION_SCHEMA.Tables where table_name ='" + sTableName + "';";
            
             //dataAdapter.Fill(stable);
             //dataAdapter.Update(stable,sTableName);
             MySqlCommand chectDatabaseCommand = new MySqlCommand(selectsql, dConnection);
             MySqlDataReader checkDatabaseReader = chectDatabaseCommand.ExecuteReader();
             int count = 0;
             try
             {
                 while (checkDatabaseReader.Read())
                 {
                     Console.WriteLine(String.Format("{0}",
                         checkDatabaseReader[0]));
                     count++;
                 }
             }
             finally
             {
                 // Always call Close when done reading.
                 checkDatabaseReader.Close();
             }
             

             if (count <= 0)
             {
             //create table
             string insertTable = "Create table " + sTableName + " ( ";
             int i = 0; 
            foreach(string colname in field )  
            {
                if (key[i].Contains("PRI"))
                {
                    insertTable = insertTable + colname + " " + type[i] + " NOT NULL PRIMARY KEY  , ";
                }
                else
                {
                    insertTable = insertTable + colname + " " + type[i] + ", ";
                }
                
                i++;
            }
            insertTable = insertTable.Substring(0, insertTable.Length - 2) +");";
            MySqlCommand insertCommand = new MySqlCommand(insertTable, dConnection);
            insertCommand.ExecuteNonQuery();
             
            }
             MySqlDataAdapter dataAdapter = new MySqlDataAdapter(selectsql1, dConnection);
             MySqlCommandBuilder builder = new MySqlCommandBuilder(dataAdapter);

             // add rows to dataset

            dataAdapter.InsertCommand = builder.GetInsertCommand();
            //dataAdapter.UpdateCommand = builder.GetUpdateCommand();
            DataTable dataTable = stable.Tables[sTableName];
            foreach (DataColumn data in dataTable.Columns)
            {
                Console.WriteLine("***"+data.ColumnName);
            }
            dataAdapter.Update(stable.Tables[sTableName]);
            dConnection.Close();
            return 0;
         }

    }
}
