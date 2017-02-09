using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Xml;
using System.IO.Compression;
using System.Resources;
using System.IO;

namespace ScreenSaver
{
    
    public partial class ScreenSaverForm : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        private Point mouseLocation;
        private Random rand = new Random();
        private bool previewMode = false;
        private XmlDocument doc = new XmlDocument();
        private GZipStream clockGZ = new GZipStream(new MemoryStream(Properties.Resources.bbc1_clock_84), CompressionMode.Decompress);

        public ScreenSaverForm()
        {
            InitializeComponent();
            LoadClock();
        }

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            LoadClock();
            this.Bounds = Bounds;
        }
        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();
            LoadClock();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            // Make text smaller
            textLabel.Font = new System.Drawing.Font("Arial", 6);

            previewMode = true;
        }

        private void LoadClock()
        {
            using(MemoryStream dest = new MemoryStream())
            {
                clockGZ.CopyTo(dest);
                doc.Load(dest);
            }
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            // Use the string from the Registry if it exists
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Demo_ScreenSaver");

            if (key == null)
                textLabel.Text = "Demo";
            else
                textLabel.Text = (string)key.GetValue("text");

            Cursor.Hide();
            TopMost = true;

            moveTimer.Interval = 1000;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {

            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    if (Math.Abs(mouseLocation.X - e.X) > 5 || Math.
                        Abs(mouseLocation.Y - e.Y) > 5)
                    {
                        Application.Exit();
                    }
                }
                mouseLocation = e.Location;
            }
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }
        
        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }

        private void moveTimer_Tick(object sender, EventArgs e)
        {
            DateTime localTime = DateTime.Now;

            textLabel.Left = rand.Next(Math.Max(1, Bounds.Width - textLabel.Width));
            textLabel.Top = rand.Next(Math.Max(1, Bounds.Height - textLabel.Height));
            textLabel.Text = localTime.ToLongTimeString();
        }
    }
}
