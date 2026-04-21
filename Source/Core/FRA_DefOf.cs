using RimWorld; 
using Verse; 

namespace FamilyRelationsAdoption
{
    [DefOf]
    public static class FRA_DefOf
    {
        public static PawnRelationDef FRA_AdoptiveParent; 

        public static PawnRelationDef FRA_AdoptedChild; 

        public static PawnRelationDef FRA_AdoptedSibling; 

        public static PawnRelationDef FRA_AdoptiveGrandparent; 

        public static PawnRelationDef FRA_AdoptedGrandchild; 

        public static PawnRelationDef FRA_AdoptedNephewOrNiece; 

        public static PawnRelationDef FRA_AdoptiveUncleOrAunt; 

        public static PawnRelationDef FRA_AdoptedCousin; 

        public static PawnRelationDef FRA_AdoptiveGreatGrandparent; 

        public static PawnRelationDef FRA_AdoptedGreatGrandchild; 

        public static PawnRelationDef FRA_AdoptiveGranduncleOrGrandaunt; 

        public static PawnRelationDef FRA_AdoptedGrandnephewOrGrandniece; 

        public static PawnRelationDef FRA_AdoptedCousinOnceRemoved; 

        public static PawnRelationDef FRA_AdoptedSecondCousin; 

        public static PawnRelationDef FRA_AdoptedKin; 

        public static JobDef FRA_AdoptJob; 

        public static InteractionDef FRA_AdoptionProposal; 

        public static RulePackDef FRA_Sentence_AdoptionProposalAccepted; 

        public static RulePackDef FRA_Sentence_AdoptionProposalRejected; 

        public static TaleDef FRA_WasAdopted; 

        public static ThoughtDef FRA_JustGotAdopted; 

        public static ThoughtDef FRA_JustAdoptedChild; 

        public static ThoughtDef FRA_FailedAdoptionProposalOnMe; 

        public static ThoughtDef FRA_RejectedMyAdoptionProposal; 

        public static ThoughtDef FRA_RejectedMyAdoptionProposalMood; 

        static FRA_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FRA_DefOf));
        }
    }
}
