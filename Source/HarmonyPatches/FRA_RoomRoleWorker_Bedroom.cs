using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib; 

namespace FamilyRelationsAdoption
{
    public class FRA_RoomRoleWorker_Bedroom
    {
        [HarmonyPatch(typeof(RoomRoleWorker_Bedroom), "IsBedroomHelper")]
        public static class FRA_IsBedroomHelper
        {
            static void Postfix(ref bool __result, List<Building_Bed> beds)
            {
                if (!__result)
                {
                    List<Pawn> childrenPatch = []; 
                    List<Pawn> adultsPatch = []; 
                    List<Pawn> listPatch = null;

                    foreach (Building_Bed bed in beds)
                    {
                        List<Pawn> ownersForReading = bed.OwnersForReading; 
                        if (ownersForReading.NullOrEmpty())
                        {
                            continue; 
                        }

                        foreach (Pawn item in ownersForReading)
                        {
                            if (item.DevelopmentalStage.Juvenile())
                            {
                                childrenPatch.Add(item); 
                                continue; 
                            }

                            adultsPatch.Add(item); 
                            listPatch ??= item.GetLoveCluster();
                            if (!listPatch.Contains(item))
                            {
                                return; // ?? idk i don't even want to be dealing with this case
                            }
                        }
                    }

                    foreach (Pawn child in childrenPatch)
                    {
                        List<Pawn> parents = child.GetAdoptiveParents(); 
                        parents.Add(child.GetMother()); 
                        parents.Add(child.GetFather()); 
                        
                        if (!adultsPatch.Any(parents.Contains))
                        {
                            __result = false; 
                            return; 
                        }
                        __result = true; 
                    }
                }
            }
        }
    }
}