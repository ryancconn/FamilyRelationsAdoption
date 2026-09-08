using HarmonyLib;
using RimWorld;
using Verse;

namespace FamilyRelationsAdoption
{
    public class FRA_PawnRelationWorker_Stepfamily
    {
        public static bool CheckIfMarriedToNonDonorParent(Pawn potentialStepparent, Pawn potentialStepchild)
        {
            PawnRelationWorker workerSpouse = PawnRelationDefOf.Spouse.Worker; 
            PawnRelationWorker workerDonorChild = FRA_DefOf.FRA_DonorChild.Worker; 
            bool marriedToNonDonorMother = false; 
            bool marriedToNonDonorFather = false; 

            if (potentialStepchild.GetMother() != null && workerSpouse.InRelation(potentialStepchild.GetMother(), potentialStepparent))
            {
                marriedToNonDonorMother = !workerDonorChild.InRelation(potentialStepchild.GetMother(), potentialStepchild);
            }
            if (potentialStepchild.GetFather() != null && workerSpouse.InRelation(potentialStepchild.GetFather(), potentialStepparent))
            {
                marriedToNonDonorFather = !workerDonorChild.InRelation(potentialStepchild.GetFather(), potentialStepchild);
            }
            
            return marriedToNonDonorMother || marriedToNonDonorFather;
        }

        public static bool VerifyNotAdoptiveParent(Pawn potentialAdoptiveParent, Pawn potentialAdoptedChild)
        {
            return !FRA_DefOf.FRA_AdoptedChild.Worker.InRelation(potentialAdoptiveParent, potentialAdoptedChild); 
        }

        [HarmonyPatch(typeof(PawnRelationWorker_Stepparent), nameof(PawnRelationWorker_Stepparent.InRelation))]
        public static class FRA_PawnRelationWorker_Stepparent_InRelation
        {
            static void Postfix(ref bool __result, Pawn me, Pawn other)
            {
                if (__result)
                {
                    if (VerifyNotAdoptiveParent(other, me))
                    {
                        __result = CheckIfMarriedToNonDonorParent(other, me);
                    }
                    else
                    {
                        __result = false; 
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PawnRelationWorker_Stepchild), nameof(PawnRelationWorker_Stepchild.InRelation))]
        public static class FRA_PawnRelationWorker_Stepchild_InRelation
        {
            static void Postfix(ref bool __result, Pawn me, Pawn other)
            {
                if (__result)
                {
                    if (VerifyNotAdoptiveParent(other, me))
                    {
                        __result = CheckIfMarriedToNonDonorParent(other, me);
                    }
                    else
                    {
                        __result = false; 
                    }
                }
            }
        }
    }
}