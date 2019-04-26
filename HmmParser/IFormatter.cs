namespace HmmParser
{
    /// <summary>
    /// Provides methods for formatting <see cref="string"/>s in parsed <typeparamref name="T"/>
    /// </summary>
    public interface IFormatter<T>
    {
        /// <summary>
        /// Formats <see cref="string"/>s in parsed class <typeparamref name="T"/>
        /// </summary>
        /// <param name="parsed">the output of parsing with unformatted property values</param>
        /// <returns>A modified instance of the provided class <typeparamref name="T"/></returns>
        T Format(T parsed);
    }
}
