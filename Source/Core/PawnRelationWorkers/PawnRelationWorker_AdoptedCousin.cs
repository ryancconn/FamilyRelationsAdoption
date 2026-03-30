using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedCousin : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            // Check if "other" is the bio-child of "me"'s adoptive uncle or aunt 
            PawnRelationWorker workerAdoptiveUncleOrAunt = FRA_DefOf.FRA_AdoptiveUncleOrAunt.Worker;
            if ((other.GetMother() != null && workerAdoptiveUncleOrAunt.InRelation(me, other.GetMother())) || (other.GetFather() != null && workerAdoptiveUncleOrAunt.InRelation(me, other.GetFather())))
            {
                return true;
            }

            // Check if "other" is the adopted child of "me"'s bio-uncle or -aunt
            // or of "me"'s adoptive uncle or aunt 
            PawnRelationWorker workerUncleOrAunt = PawnRelationDefOf.UncleOrAunt.Worker; 
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (workerUncleOrAunt.InRelation(me, ap) || workerAdoptiveUncleOrAunt.InRelation(me, ap))
                {
                    return true; 
                }
            }

            return false;
        }
    }
}