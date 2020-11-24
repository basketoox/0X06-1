using System;
using System.Drawing;
using System.Windows.Forms;

namespace Motion.Enginee
{
    public partial class CylinderOperate : UserControl
    {
        private readonly Action _manualOperate;
        public CylinderOperate()
        {
            InitializeComponent();
        }
        public CylinderOperate(Action ManualOperate) : this()
        {
            _manualOperate = ManualOperate;
        }
        public string CylinderName
        {
            set
            {
                btn.Text = value;
            }
        }
        /// <summary>
        /// 无原点屏蔽
        /// </summary>
        public bool NoOriginShield { get; set; }
        /// <summary>
        /// 无动点屏蔽
        /// </summary>
        public bool NoMoveShield { get; set; }
        public bool InOrigin
        {
            set
            {
                if (value)
                    picOrigin.BackColor = NoOriginShield? Color.White : Color.Red;
                else
                    picOrigin.BackColor = NoOriginShield ? Color.White : Color.Gray;
            }
        }
        public bool InMove
        {
            set
            {
                if (value)
                    picMove.BackColor = NoOriginShield ? Color.White : Color.Red;
                else
                    picMove.BackColor = NoOriginShield ? Color.White : Color.Gray;
            }
        }
        public bool OutMove
        {
            set
            {
                btn.BackColor = value ? Color.PaleGreen : Color.Transparent;
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            _manualOperate();
        }
    }
}
