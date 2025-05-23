namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
    public class RequiredMemberAttribute : Attribute { }
    internal static class IsExternalInit { }
    public class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string featureName)
        {
            FeatureName = featureName;
        }
        public string FeatureName { get; }
        public bool IsOptional { get; init; }
        public const string RefStructs = nameof(RefStructs);
        public const string RequiredMembers = nameof(RequiredMembers);
    }
}