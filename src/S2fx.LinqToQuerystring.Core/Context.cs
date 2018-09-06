namespace LinqToQuerystring
{
    using System;
    using System.Collections.Generic;

    using LinqToQuerystring.Utils;

    public class Context
    {
        public Func<Type, Type> DefaultTypeMap = (type) => type;

        public Func<Type, Type, Type> DefaultTypeConversionMap = (from, to) => to;

        /// <summary>
        /// Exstensibility point for specifying an alternate type mapping when casting to IEnumerable
        /// </summary>
        public Func<Type, Type> EnumerableTypeMap { get; set; }

        /// <summary>
        /// Exstensibility point for specifying an alternate type mapping when casting values
        /// </summary>
        public Func<Type, Type, Type> TypeConversionMap { get; set; }

        /// <summary>
        /// Allows the specification of custom tree nodes for particular situations, i.e Entity Framework include
        /// </summary>
        public IDictionary<string, CustomNodeMappings> CustomNodes { get; } = new Dictionary<string, CustomNodeMappings>();

        public void Reset()
        {
            EnumerableTypeMap = DefaultTypeMap;
            TypeConversionMap = DefaultTypeConversionMap;
            CustomNodes.Clear();
        }

        public Context()
        {
            Reset();
        }


        public static Context GlobalContext { get; } = new Context();
    }
}
