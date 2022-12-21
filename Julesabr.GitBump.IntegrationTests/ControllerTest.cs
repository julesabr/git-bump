using System;
using System.IO;
using FluentAssertions;
using Julesabr.IO;
using Julesabr.LibGit;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.IntegrationTests {
    public class ControllerTest {
        #region Default Options

        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndDefaultOptions_ThenDontBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void GitBump_GivenRepositoryWithABugFix_AndDefaultOptions_ThenBumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.2.4");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void GitBump_GivenRepositoryWithAFeature_AndDefaultOptions_ThenBumpTagAsMinor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABreakingChange_AndDefaultOptions_ThenBumpTagAsMajor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("2.0.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void GitBump_GivenRepositoryWithNoTag_AndDefaultOptions_ThenBumpFirstTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("0.1.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion

        #region Prerelease Enabled

        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndIsPrerelease_ThenDontBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void GitBump_GivenRepositoryWithABugFix_AndIsPrerelease_ThenBumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.2.4.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void GitBump_GivenRepositoryWithAFeature_AndIsPrerelease_ThenBumpTagAsMinor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABreakingChange_AndIsPrerelease_ThenBumpTagAsMajor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("2.0.0.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAPrereleaseChange_AndIsPrerelease_ThenBumpTagAsPrerelease_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithPrereleaseChangeOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.2.4.dev.2");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABugFix_IsPrerelease_AndChannelIsStaging_ThenBumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Prerelease = true,
                Channel = "staging"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.2.4.staging.1");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void GitBump_GivenRepositoryWithNoTag_AndIsPrerelease_ThenBumpFirstTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("0.1.0.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion

        #region Tagging Enabled

        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndTaggingIsEnabled_ThenDontBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABugFix_AndTaggingIsEnabled_ThenBumpTagAsPatch_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.2.4", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.2.4");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAFeature_AndTaggingIsEnabled_ThenBumpTagAsMinor_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.3.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABreakingChange_AndTaggingIsEnabled_ThenBumpTagAsMajor_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v2.0.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("2.0.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithNoTag_AndTaggingIsEnabled_ThenBumpFirstTag_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v0.1.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("0.1.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAFeature_TaggingIsEnabled_AndPrefixIsSet_ThenBumpTagAsMinor_CreateAGitTagWithNewPrefix_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("ver");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Tag = true,
                Prefix = "ver"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("ver1.3.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAFeature_TaggingIsEnabled_AndSuffixIsSet_ThenBumpTagAsMinor_CreateAGitTagWithNewSuffix_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("v", "-preview");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Tag = true,
                Suffix = "-preview"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.3.0-preview", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion

        #region Push Enabled

        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndPushIsEnabled_ThenDontBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Push = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABugFix_AndPushIsEnabled_ThenBumpTagAsPatch_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Push = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.2.4", "");
            repository.Network.Received().PushTag("v1.2.4");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.2.4");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAFeature_AndPushIsEnabled_ThenBumpTagAsMinor_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Push = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.3.0", "");
            repository.Network.Received().PushTag("v1.3.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABreakingChange_AndPushIsEnabled_ThenBumpTagAsMajor_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Push = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v2.0.0", "");
            repository.Network.Received().PushTag("v2.0.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("2.0.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithNoTag_AndPushIsEnabled_ThenBumpFirstTag_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Push = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v0.1.0", "");
            repository.Network.Received().PushTag("v0.1.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("0.1.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAFeature_PushIsEnabled_AndPrefixIsSet_ThenBumpTagAsMinor_CreateAGitTagWithNewPrefix_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("ver");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Push = true,
                Prefix = "ver"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("ver1.3.0", "");
            repository.Network.Received().PushTag("ver1.3.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAFeature_PushIsEnabled_AndSuffixIsSet_ThenBumpTagAsMinor_CreateAGitTagWithNewSuffix_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("v", "-preview");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                Push = true,
                Suffix = "-preview"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.3.0-preview", "");
            repository.Network.Received().PushTag("v1.3.0-preview");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion

        #region VersionOutput Option

        [Test]
        public void
            GitBump_GivenRepositoryWithNoSignificantChange_AndVersionOutputIsSet_ThenDontBumpTag_WriteNoneToFile_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                VersionOutput = "./version.txt"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(1).Create(options.VersionOutput);
            text.Received(1).Write(Controller.ReturnNone);
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABugFix_AndVersionOutputIsSet_ThenBumpTagAsPatch_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                VersionOutput = "./version.txt"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(1).Create(options.VersionOutput);
            text.Received(1).Write("1.2.4");
            stdOut.ToString().Trim().Should().Be("1.2.4");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithAFeature_AndVersionOutputIsSet_ThenBumpTagAsMinor_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                VersionOutput = "./version.txt"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(1).Create(options.VersionOutput);
            text.Received(1).Write("1.3.0");
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithABreakingChange_AndVersionOutputIsSet_ThenBumpTagAsMajor_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                VersionOutput = "./version.txt"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(1).Create(options.VersionOutput);
            text.Received(1).Write("2.0.0");
            stdOut.ToString().Trim().Should().Be("2.0.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        [Test]
        public void
            GitBump_GivenRepositoryWithNoTag_AndVersionOutputIsSet_ThenBumpFirstTag_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            Options options = new Options {
                VersionOutput = "./version.txt"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(1).Create(options.VersionOutput);
            text.Received(1).Write("0.1.0");
            stdOut.ToString().Trim().Should().Be("0.1.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion
    }
}
