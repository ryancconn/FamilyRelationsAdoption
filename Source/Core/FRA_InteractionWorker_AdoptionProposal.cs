using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System;
using System.Linq;

namespace FamilyRelationsAdoption
{
    public class FRA_InteractionWorker_AdoptionProposal : InteractionWorker
    {
        private const float MinAdoptionChanceForAttempt = 0.6f; 

        private const float BaseSelectionWeight = 1.1f; 

        private const int TryAdoptionCooldownTicks = 900000; 

        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (!AllowSpontaneousAdoptionProposal(initiator, recipient))
            {
                return 0f; 
            }

            float num = FamilyRelationsAdoptionMod.settings.baseSelectionWeight; 

            num *= OpinionFactor(initiator, recipient); 
            num *= OpinionFactor(recipient, initiator); 

            num *= ParentPartnerFactor(initiator, recipient); 

            num *= ChildAgeFactor(recipient); 

            return num; 
        }

        private static bool AllowSpontaneousAdoptionProposal(Pawn initiator, Pawn recipient)
        {
            if (!FamilyRelationsAdoptionMod.settings.allowSpontaneousAdoption)
            {
                return false; 
            }
            if (TutorSystem.TutorialMode)
            {
                return false;
            }
            if (initiator.Inhumanized())
            {
                return false;
            }
            if (!initiator.DevelopmentalStage.Adult() || recipient.DevelopmentalStage.Adult())
            {
                return false; 
            }
            if (recipient.GetBioAndAdoptiveParents().Contains(initiator))
            {
                return false; 
            }
            if (initiator.ageTracker.AgeBiologicalYears - recipient.ageTracker.AgeBiologicalYears < FamilyRelationsAdoptionMod.settings.minAgeDifferenceSpontaneous)
            {
                return false; 
            }

            // does this child have alive, non-absent, non-donor parents already? 
            // alive? 
            // in our faction? 
            int validBioParents = 0; 
            List<Pawn> bioParents = recipient.GetBioParents(false); 
            foreach (Pawn bioParent in bioParents)
            {
                if (!bioParent.Dead)
                {
                    if (bioParent.Faction == Faction.OfPlayer)
                    {
                        validBioParents++; 
                    }
                    else if (!FamilyRelationsAdoptionMod.settings.allowSpontaneousAdoptionForChildWithNonHostileParents) 
                    {
                        if (!bioParent.Faction.HostileTo(Faction.OfPlayer))
                        {
                            validBioParents++; 
                        }
                    }
                }
            }
            // child has a two-parent household already. sorry. 
            // or they have two divorced parents who are still alive. be happy with being a stepparent. 
            if (validBioParents == 2)
            {
                return false; 
            }
            // is initiator a lover of the one parent?
            else if (validBioParents == 1)
            {
                if (!bioParents[0].GetLoveCluster().Contains(initiator))
                {
                    return false; 
                }
            }
            // child has no non-donor parents either alive or in this colony 
            else // validParents == 0
            {
                // if initiator and recipient are bio-related, only return true if it fits the user settings criteria 
                foreach (PawnRelationDef relation in initiator.GetRelations(recipient))
                {
                    if (relation.familyByBloodRelation)
                    {
                        return FamilyRelationsAdoptionMod.settings.allowSpontaneousAdoptionForFamilyByBlood && recipient.ageTracker.AgeBiologicalYears <= FamilyRelationsAdoptionMod.settings.maxAgeForSpontaneousFamilyByBloodAdoption;
                    }
                }
            }

            List<Pawn> adoptiveParents = recipient.GetAdoptiveParents(); 
            if (adoptiveParents.Count == 2)
            {
                return false; 
            }
            if (adoptiveParents.Count == 1)
            {
                if (!adoptiveParents[0].GetLoveCluster().Contains(initiator))
                {
                    return false; 
                }
            }

            foreach (Thought_Memory thought in initiator.needs.mood?.thoughts.memories.Memories.FindAll(t => t.def == FRA_DefOf.FRA_RejectedMyAdoptionProposal))
            {
                if (((Thought_MemorySocial)thought).OtherPawn() == recipient)
                {
                    return false; 
                }
            }
            
            if (!MeetsOpinionThreshold(initiator, recipient))
            {
                return false; 
            }

            return true; 
        }

        private static bool MeetsOpinionThreshold(Pawn initiator, Pawn recipient)
        {
            float initOpinionOfRec = initiator.relations.OpinionOf(recipient); 
            float recOpinionOfInit = recipient.relations.OpinionOf(initiator); 

            if (initOpinionOfRec < FamilyRelationsAdoptionMod.settings.minOpinionForAdoptionProposal && recOpinionOfInit < FamilyRelationsAdoptionMod.settings.minOpinionForAdoptionProposal)
            {
                return false; 
            }
            return true; 
        }

        public static float SuccessChance(Pawn initiator, Pawn recipient)
        {
            if (recipient.Inhumanized())
            {
                return 0f;
            }
            // if (!initiator.DevelopmentalStage.Adult() || recipient.DevelopmentalStage.Adult())
            // {
            //     return 0f; 
            // }
            if (initiator.ageTracker.AgeBiologicalYears < 18 || recipient.ageTracker.AgeBiologicalYears >= 18)
            {
                return 0f; 
            }
            if (recipient.ageTracker.AgeBiologicalYears < 3)
            {
                return 1f; 
            }
            if (FamilyRelationsAdoptionMod.settings.allowAutoSuccessAdoption)
            {
                return 1f; 
            }

            float num = FamilyRelationsAdoptionMod.settings.baseAdoptionSuccessChance; 

            // Account for child's opinion of initiator
            num *= OpinionFactor(initiator, recipient); 

            // Check if initiator is a lover of one of child's existing parents
            num *= ParentPartnerFactor(initiator, recipient); 

            // Account for child's age 
            // The younger they are, the more likely to accept an adoption proposal 
            num *= ChildAgeFactor(recipient); 

            return Math.Clamp(num, 0f, 1f); 
        }

        private static float OpinionFactor(Pawn initiator, Pawn recipient)
        {
            float recOpinionOfInit = recipient.relations.OpinionOf(initiator); 
            if (recipient.ageTracker.AgeBiologicalYears < 3)
            {   // babies don't get a decision. 
                return 1f; 
            }
            if (recOpinionOfInit < FamilyRelationsAdoptionMod.settings.minOpinionForAdoptionProposal)
            {
                return 0f; 
            }
            return 0.0075f * recOpinionOfInit + 0.25f; 
        }

        private static float ParentPartnerFactor(Pawn initiator, Pawn recipient)
        {
            foreach (Pawn bioParent in recipient.GetBioParents(false))
            {
                foreach (DirectPawnRelation dpr in bioParent.GetLoveRelations(false))
                {
                    if (dpr.otherPawn == initiator)
                    {
                        float factor = 1.25f; 
                        if (dpr.def == PawnRelationDefOf.Spouse)
                        {
                            factor = 2f; 
                        }
                        if (dpr.def == PawnRelationDefOf.Fiance)
                        {
                            factor = 1.5f; 
                        }
                        if (bioParent.Dead)
                        {
                            factor *= 0.875f; 
                        }
                        return factor; 
                    }
                }
            }
            foreach (Pawn adoptiveParent in recipient.GetAdoptiveParents())
            {
                foreach (DirectPawnRelation dpr in adoptiveParent.GetLoveRelations(false))
                {
                    if (dpr.otherPawn == initiator)
                    {
                        float factor = 1.2f; 
                        if (dpr.def == PawnRelationDefOf.Spouse)
                        {
                            factor = 1.95f; 
                        }
                        if (dpr.def == PawnRelationDefOf.Fiance)
                        {
                            factor = 1.45f; 
                        }
                        if (adoptiveParent.Dead)
                        {
                            factor *= 0.875f; 
                        }
                        return factor; 
                    }
                }
            }
            return 1f; 
        }

        private static float ChildAgeFactor(Pawn recipient)
        {
            if (recipient.ageTracker.AgeBiologicalYears < 3)
            {
                return 1f; 
            }
            return (-(1f / 512f) * Mathf.Pow(recipient.ageTracker.AgeBiologicalYearsFloat, 2f)) + 1f;
        }

        public static string AdoptionFactors(Pawn adopter, Pawn adoptee)
        {
            StringBuilder stringBuilder = new(); 
            if (FamilyRelationsAdoptionMod.settings.baseAdoptionSuccessChance != 1f)
            {
                stringBuilder.AppendLine(AdoptionFactorLine("FRA_AdoptionChanceBase".Translate(), FamilyRelationsAdoptionMod.settings.baseAdoptionSuccessChance));
            }
            if (adoptee.ageTracker.AgeBiologicalYears >= 3)
            {
                stringBuilder.AppendLine(AdoptionFactorLine("FRA_AdoptionChanceOpinionFactor".Translate(), OpinionFactor(adopter, adoptee))); 
                stringBuilder.AppendLine(AdoptionFactorLine("FRA_AdoptionChanceChildAgeFactor".Translate(), ChildAgeFactor(adoptee))); 
            }
            if (ParentPartnerFactor(adopter, adoptee) != 1f)
            {
                stringBuilder.AppendLine(AdoptionFactorLine("FRA_AdoptionChanceParentPartnerFactor".Translate(), ParentPartnerFactor(adopter, adoptee))); 
            }
            return stringBuilder.ToString(); 
        }

        private static string AdoptionFactorLine(string label, float value)
        {
            return " - " + label + ": x".ToLower() + value.ToStringPercent(); 
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            if (Rand.Value < SuccessChance(initiator, recipient))
            {
                recipient.SetAdoptiveParent(initiator); 

                initiator.needs.mood?.thoughts.memories.TryGainMemory(FRA_DefOf.FRA_JustAdoptedChild, recipient);
                recipient.needs.mood?.thoughts.memories.TryGainMemory(FRA_DefOf.FRA_JustGotAdopted, initiator);

                TaleRecorder.RecordTale(FRA_DefOf.FRA_WasAdopted, initiator, recipient); 

                letterLabel = "FRA_LetterLabelAdoption".Translate(); 
                letterDef = LetterDefOf.PositiveEvent; 
                letterText = "FRA_LetterAdoption".Translate(initiator.Name.ToString(), recipient.Name.ToString(), recipient.gender == Gender.Female ? "daughter" : "son");
                lookTargets = new LookTargets(initiator, recipient);
                extraSentencePacks.Add(FRA_DefOf.FRA_Sentence_AdoptionProposalAccepted);
                
                Messages.Message("FRA_HasAdoptedAsChild".Translate(initiator, recipient, initiator.gender == Gender.Female ? "her" : "his"), MessageTypeDefOf.PositiveEvent, historical: false);
            }
            else
            {
                initiator.needs.mood?.thoughts.memories.TryGainMemory(FRA_DefOf.FRA_RejectedMyAdoptionProposal, recipient);
                recipient.needs.mood?.thoughts.memories.TryGainMemory(FRA_DefOf.FRA_FailedAdoptionProposalOnMe, initiator);

                extraSentencePacks.Add(FRA_DefOf.FRA_Sentence_AdoptionProposalRejected); 
                letterLabel = null; 
                letterDef = null; 
                letterText = null; 
                lookTargets = null; 
                
                if (initiator.CurJob?.def == FRA_DefOf.FRA_AdoptJob)
                {
                    Messages.Message("FRA_AdoptionRejected".Translate(initiator, recipient), MessageTypeDefOf.NegativeEvent, historical: false);
                }
            }
        }
    }
}