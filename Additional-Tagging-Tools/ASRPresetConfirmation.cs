using System;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public partial class ASRPresetConfirmation : PluginWindowTemplate
    {
        private string presetName;
        private string trackUrl;

        public bool apply = false;

        public ASRPresetConfirmation(Plugin tagToolsPluginParam, string presetNameParam, string trackUrlParam)
        {
            InitializeComponent();

            presetName = presetNameParam;
            trackUrl = trackUrlParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            label1.Text = Regex.Replace(label1.Text, "%%preset%%", presetName);
            label1.Text = Regex.Replace(label1.Text, "%%track%%", Plugin.GetTrackRepresentation(trackUrl));
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            apply = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
