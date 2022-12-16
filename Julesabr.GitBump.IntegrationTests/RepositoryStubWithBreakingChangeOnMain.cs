using System.Collections.Generic;
using Julesabr.LibGit;
using NSubstitute;

namespace Julesabr.GitBump.IntegrationTests {
    public static class RepositoryStubWithBreakingChangeOnMain {
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
            
            Tag tag2 = Substitute.For<Tag>();
            tag2.Name.Returns("startup-bug");
            tag2.IsAnnotated.Returns(false);
            tag2.Target.Returns(commit3);
            
            Commit commit4 = Substitute.For<Commit>();
            commit4.Sha.Returns("b2483cc69eb53cfec2f29bd295bd73823c2c7c23");
            commit4.Message.Returns("fix: Failure to startup");
            commit4.MessageFull.Returns("fix: Failure to startup");
            
            Tag tag3 = Substitute.For<Tag>();
            tag3.Name.Returns("startup-bug-fix");
            tag3.IsAnnotated.Returns(false);
            tag3.Target.Returns(commit4);
            
            Commit commit5 = Substitute.For<Commit>();
            commit5.Sha.Returns("35e7cc9722428a38d5b8e3feb51b665c000aa98f");
            commit5.Message.Returns("docs: Setup");
            commit5.MessageFull.Returns("docs: Setup");
            
            Commit commit6 = Substitute.For<Commit>();
            commit6.Sha.Returns("d642c5c69cd0712fbd040d43d4b710316e55e933");
            commit6.Message.Returns("feat: Add login");
            commit6.MessageFull.Returns("feat: Add login");
            
            Commit commit7 = Substitute.For<Commit>();
            commit7.Sha.Returns("d642c5c69cd0712fbd040d43d4b710316e55e933");
            commit7.Message.Returns("feat!: Update messaging");
            commit7.MessageFull.Returns("feat!: Updated messaging\n\nBREAKING CHANGE: Move location of IM on UI");

            IList<Commit> commits = new List<Commit>();
            commits.Add(commit7);
            commits.Add(commit6);
            commits.Add(commit5);
            commits.Add(commit4);
            commits.Add(commit3);
            commits.Add(commit2);
            commits.Add(commit1);
            
            IList<Tag> tags = new List<Tag>();
            tags.Add(tag3);
            tags.Add(tag2);
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
