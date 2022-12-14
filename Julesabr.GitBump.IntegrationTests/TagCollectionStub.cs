using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.IntegrationTests {
    public class TagCollectionStub : TagCollection {
        public TagCollectionStub(IList<Tag> tags) : base(tags) {
        }
    }
}
