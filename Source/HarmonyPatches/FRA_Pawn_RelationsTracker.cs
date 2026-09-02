using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;

namespace FamilyRelationsAdoption
{
    public class FRA_Pawn_RelationsTracker
    {
        static List<PawnRelationDef> alwaysIncludeRelations = [];

        private static bool ShouldAutoIncludeThisRelation(PawnRelationDef relation)
        {
            if (alwaysIncludeRelations.NullOrEmpty())
            {
                alwaysIncludeRelations = [PawnRelationDefOf.ParentBirth, PawnRelationDefOf.ExLover, PawnRelationDefOf.ExSpouse, PawnRelationDefOf.Fiance, PawnRelationDefOf.Lover, PawnRelationDefOf.Spouse];
            }
            return alwaysIncludeRelations.Contains(relation); 
        }
        
        public static IEnumerable<PawnRelationDef> RemoveExtraRelationsForOpinion(IEnumerable<PawnRelationDef> relations)
        {
            bool adopted = false; 
            PawnRelationDef mostImportantCandidateRelation = null; 
            List<PawnRelationDef> relationsToUse = []; 
            foreach (PawnRelationDef relation in relations)
            {
                if (relation.defName.Contains("Adopt"))
                {
                    adopted = true; 
                    break; 
                }
            }
            if (adopted)
            {
                foreach (PawnRelationDef relation in relations)
                {
                    if (ShouldAutoIncludeThisRelation(relation))
                    {
                        relationsToUse.Add(relation); 
                    }
                    else
                    {
                        if (mostImportantCandidateRelation == null)
                        {
                            mostImportantCandidateRelation = relation; 
                        }
                        else
                        {
                            if (relation.importance > mostImportantCandidateRelation.importance)
                            {
                                mostImportantCandidateRelation = relation; 
                            }
                        }
                    }
                }
                if (mostImportantCandidateRelation != null) 
                {
                    relationsToUse.Add(mostImportantCandidateRelation); 
                }
            }
            else
            {
                return relations; 
            }
            return relationsToUse; 
        }

        public static IEnumerable<CodeInstruction> TranspilerWrapper(IEnumerable<CodeInstruction> instructions)
        {
            bool done = false; 

            foreach (var i in instructions)
            {
                yield return i; 
                if (!done && i.opcode == OpCodes.Call && i.Calls(AccessTools.Method(typeof(PawnRelationUtility), nameof(PawnRelationUtility.GetRelations))))    // IL_00d0: call class [mscorlib]System.Collections.Generic.IEnumerable`1<class RimWorld.PawnRelationDef> RimWorld.PawnRelationUtility::GetRelations(class Verse.Pawn, class Verse.Pawn) /* 0600C8D1 */
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FRA_Pawn_RelationsTracker), nameof(FRA_Pawn_RelationsTracker.RemoveExtraRelationsForOpinion)));
                    done = true; 
                }
            }
            if (!done)
            {
                Log.Message("[FamilyRelationsAdoption] Pawn_RelationsTracker.OpinionOf and Pawn_RelationsTracker.OpinionExplanation patch didn't work");
            }
        }

        public static readonly FieldInfo pawnReflection = AccessTools.Field(typeof(Pawn_RelationsTracker), "pawn"); 

        [HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.OpinionOf))]
        public static class FRA_OpinionOf
        {
            // static bool Prefix(ref int __result, Pawn_RelationsTracker __instance, Pawn other)
            // {
            //     Pawn pawn = (Pawn)pawnReflection.GetValue(__instance);
            //     IEnumerable<PawnRelationDef> relations = pawn.GetRelations(other); 
            //     bool adopted = false; 
            //     foreach (PawnRelationDef relation in relations)
            //     {
            //         if (relation.defName.Contains("Adopt"))
            //         {
            //             adopted = true; 
            //             break; 
            //         }
            //     }
            //     if (adopted)
            //     {
            //         if (!other.RaceProps.Humanlike || pawn == other || pawn.Dead)
            //         {
            //             __result = 0; 
            //             return false; 
            //         }
            //         Log.Message("pawn : " + pawn.Name + "; other : " + other.Name + "; relations : " + ListToString(relations));
            //         int num = 0;
            //         PawnRelationDef mostImportantCandidateRelation = null; 
            //         List<PawnRelationDef> alwaysIncludeRelations = [PawnRelationDefOf.ParentBirth, PawnRelationDefOf.ExLover, PawnRelationDefOf.ExSpouse, PawnRelationDefOf.Fiance, PawnRelationDefOf.Lover, PawnRelationDefOf.Spouse];
            //         foreach (PawnRelationDef relation in relations)
            //         {
            //             if (!alwaysIncludeRelations.Contains(relation))
            //             {
            //                 if (mostImportantCandidateRelation == null)
            //                 {
            //                     mostImportantCandidateRelation = relation; 
            //                 }
            //                 else
            //                 {
            //                     if (relation.importance > mostImportantCandidateRelation.importance)
            //                     {
            //                         mostImportantCandidateRelation = relation; 
            //                     }
            //                 }
            //             }
            //             else
            //             {
            //                 num += relation.opinionOffset; 
            //             }
            //         }
            //         num += mostImportantCandidateRelation.opinionOffset; 
            //         if (pawn.RaceProps.Humanlike && pawn.needs.mood != null)
            //         {
            //             num += pawn.needs.mood.thoughts.TotalOpinionOffset(other); 
            //         }
            //         if (num != 0)
            //         {
            //             float num2 = 1f; 
            //             List<Hediff> hediffs = pawn.health.hediffSet.hediffs; 
            //             for (int i = 0; i < hediffs.Count; i++)
            //             {
            //                 if (hediffs[i].CurStage != null)
            //                 {
            //                     num2 *= hediffs[i].CurStage.opinionOfOthersFactor; 
            //                 }
            //             }
            //             num = Mathf.RoundToInt((float)num * num2); 
            //         }
            //         if (num > 0 && pawn.HostileTo(other))
            //         {
            //             num = 0; 
            //         }
            //         __result = Mathf.Clamp(num, -100, 100); 
            //         return false; 
            //     }
            //     return true; 
            // }
        
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return TranspilerWrapper(instructions); 
            }
        }

        [HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.OpinionExplanation))]
        public static class FRA_OpinionExplanation
        {   
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return TranspilerWrapper(instructions); 
            }
        }

        public static string ListToString(IEnumerable<PawnRelationDef> rels)
            {
                StringBuilder sb = new(); 
                foreach (PawnRelationDef rel in rels)
                {
                    sb.Append(rel.defName + ", ");
                }
                return sb.ToString(); 
            }
    }
}