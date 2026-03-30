using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace FamilyRelationsAdoption
{
    public class FRA_InteractionWorker_AdoptionProposal : InteractionWorker
    {
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (!initiator.DevelopmentalStage.Adult() || recipient.DevelopmentalStage.Adult())
            {
                return 0f; 
            }
            // Currently trying to make this not happen without player action
            return 0f; 
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            recipient.SetAdoptiveParent(initiator); 

            TaleRecorder.RecordTale(FRA_DefOf.FRA_WasAdopted, initiator, recipient); 

            letterLabel = "FRA_LetterLabelAdoption".Translate(); 
            letterDef = LetterDefOf.PositiveEvent; 
            letterText = "FRA_LetterAdoption".Translate(initiator.Name.ToString(), recipient.Name.ToString(), recipient.gender == Gender.Female ? "daughter" : "son");
            lookTargets = new LookTargets(initiator, recipient);
            extraSentencePacks.Add(FRA_DefOf.FRA_Sentence_AdoptionProposalAccepted);
            
            Messages.Message("FRA_HasAdoptedAsChild".Translate(initiator, recipient, initiator.gender == Gender.Female ? "her" : "his"), MessageTypeDefOf.PositiveEvent, historical: false);
        }
    }
}