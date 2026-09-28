using Verse; 
using RimWorld;
using Steamworks;

namespace FamilyRelationsAdoption
{
    public class FamilyRelationsAdoptionSettings : ModSettings
    {
        // General Settings
        public bool allowAutoSuccessAdoption = false; 
        public int minOpinionForAdoptionProposal = 15;
        public float baseAdoptionSuccessChance = 1f; 
        public int minAgeDifferenceManual = 15; 
        public int maxAdoptionsPerChild = 2; 

        // Spontaneous Adoption Settings
        public bool allowSpontaneousAdoption = true; 
        public bool allowSpontaneousAdoptionForChildWithNonHostileParents = false; 
        public float baseSelectionWeight = 1.1f; 
        public int minAgeDifferenceSpontaneous = 15; 
        public bool allowSpontaneousAdoptionForFamilyByBlood = false; 
        public int maxAgeForSpontaneousFamilyByBloodAdoption = 6; 

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref allowAutoSuccessAdoption, "allowAutoSuccessAdoption", allowAutoSuccessAdoption, true); 
            Scribe_Values.Look(ref minOpinionForAdoptionProposal, "minOpinionForAdoptionProposal", minOpinionForAdoptionProposal, true); 
            Scribe_Values.Look(ref baseAdoptionSuccessChance, "baseAdoptionSuccessChance", baseAdoptionSuccessChance, true); 
            Scribe_Values.Look(ref minAgeDifferenceManual, "minAgeDifferenceManual", minAgeDifferenceManual, true);
            Scribe_Values.Look(ref maxAdoptionsPerChild, "maxAdoptionsPerChild", maxAdoptionsPerChild, true); 
            
            Scribe_Values.Look(ref allowSpontaneousAdoption, "allowSpontaneousAdoption", allowSpontaneousAdoption, true);
            Scribe_Values.Look(ref allowSpontaneousAdoptionForChildWithNonHostileParents, "allowSpontaneousAdoptionForChildWithNonHostileParents", allowSpontaneousAdoptionForChildWithNonHostileParents, true); 
            Scribe_Values.Look(ref baseSelectionWeight, "baseSelectionWeight", baseSelectionWeight, true); 
            Scribe_Values.Look(ref minAgeDifferenceSpontaneous, "minAgeDifferenceSpontaneous", minAgeDifferenceSpontaneous, true); 
            Scribe_Values.Look(ref allowSpontaneousAdoptionForFamilyByBlood, "allowSpontaneousAdoptionForFamilyByBlood", allowSpontaneousAdoptionForFamilyByBlood, true); 
            Scribe_Values.Look(ref maxAgeForSpontaneousFamilyByBloodAdoption, "maxAgeForSpontaneousFamilyByBloodAdoption", maxAgeForSpontaneousFamilyByBloodAdoption, true); 
        }
    }
}