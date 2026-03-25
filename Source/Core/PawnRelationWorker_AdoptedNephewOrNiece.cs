using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedNephewOrNiece : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            if (PawnRelationDefOf.Child.Worker.InRelation(me, other) || FRA_DefOf.FRA_AdoptedChild.Worker.InRelation(me, other))
            {
                return false;
            }
            // Check if "other" is adopted child of "me"'s bio-sibling, bio-half-sibling, or adopted sibling
            PawnRelationWorker workerSibling = PawnRelationDefOf.Sibling.Worker; 
            PawnRelationWorker workerHalfSibling = PawnRelationDefOf.HalfSibling.Worker; 
            PawnRelationWorker workerAdoptedSibling = FRA_DefOf.FRA_AdoptedSibling.Worker; 
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (workerSibling.InRelation(me, ap) || workerHalfSibling.InRelation(me, ap) || workerAdoptedSibling.InRelation(me, ap))
                {
                    return true; 
                }
            }

            // Check if "other" is a bio-child of "me"'s adopted sibling
            if ((other.GetMother() != null && workerAdoptedSibling.InRelation(me, other.GetMother())) || (other.GetFather() != null && workerAdoptedSibling.InRelation(me, other.GetFather())))
            {
                return true;
            }

            return false;
        }
    }
}