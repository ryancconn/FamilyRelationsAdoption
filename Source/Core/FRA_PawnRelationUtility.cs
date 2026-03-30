using System.Collections.Generic;
using RimWorld;
using Verse; 

namespace FamilyRelationsAdoption
{
    public static class FRA_PawnRelationUtility
    {
        public static List<Pawn> GetAdoptiveParents(this Pawn pawn)
        {
            if (!pawn.RaceProps.IsFlesh)
            {
                return null; 
            }
            if (pawn.relations == null)
            {
                return null; 
            }
            List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
            List<Pawn> adoptiveParents = []; 
            for (int i = 0; i < directRelations.Count; i++)
            {
                DirectPawnRelation directPawnRelation = directRelations[i]; 
                if (directPawnRelation.def == FRA_DefOf.FRA_AdoptiveParent)
                {
                    adoptiveParents.Add(directPawnRelation.otherPawn);                     
                }
            }
            return adoptiveParents;
        }

        public static void SetAdoptiveParent(this Pawn pawn, Pawn newParent)
        {
            if (newParent == null)
            {
                Log.Warning("Tried to set null pawn as " + pawn.ToString() + "'s adoptive parent.");
                return; 
            }
            // TODO: removal is not working. They can have unlimited adopted parents right now
            pawn.relations.AddDirectRelation(FRA_DefOf.FRA_AdoptiveParent, newParent); 
        }

        public static bool HasCommonParent(Pawn pawn, Pawn other)
        {
            if (!pawn.RaceProps.IsFlesh)
            {
                return false;
            }
            if (pawn.relations == null)
            {
                return false;
            }
            List<Pawn> pawnAdoptiveParents = pawn.GetAdoptiveParents(); 
            List<Pawn> otherAdoptiveParents = other.GetAdoptiveParents(); 
            if (pawnAdoptiveParents.Count > 0)
            {
                if (otherAdoptiveParents.Count > 0)
                {
                    return pawnAdoptiveParents.SharesElementWith(otherAdoptiveParents); 
                }
                else
                {
                    if (pawnAdoptiveParents.Contains(other.GetFather()) || pawnAdoptiveParents.Contains(other.GetMother()))
                    {
                        return true; 
                    }
                }
            }
            else
            {
                if (otherAdoptiveParents.Count > 0)
                {
                    if (otherAdoptiveParents.Contains(pawn.GetFather()) || otherAdoptiveParents.Contains(pawn.GetMother()))
                    {
                        return true; 
                    }
                }
            }
            return false; 
        }
    }
}