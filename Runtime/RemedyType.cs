using System;

namespace RemedySystem
{
    [Serializable]
    public abstract class RemedyType
    {
        public abstract string TypeName { get; }
        public abstract RemedyTypeSettings GetSettings(RemedyConfig remedyConfig); // Would rather this be in RemedyConfig but no clean way to make it source gen friendly.
    }
}