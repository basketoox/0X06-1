using System;
using System.Windows.Forms;

namespace Motion.Enginee
{
    public partial class MoveSelectHorizontal : UserControl
    {
        public MoveSelectHorizontal()
        {
            InitializeComponent();
            rbnContinueMoveSelect.Checked = true;
            rbnPos10um.Checked = true;
        }

        public AxisMoveMode MoveMode { get;private set; }
        private void rbnContinueMoveSelect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnContinueMoveSelect") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Continue = rbnContinueMoveSelect.Checked;
            MoveMode = mode;
        }

        private void rbnLocationMoveSelect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnLocationMoveSelect") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Continue = !rbnLocationMoveSelect.Checked;
            MoveMode = mode;
        }

        private void rbnPos10um_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPos10um") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = 0.01;
            MoveMode = mode;
        }

        private void rbnPos100um_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPos100um") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = 0.10;
            MoveMode = mode;
        }

        private void rbnPos1000um_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPos1000um") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = 1.00;
            MoveMode = mode;
        }

        private void rbnPosOtherum_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton temp = sender as RadioButton;
            if (temp == null || temp.Name != "rbnPosOtherum") return;
            var mode = new AxisMoveMode();
            mode = MoveMode;
            mode.Distance = (double)ndnPosOther.Value ;
            MoveMode = mode;
        }
    }
}
