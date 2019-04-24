namespace Common
{
    /// <summary>
    /// Retrieves reference arrays for name-parts for use with
    /// <see cref="ITagger"/> and <see cref="IIndividualChecker"/>.
    /// </summary>
    public interface IReferenceLoader
    {
        /// <summary>
        /// Gets reference array for prefixes.
        /// </summary>
        /// <returns></returns>
        string[] GetPrefixes();

        /// <summary>
        /// Gets reference array for given names.
        /// </summary>
        /// <returns></returns>
        string[] GetGivenNames();

        /// <summary>
        /// Gets reference array for surnames.
        /// </summary>
        /// <returns></returns>
        string[] GetSurnames();

        /// <summary>
        /// Gets reference array for suffixes.
        /// </summary>
        /// <returns></returns>
        string[] GetSuffixes();

        /// <summary>
        /// Gets reference array for words that indicate that name is of non-individual.
        /// For use with <see cref="IIndividualChecker"/>
        /// </summary>
        /// <returns></returns>
        string[] GetOrgIndicators();
    }
}
