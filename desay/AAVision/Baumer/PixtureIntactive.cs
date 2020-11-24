using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace desay
{
   public  class PixtureIntactive
    {

        private PictureBox _InteractWindow;

        public PictureBox InteractWindow
        {
            get { return _InteractWindow; }
            set { _InteractWindow = value; }
        }

        public PixtureIntactive()
        {
            _InteractWindow = new PictureBox();

        }



     public  void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            this._InteractWindow.Dock = DockStyle.None;
            this._InteractWindow.Anchor = AnchorStyles.None;
            if (e.Delta < 0)
            {
                _InteractWindow.Width = _InteractWindow.Width * 9 / 10;
                _InteractWindow.Height = _InteractWindow.Height * 9 / 10;
            }
            else
            {
                _InteractWindow.Width = _InteractWindow.Width * 11 / 10;
                _InteractWindow.Height = _InteractWindow.Height * 11 / 10;
            }
        }

    }
}
