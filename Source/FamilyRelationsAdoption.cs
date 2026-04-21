using UnityEngine; 
using Verse; 
using RimWorld; 

namespace FamilyRelationsAdoption;

public class FamilyRelationsAdoptionMod : Mod
{
    public static FamilyRelationsAdoptionSettings settings; 

    public FamilyRelationsAdoptionMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<FamilyRelationsAdoptionSettings>(); 
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listingStandard = new Listing_Standard(); 
        listingStandard.Begin(inRect); 

        // ==========================================================================
        // ADOPTION PROPOSAL SETTINGS
        // ==========================================================================
        // FIRST ROW
        // - Slider: base success chance for adoption proposal
        // - Slider: min opinion threshold for adoption proposal 

        Rect baseSuccessChanceAndMinOpinionProposal_Rect = listingStandard.GetRect(50f); 
        Listing_Standard baseSuccessChanceAndMinOpinionProposal_Section = new Listing_Standard(); 

        baseSuccessChanceAndMinOpinionProposal_Section.Begin(baseSuccessChanceAndMinOpinionProposal_Rect); 
        baseSuccessChanceAndMinOpinionProposal_Section.ColumnWidth = (baseSuccessChanceAndMinOpinionProposal_Rect.width - 17f) / 2f; 

        baseSuccessChanceAndMinOpinionProposal_Section.Label(
            "FRA_BaseAdoptionSuccess".Translate() + ": " + settings.baseAdoptionSuccessChance, 
            -1f, 
            new TipSignal("FRA_BaseAdoptionSuccessTooltip".Translate())
        );
        settings.baseAdoptionSuccessChance = baseSuccessChanceAndMinOpinionProposal_Section.Slider(settings.baseAdoptionSuccessChance, 0f, 3f); 

        baseSuccessChanceAndMinOpinionProposal_Section.NewColumn(); 

        baseSuccessChanceAndMinOpinionProposal_Section.Label(
            "FRA_MinOpinionProposal".Translate() + ": " + settings.minOpinionForAdoptionProposal, 
            -1f, 
            new TipSignal("FRA_MinOpinionProposalTooltip".Translate())
        ); 
        settings.minOpinionForAdoptionProposal = (int)baseSuccessChanceAndMinOpinionProposal_Section.Slider(settings.minOpinionForAdoptionProposal, -100, 100); 

        listingStandard.EndSection(baseSuccessChanceAndMinOpinionProposal_Section); 

        // ==========================================================================
        // SECOND ROW
        // - Checkbox: auto-success for adoption proposals 

        Rect autoSuccess_Rect = listingStandard.GetRect(75f); 
        Listing_Standard autoSuccess_Section = new Listing_Standard(); 

        autoSuccess_Section.Begin(autoSuccess_Rect); 
        autoSuccess_Section.ColumnWidth = (autoSuccess_Rect.width - 17f) / 2f; 

        autoSuccess_Section.CheckboxLabeled(
            "FRA_AutoSuccessProposal".Translate(), 
            ref settings.autoSuccessAdoption, 
            "FRA_AutoSuccessProposalTooltip".Translate() 
        );

        autoSuccess_Section.NewColumn(); 

        if (autoSuccess_Section.ButtonText("FRA_ResetToDefaults".Translate()))
        {
            settings.baseAdoptionSuccessChance = 1f; 
            settings.minOpinionForAdoptionProposal = 15; 
            settings.autoSuccessAdoption = false; 
        }

        listingStandard.EndSection(autoSuccess_Section);

        // ==========================================================================
        // End of settings

        listingStandard.End(); 
        base.DoSettingsWindowContents(inRect); 
    }

    public override string SettingsCategory()
    {
        return "Family Relations: Adoption";
    }
}
