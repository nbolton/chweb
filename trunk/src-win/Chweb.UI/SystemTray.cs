using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chweb.UI.Properties;

namespace Chweb.UI
{
    public class SystemTray
    {
        public event EventHandler ShowRequest;
        public event EventHandler ExitRequest;
        public event EventHandler SettingsRequest;
        public event EventHandler RefreshRequest;

        private NotifyIcon icon = new NotifyIcon();

        private ContextMenuStrip menu
        {
            get { return icon.ContextMenuStrip; }
        }

        public void Initialize()
        {
            icon.Icon = Resources.Chweb;
            icon.Visible = true;
            icon.ContextMenuStrip = new ContextMenuStrip();
            icon.MouseClick += new MouseEventHandler(icon_MouseClick);

            var refresh = new ToolStripMenuItem("Refresh");
            refresh.Click += new EventHandler(refresh_Click);
            menu.Items.Add(refresh);

            var settings = new ToolStripMenuItem("Settings");
            settings.Click += new EventHandler(settings_Click);
            menu.Items.Add(settings);

            var show = new ToolStripMenuItem("Show");
            show.Click += new EventHandler(show_Click);
            menu.Items.Add(show);

            var exit = new ToolStripMenuItem("Exit");
            exit.Click += new EventHandler(exit_Click);
            menu.Items.Add(exit);
        }

        void refresh_Click(object sender, EventArgs e)
        {
            OnRefreshRequest(EventArgs.Empty);
        }

        void icon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OnShowRequest(EventArgs.Empty);
            }
        }

        void settings_Click(object sender, EventArgs e)
        {
            OnSettingsRequest(EventArgs.Empty);
        }

        void exit_Click(object sender, EventArgs e)
        {
            OnExitRequest(EventArgs.Empty);
        }

        void show_Click(object sender, EventArgs e)
        {
            OnShowRequest(EventArgs.Empty);
        }

        private void OnRefreshRequest(EventArgs e)
        {
            if (RefreshRequest != null)
            {
                RefreshRequest(this, e);
            }
        }

        public void OnSettingsRequest(EventArgs e)
        {
            if (SettingsRequest != null)
            {
                SettingsRequest(this, e);
            }
        }

        public void OnShowRequest(EventArgs e)
        {
            if (ShowRequest != null)
            {
                ShowRequest(this, e);
            }
        }

        public void OnExitRequest(EventArgs e)
        {
            if (ExitRequest != null)
            {
                ExitRequest(this, e);
            }
        }

        internal void Hide()
        {
            icon.Visible = false;
        }
    }
}
