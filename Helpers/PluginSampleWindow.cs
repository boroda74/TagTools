namespace MusicBeePlugin
{
    public partial class PluginSampleWindow : PluginWindowTemplate
    {
        internal PluginSampleWindow(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        internal float getButtonHeightDpiFontScaling()
        {
            return squareButton.Height / 23f;
        }
    }
}

