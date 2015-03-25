using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DatabaseConnector db;
        private string sDatabase;
        private string dDatabase;
        private string sIP;
        private string dIP;
        private string sUserName;
        private string dUserName;
        private string sPassword;
        private string dPassword;
        private string sDatabaseType;
        private string dDatabaseType;

        private void testSourceButton_Click(object sender, EventArgs e)
        {
            //sDatabase = dataBaseTextBox.Text;
            //sIP = ipTb.Text;
            //sUserName = userNameTextBox.Text;
            //sPassword = passwordTextBox.Text;
            //if (sDataTypeComboBox.SelectedItem.ToString()== "MySQL")
            //{
            //    db = new MySqlDatabaseConnector(sIP, sDatabase, sUserName, sPassword);
            //}
            //else
            //{
            //    db = new DatabaseConnector(sIP, sDatabase, sUserName, sPassword);
            //}
            
            
            //int result = db.OpenConnection();
            //db.closeConnection();
            MySqlDatabaseConnector db1 = new MySqlDatabaseConnector(sIP, sDatabase, sUserName, sPassword);
            db1.TransformTable1(null,null, "","","");
            //if (result == 1)
            //{
            //    string sql = db.GetTablesSql();
            //    List <string> tables = db.executeQuerySql(sql);
            //    foreach (string name in tables){
            //        Console.WriteLine(name);
            //    }
            //}
            //else
            //{
                
            //}
            
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
           
            sIP = ipTb.Text;
            sUserName = userNameTextBox.Text;
            sPassword = passwordTextBox.Text;
            sDatabaseType = sDataTypeComboBox.SelectedItem.ToString();
            dIP = dIPTextBox.Text;
            dUserName = dUserNameTextBox.Text;
            dPassword = dPasswordtextBox.Text;
            dDatabaseType = dDataTypeComboBox.SelectedItem.ToString();
            TableForm tableForm = new TableForm(sIP, sUserName, sPassword, sDatabaseType,  dIP,  dUserName,  dPassword,  dDatabaseType);
            //tableForm.DatabaseConnector = this.db;
            tableForm.GenerateData();
            tableForm.Show();
           
        }

        
    }
}
