namespace MusicBeePlugin
{
    public partial class PluginSampleWindow : PluginWindowTemplate
    {
        internal PluginSampleWindow(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        //Returns: button height DPI/font scaling factor
        internal (float, float) getButtonHeightDpiFontScaling()
        {
            return (squareButton.Height / 23f, textBox.Height / 23F);
        }
    }
}

