using System.Collections.Generic;
using Exebite.Converters.Delimiters;

namespace Exebite.Converters
{
    /// <summary>
    /// Value separated converter
    /// </summary>
    public interface IValueSeparatedConverter
    {
        /// <summary>
        /// Accepts a list of objects to be serialized to value separated file.
        /// </summary>
        /// <typeparam name="T">Object definition to be serialized</typeparam>
        /// <param name="input">Collection of objects to be serialized</param>
        /// <param name="delimiter">Delimiter to be used</param>
        /// <returns>CSV string</returns>
        string Serialize<T>(IEnumerable<T> input, Delimiter delimiter) where T : class;

        /// <summary>
        /// Deserializes the value separated file and maps output to collection of objects.
        /// </summary>
        /// <typeparam name="T">Object definition to be deserialized</typeparam>
        /// <param name="inputLines">Lines to be split</param>
        /// <param name="delimiter">The delimiter</param>
        /// <returns>Collection if the <typeparamref name="T"/></returns>
        IEnumerable<T> Deserialize<T>(string[] inputLines, Delimiter delimiter) where T : new();
    }
}
