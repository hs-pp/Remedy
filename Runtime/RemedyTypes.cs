namespace RemedySystem
{
    /// <summary>
    /// When calling Remedy.Log(), use RemedyTypes.ABC for quick access to all remedy types.
    /// The Remedy source generator will create a static class with all RemedyTypes like this:
    /// 
    ///     public partial class RemedyTypes
    ///     {
    ///         public static readonly RemedyType ABC = new ABCRemedyType(); // - ABC.cs
    ///         public static readonly RemedyType PrimaryLayout = new PrimaryLayoutRemedyType(); // - PrimaryLayout.cs
    ///         public static readonly RemedyType UICore = new UICoreRemedyType(); // - UICore.cs
    ///     }
    ///
    /// ex.
    ///     Remedy.Log(RemedyTypes.PrimaryLayout, LogVerbosity.Normal, "This is a debug message.");
    /// </summary>
    public abstract class RemedyType
    {
        public abstract string Name { get; }
    }
}