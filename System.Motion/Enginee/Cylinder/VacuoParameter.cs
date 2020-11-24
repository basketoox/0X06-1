using System.Windows.Forms;
namespace Motion.Enginee
{
    public partial class VacuoParameter : UserControl
    {
        private VacuoDelay m_vacuoDelay;
        public VacuoParameter()
        {
            InitializeComponent();
        }
        public VacuoParameter(VacuoDelay vacuoDelay) : this()
        {
            m_vacuoDelay = vacuoDelay;
            txtInhaleDelay.Text = (m_vacuoDelay.InhaleTime / 1000.00).ToString("0.00");
            txtBrokenDelay.Text = (m_vacuoDelay.BrokenTime / 1000.00).ToString("0.00");
            txtAlarmDelay.Text = (m_vacuoDelay.AlarmTime / 1000.00).ToString("0.00");
        }
        public string Name
        {
            set
            {
                gbxName.Text = value;
            }
        }
        public VacuoDelay Save
        {
            get
            {
                m_vacuoDelay.InhaleTime = (int)(double.Parse(txtInhaleDelay.Text) * 1000);
                m_vacuoDelay.BrokenTime = (int)(double.Parse(txtBrokenDelay.Text) * 1000);
                m_vacuoDelay.AlarmTime = (int)(double.Parse(txtAlarmDelay.Text) * 1000);
                return m_vacuoDelay;
            }
        }
    }
}
