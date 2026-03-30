using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace FamilyRelationsAdoption
{
    public class PawnRelationWorker_AdoptedKin : PawnRelationWorker
    {
        public override bool InRelation(Pawn me, Pawn other)
        {
            if (me == other || me.RaceProps.Animal != other.RaceProps.Animal)
            {
                return false;
            }
            
            // TODO: I have no idea how to do this yet because the bio-kin implementation 
            // is done using me.relations.FamilyByBlood 
            return false; 
        }
    }
}