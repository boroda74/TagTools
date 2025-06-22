using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class About : PluginWindowTemplate
    {
        public About(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowIcon = AboutIcon;
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            versionLabel.Text = PluginVersion;

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
