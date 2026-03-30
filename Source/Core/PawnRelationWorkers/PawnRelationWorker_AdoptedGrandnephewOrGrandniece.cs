using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedGrandnephewOrGrandniece : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }

            PawnRelationWorker workerNephewOrNiece = PawnRelationDefOf.NephewOrNiece.Worker;
            PawnRelationWorker workerAdoptedNephewOrNiece = FRA_DefOf.FRA_AdoptedNephewOrNiece.Worker; 

            // Check if "other" is the adopted child of the bio-nephew or -niece of "me" 
            // or of the adopted nephew or niece of "me" 
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (workerNephewOrNiece.InRelation(me, ap) || workerAdoptedNephewOrNiece.InRelation(me, ap))
                {
                    return true; 
                }
            }

            // Check if "other" is the bio-child of the adopted nephew or niece of "me" 
            if ((other.GetMother() != null && workerAdoptedNephewOrNiece.InRelation(me, other.GetMother())) || (other.GetFather() != null && workerAdoptedNephewOrNiece.InRelation(me, other.GetFather())))
            {
                return true;
            }
            return false;
        }
    }
}