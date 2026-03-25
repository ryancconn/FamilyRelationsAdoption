using Verse;
using RimWorld; 

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptiveParent : PawnRelationWorker
    {
        private const float PlayerStartRelationFactor = 10f;

        public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            float num = 0f;
            // TODO: Not implementing chance of naturally generating adoptive relationships yet 
            return num; 
        }

        public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
        {
            generated.SetAdoptiveParent(other);
        }
    }
}
