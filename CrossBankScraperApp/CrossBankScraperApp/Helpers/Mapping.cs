using System;
using System.Linq.Expressions;

namespace CrossBankScraperApp.Helpers
{
    public abstract class Mapping<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> _projectFunc;

        public Mapping()
        {
            Projection = BuildProjection();
            _projectFunc = Projection.Compile();
        }

        public Expression<Func<TSource, TDestination>> Projection { get; }

        public TDestination Project(TSource source) { return _projectFunc(source); }

        protected abstract Expression<Func<TSource, TDestination>> BuildProjection();
    }
}