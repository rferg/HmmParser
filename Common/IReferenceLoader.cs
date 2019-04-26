namespace Common
{
    /// <summary>
    /// Retrieves reference arrays for name/address-parts for use with
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

        /// <summary>
        /// Gets reference array for surname prefixes, e.g., 'Van' or 'De'
        /// </summary>
        /// <returns></returns>
        string[] GetSurnamePrefixes();

        /// <summary>
        /// Gets reference array for street types, e.g. 'St.', 'Blvd.'
        /// </summary>
        /// <returns></returns>
        string[] GetStreetTypes();

        /// <summary>
        /// Gets reference array for unit types, e.g., 'Apt.'
        /// </summary>
        /// <returns></returns>
        string[] GetUnitTypes();

        /// <summary>
        /// Gets reference array for PO Box indicators
        /// </summary>
        /// <returns></returns>
        string[] GetBoxes();

        /// <summary>
        /// Gets reference array for U.S. State abbreviations
        /// </summary>
        /// <returns></returns>
        string[] GetUSStates();

        /// <summary>
        /// Gets reference array for words that indicate city
        /// </summary>
        /// <returns></returns>
        string[] GetCityIndicators();

        /// <summary>
        /// Gets reference array for direction words, e.g. NW
        /// </summary>
        /// <returns></returns>
        string[] GetDirections();

        /// <summary>
        /// Gets reference array of common US street names
        /// </summary>
        /// <returns></returns>
        string[] GetCommonStreetNames();
    }
}
