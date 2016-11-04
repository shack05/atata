﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class FindAttribute : Attribute
    {
        private const ScopeSource DefaultScope = ScopeSource.Parent;

        protected FindAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the index of the control. The default value is -1, meaning that the index is not used.
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Gets or sets the scope source. The default value is Inherit.
        /// </summary>
        public ScopeSource ScopeSource { get; set; }

        /// <summary>
        /// Gets or sets the strategy type for the control search. Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>. The default value is null, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy { get; set; }

        /// <summary>
        /// Gets the default strategy type for the control search. Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>.
        /// </summary>
        protected abstract Type DefaultStrategy { get; }

        public IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            Type strategyType = ResolveStrategyType(metadata);
            object[] strategyArguments = GetStrategyArguments().ToArray();

            return (IComponentScopeLocateStrategy)Activator.CreateInstance(strategyType, strategyArguments);
        }

        protected virtual IEnumerable<object> GetStrategyArguments()
        {
            yield break;
        }

        public Type ResolveStrategyType(UIComponentMetadata metadata)
        {
            return Strategy ?? GetFindSettings(metadata, x => x.Strategy != null)?.Strategy ?? DefaultStrategy;
        }

        public ScopeSource ResolveScopeSource(UIComponentMetadata metadata)
        {
            return ScopeSource != ScopeSource.Inherit
                ? ScopeSource
                : GetFindSettings(metadata, x => x.ScopeSource != ScopeSource.Inherit)?.ScopeSource ?? DefaultScope;
        }

        public int? ResolveIndex(UIComponentMetadata metadata)
        {
            return Index >= 0
                ? Index
                : GetFindSettings(metadata, x => x.Index >= 0)?.Index;
        }

        private FindSettingsAttribute GetFindSettings(UIComponentMetadata metadata, Func<FindSettingsAttribute, bool> predicate)
        {
            Type thisType = GetType();
            return metadata.GetFirstOrDefaultAttribute<FindSettingsAttribute>(x => x.FindAttributeType == thisType && predicate(x));
        }
    }
}
