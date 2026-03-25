using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedGreatGrandchild : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }

            // Check if "other" is the bio-child of "me"'s adoptive grandchild 
            PawnRelationWorker workerAdoptedGrandchild = FRA_DefOf.FRA_AdoptedGrandchild.Worker; 
            if ((other.GetMother() != null && workerAdoptedGrandchild.InRelation(me, other.GetMother())) || (other.GetFather() != null && workerAdoptedGrandchild.InRelation(me, other.GetFather())))
            {
                return true; 
            }
            
            // Check if "other" is the adopted child of "me"'s adoptive grandchild
            // or of "me"'s bio-grandchild 
            PawnRelationWorker workerGrandchild = PawnRelationDefOf.Grandchild.Worker;
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (workerAdoptedGrandchild.InRelation(me, ap) || workerGrandchild.InRelation(me, ap))
                {
                    return true; 
                }
            }

            return false;
        }
    }
}