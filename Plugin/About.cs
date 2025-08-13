using System;
using System.Diagnostics;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class About : PluginWindowTemplate
    {
        public About(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowMenuIcon = AboutMenuIcon;
            WindowIcon = AboutIcon;
            WindowIconInactive = AboutIconInactive;
            TitleBarText = this.Text;
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            versionLabel.Text = PluginVersion;

            if (useSkinColors)
            {
                creditLinkLabel.DisabledLinkColor = DimmedAccentColor;
                creditLinkLabel.LinkColor = HighlightColor;
                creditLinkLabel.ActiveLinkColor = DimmedHighlightColor;

                iconSetLinkLabel.DisabledLinkColor = DimmedAccentColor;
                iconSetLinkLabel.LinkColor = HighlightColor;
                iconSetLinkLabel.ActiveLinkColor = DimmedHighlightColor;
            }

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void creditLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://getmusicbee.com/forum/index.php?action=profile;u=68772");
        }

        private void iconSetLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://getmusicbee.com/forum/index.php?topic=37522.0");
        }
    }
}
