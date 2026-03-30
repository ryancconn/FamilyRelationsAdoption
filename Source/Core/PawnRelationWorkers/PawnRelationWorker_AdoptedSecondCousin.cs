using Verse;
using RimWorld;
using System.Collections.Generic;
using System;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedSecondCousin : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            
            PawnRelationWorker worker = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
            Pawn mother = other.GetMother();

            // Check if "other"'s parent's parent is "me"'s granduncle or grandaunt
            if (mother != null && ((mother.GetMother() != null && worker.InRelation(me, mother.GetMother())) || (mother.GetFather() != null && worker.InRelation(me, mother.GetFather()))))
            {
                return true;
            }
            Pawn father = other.GetFather();
            if (father != null && ((father.GetMother() != null && worker.InRelation(me, father.GetMother())) || (father.GetFather() != null && worker.InRelation(me, father.GetFather()))))
            {
                return true;
            }

            PawnRelationWorker workerGranduncleOrGrandaunt = PawnRelationDefOf.GranduncleOrGrandaunt.Worker; 
            PawnRelationWorker workerAdoptiveGranduncleOrGrandaunt = FRA_DefOf.FRA_AdoptiveGranduncleOrGrandaunt.Worker; 

            // Check if "other"'s bio-grandparent is "me"'s adoptive granduncle or grandaunt
            Pawn otherMother = other.GetMother(); 
            if (otherMother != null && ((otherMother.GetMother() != null && workerAdoptiveGranduncleOrGrandaunt.InRelation(me, otherMother.GetMother())) || (otherMother.GetFather() != null && workerAdoptiveGranduncleOrGrandaunt.InRelation(me, otherMother.GetFather()))))
            {
                return true; 
            }
            Pawn otherFather = other.GetFather(); 
            if (otherFather != null && ((otherFather.GetMother() != null && workerAdoptiveGranduncleOrGrandaunt.InRelation(me, otherFather.GetMother())) || (otherFather.GetFather() != null && workerAdoptiveGranduncleOrGrandaunt.InRelation(me, otherFather.GetFather()))))
            {
                return true; 
            }

            // Check if "other"'s bio-parent is the adopted child of "me"'s adoptive granduncle or grandaunt 
            // or of "me"'s bio-granduncle or -grandaunt
            if (otherMother != null)
            {
                List<Pawn> otherMotherAdoptiveParents = otherMother.GetAdoptiveParents(); 
                foreach (Pawn ap in otherMotherAdoptiveParents)
                {
                    if (workerAdoptiveGranduncleOrGrandaunt.InRelation(me, ap) || workerGranduncleOrGrandaunt.InRelation(me, ap))
                    {
                        return true; 
                    }
                }
            }
            if (otherFather != null)
            {
                List<Pawn> otherFatherAdoptiveParents = otherFather.GetAdoptiveParents(); 
                foreach (Pawn ap in otherFatherAdoptiveParents)
                {
                    if (workerAdoptiveGranduncleOrGrandaunt.InRelation(me, ap) || workerGranduncleOrGrandaunt.InRelation(me, ap))
                    {
                        return true; 
                    }
                }
            }

            // Check if "other"'s adopted parent is the bio-child of "me"'s adoptive granduncle or grandaunt
            // or of "me"'s bio-granduncle or -grandaunt
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                if (ap.GetMother() != null)
                {
                    if (workerAdoptiveGranduncleOrGrandaunt.InRelation(me, ap.GetMother()) || workerGranduncleOrGrandaunt.InRelation(me, ap.GetMother()))
                    {
                        return true; 
                    }
                }
                if (ap.GetFather() != null)
                {
                    if (workerAdoptiveGranduncleOrGrandaunt.InRelation(me, ap.GetFather()) || workerGranduncleOrGrandaunt.InRelation(me, ap.GetFather()))
                    {
                        return true; 
                    }

                }
            }

            // Check if "other"'s adopted parent is the adopted child of "me"'s adoptive granduncle or grandaunt
            // or of "me"'s bio-granduncle or -grandaunt 
            foreach (Pawn ap in otherAdoptiveParents)
            {
                List<Pawn> apAdoptiveParents = ap.GetAdoptiveParents(); 
                foreach (Pawn apAp in apAdoptiveParents)
                {
                    if (workerAdoptiveGranduncleOrGrandaunt.InRelation(me, apAp) || workerGranduncleOrGrandaunt.InRelation(me, apAp))
                    {
                        return true; 
                    }
                }
            }

            return false;
        }
    }
}