using Verse;
using RimWorld;
using System.Collections.Generic;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedSibling : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other)
            {
                return false;
            }
            if (PawnRelationDefOf.Sibling.Worker.InRelation(me, other))
            {
                return false; 
            }
            List<Pawn> commonParents = [.. me.GetCommonParents(other)]; 
            if (commonParents.Count == 0)
            {
                return false;
            }
            else 
            {
                if (PawnRelationDefOf.HalfSibling.Worker.InRelation(me, other))
                {
                    if (me.HasSameFather(other))
                    {
                        commonParents.Remove(me.GetFather()); 
                    }
                    else if (me.HasSameMother(other))
                    {
                        commonParents.Remove(me.GetMother()); 
                    }
                }
                return commonParents.Count > 0; 
            }
        }

        public override float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            float num = 1f;
            return num; 
        }

        public override void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
        {
            
        }
    }
}
