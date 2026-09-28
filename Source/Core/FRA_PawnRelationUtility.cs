using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace FamilyRelationsAdoption
{
    public static class FRA_PawnRelationUtility
    {
        public static List<Pawn> GetAdoptiveParents(this Pawn pawn, bool includeDead = true)
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
                    if (!includeDead && !directPawnRelation.otherPawn.Dead)
                    {
                        adoptiveParents.Add(directPawnRelation.otherPawn); 
                    }
                    else
                    {
                        adoptiveParents.Add(directPawnRelation.otherPawn); 
                    }
                }
            }
            return adoptiveParents;
        }

        public static List<Pawn> GetBioParents(this Pawn pawn, bool includeDonorParents = true)
        {
            List<Pawn> bioParents = []; 
            Pawn father = pawn.GetFather(); 
            Pawn mother = pawn.GetMother(); 
            if (father != null)
            {
                if (includeDonorParents)
                {
                    bioParents.Add(father); 
                }
                else
                {
                    if (!FRA_DefOf.FRA_DonorChild.Worker.InRelation(father, pawn))
                    {
                        bioParents.Add(father); 
                    }
                }
            }
            if (mother != null)
            {
                if (includeDonorParents)
                {
                    bioParents.Add(mother); 
                }
                else
                {
                    if (!FRA_DefOf.FRA_DonorChild.Worker.InRelation(mother, pawn))
                    {
                        bioParents.Add(mother); 
                    }
                }
            }
            return bioParents; 
        }

        public static List<Pawn> GetBioAndAdoptiveParents(this Pawn pawn, bool includeDonorParents = true)
        {
            List<Pawn> allParents = pawn.GetAdoptiveParents(); 
            allParents.AddRange(pawn.GetBioParents(includeDonorParents)); 
            return allParents; 
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

        public static IEnumerable<Pawn> GetCommonParents(this Pawn pawn, Pawn other, bool includeDonorParents = false)
        {
            List<Pawn> pawnAllParents = pawn.GetBioAndAdoptiveParents(includeDonorParents); 
            List<Pawn> otherAllParents = other.GetBioAndAdoptiveParents(includeDonorParents);
            if (pawnAllParents.Count > 0 && otherAllParents.Count > 0)
            {
                return pawnAllParents.Intersect(otherAllParents); 
            }
            return []; 
        }

        public static void SetDonorParent(this Pawn pawn, Pawn newParent)
        {
            if (newParent == null)
            {
                Log.Warning("Tried to set null pawn as " + pawn.ToString() + "'s donor parent.");
                return;
            }
            pawn.relations.AddDirectRelation(FRA_DefOf.FRA_DonorParent, newParent); 
        }
    }
}