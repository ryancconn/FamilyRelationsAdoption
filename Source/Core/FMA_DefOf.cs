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

        static FRA_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FRA_DefOf));
        }
    }
}
