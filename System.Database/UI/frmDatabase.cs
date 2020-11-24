using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace System.Database.UI
{
    public partial class frmDatabase<T>: Form where T:new()
    {
        public frmDatabase()
        {
            InitializeComponent();
            DB.BeginTransaction();
        }
        Type m_type=typeof(T);
        DBHelper DB = new DBHelper();
        private List<T> m_stuList = new List<T>();

        private void frmDatabase_Load(object sender, EventArgs e)
        {
            try
            {
                m_stuList = DB.FindAll<T>();
                dataGridView1.DataSource = m_stuList;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
