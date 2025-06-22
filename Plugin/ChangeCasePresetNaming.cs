using System;

using ExtensionMethods;

using static MusicBeePlugin.AdvancedSearchAndReplace;
using static MusicBeePlugin.ChangeCase;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class ChangeCasePresetNaming : PluginWindowTemplate
    {
        private CustomComboBox languagesCustom;


        private ChangeCasePreset preset;
        private string currentLanguage;

        private bool settingsSaved;

        public ChangeCasePresetNaming(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowIcon = ChangeCaseIcon;

            new ControlBorder(this.nameBox);
            new ControlBorder(this.descriptionBox);
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            languagesCustom = namesComboBoxes["languages"];
            currentLanguage = null;

            var englishIsAvailable = false;
            var nativeLanguageIsAvailable = false;

            foreach (var language in preset.names.Keys)
            {
                languagesCustom.Add(language);

                if (language == Language)
                    nativeLanguageIsAvailable = true;

                if (language == "en")
                    englishIsAvailable = true;
            }

            if (!nativeLanguageIsAvailable)
                languagesCustom.Add(Language);

            if (!englishIsAvailable)
                languagesCustom.Add("en");

            if (nativeLanguageIsAvailable)
                currentLanguage = Language;
            else if (englishIsAvailable)
                currentLanguage = "en";
            else
                currentLanguage = languagesCustom.Items[0] as string;

            nameBox.Text = GetDictValue(preset.names, currentLanguage);
            descriptionBox.Text = GetDictValue(preset.descriptions, currentLanguage);

            languagesCustom.SelectedItem = currentLanguage;
            languagesCustom.Text = Language;

            descriptionBox.TextChanged += descriptionBox_TextChanged;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        internal bool editPreset(ChangeCasePreset preset)
        {
            this.preset = preset;

            Display(this, true);

            return settingsSaved;
        }

        private bool saveSettings()
        {
            SetDictValue(preset.names, currentLanguage, nameBox.Text);
            SetDictValue(preset.descriptions, currentLanguage, descriptionBox.Text);

            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            settingsSaved = saveSettings();
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            settingsSaved = false;
            Close();
        }

        private void languages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentLanguage == null || currentLanguage == languagesCustom.SelectedItem as string)
                return;


            var prevName = nameBox.Text;
            var prevDescription = descriptionBox.Text;

            SetDictValue(preset.names, currentLanguage, prevName);
            SetDictValue(preset.descriptions, currentLanguage, prevDescription);


            currentLanguage = languagesCustom.SelectedItem as string;
            nameBox.Text = GetDictValue(preset.names, currentLanguage);
            descriptionBox.Text = GetDictValue(preset.descriptions, currentLanguage);

            if (string.IsNullOrEmpty(nameBox.Text))
                nameBox.Text = prevName;

            if (string.IsNullOrEmpty(descriptionBox.Text))
                descriptionBox.Text = prevDescription;
        }

        private void descriptionBox_TextChanged(object sender, EventArgs e)
        {
            updateCustomScrollBars(descriptionBox);
            descriptionBox.ScrollToCaret();
        }

        private void ChangeCasePresetNaming_Load(object sender, EventArgs e)
        {
            updateCustomScrollBars(descriptionBox);
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameBox.Text))
                buttonOK.Enable(false);
            else
                buttonOK.Enable(true);
        }
    }
}
