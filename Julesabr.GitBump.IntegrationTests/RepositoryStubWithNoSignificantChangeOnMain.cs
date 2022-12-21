using System.Collections.Generic;
using Julesabr.LibGit;
using NSubstitute;

namespace Julesabr.GitBump.IntegrationTests {
    public static class RepositoryStubWithNoSignificantChangeOnMain {
        public static IRepository Create() {
            Commit commit1 = Substitute.For<Commit>();
            commit1.Sha.Returns("62e880e5f27b18e0e54fd655e294c85aaf152a6b");
            commit1.Message.Returns("Initial Commit");
            commit1.MessageFull.Returns("Initial Commit");

            Commit commit2 = Substitute.For<Commit>();
            commit2.Sha.Returns("71db3ceec751d8124dc1b9ab9d7247c7ca349ec0");
            commit2.Message.Returns("chore: Update README.md");
            commit2.MessageFull.Returns("chore: Update README.md");

            Commit commit3 = Substitute.For<Commit>();
            commit3.Sha.Returns("8d8841f8cff4dc50701ec08f149ae09c16c98b39");
            commit3.Message.Returns("chore: Setup");
            commit3.MessageFull.Returns("chore: Setup");

            Tag tag1 = Substitute.For<Tag>();
            tag1.Name.Returns("v1.2.3");
            tag1.IsAnnotated.Returns(true);
            tag1.Target.Returns(commit3);

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);

            IList<Tag> tags = new List<Tag>();
            tags.Add(tag1);

            IQueryableCommitLog log = new QueryableCommitLogStub(commits);
            TagCollection tagCollection = new TagCollectionStub(tags);

            Branch head = Substitute.For<Branch>();
            head.Name.Returns("main");

            IRepository repository = Substitute.For<IRepository>();
            repository.Commits.Returns(log);
            repository.Tags.Returns(tagCollection);
            repository.Head.Returns(head);

            return repository;
        }
    }
}
