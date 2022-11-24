using System.Collections.Generic;
using LibGit2Sharp;

namespace Julesabr.GitBump.Tests {
    public class TagCollectionStub : TagCollection {
        private readonly IList<Tag> tags;

        public TagCollectionStub(IList<Tag> tags) {
            this.tags = tags;
        }

        public override IEnumerator<Tag> GetEnumerator() {
            return tags.GetEnumerator();
        }
    }
}
