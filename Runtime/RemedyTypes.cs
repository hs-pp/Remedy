namespace RemedySystem
{
    // THIS ERROR:
    //      warning CS0436: The type 'RemedyTypes' in '.../Runtime/RemedyTypes.cs' conflicts with the imported type 'RemedyTypes' in 'AssetValidator.Demo,
    //      Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'. Using the type defined in '.../Runtime/RemedyTypes.cs'.
    // This is because if the source generator is not in an asmdef, it runs for all DLLs in the project.
    // To avoid this error, have the source generator only generate code when necessary or mark classes as internal.
    // More help: https://discussions.unity.com/t/roslyn-analyzer-generator-scope-works-unexpectedly/901458/4
    
    public static partial class RemedyTypes
    {
        public static RemedyType Uncategorized = new UncategorizedType();
    }

    public class UncategorizedType : RemedyType
    {
        public override string TypeName => "Uncategorized";
        public override RemedyTypeSettings GetSettings(RemedyConfig remedyConfig)
        {
            return remedyConfig.UnorganizedTypeSettings;
        }
    }
}