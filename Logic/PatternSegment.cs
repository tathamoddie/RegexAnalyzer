using System;

namespace TathamOddie.RegexAnalyzer.Logic
{
    internal class PatternSegment<T>
    {
        readonly Func<T, bool> predicate;
        readonly int? count;

        public PatternSegment(Func<T, bool> predicate)
        {
            this.predicate = predicate;
            count = null;
        }

        public PatternSegment(Func<T, bool> predicate, int count)
        {
            this.predicate = predicate;
            this.count = count;
        }

        public Func<T, bool> Predicate
        {
            get { return predicate; }
        }

        public int? Count
        {
            get { return count; }
        }
    }
}