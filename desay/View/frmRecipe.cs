using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using log4net;
using System.Linq;
namespace desay
{
    public partial class frmRecipe : Form
    {
        static ILog log = LogManager.GetLogger(typeof(frmRecipe));
        private readonly Action m_Load;
        private readonly Action m_Save;
        public frmRecipe()
        {
            InitializeComponent();
        }
        public frmRecipe(string directoryPath,string currentProductType, Action Load = null, Action Save = null) : this()
        {
            DirectoryPath = directoryPath;
            CurrentProductType = currentProductType;
            m_Load = Load;
            m_Save = Save;
        }
        public static string DirectoryPath { get; private set; }
        public static string CurrentProductType { get; private set; }
        private void FrmRecipe_Load(object sender, EventArgs e)
        {
            RefreshFileList();
            RefreshInfo();
        }
        private void RefreshFileList()
        {
            lstProductType.Items.Clear();
            FileInfo[] files = new DirectoryInfo(DirectoryPath).GetFiles();
            foreach (FileInfo file in files)
            {
                lstProductType.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
            }
        }

        private void RefreshInfo()
        {
            txtCurrentType.Text = CurrentProductType;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            log.Info("用户点击进行机种新增操作！");
            string newType = txtTargetType.Text.Trim();
            if (string.IsNullOrEmpty(newType))
            {
                MessageBox.Show("目标型号不能为空！");
                return;
            }

            if (lstProductType.Items.Contains(newType))
            {
                MessageBox.Show("列表中已有相同型号，不能再次新增！");
                return;
            }

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (newType.Contains(c))
                {
                    MessageBox.Show($"名称中不能包含特殊字符 '{c}' 请重新命名！");
                    return;
                }
            }
            CurrentProductType = newType;
            m_Save?.Invoke();
            RefreshFileList();
            RefreshInfo();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Info("用户点击进行机种删除操作！");
            string deleteType = txtTargetType.Text.Trim();
            if (string.IsNullOrEmpty(deleteType))
            {
                MessageBox.Show("目标型号不能为空！");
                return;
            }

            if (deleteType == CurrentProductType)
            {
                MessageBox.Show("目标型号正在使用，不能删除！");
                return;
            }

            if (!lstProductType.Items.Contains(deleteType))
            {
                MessageBox.Show("列表中未找到目标，无法删除！");
                return;
            }

            if (MessageBox.Show(string.Format("是否删除型号：{0}", deleteType), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FileInfo[] files = new DirectoryInfo(DirectoryPath).GetFiles();
                foreach (FileInfo file in files)
                {
                    if (Path.GetFileNameWithoutExtension(file.Name) == deleteType)
                    {
                        file.Delete();
                        break;
                    }
                }
                RefreshFileList();
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            log.Info("用户点击进行机种切换操作！");
            string selectType = txtTargetType.Text.Trim();
            if (string.IsNullOrEmpty(selectType))
            {
                MessageBox.Show("目标型号值为空，请确认是否选中对应型号！");
                return;
            }

            if (selectType == CurrentProductType)
            {
                MessageBox.Show("目标型号与正在使用的型号一直，无需切换！");
                return;
            }

            if (MessageBox.Show($"是否保存型号 {CurrentProductType} 的数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                m_Save?.Invoke();
            }

            CurrentProductType = selectType;
            m_Load?.Invoke(); ;
            RefreshInfo();
        }
        private void lstProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTargetType.Text = lstProductType.SelectedItem.ToString();
        }
    }
}
