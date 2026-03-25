using Verse;
using RimWorld; 

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedChild : PawnRelationWorker
    {
        private const float PlayerStartRelationFactor = 10f;

        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            if (other.GetAdoptiveParents().Contains(me))
            {
                return true; 
            }
            return false;
        }

        public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            float num = 0f;
            // TODO: Not implementing chance of naturally generating adoptive relationships yet 
            return num; 
        }

        public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
        {
            other.SetAdoptiveParent(generated); 
        }
    }
}
