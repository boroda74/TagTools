namespace MusicBeePlugin
{
    public partial class PluginSampleWindow : PluginWindowTemplate
    {
        internal PluginSampleWindow(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        internal float getButtonHeightDpiFontScaling()
        {
            return squareButton.Height / 23f;
        }
    }
}

