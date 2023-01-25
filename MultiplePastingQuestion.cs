using System;


namespace MusicBeePlugin
{
    public partial class MultiplePastingQuestion : PluginWindowTemplate
    {
        private int _fileTagsLength = 0;
        private int _filesLength = 0;
        public bool PasteAnyway = false;

        public MultiplePastingQuestion()
        {
            InitializeComponent();
        }

        public MultiplePastingQuestion(Plugin tagToolsPluginParam, int fileTagsLength, int filesLength)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;
            _fileTagsLength = fileTagsLength;
            _filesLength = filesLength;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            label1.Text = Plugin.MsgNumberOfTracksInClipboard + _fileTagsLength + Plugin.MsgDoesntCorrespondToNumberOfSelectedTracksC + _filesLength;
            label1.Text += Plugin.MsgMessageEndC + Plugin.MsgDoYouWantToPasteAnyway;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            PasteAnyway = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
