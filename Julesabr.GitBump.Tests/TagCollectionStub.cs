using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.Tests {
    public class TagCollectionStub : TagCollection {
        public TagCollectionStub(IList<Tag> tags) : base(tags) {
        }
    }
}
