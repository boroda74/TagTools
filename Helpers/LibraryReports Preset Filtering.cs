using ExtensionMethods;
using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;

namespace MusicBeePlugin
{
    partial class LibraryReports
    {
        #region Adjusting UI
        private void adjustFilteringPresetUI(CustomComboBox foundPresetRefs, ReportPresetReference selectedPresetRef,
            bool presetCheckStatusIsSenseless, bool presetCheckStatusIsBroken)
        {
            if (selectedPresetRef.permanentGuid == Guid.Empty)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip);
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBoxLabel, UseAnotherPresetAsSourceToolTip);
                toolTip1.SetToolTip(presetChainIsWrongPictureBox, string.Empty);
                presetChainIsWrongPictureBox.Image = Resources.transparent_15;
                SetComboBoxCue(foundPresetRefs, string.Empty);
            }
            else if (!presetCheckStatusIsSenseless && !presetCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip);
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBoxLabel, UseAnotherPresetAsSourceToolTip);
                toolTip1.SetToolTip(presetChainIsWrongPictureBox, string.Empty);
                presetChainIsWrongPictureBox.Image = Resources.transparent_15;
                SetComboBoxCue(foundPresetRefs, string.Empty);

                foundPresetRefs.SelectedItem = selectedPresetRef;
            }
            else if (presetCheckStatusIsSenseless && !presetCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsSenselessToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBoxLabel, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsSenselessToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                toolTip1.SetToolTip(presetChainIsWrongPictureBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsSenselessToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                presetChainIsWrongPictureBox.Image = error;
                SetComboBoxCue(foundPresetRefs, selectedPresetRef.name);

                foundPresetRefs.SelectedIndex = -1;
            }
            else if (!presetCheckStatusIsSenseless && presetCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBoxLabel, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                toolTip1.SetToolTip(presetChainIsWrongPictureBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                presetChainIsWrongPictureBox.Image = fatalError;
                SetComboBoxCue(foundPresetRefs, selectedPresetRef.name);

                foundPresetRefs.SelectedIndex = -1;
            }
            else //if (relatedPresetsCheckStatusIsSenseless && relatedPresetsCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() + "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBoxLabel, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() + "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                toolTip1.SetToolTip(presetChainIsWrongPictureBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() +
                    "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper() + "\n\n" + UseAnotherPresetAsSourceCheckPresetChainToolTip.ToUpper());
                presetChainIsWrongPictureBox.Image = errorFatalError;
                SetComboBoxCue(foundPresetRefs, selectedPresetRef.name);

                foundPresetRefs.SelectedIndex = -1;
            }
        }

        //ON CHANGING CONDITION/FILTERING:
        //  conditionCheckBox_CheckedChanged(), useAnotherPresetAsSourceCheckBox_CheckedChanged(), useAnotherPresetAsSourceComboBox_SelectedIndexChanged()
        private void checkAdjustAllPresetChainsUI()
        {
            var (presetsCheckStatusIsSenseless, presetsCheckStatusIsBroken) = checkAllPresetChains(true);


            buttonSaveClose.Enable(!presetsCheckStatusIsSenseless && !presetsCheckStatusIsBroken);

            if (!presetsCheckStatusIsSenseless && !presetsCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
                buttonClose.Image = unsavedChanges ? warningWide : Resources.transparent_15;
            }
            else if (presetsCheckStatusIsSenseless && !presetsCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip + "\n\n" + UseAnotherPresetAsSourceIsSenselessToolTip.ToUpper());
                buttonClose.Image = errorWide;
            }
            else if (!presetsCheckStatusIsSenseless && presetsCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper());
                buttonClose.Image = fatalErrorWide;
            }
            else //if (relatedPresetsCheckStatusIsSenseless && relatedPresetsCheckStatusIsBroken)
            {
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip + "\n\n" + UseAnotherPresetAsSourceIsSenselessToolTip.ToUpper()
                    + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper());
                buttonClose.Image = errorFatalErrorWide;
            }
        }
        #endregion

        #region Finding related presets
        //ON CHAGING SELECTED PRESET:
        //  presetListSelectedIndexChanged()
        private void findFilteringPresetsUI(CustomComboBox foundPresetRefs, ReportPreset referringPreset)
        {
            foundPresetRefs.ItemsClear();

            var currentPresets = getReportPresetsArrayUI();

            var presetCheckStatusIsSenseless = false;
            var presetCheckStatusIsBroken = false;

            foreach (var preset in currentPresets)
            {
                if (preset != referringPreset)
                {
                    var (relatedPresetsCheckStatusIsAppropriateForConditionalFiltering, relatedPresetsCheckStatusIsSenseless, relatedPresetsCheckStatusIsBroken) =
                        checkPresetChainDeeper(currentPresets, preset, referringPreset, false);

                    if (relatedPresetsCheckStatusIsAppropriateForConditionalFiltering)
                        foundPresetRefs.Items.Add(new ReportPresetReference(preset));

                    presetCheckStatusIsSenseless |= relatedPresetsCheckStatusIsSenseless;
                    presetCheckStatusIsBroken |= relatedPresetsCheckStatusIsBroken;
                }
            }

            adjustFilteringPresetUI(foundPresetRefs, referringPreset.anotherPresetAsSource, presetCheckStatusIsSenseless, presetCheckStatusIsBroken);
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
            if (initialPreset.useAnotherPresetAsSource)
            {
                reportChain.AddSkip(initialPreset.guid);
                var nextPreset = initialPreset.anotherPresetAsSource.findPreset(currentReports);

                FillPresetFilteringChain(currentReports, reportChain, nextPreset); //-V3080
            }
        }

        //SERVICE FUNCTION
        //ON CANGING CONDITION/FILTERING:
        //  checkRelatedPresetsChain()
        //
        //RETURNS:
        //  allPresetsCheckStatusIsSenseless = true: reference chain is senseless (there is !conditionIsChecked),
        //  allPresetsCheckStatusIsBroken = true: reference chain is bad (looped references)
        private (bool, bool) checkAllPresetChains(bool updateStatuses)
        {
            var currentPresets = getReportPresetsArrayUI();

            var senselessReferringPresetGuids = new SortedDictionary<Guid, bool>();
            var brokenReferringPresetGuids = new SortedDictionary<Guid, bool>();

            var presetsCheckStatusIsSenseless = false;
            var presetsCheckStatusIsBroken = false;

            foreach (var preset in currentPresets)
            {
                var (_, presetCheckStatusIsSenseless, presetCheckStatusIsBroken) = checkPresetChainDeeper(currentPresets, preset, null, updateStatuses,
                    senselessReferringPresetGuids, brokenReferringPresetGuids);

                presetsCheckStatusIsSenseless |= presetCheckStatusIsSenseless;
                presetsCheckStatusIsBroken |= presetCheckStatusIsBroken;
            }

            if (updateStatuses)
                presetList.Refresh();

            return (presetsCheckStatusIsSenseless, presetsCheckStatusIsBroken);
        }

        //SERVICE FUNCTION
        //RETURNS RESULTS IN:
        //  referredReferencingPresetGuids
        private void findNewReferencingPresetGuidsInternal(SortedDictionary<Guid, SortedDictionary<Guid, bool>> referredReferencingPresetGuids,
            SortedDictionary<Guid, bool> referencingPresetGuids, Guid referredPresetGuid)
        {
            var newReferencingPresetGuids = new SortedDictionary<Guid, bool>();

            if (referredReferencingPresetGuids.TryGetValue(referredPresetGuid, out var referredPresetReferencingPresetGuids))
            {
                foreach (var newReferencingPresetGuid in referredPresetReferencingPresetGuids.Keys)
                    referencingPresetGuids.AddSkip(newReferencingPresetGuid);

                foreach (var newReferencingPresetGuid in referredPresetReferencingPresetGuids.Keys)
                    newReferencingPresetGuids.AddSkip(newReferencingPresetGuid);
            }


            if (newReferencingPresetGuids.Count > 0)
            {
                foreach (var newReferredPresetGuid in newReferencingPresetGuids.Keys)
                    findNewReferencingPresetGuidsInternal(referredReferencingPresetGuids, referencingPresetGuids, newReferredPresetGuid);
            }
        }

        //SERVICE FUNCTION
        private List<ReportPreset> findReferencingPresetsInternal(ReportPreset[] currentPresets, ReportPreset referredPreset)
        {
            //referencingPresetGuids: <referred preset permanentGuid, <referencing preset permanentGuid, false>>
            var referredReferencingPresetGuids = new SortedDictionary<Guid, SortedDictionary<Guid, bool>>();

            SortedDictionary<Guid, bool> referencingPresetGuids;

            foreach (var preset in currentPresets)
            {
                if (preset.permanentGuid != referredPreset.permanentGuid && preset.useAnotherPresetAsSource)
                {
                    if (!referredReferencingPresetGuids.TryGetValue(preset.anotherPresetAsSource.permanentGuid, out referencingPresetGuids))
                    {
                        referencingPresetGuids = new SortedDictionary<Guid, bool>();
                        referredReferencingPresetGuids.AddSkip(preset.anotherPresetAsSource.permanentGuid, referencingPresetGuids);
                    }

                    referencingPresetGuids.AddSkip(preset.permanentGuid);
                }
            }


            referencingPresetGuids = new SortedDictionary<Guid, bool>();

            if (referredReferencingPresetGuids.ContainsKey(referredPreset.permanentGuid))
                findNewReferencingPresetGuidsInternal(referredReferencingPresetGuids, referencingPresetGuids, referredPreset.permanentGuid);


            var referencingPresets = new List<ReportPreset>();
            foreach (var preset in currentPresets)
            {
                if (referencingPresetGuids.ContainsKey(preset.permanentGuid))
                    referencingPresets.Add(preset);
            }

            return referencingPresets;
        }


        #endregion

        #region Checking correctness/adjusting UI
        //SERVICE FUNCTION
        private void checkPresetChainDeeperInternal(ReportPreset[] currentPresets, ReportPreset referringPreset, ReportPreset currentPreset,
            SortedDictionary<Guid, bool> processedReferringPresetGuids,
            SortedDictionary<Guid, bool> correctSelfDeeperConditionalPresetGuids,
            SortedDictionary<Guid, bool> senselessReferringPresetGuids,
            SortedDictionary<Guid, bool> brokenReferringPresetGuids)
        {
            if (processedReferringPresetGuids.ContainsKey(referringPreset.permanentGuid))
            {
                brokenReferringPresetGuids.AddSkip(referringPreset.permanentGuid);
                return;
            }

            processedReferringPresetGuids.Add(referringPreset.permanentGuid);

            if (!referringPreset.useAnotherPresetAsSource && currentPreset == null)
                return;

            if (referringPreset.conditionIsChecked && referringPreset != currentPreset)
                correctSelfDeeperConditionalPresetGuids.AddSkip(referringPreset.permanentGuid);

            if (referringPreset.useAnotherPresetAsSource)
            {
                if (referringPreset.anotherPresetAsSource.permanentGuid == Guid.Empty) //ReportPresetReference is struct, so can't be null
                {
                    senselessReferringPresetGuids.AddSkip(referringPreset.permanentGuid);
                    return;
                }

                var referredPreset = referringPreset.anotherPresetAsSource.findPreset(currentPresets); //It's senseless, must never happen
                if (referredPreset == null)
                {
                    senselessReferringPresetGuids.AddSkip(referringPreset.permanentGuid);
                    brokenReferringPresetGuids.AddSkip(referringPreset.permanentGuid);
                    return;
                }

                if (!referredPreset.conditionIsChecked)
                {
                    senselessReferringPresetGuids.AddSkip(referringPreset.permanentGuid);
                    return;
                }

                checkPresetChainDeeperInternal(currentPresets, referredPreset, currentPreset,
                    processedReferringPresetGuids, correctSelfDeeperConditionalPresetGuids, senselessReferringPresetGuids, brokenReferringPresetGuids);
            }
        }

        //SERVICE FUNCTION
        //ON CHANGING SELECTED PRESET, CHANGING CONDITION/FILTERING:
        //  findFilteringPresetsUI()/checkAdjustAllPresetChainsUI()
        //
        //RETURNS:
        //  relatedPresetsCheckStatusIsAppropriateForConditionalFiltering = true: reference chain has at least one conditional preset,
        //  relatedPresetsCheckStatusIsSenseless = true: reference chain is senseless (there is !conditionIsChecked),
        //  relatedPresetsCheckStatusIsBroken = true: reference chain is bad (looped references)
        private (bool, bool, bool) checkPresetChainDeeper(ReportPreset[] currentPresets, ReportPreset referringPreset, ReportPreset currentPreset,
            bool updateCurrentPresetsCheckStatus,
            SortedDictionary<Guid, bool> senselessReferringPresetGuids = null,
            SortedDictionary<Guid, bool> brokenReferringPresetGuids = null)
        {
            var processedReferringPresetGuids = new SortedDictionary<Guid, bool>();
            var correctSelfDeeperConditionalPresetGuids = new SortedDictionary<Guid, bool>();

            if (senselessReferringPresetGuids == null)
                senselessReferringPresetGuids = new SortedDictionary<Guid, bool>();

            if (brokenReferringPresetGuids == null)
                brokenReferringPresetGuids = new SortedDictionary<Guid, bool>();


            checkPresetChainDeeperInternal(currentPresets, referringPreset, currentPreset,
                processedReferringPresetGuids, correctSelfDeeperConditionalPresetGuids, senselessReferringPresetGuids, brokenReferringPresetGuids);


            if (updateCurrentPresetsCheckStatus)
            {
                foreach (var preset in currentPresets)
                    preset.setSenselessBrokenReferringPresetsInChain(senselessReferringPresetGuids.ContainsKey(preset.permanentGuid),
                        brokenReferringPresetGuids.ContainsKey(preset.permanentGuid));
            }


            return (correctSelfDeeperConditionalPresetGuids.Count > 0, senselessReferringPresetGuids.Count > 0, brokenReferringPresetGuids.Count > 0);
        }

        //ON CHANGING SELECTED PRESET, CONDITION/FILTERING:
        //  checkPresetChainDeeperAdjustUI() and conditionCheckBox_CheckedChanged()
        //
        //RETURNS:
        //  initialPresetsCheckStatusIsSenseless = true: reference chain is senseless (there is !conditionIsChecked),
        //  initialPresetsCheckStatusIsBroken = true: reference chain is bad (looped references)
        private (bool, bool) checkRelatedPresetsChain(ReportPreset initialPreset, ReportPreset currentPreset,
            bool updateInitialPresetReferencingChainCheckStatus, bool updateInitialPresetReferredChainCheckStatus)
        {
            var initialPresetsCheckStatusIsSenseless = false;
            var initialPresetsCheckStatusIsBroken = false;


            if (initialPreset != null)
            {
                var currentPresets = getReportPresetsArrayUI();
                var referencingPresets = findReferencingPresetsInternal(currentPresets, initialPreset);

                var senselessReferringPresetGuids = new SortedDictionary<Guid, bool>();
                var brokenReferringPresetGuids = new SortedDictionary<Guid, bool>();

                var (_, relatedPresetsCheckStatusIsSenseless, relatedPresetsCheckStatusIsBroken) = checkPresetChainDeeper(currentPresets, initialPreset, currentPreset,
                    updateInitialPresetReferencingChainCheckStatus,
                    senselessReferringPresetGuids, brokenReferringPresetGuids);

                initialPresetsCheckStatusIsSenseless = relatedPresetsCheckStatusIsSenseless;
                initialPresetsCheckStatusIsBroken = relatedPresetsCheckStatusIsBroken;

                foreach (var referencingPreset in referencingPresets)
                {
                    (_, relatedPresetsCheckStatusIsSenseless, relatedPresetsCheckStatusIsBroken) = checkPresetChainDeeper(currentPresets, referencingPreset, currentPreset, updateInitialPresetReferredChainCheckStatus,
                        senselessReferringPresetGuids, brokenReferringPresetGuids);

                    initialPresetsCheckStatusIsSenseless |= relatedPresetsCheckStatusIsSenseless;
                    initialPresetsCheckStatusIsBroken |= relatedPresetsCheckStatusIsBroken;
                }

                if (initialPresetsCheckStatusIsSenseless || initialPresetsCheckStatusIsBroken)
                    setUnsavedChanges(true);
            }


            if (updateInitialPresetReferencingChainCheckStatus || updateInitialPresetReferredChainCheckStatus)
                presetList.Refresh();

            return (initialPresetsCheckStatusIsSenseless, initialPresetsCheckStatusIsBroken);
        }

        //ON CHANGING SELECTED PRESET, SAVING PRESETS, CHANGING FILTERING:
        //  saveSettings(), presetListSelectedIndexChanged(), useAnotherPresetAsSourceComboBox_SelectedIndexChanged(), useAnotherPresetAsSourceCheckBox_CheckedChanged()
        //
        //RETURNS:
        //  presetsCheckStatusIsSenseless = true: reference chain is senseless (there is !conditionIsChecked),
        //  presetsCheckStatusIsBroken = true: reference chain is bad (looped references)
        private void checkAdjustFilteringPresetUI()
        {
            if (selectedPreset != null)
            {
                setPresetChanged();

                var (presetCheckStatusIsSenseless, presetCheckStatusIsBroken) = checkRelatedPresetsChain(selectedPreset, selectedPreset, true, true);

                if (useAnotherPresetAsSourceComboBoxCustom.SelectedItem == null)
                    adjustFilteringPresetUI(useAnotherPresetAsSourceComboBoxCustom, default,
                        presetCheckStatusIsSenseless, presetCheckStatusIsBroken);
                else
                    adjustFilteringPresetUI(useAnotherPresetAsSourceComboBoxCustom, (ReportPresetReference)useAnotherPresetAsSourceComboBoxCustom.SelectedItem,
                        presetCheckStatusIsSenseless, presetCheckStatusIsBroken);
            }
        }
        #endregion
    }
}
