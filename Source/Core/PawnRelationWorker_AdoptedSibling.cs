using UnityEngine;
using Verse;
using RimWorld; 

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedSibling : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            return FRA_PawnRelationUtility.HasCommonParent(me, other); 
        }

        public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            float num = 1f;
            return num; 
        }

        public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
        {
            
        }
    }
}
