using System.Collections.Generic;
using Verse;

namespace FamilyRelationsAdoption
{
    public static class FRA_GeneralUtility
    {
        private static float GetAdultMinAge(Pawn pawn)
        {
            try {
                List<LifeStageAge> lifeStages = pawn.RaceProps.lifeStageAges; 
                if (lifeStages[^1].def.defName.Contains("SZHumanSize"))
                {
                    return lifeStages[^2].minAge; 
                }
                else
                {
                    return lifeStages[^1].minAge; 
                }
            }
            catch
            {
                Log.Message("FRA :: Error getting min adult age from RaceProps; using default of 18 years");
                return 18.0f; 
            }
        }

        public static bool IsAdult(Pawn pawn)
        {
            return pawn.ageTracker.AgeBiologicalYearsFloat >= GetAdultMinAge(pawn); 
        }
    }
}