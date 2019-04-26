namespace HmmNameParser
{
    /// <summary>
    /// Provides method for parsing
    /// </summary>
    /// <typeparam name="T">The type representing the parsed output.</typeparam>
    public interface IParser<T>
    {
        /// <summary>
        /// Parses the input <see cref="string"/> and returns <typeparamref name="T"/>, the type
        /// representing the parsed output
        /// </summary>
        /// <param name="input">The input <see cref="string"/> to parse</param>
        /// <returns></returns>
        T Parse(string input);
    }
}
