using System.Collections.Generic;
using Julesabr.LibGit;

namespace Julesabr.GitBump.Tests.GitDetailsModel {
    internal class GivenBuilder {
        public IGitTag? LatestTag { get; set; }
        public IGitTag? LatestPrereleaseTag { get; set; }
        public IEnumerable<Commit>? LatestCommits { get; set; }
        public Options? Options { get; set; }

        public GivenBuilder And() {
            return this;
        }

        public When<GitDetails> When() {
            return new When<GitDetails>(new GitDetails(LatestTag!, LatestPrereleaseTag!, LatestCommits!, Options!));
        }
    }
}
