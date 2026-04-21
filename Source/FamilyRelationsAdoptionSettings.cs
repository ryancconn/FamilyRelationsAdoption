using Verse; 
using RimWorld; 

namespace FamilyRelationsAdoption
{
    public class FamilyRelationsAdoptionSettings : ModSettings
    {
        public int minOpinionForAdoptionProposal = 15;

        public float baseAdoptionSuccessChance = 1f; 

        public bool autoSuccessAdoption = false; 

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref minOpinionForAdoptionProposal, "minOpinionForAdoptionProposal", minOpinionForAdoptionProposal, true); 

            Scribe_Values.Look(ref baseAdoptionSuccessChance, "baseAdoptionSuccessChance", baseAdoptionSuccessChance, true); 

            Scribe_Values.Look(ref autoSuccessAdoption, "autoSuccessAdoption", autoSuccessAdoption, true); 
        }
    }
}