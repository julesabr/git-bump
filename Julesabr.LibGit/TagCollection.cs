using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Julesabr.LibGit {
    public class TagCollection : IEnumerable<Tag> {
        private readonly IList<Tag> tags;

        protected internal TagCollection(IList<Tag> tags) {
            this.tags = tags;
        }

        [Pure]
        public virtual IEnumerator<Tag> GetEnumerator() {
            return tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
