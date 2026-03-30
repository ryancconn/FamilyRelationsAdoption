using Verse; 
using RimWorld;
using Verse.AI;

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

            if (context.FirstSelectedPawn.ageTracker.AgeBiologicalYears < 18)
            {
                return new FloatMenuOption("FRA_MustBeAdultToAdopt".Translate(), null);
            }
            if (clickedPawn.ageTracker.AgeBiologicalYears > 18)
            {
                return new FloatMenuOption("FRA_CantAdoptAdult".Translate(), null); 
            }
            
            return new FloatMenuOption("FRA_AdoptAsChild".Translate(clickedPawn), () =>
            {
                Job job = JobMaker.MakeJob(FRA_DefOf.FRA_AdoptJob, clickedPawn);
                job.interaction = FRA_DefOf.FRA_AdoptionProposal; 
                context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc); 
            }); 
        }
    }
}