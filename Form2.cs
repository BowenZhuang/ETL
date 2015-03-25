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
    public partial class TableForm : Form
    {
        private string sIP;
        private string sDatabase;
        private string sUserName;
        private string sPassword;
        private List<string> sDBList;
        private string sDatabaseType;
        private string dIP;
        private string dDatabase;
        private string dUserName;
        private string dPassword;
        private List<string> dDBList;
        private string dDatabaseType;

        public DatabaseConnector sDatabaseConnector{set;get;}
        public DatabaseConnector dDatabaseConnector { set; get; }
        public TableForm()
        {
            InitializeComponent();
            
            
        }



        public TableForm(string sIP, string sUserName, string sPassword, string sDatabaseType, string dIP, string dUserName, string dPassword, string dDatabaseType)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.sIP = sIP; 
            this.sUserName = sUserName;
            this.sPassword = sPassword;
            this.sDatabaseType = sDatabaseType;
            if (sDatabaseType == "MySQL")
            {
                sDatabaseConnector = new MySqlDatabaseConnector(sIP, sDatabase, sUserName, sPassword);
            }
            else
            {
                sDatabaseConnector = new DatabaseConnector(sIP, sDatabase, sUserName, sPassword);
            }

            this.dIP = dIP;
            this.dUserName = dUserName;
            this.dPassword = dPassword;
            this.dDatabaseType = dDatabaseType;
            if (dDatabaseType == "MySQL")
            {
                dDatabaseConnector = new MySqlDatabaseConnector(sIP, sDatabase, sUserName, sPassword);
            }
            else
            {
                dDatabaseConnector = new DatabaseConnector(sIP, sDatabase, sUserName, sPassword);
            }
             
        }

        public void GenerateData()
        {
            string sql = this.sDatabaseConnector.GetTablesSql();
            sDBList = sDatabaseConnector.executeQuerySql(sql);
            listBox1.DataSource = sDBList;
            ((CurrencyManager)listBox1.BindingContext[sDBList]).Refresh();
            string dSql = this.sDatabaseConnector.GetTablesSql();
            dDBList = sDatabaseConnector.executeQuerySql(dSql);
            listBox2.DataSource = dDBList;
            ((CurrencyManager)listBox2.BindingContext[dDBList]).Refresh();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!listBox2.Items.Contains(listBox1.SelectedItem.ToString()))
            {
                listBox2.Items.Add(listBox1.SelectedItem.ToString());
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                string item = listBox2.SelectedItem.ToString();
                listBox2.Items.Remove(item);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
