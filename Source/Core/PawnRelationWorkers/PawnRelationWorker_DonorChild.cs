using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_DonorChild : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            List<DirectPawnRelation> directRelations = other.relations.DirectRelations; 
            foreach (DirectPawnRelation relation in directRelations)
            {
                if (relation.def == FRA_DefOf.FRA_DonorParent && relation.otherPawn == me)
                {
                    return true; 
                }
            }
            return false;
        }

        public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
        {
            other.SetDonorParent(generated); 
        }
    }
}