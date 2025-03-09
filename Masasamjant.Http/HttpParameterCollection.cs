using System.Collections;
using System.Reflection;

namespace Masasamjant.Http
{
    /// <summary>
    /// Represents collection of HTTP parameters.
    /// </summary>
    public sealed class HttpParameterCollection : ICollection<HttpParameter>, IEnumerable<HttpParameter>
    {
        private readonly HashSet<HttpParameter> parameters;

        /// <summary>
        /// Initializes new instance of the <see cref="HttpParameterCollection"/> class.
        /// </summary>
        public HttpParameterCollection()
        {
            parameters = new HashSet<HttpParameter>();
        }

        /// <summary>
        /// Gets count of parameters in collection.
        /// </summary>
        public int Count => parameters.Count;

        /// <summary>
        /// Add specified <see cref="HttpParameter"/> to collection.
        /// </summary>
        /// <param name="parameter">The <see cref="HttpParameter"/> to add.</param>
        /// <exception cref="ArgumentException">If collection already contains parameter with same name.</exception>
        public void Add(HttpParameter parameter)
        {
            if (!parameters.Add(parameter))
                throw new ArgumentException($"The collection already contains '{parameter.Name}' parameter.", nameof(parameter));   
        }

        /// <summary>
        /// Add <see cref="HttpParameter"/> with specified name and value.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>A added <see cref="HttpParameter"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is empty or contains only white-space characters.</exception>
        /// <exception cref="ArgumentException">If collection already contains parameter with same name.</exception>
        public HttpParameter Add(string name, object? value = null)
        {
            var parameter = new HttpParameter(name, value);
            Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Clear collection.
        /// </summary>
        public void Clear() => parameters.Clear();

        /// <summary>
        /// Check if collection contains specified <see cref="HttpParameter"/>.
        /// </summary>
        /// <param name="parameter">The <see cref="HttpParameter"/>.</param>
        /// <returns><c>true</c> if collection contains <paramref name="parameter"/>; <c>false</c> otherwise.</returns>
        public bool Contains(HttpParameter parameter) => parameters.Contains(parameter);

        /// <summary>
        /// Check if collection contains <see cref="HttpParameter"/> with specified name.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <returns><c>true</c> if collection contains <see cref="HttpParameter"/> with specified name; <c>false</c> otherwise.</returns>
        public bool Contains(string name)
        {
            return parameters.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Creates <see cref="HttpParameterCollection"/> from properties of specified object instance. 
        /// Properties decorated with <see cref="HttpParameterAttribute"/> are considered to be HTTP parameters.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <returns>A <see cref="HttpParameterCollection"/>.</returns>
        public static HttpParameterCollection Create(object instance)
        {
            var collection = new HttpParameterCollection();

            var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (var property in properties)
            {
                var parameterAttribute = property.GetCustomAttribute<HttpParameterAttribute>(false);

                if (parameterAttribute == null)
                    continue;

                var parameterName = parameterAttribute.ParameterName;
                var parameterValue = property.GetValue(instance, null);
                collection.Add(parameterName, parameterValue);
            }

            return collection;
        }

        /// <summary>
        /// Gets enumerator to iterate all parameters in collection.
        /// </summary>
        /// <returns>A enumerator to iterate parameters.</returns>
        public IEnumerator<HttpParameter> GetEnumerator()
        {
            foreach (var parameter in parameters)
                yield return parameter;
        }

        /// <summary>
        /// Removes specified <see cref="HttpParameter"/> from collection.
        /// </summary>
        /// <param name="parameter">The <see cref="HttpParameter"/> to remove.</param>
        /// <returns><c>true</c> <paramref name="parameter"/> was removed; <c>false</c> if not in collection.</returns>
        public bool Remove(HttpParameter parameter)
        {
            return parameters.Remove(parameter);
        }

        /// <summary>
        /// Removes <see cref="HttpParameter"/> specified by name from collection.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <returns><c>true</c> if collection contained parameter with specified name; <c>false</c> otherwise.</returns>
        public bool Remove(string name)
        {
            var parameter = parameters.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

            if (parameter == null)
                return false;

            return parameters.Remove(parameter);
        }

        bool ICollection<HttpParameter>.IsReadOnly => false;

        void ICollection<HttpParameter>.CopyTo(HttpParameter[] array, int arrayIndex)
        {
            parameters.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
