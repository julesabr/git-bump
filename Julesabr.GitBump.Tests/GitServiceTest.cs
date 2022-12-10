using System;
using FluentAssertions;
using LibGit2Sharp;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests {
    public class GitServiceTest {
        private Configuration config = null!;
        private IGitService gitService = null!;
        private IRepository repository = null!;
        private TagCollection tags = null!;

        [SetUp]
        public void Setup() {
            Branch head = Substitute.For<Branch>();
            Commit tip = Substitute.For<Commit>();

            config = Substitute.For<Configuration>();
            tags = Substitute.For<TagCollection>();
            repository = Substitute.For<IRepository>();
            gitService = IGitService.Create(repository);

            head.Tip.Returns(tip);
            repository.Head.Returns(head);
            repository.Tags.Returns(tags);
            repository.Config.Returns(config);
        }

        [Test]
        public void
            CreateAnnotatedTag_GivenGitTagIsNotNull_CanGetTaggerFromConfig_AndRepositoryAppliesTag_ThenReturnSuccessfully() {
            IGitTag tag = IGitTag.Create(IVersion.From(1, 2, 3));
            Signature tagger = new("foo", "bar", DateTimeOffset.MaxValue);
            config.BuildSignature(Arg.Any<DateTimeOffset>()).Returns(tagger);
            tags.Add(Arg.Any<string>(), Arg.Any<GitObject>(), Arg.Any<Signature>(), Arg.Any<string>())
                .Returns(Substitute.For<Tag>());

            gitService.CreateAnnotatedTag(tag);

            tags.Received(1).Add(tag.ToString(), Arg.Any<GitObject>(), tagger, "");
        }

        [Test]
        public void CreateAnnotatedTag_WhenRepositoryFailedToApplyTag_ThenThrowOperationFailedException() {
            IGitTag tag = IGitTag.Create(IVersion.From(1, 2, 3));
            Signature tagger = new("foo", "bar", DateTimeOffset.MaxValue);
            config.BuildSignature(Arg.Any<DateTimeOffset>()).Returns(tagger);
            tags.Add(Arg.Any<string>(), Arg.Any<GitObject>(), Arg.Any<Signature>(), Arg.Any<string>())
                .Returns((Tag?)null);

            Action action = () => gitService.CreateAnnotatedTag(tag);

            action.Should()
                .Throw<OperationFailedException>()
                .WithMessage(string.Format(IGitService.ApplyTagFailedError, tag));
        }

        [Test]
        public void CreateAnnotatedTag_WhenTaggerCannotBeRetrievedFromConfig_ThenThrowInvalidOperationException() {
            IGitTag tag = IGitTag.Create(IVersion.From(1, 2, 3));
            config.BuildSignature(Arg.Any<DateTimeOffset>()).Returns((Signature?)null);
            tags.Add(Arg.Any<string>(), Arg.Any<GitObject>(), Arg.Any<Signature>(), Arg.Any<string>())
                .Returns(Substitute.For<Tag>());

            Action action = () => gitService.CreateAnnotatedTag(tag);

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(IGitService.TaggerNotFoundError);
        }

        [Test]
        public void CreateAnnotatedTag_GivenGitTagIsNull_ThenThrowArgumentNullException() {
            Signature tagger = new("foo", "bar", DateTimeOffset.MaxValue);
            config.BuildSignature(Arg.Any<DateTimeOffset>()).Returns(tagger);
            tags.Add(Arg.Any<string>(), Arg.Any<GitObject>(), Arg.Any<Signature>(), Arg.Any<string>())
                .Returns(Substitute.For<Tag>());

            Action action = () => gitService.CreateAnnotatedTag(null!);

            action.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'tag')");
        }
    }
}
