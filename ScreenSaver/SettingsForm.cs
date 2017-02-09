using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ScreenSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void SaveSettings()
        {
            // Create or get existing Registry subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Demo_ScreenSaver");

            key.SetValue("text", textBox.Text);
        }

        private void LoadSettings()
        {
            // Get the value stored in the Registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Demo_ScreenSaver");
            if (key == null)
                textBox.Text = "C# Screen Saver";
            else
                textBox.Text = (string)key.GetValue("text");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
