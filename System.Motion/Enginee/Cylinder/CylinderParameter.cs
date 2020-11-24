using System.Windows.Forms;
namespace Motion.Enginee
{
    public partial class CylinderParameter : UserControl
    {
        private CylinderDelay m_cylinderDelay;
        public CylinderParameter()
        {
            InitializeComponent();
        }
        public CylinderParameter(CylinderDelay cylinderDelay):this()
        {
            m_cylinderDelay = cylinderDelay;
            txtOriginDelay.Text= (m_cylinderDelay.OriginTime / 1000.00).ToString("0.00");
            txtMoveDelay.Text = (m_cylinderDelay.MoveTime / 1000.00).ToString("0.00");
            txtAlarmDelay.Text= (m_cylinderDelay.AlarmTime / 1000.00).ToString("0.00");
        }
        public string Name
        {
            set
            {
                gbxName.Text = value;
            }
        }
        public CylinderDelay Save
        {
            get
            {
                m_cylinderDelay.OriginTime = (int)(double.Parse(txtOriginDelay.Text) * 1000);
                m_cylinderDelay.MoveTime = (int)(double.Parse(txtMoveDelay.Text) * 1000);
                m_cylinderDelay.AlarmTime = (int)(double.Parse(txtAlarmDelay.Text) * 1000);
                return m_cylinderDelay;
            }
        }
    }
}
