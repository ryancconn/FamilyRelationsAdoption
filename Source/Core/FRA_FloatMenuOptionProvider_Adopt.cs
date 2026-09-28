using Verse;
using RimWorld;
using Verse.AI;
using System.Text;

namespace FamilyRelationsAdoption
{
    public class FRA_FloatMenuOptionProvider_Adopt : FloatMenuOptionProvider
    {
        protected override bool Drafted => false; 
        protected override bool Undrafted => true; 
        protected override bool Multiselect => false; 

        protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
        {
            if (clickedPawn.Faction != Faction.OfPlayer || !clickedPawn.IsColonist)
            {
                return null; 
            }
            if (PawnRelationDefOf.Child.Worker.InRelation(context.FirstSelectedPawn, clickedPawn) || FRA_DefOf.FRA_AdoptedChild.Worker.InRelation(context.FirstSelectedPawn, clickedPawn))
            {
                return null; 
            }

            if (!FRA_GeneralUtility.IsAdult(context.FirstSelectedPawn))
            {
                return new FloatMenuOption("FRA_MustBeAdultToAdopt".Translate(), null);
            }
            if (FRA_GeneralUtility.IsAdult(clickedPawn))
            {
                return new FloatMenuOption("FRA_CannotAdoptAdult".Translate(), null); 
            }
            if (context.FirstSelectedPawn.ageTracker.AgeBiologicalYears - clickedPawn.ageTracker.AgeBiologicalYears < FamilyRelationsAdoptionMod.settings.minAgeDifferenceManual)
            {
                return new FloatMenuOption("FRA_TooCloseInAgeToAdopt".Translate(), null); 
            }
            if (clickedPawn.GetAdoptiveParents(false).Count >= FamilyRelationsAdoptionMod.settings.maxAdoptionsPerChild && FamilyRelationsAdoptionMod.settings.maxAdoptionsPerChild < 10)
            {
                return new FloatMenuOption("FRA_ChildAtMaxAdoptions".Translate(clickedPawn.Name.ToStringShort, FamilyRelationsAdoptionMod.settings.maxAdoptionsPerChild), null); 
            }

            foreach (Thought_Memory thought in context.FirstSelectedPawn.needs.mood?.thoughts.memories.Memories.FindAll(t => t.def == FRA_DefOf.FRA_RejectedMyAdoptionProposal))
            {
                if (((Thought_MemorySocial)thought).OtherPawn() == clickedPawn)
                {
                    return new FloatMenuOption("FRA_CannotAdoptCooldown".Translate(GenDate.ToStringTicksToPeriod(thought.DurationTicks - thought.age)), null); 
                }
            }
            
            float chance = FRA_InteractionWorker_AdoptionProposal.SuccessChance(context.FirstSelectedPawn, clickedPawn); 
            string chanceStr = "(" + chance.ToStringPercent() + " chance)";
            StringBuilder stringBuilder = new(); 
            stringBuilder.AppendLine(FRA_InteractionWorker_AdoptionProposal.AdoptionFactors(context.FirstSelectedPawn, clickedPawn)); 
            return new FloatMenuOption("FRA_AdoptAsChild".Translate(clickedPawn, chanceStr, stringBuilder.ToString()), () =>
            {
                Job job = JobMaker.MakeJob(FRA_DefOf.FRA_AdoptJob, clickedPawn);
                job.interaction = FRA_DefOf.FRA_AdoptionProposal; 
                context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc); 
            }); 
        }
    }
}