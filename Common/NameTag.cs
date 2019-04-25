namespace Common
{
    /// <summary>
    /// Name-part tags that apply to words in a name and are the inputs to the HMM.
    /// </summary>
    public enum NameTag
    {
        Unknown = 0,
        Prefix = 1,
        GivenName = 2,
        Surname = 3,
        Suffix = 4,
        SurnamePrefix = 5
    }
}
