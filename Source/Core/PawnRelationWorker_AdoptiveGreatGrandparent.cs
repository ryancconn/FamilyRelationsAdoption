using Verse;
using RimWorld;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptiveGreatGrandparent : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            return FRA_DefOf.FRA_AdoptedGreatGrandchild.Worker.InRelation(other, me); 
        }
    }
}