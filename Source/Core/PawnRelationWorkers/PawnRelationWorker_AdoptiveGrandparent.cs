using Verse;
using RimWorld; 

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptiveGrandparent : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            return FRA_DefOf.FRA_AdoptedGrandchild.Worker.InRelation(other, me);
        }
    }
}
