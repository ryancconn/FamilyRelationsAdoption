using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedCousinOnceRemoved : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }

            PawnRelationWorker workerCousin = PawnRelationDefOf.Cousin.Worker;
            PawnRelationWorker workerGranduncleOrGrandaunt = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
            PawnRelationWorker workerAdoptedCousin = FRA_DefOf.FRA_AdoptedCousin.Worker; 
            PawnRelationWorker workerAdoptiveGranduncleOrGrandaunt = FRA_DefOf.FRA_AdoptiveGranduncleOrGrandaunt.Worker; 

            // Check if "other"'s bio-parent is "me"'s adopted first cousin 
            if ((other.GetMother() != null && workerAdoptedCousin.InRelation(me, other.GetMother())) || (other.GetFather() != null && workerAdoptedCousin.InRelation(me, other.GetFather())))
            {
                return true;
            }

            // Check if "other"'s bio-parent is "me"'s adoptive granduncle or grandaunt 
            if ((other.GetMother() != null && workerAdoptiveGranduncleOrGrandaunt.InRelation(me, other.GetMother())) || (other.GetFather() != null && workerAdoptiveGranduncleOrGrandaunt.InRelation(me, other.GetFather())))
            {
                return true;
            }

            // Check if "other"'s adoptive parent is "me"'s bio-first-cousin or adopted first cousin 
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (workerCousin.InRelation(me, ap) || workerAdoptedCousin.InRelation(me, ap))
                {
                    return true; 
                }
            }

            // Check if "other"'s adoptive parent is "me"'s bio-granduncle or -grandaunt
            // or "me"'s adoptive granduncle or grandaunt
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (workerGranduncleOrGrandaunt.InRelation(me, ap) || workerAdoptiveGranduncleOrGrandaunt.InRelation(me, ap))
                {
                    return true; 
                }
            }

            return false;
        }
    }
}