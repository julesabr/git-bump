using System;
using FluentAssertions;
using Julesabr.LibGit;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitServiceTest {
        private IGitService gitService = null!;
        private IRepository repository = null!;

        [SetUp]
        public void Setup() {
            repository = Substitute.For<IRepository>();
            gitService = IGitService.Create(repository);
        }

        [Test]
        public void
            CreateAnnotatedTag_GivenGitTagIsNotNull_CanGetTaggerFromConfig_AndRepositoryAppliesTag_ThenReturnSuccessfully() {
            IGitTag tag = IGitTag.Create(IVersion.From(1, 2, 3));
            gitService.CreateAnnotatedTag(tag);
            repository.Received(1).ApplyTag(tag.ToString(), "");
        }

        [Test]
        public void CreateAnnotatedTag_GivenGitTagIsNull_ThenThrowArgumentNullException() {
            Action action = () => gitService.CreateAnnotatedTag(null!);

            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'tag')");
        }

        [Test]
        public void PushTags_CallRepositoryNetwork() {
            INetwork network = Substitute.For<INetwork>();
            repository.Network.Returns(network);
            
            gitService.PushTags();
            
            network.Received(1).PushTags();
        }
    }
}
