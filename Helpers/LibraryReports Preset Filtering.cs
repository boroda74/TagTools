using System;
using System.Collections.Generic;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;
using MusicBeePlugin.Properties;

namespace MusicBeePlugin
{
    partial class LibraryReports
    {
        #region Finding related presets
        private static bool ExcludePresetFromChain(ReportPreset selected, ReportPreset current)
        {
            if (selected == current)
                return false;

            ReportPreset next = current.anotherPresetAsSource.findPreset();

            if (next != null)
                return ExcludePresetFromChain(selected, next);

            return true;
        }

        //ON CHAGING SELECTED PRESET:
        //  initPreset()
        private void findFilteringPresetsUI(CustomComboBox foundPresetRefs, ReportPreset referringPreset, ReportPresetReference referredReference) //-V3196
        {
            foundPresetRefs.HideDropDownContent();

            var currentPresets = getReportPresetsArrayUI();
            ReportPreset referredPreset = referredReference.findPreset();
            List<object> filteringPresetList = new List<object>();

            if (FilterList(filteringPresetList, currentPresets, selectedPreset, referredPreset, ExcludePresetFromChain, string.Empty, ExcludedCainChars))
            {
                for (int i = 0; i < filteringPresetList.Count; i++)
                    filteringPresetList[i] = new ReportPresetReference(filteringPresetList[i] as ReportPreset);

                FillListByList(foundPresetRefs.Items, filteringPresetList);
                foundPresetRefs.SelectedItem = referredReference;
            }

            foundPresetRefs.ShowDropDownContent();
        }

        //ON LR INIT AND IN INTERACTIVE LR:
        //  InitLrPresetsAutoApplying(), InitLrPresetsFunctionIds(), InitLrPresetsHotkeys(), in LR Form
        //
        //NOTE:
        //  
        //  SortedDictionary<Guid, ReportPreset> reportChain: <guid, ReportPreset>
        //
        //RETURNS:
        //  Report chain in reportChain
        internal static void FillPresetFilteringChain(ReportPreset[] currentReports, SortedDictionary<Guid, bool> reportChain, ReportPreset initialPreset)
        {
            reportChain.AddSkip(initialPreset.guid);

            if (initialPreset.useAnotherPresetAsSource)
            {
                var nextPreset = initialPreset.anotherPresetAsSource.findPreset();
                FillPresetFilteringChain(currentReports, reportChain, nextPreset); //-V3080
            }
        }
        #endregion
    }
}
