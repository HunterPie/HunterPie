using HunterPie.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace HunterPie.Core.Domain.Mapper
{
    /// <summary>
    /// HunterPie's Map Factory for parsing objects into others
    /// easily.
    /// </summary>
    public static class MapFactory
    {
        private static readonly Dictionary<Type, Dictionary<Type, Type>> _mappers = new();

        /// <summary>
        /// Adds a new IMapper to the MapFactory
        /// </summary>
        /// <typeparam name="TIn">Type to map from</typeparam>
        /// <typeparam name="TOut">Type to map to</typeparam>
        /// <param name="mapper">Mapper implementation</param>
        public static void Add<TIn, TOut>(IMapper<TIn, TOut> mapper)
        {
            if (!_mappers.ContainsKey(typeof(TIn)))
                _mappers[typeof(TIn)] = new Dictionary<Type, Type>();

            if (_mappers[typeof(TIn)].ContainsKey(typeof(TOut)))
                return;

            _mappers[typeof(TIn)][typeof(TOut)] = mapper.GetType();
        }

        /// <summary>
        /// Find the mapper for a certain object into another and returns the mapped
        /// result
        /// </summary>
        /// <typeparam name="TIn">Type to map from</typeparam>
        /// <typeparam name="TOut">Type to map to</typeparam>
        /// <param name="object">Object to be mapped</param>
        /// <returns>Mapped object</returns>
        public static TOut Map<TIn, TOut>(TIn @object)
        {
            if (!_mappers.ContainsKey(@object.GetType()))
                return default;

            if (!_mappers[@object.GetType()].ContainsKey(typeof(TOut)))
                return default;

            Type mapperType = _mappers[@object.GetType()][typeof(TOut)];

            IMapper<TIn, TOut> mapper = (IMapper<TIn, TOut>)Activator.CreateInstance(mapperType);

            return mapper.Map(@object);
        }
    }
}
