using Verse;
using RimWorld;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptiveUncleOrAunt : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            if (PawnRelationDefOf.Parent.Worker.InRelation(me, other) || FRA_DefOf.FRA_AdoptiveParent.Worker.InRelation(me, other))
            {
                return false;
            }

            return FRA_DefOf.FRA_AdoptedNephewOrNiece.Worker.InRelation(other, me); 
        }
    }
}