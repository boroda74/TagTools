﻿using System;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    partial class LibraryReports
    {
        #region Finding related presets
        private static ReportPreset GetNextItem(ReportPreset current)
        {
            if (current.useAnotherPresetAsSource)
                return current.anotherPresetAsSource.findPreset();
            else
                return null;
        }

        private static bool AddSkipItem(IList list, ReportPreset value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
                return true;
            }

            return false;
        }

        //ON CHAGING SELECTED PRESET:
        //  initPreset()
        private void findFilteringPresetsUI(CustomComboBox foundPresetRefs, ReportPreset referringPreset, ReportPresetReference referredReference) //-V3196
        {
            foundPresetRefs.HideDropDownContent();

            var currentPresets = getReportPresetsArrayUI();
            ReportPreset referredPreset = referredReference.findPreset();
            List<object> presetChain = new List<object>();

            BuildItemChain(currentPresets, presetChain, selectedPreset, AddSkipItem, GetNextItem);

            List<ReportPresetReference> filteringPresetList = new List<ReportPresetReference>();

            foreach (var preset in currentPresets)
                if (preset.conditionIsChecked && !presetChain.Contains(preset))
                    filteringPresetList.Add(new ReportPresetReference(preset));

            FillListByList(foundPresetRefs.Items, filteringPresetList);
            foundPresetRefs.SelectedItem = referredReference;

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
