using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using UnityEngine.AI;
using System.Security.Cryptography.Pkcs;
using System.Configuration;

namespace FamilyRelationsAdoption
{
    public class FRA_InteractionWorker_AdoptionProposal : InteractionWorker
    {
        private const float MinAdoptionChanceForAttempt = 0.6f; 


        private const float BaseSelectionWeight = 0.8f; 


        private const int TryAdoptionCooldownTicks = 900000; 

        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (!initiator.DevelopmentalStage.Adult() || recipient.DevelopmentalStage.Adult())
            {
                return 0f; 
            }
            
            float num = BaseSelectionWeight; 
            float initOpinionOfRec = initiator.relations.OpinionOf(recipient); 
            float recOpinionOfInit = recipient.relations.OpinionOf(initiator); 

            if (initOpinionOfRec < 10f && recOpinionOfInit < 10f)
            {
                return 0f; 
            }

            num *= Mathf.InverseLerp(0f, 50f, initOpinionOfRec); 
            num *= Mathf.InverseLerp(0f, 50f, recOpinionOfInit); 

            // Make higher if child already has a parent (bio or adopted) that the initiator is 
            // the spouse of 

            // return num; 
            return 0f; 
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

            if (FamilyRelationsAdoptionMod.settings.autoSuccessAdoption)
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

            return num; 
        }

        private static float OpinionFactor(Pawn initiator, Pawn recipient)
        {
            float recOpinionOfInit = recipient.relations.OpinionOf(initiator); 
            // return Mathf.Clamp(Mathf.InverseLerp((float) MinOpinionForAdoptionProposal, 100f, recOpinionOfInit), 0.2f, 1f);  
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
            // TODO: give more weight to spouses of existing parents over lovers of them 
            // TODO: make dead spouses/lovers count for something but not as much 
            if (recipient.GetFather() != null)
            {
                foreach (DirectPawnRelation dpr in recipient.GetFather().GetLoveRelations(false))
                {
                    if (dpr.otherPawn == initiator)
                    {
                        return 2f; 
                    }
                }
            } 
            if (recipient.GetMother() != null)
            {
                foreach (DirectPawnRelation dpr in recipient.GetMother().GetLoveRelations(false))
                {
                    if (dpr.otherPawn == initiator)
                    {
                        return 2f; 
                    }
                }
            } 
            foreach (Pawn adoptiveParent in recipient.GetAdoptiveParents())
            {
                foreach (DirectPawnRelation dpr in adoptiveParent.GetLoveRelations(false))
                {
                    if (dpr.otherPawn == initiator)
                    {
                        return 1.9f; 
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
            StringBuilder stringBuilder = new StringBuilder(); 
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