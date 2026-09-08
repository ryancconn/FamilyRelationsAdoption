using Verse;
using RimWorld; 

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_DonorParent : PawnRelationWorker
    {
        public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
        {
            generated.SetDonorParent(other); 
        }
    }
}