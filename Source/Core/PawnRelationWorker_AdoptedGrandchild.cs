using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption 
{
    public class PawnRelationWorker_AdoptedGrandchild : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }

            // Check if "other" is bio-child of "me"'s adopted child 
            PawnRelationWorker worker = FRA_DefOf.FRA_AdoptedChild.Worker; 
            if ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())))
            {
                return true;
            }
            // Check if "other" is an adopted child of "me"'s child 
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (me.gender == Gender.Male)
                {
                    if (ap.GetFather() == me)
                    {
                        return true; 
                    }
                }
                if (me.gender == Gender.Female)
                {
                    if (ap.GetMother() == me)
                    {
                        return true; 
                    }
                }
                // Or if "other" is an adopted child of "me"'s adopted child
                if (ap.GetAdoptiveParents().Contains(me))
                {
                    return true; 
                }
            }
            
            return false;
        }
    }
}