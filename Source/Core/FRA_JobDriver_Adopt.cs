using Verse; 
using RimWorld;
using Verse.AI;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class FRA_JobDriver_Adopt : JobDriver
    {
        private const TargetIndex AdopteePawnInd = TargetIndex.A;

        private Pawn AdopteePawn => (Pawn)job.GetTarget(TargetIndex.A).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.GetTarget(TargetIndex.A), job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);

            // Go to the to-be-adopted pawn 
            // yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            Toil toil1 = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
            toil1.socialMode = RandomSocialMode.Off; 
            yield return toil1; 

            Toil toil2 = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
            yield return Toils_Jump.JumpIf(toil2, () => !AdopteePawn.Awake());

            Toil toil3 = Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
            toil3.socialMode = RandomSocialMode.Off;
            yield return toil3;

            toil2.socialMode = RandomSocialMode.Off; 
            yield return toil2; 

            yield return Toils_General.Do(delegate
            {
                if (!AdopteePawn.Awake())
                {
                    AdopteePawn.jobs.SuspendCurrentJob(JobCondition.InterruptForced);
                    if (!pawn.interactions.CanInteractNowWith(AdopteePawn, FRA_DefOf.FRA_AdoptionProposal))
                    {
                        Messages.Message("FRA_AdoptionFailedUnexpected".Translate(pawn, AdopteePawn), MessageTypeDefOf.NegativeEvent, historical: false);
                    }
                }
            });
            yield return Toils_Interpersonal.Interact(TargetIndex.A, job.interaction); 
        }
    }
}