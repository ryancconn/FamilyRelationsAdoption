using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptiveGranduncleOrGrandaunt : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            if (PawnRelationDefOf.Grandparent.Worker.InRelation(me, other) || FRA_DefOf.FRA_AdoptiveGrandparent.Worker.InRelation(me, other))
            {
                return false;
            }

            PawnRelationWorker workerGreatGrandparent = PawnRelationDefOf.GreatGrandparent.Worker; 
            PawnRelationWorker workerAdoptiveGreatGrandparent = FRA_DefOf.FRA_AdoptiveGreatGrandparent.Worker; 

            // Check if "other" is the bio-great-grandchild of "me"s adoptive parent
            // or if "other" is the adopted great grandchild of "me"'s adoptive parent 
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (workerGreatGrandparent.InRelation(me, ap) || workerAdoptiveGreatGrandparent.InRelation(me, ap))
                {
                    return true; 
                }
            }

            // Check if "other" is the adopted great grandchild of "me"'s bio-parent
            if ((other.GetMother() != null && workerAdoptiveGreatGrandparent.InRelation(me, other.GetMother())) || (other.GetFather() != null && workerAdoptiveGreatGrandparent.InRelation(me, other.GetFather())))
            {
                return true; 
            }

            return false;
        }
    }
}