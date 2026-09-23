using UnityEngine; 
using Verse; 
using RimWorld;
using HarmonyLib;

namespace FamilyRelationsAdoption;

public class FamilyRelationsAdoptionMod : Mod
{
    public static FamilyRelationsAdoptionSettings settings; 

    public FamilyRelationsAdoptionMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<FamilyRelationsAdoptionSettings>(); 
        var harmony = new Harmony("Rycon.FamilyRelationsAdoption"); 
        harmony.PatchAll(); 
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listingStandard = new(); 
        listingStandard.Begin(inRect); 

        // ==========================================================================
        // GENERAL SETTINGS
        // ==========================================================================
        // TITLE ROW

        Rect generalTitleRow_Rect = listingStandard.GetRect(50f); 
        Listing_Standard generalTitleRow_Section =  new(); 

        generalTitleRow_Section.Begin(generalTitleRow_Rect); 

        generalTitleRow_Section.SubLabel("FRA_GeneralSettings".Translate(), 1f); 
        generalTitleRow_Section.GapLine(); 
        
        listingStandard.EndSection(generalTitleRow_Section); 

        // ==========================================================================
        // FIRST ROW
        // - Checkbox: allow auto success adoptions 
        // - Slider: min opinion threshold for adoption proposal 

        Rect generalRow1_Rect = listingStandard.GetRect(50f); 
        Listing_Standard generalRow1_Section = new(); 

        generalRow1_Section.Begin(generalRow1_Rect); 
        generalRow1_Section.ColumnWidth = (generalRow1_Rect.width - 17f) / 2f; 

        generalRow1_Section.CheckboxLabeled(
            "FRA_AutoSuccessProposal".Translate(), 
            ref settings.allowAutoSuccessAdoption, 
            "FRA_AutoSuccessProposalTooltip".Translate() 
        );

        generalRow1_Section.NewColumn(); 

        generalRow1_Section.Label(
            "FRA_MinOpinionProposal".Translate() + ": " + settings.minOpinionForAdoptionProposal, 
            -1f, 
            new TipSignal("FRA_MinOpinionProposalTooltip".Translate())
        ); 
        settings.minOpinionForAdoptionProposal = (int)generalRow1_Section.Slider(settings.minOpinionForAdoptionProposal, -100, 100); 

        listingStandard.EndSection(generalRow1_Section); 

        // ==========================================================================
        // SECOND ROW
        // - Slider: base success chance for adoption proposal
        // - Slider: min age difference for manual adoption 

        Rect generalRow2_Rect = listingStandard.GetRect(50f); 
        Listing_Standard generalRow2_Section = new(); 

        generalRow2_Section.Begin(generalRow2_Rect); 
        generalRow2_Section.ColumnWidth = (generalRow2_Rect.width - 17f) / 2f; 

        generalRow2_Section.Label(
            "FRA_BaseAdoptionSuccess".Translate() + ": " + settings.baseAdoptionSuccessChance, 
            -1f, 
            new TipSignal("FRA_BaseAdoptionSuccessTooltip".Translate())
        );
        settings.baseAdoptionSuccessChance = Mathf.Round(generalRow2_Section.Slider(settings.baseAdoptionSuccessChance, 0f, 3f) * 10.0f) * 0.1f; 

        generalRow2_Section.NewColumn(); 

        generalRow2_Section.Label(
            "FRA_MinAgeDifferenceManual".Translate() + ": " + settings.minAgeDifferenceManual, 
            -1f, 
            new TipSignal("FRA_MinAgeDifferenceManualTooltip".Translate())
        ); 
        settings.minAgeDifferenceManual = (int)generalRow2_Section.Slider(settings.minAgeDifferenceManual, 0, 50); 

        listingStandard.EndSection(generalRow2_Section); 

        // ==========================================================================
        // THIRD ROW
        // - empty space
        // - Button: reset to defaults

        Rect generalRow3_Rect = listingStandard.GetRect(50f); 
        Listing_Standard generalRow3_Section = new(); 

        generalRow3_Section.Begin(generalRow3_Rect); 
        generalRow3_Section.ColumnWidth = (generalRow3_Rect.width - 17f) / 2f; 
        generalRow3_Section.NewColumn(); 

        if (generalRow3_Section.ButtonText("FRA_ResetToDefaults".Translate()))
        {
            settings.allowAutoSuccessAdoption = false;
            settings.minOpinionForAdoptionProposal = 15; 
            settings.baseAdoptionSuccessChance = 1f;  
            settings.minAgeDifferenceManual = 15;             
        }

        listingStandard.EndSection(generalRow3_Section); 

        // ==========================================================================
        // CLOSER LINE ROW 
        Rect generalCloserRow_Rect = listingStandard.GetRect(50f); 
        Listing_Standard generalCloserRow_Section = new(); 

        generalCloserRow_Section.Begin(generalCloserRow_Rect); 
        generalCloserRow_Section.GapLine(); 

        listingStandard.EndSection(generalCloserRow_Section); 

        // ==========================================================================
        // SPONTANEOUS ADOPTION SETTINGS
        // ==========================================================================
        // TITLE ROW

        Rect spontaneousTitleRow_Rect = listingStandard.GetRect(50f); 
        Listing_Standard spontaneousTitleRow_Section =  new(); 

        spontaneousTitleRow_Section.Begin(spontaneousTitleRow_Rect); 

        spontaneousTitleRow_Section.SubLabel("FRA_SpontaneousAdoptionSettings".Translate(), 1f); 
        spontaneousTitleRow_Section.GapLine(); 
        
        listingStandard.EndSection(spontaneousTitleRow_Section); 

        // ==========================================================================
        // FIRST ROW
        // - Checkbox: allow spontaneous adoption proposals
        // - Checkbox: allow if child has living parents in a non-hostile faction

        Rect spontaneousRow1_Rect = listingStandard.GetRect(50f); 
        Listing_Standard spontaneousRow1_Section = new(); 

        spontaneousRow1_Section.Begin(spontaneousRow1_Rect); 
        spontaneousRow1_Section.ColumnWidth = (spontaneousRow1_Rect.width - 17f) / 2f; 

        spontaneousRow1_Section.CheckboxLabeled(
            "FRA_AllowSpontaneousAdoption".Translate(), 
            ref settings.allowSpontaneousAdoption, 
            "FRA_AllowSpontaneousAdoptionTooltip".Translate() 
        );

        spontaneousRow1_Section.NewColumn(); 

        spontaneousRow1_Section.Label(
            "FRA_BaseSelectionWeight".Translate() + ": " + settings.baseSelectionWeight.ToString(), 
            -1f, 
            "FRA_BaseSelectionWeightTooltip".Translate() 
        );
        settings.baseSelectionWeight = Mathf.Round(spontaneousRow1_Section.Slider(settings.baseSelectionWeight, 0f, 2f) * 10.0f) * 0.1f; 

        listingStandard.EndSection(spontaneousRow1_Section); 

        // ==========================================================================
        // SECOND ROW; 
        // - Slider: base selection weight for spontaneous adoption proposals
        // - Slider: min age difference for spontaneous adoption 

        Rect spontaneousRow2_Rect = listingStandard.GetRect(50f); 
        Listing_Standard spontaneousRow2_Section = new(); 

        spontaneousRow2_Section.Begin(spontaneousRow2_Rect); 
        spontaneousRow2_Section.ColumnWidth = (spontaneousRow2_Rect.width - 17f) / 2f; 

        spontaneousRow2_Section.CheckboxLabeled(
            "FRA_AllowSpontaneousAdoptionForChildWithNonHostileParents".Translate(), 
            ref settings.allowSpontaneousAdoptionForChildWithNonHostileParents, 
            "FRA_AllowSpontaneousAdoptionForChildWithNonHostileParentsTooltip".Translate()
        ); 

        spontaneousRow2_Section.NewColumn(); 

        spontaneousRow2_Section.Label(
            "FRA_MinAgeDifferenceSpontaneous".Translate() + ": " + settings.minAgeDifferenceSpontaneous.ToString(), 
            -1f, 
            "FRA_MinAgeDifferenceSpontaneousTooltip".Translate()
        ); 
        settings.minAgeDifferenceSpontaneous = (int)spontaneousRow2_Section.Slider(settings.minAgeDifferenceSpontaneous, 0, 50); 

        listingStandard.EndSection(spontaneousRow2_Section); 

        // ==========================================================================
        // THIRD ROW; 
        // - Checkbox: allow for family-by-blood
        // - Slider: max age for family-by-blood 

        Rect spontaneousRow3_Rect = listingStandard.GetRect(50f); 
        Listing_Standard spontaneousRow3_Section = new(); 

        spontaneousRow3_Section.Begin(spontaneousRow3_Rect); 
        spontaneousRow3_Section.ColumnWidth = (spontaneousRow3_Rect.width - 17f) / 2f; 

        spontaneousRow3_Section.CheckboxLabeled(
            "FRA_AllowSpontaneousAdoptionFamilyByBlood".Translate(), 
            ref settings.allowSpontaneousAdoptionForFamilyByBlood, 
            "FRA_AllowSpontaneousAdoptionFamilyByBloodTooltip".Translate() 
        );

        spontaneousRow3_Section.NewColumn(); 

        spontaneousRow3_Section.Label(
            "FRA_SpontaneousAdoptionMaxAgeFamilyByBlood".Translate() + ": " + settings.maxAgeForSpontaneousFamilyByBloodAdoption.ToString(), 
            -1f, 
            "FRA_SpontaneousAdoptionMaxAgeFamilyByBloodTooltip".Translate()
        ); 
        settings.maxAgeForSpontaneousFamilyByBloodAdoption = (int)spontaneousRow3_Section.Slider(settings.maxAgeForSpontaneousFamilyByBloodAdoption, 0, 18); 

        listingStandard.EndSection(spontaneousRow3_Section); 

        // ==========================================================================
        // THIRD ROW
        // - empty space
        // - Button: reset to defaults

        Rect spontaneousRow4_Rect = listingStandard.GetRect(50f); 
        Listing_Standard spontaneousRow4_Section = new(); 

        spontaneousRow4_Section.Begin(spontaneousRow4_Rect); 
        spontaneousRow4_Section.ColumnWidth = (spontaneousRow4_Rect.width - 17f) / 2f; 
        spontaneousRow4_Section.NewColumn(); 

        if (spontaneousRow4_Section.ButtonText("FRA_ResetToDefaults".Translate()))
        {
            settings.allowSpontaneousAdoption = true; 
            settings.allowSpontaneousAdoptionForChildWithNonHostileParents = false; 
            settings.baseSelectionWeight = 1.1f; 
            settings.minAgeDifferenceSpontaneous = 15; 
            settings.allowSpontaneousAdoptionForFamilyByBlood = false; 
            settings.maxAgeForSpontaneousFamilyByBloodAdoption = 6; 
        }

        listingStandard.EndSection(spontaneousRow4_Section); 

        // ==========================================================================
        // CLOSER LINE ROW 
        Rect spontaneousCloserRow_Rect = listingStandard.GetRect(50f); 
        Listing_Standard spontaneousCloserRow_Section = new(); 

        spontaneousCloserRow_Section.Begin(spontaneousCloserRow_Rect); 
        spontaneousCloserRow_Section.GapLine(); 

        listingStandard.EndSection(spontaneousCloserRow_Section); 

        // ==========================================================================
        // End of settings

        listingStandard.End(); 
        base.DoSettingsWindowContents(inRect); 
    }

    public override string SettingsCategory()
    {
        return "FRA_ModName".Translate();
    }
}
