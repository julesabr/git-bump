using System;
using System.Collections.Generic;
using System.IO;
using CommandDotNet.TestTools;
using FluentAssertions;
using Julesabr.GitBump.Controllers;
using Julesabr.GitBump.Middleware;
using Julesabr.IO;
using Julesabr.LibGit;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Julesabr.GitBump.IntegrationTests.Controllers {
    public class AppControllerTest {
        [Test]
        public void Version_Should_OutputFileNameAndVersion() {
            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver())
                .UseCustomVersion(new AppVersionInfo("git-bump", "0.1.0"))
                .RunInMem("--version");

            result.Console.Out.ToString().Should().Be("git-bump 0.1.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        #region Default Options

        [Test]
        public void RepositoryWithNoSignificantChange_Should_NotBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be(AppController.ReturnNone);
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void RepositoryWithABugFix_Should_BumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.2.4");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void RepositoryWithAFeature_Should_BumpTagAsMinor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void RepositoryWithABreakingChange_Should_BumpTagAsMajor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("2.0.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void RepositoryWithNoTag_Should_BumpFirstTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("0.1.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        #endregion

        #region Prerelease Enabled

        [Test]
        public void Prerelease_RepositoryWithNoSignificantChange_Should_NotBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--prerelease");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be(AppController.ReturnNone);
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Prerelease_RepositoryWithABugFix_Should_BumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--prerelease");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.2.4.dev.1");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Prerelease_RepositoryWithAFeature_Should_BumpTagAsMinor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--prerelease");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0.dev.1");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Prerelease_RepositoryWithABreakingChange_Should_BumpTagAsMajor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--prerelease");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("2.0.0.dev.1");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Prerelease_RepositoryWithAPrereleaseChange_Should_BumpTagAsPrerelease_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithPrereleaseChangeOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--prerelease");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.2.4.dev.2");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Prerelease_Channel_Staging_RepositoryWithABugFix_Should_BumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--prerelease --channel staging");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.2.4.staging.1");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Prerelease_RepositoryWithNoTag_Should_BumpFirstTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnDev.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--prerelease");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("0.1.0.dev.1");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        #endregion

        #region Tagging Enabled

        [Test]
        public void Tag_RepositoryWithNoSignificantChange_Should_NotBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--tag");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be(AppController.ReturnNone);
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Tag_RepositoryWithABugFix_Should_BumpTagAsPatch_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--tag");

            repository.Received().ApplyTag("v1.2.4", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.2.4");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Tag_RepositoryWithAFeature_Should_BumpTagAsMinor_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--tag");

            repository.Received().ApplyTag("v1.3.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Tag_RepositoryWithABreakingChange_Should_BumpTagAsMajor_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--tag");

            repository.Received().ApplyTag("v2.0.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("2.0.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Tag_RepositoryWithNoTag_Should_BumpFirstTag_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--tag");

            repository.Received().ApplyTag("v0.1.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("0.1.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            Tag_ChangePrefix_RepositoryWithAFeature_Should_BumpTagAsMinor_CreateAGitTagWithNewPrefix_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("ver");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--tag --prefix ver");

            repository.Received().ApplyTag("ver1.3.0", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            Tag_ChangeSuffix_RepositoryWithAFeature_Should_BumpTagAsMinor_CreateAGitTagWithNewSuffix_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("v", "-preview");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--tag --suffix \"-preview\"");

            repository.Received().ApplyTag("v1.3.0-preview", "");
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        #endregion

        #region Push Enabled

        [Test]
        public void Push_RepositoryWithNoSignificantChange_Should_NotBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--push");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be(AppController.ReturnNone);
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Push_RepositoryWithABugFix_Should_BumpTagAsPatch_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--push");

            repository.Received().ApplyTag("v1.2.4", "");
            repository.Network.Received().PushTag("v1.2.4");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.2.4");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void Push_RepositoryWithAFeature_Should_BumpTagAsMinor_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--push");

            repository.Received().ApplyTag("v1.3.0", "");
            repository.Network.Received().PushTag("v1.3.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            Push_RepositoryWithABreakingChange_Should_BumpTagAsMajor_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--push");

            repository.Received().ApplyTag("v2.0.0", "");
            repository.Network.Received().PushTag("v2.0.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("2.0.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            Push_RepositoryWithNoTag_Should_BumpFirstTag_CreateAGitTag_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--push");

            repository.Received().ApplyTag("v0.1.0", "");
            repository.Network.Received().PushTag("v0.1.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("0.1.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            Push_ChangePrefix_RepositoryWithAFeature_Should_BumpTagAsMinor_CreateAGitTagWithNewPrefix_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("ver");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--push --prefix ver");

            repository.Received().ApplyTag("ver1.3.0", "");
            repository.Network.Received().PushTag("ver1.3.0");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            Push_ChangeSuffix_RepositoryWithAFeature_Should_BumpTagAsMinor_CreateAGitTagWithNewSuffix_PushTheTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("v", "-preview");
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--push --suffix \"-preview\"");

            repository.Received().ApplyTag("v1.3.0-preview", "");
            repository.Network.Received().PushTag("v1.3.0-preview");
            fileFactory.DidNotReceive().Create(Arg.Any<string>());
            text.DidNotReceive().Write(Arg.Any<string>());
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        #endregion

        #region VersionOutput Option

        [Test]
        public void
            SetVersionOutput_RepositoryWithNoSignificantChange_Should_NotBumpTag_WriteNoneToFile_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--version-output ./version.txt");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(2).Create("./version.txt");
            text.Received(1).Write(AppController.ReturnNone);
            result.Console.Out.ToString().Should().Be(AppController.ReturnNone);
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            SetVersionOutput_RepositoryWithABugFix_Should_BumpTagAsPatch_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--version-output ./version.txt");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(2).Create("./version.txt");
            text.Received(1).Write("1.2.4");
            result.Console.Out.ToString().Should().Be("1.2.4");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            SetVersionOutput_RepositoryWithAFeature_Should_BumpTagAsMinor_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--version-output ./version.txt");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(2).Create("./version.txt");
            text.Received(1).Write("1.3.0");
            result.Console.Out.ToString().Should().Be("1.3.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            SetVersionOutput_RepositoryWithABreakingChange_Should_BumpTagAsMajor_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--version-output ./version.txt");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(2).Create("./version.txt");
            text.Received(1).Write("2.0.0");
            result.Console.Out.ToString().Should().Be("2.0.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        [Test]
        public void
            SetVersionOutput_RepositoryWithNoTag_Should_BumpFirstTag_WriteNewVersionToFile_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--version-output ./version.txt");

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTag(Arg.Any<string>());
            fileFactory.Received(2).Create("./version.txt");
            text.Received(1).Write("0.1.0");
            result.Console.Out.ToString().Should().Be("0.1.0");
            result.ExitCode.Should().Be((int)ExitCode.Success);
        }

        #endregion

        #region Exceptions

        [Test]
        public void SetVersionOutput_ToInvalidPath_Should_Fail_WithValidationError() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            IText text = Substitute.For<IText>();
            AppController controller = new(repository, fileFactory);

            fileFactory.Create(Arg.Any<string>()).Returns(text);
            text.When(t => t.Write(Arg.Any<string>()))
                .Do(_ =>
                    throw new DirectoryNotFoundException(
                        "The directory '/home/user/non_existent_directory' does not exist."));

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("--version-output /home/user/non_existent_directory/version.txt");

            result.Console.Out.ToString().Should().BeEmpty();
            result.Console.Error.ToString()
                .Should()
                .Contain(
                    "git-bump: validation error: version-output is invalid. The directory '/home/user/non_existent_directory' does not exist.\n");
            result.ExitCode.Should().Be((int)ExitCode.ValidationError);
        }

        [Test]
        public void RepositoryWithInvalidTagName_Should_Fail_WithIllegalStateError() {
            IRepository repository = GetRepositoryWithInvalidTagName();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            AppController controller = new(repository, fileFactory);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            result.Console.Out.ToString().Should().BeEmpty();
            result.Console.Error.ToString()
                .Should()
                .Be(
                    "git-bump: illegal state: 'foo' is not a valid version. All versions must be in semantic version format either 'x.y.z' or 'x.y.z.<channel>.n'.");
            result.ExitCode.Should().Be((int)ExitCode.IllegalState);
        }

        [Test]
        public void RepositoryWhereFileNotFound_Should_Fail_WithFileNotFoundError() {
            IRepository repository = GetRepositoryWhereFileNotFound();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            AppController controller = new(repository, fileFactory);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            result.Console.Out.ToString().Should().BeEmpty();
            result.Console.Error.ToString()
                .Should()
                .Be("git-bump: file not found: 'bash' was not found on this system. Please install it and try again.");
            result.ExitCode.Should().Be((int)ExitCode.FileNotFound);
        }

        [Test]
        public void RepositoryWhereOperationFailed_Should_Fail_WithOperationFailedError() {
            IRepository repository = GetRepositoryWhereOperationFailed();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            AppController controller = new(repository, fileFactory);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            result.Console.Out.ToString().Should().BeEmpty();
            result.Console.Error.ToString()
                .Should()
                .Be(
                    "git-bump: operation failed: Subcommand failed with error\n\t$ git tag (exit code: 1)\n\nPermission denied");
            result.ExitCode.Should().Be((int)ExitCode.OperationFailed);
        }

        [Test]
        public void RepositoryWhereException_Should_Fail() {
            IRepository repository = GetRepositoryWhereException();
            FileFactory fileFactory = Substitute.For<FileFactory>();
            AppController controller = new(repository, fileFactory);

            AppRunnerResult result = Program.GetAppRunner(new TestDependencyResolver { controller })
                .RunInMem("");

            result.Console.Out.ToString().Should().BeEmpty();
            result.Console.Error.ToString()
                .Should()
                .Be("git-bump: error: Operation is not valid due to the current state of the object.");
            result.ExitCode.Should().Be((int)ExitCode.Fail);
        }

        private IRepository GetRepositoryWithInvalidTagName() {
            Tag tag = Substitute.For<Tag>();
            tag.Name.Returns("vfoo");
            tag.IsAnnotated.Returns(true);
            tag.Target.Returns((Commit)null!);

            IRepository repository = Substitute.For<IRepository>();
            repository.Tags.Returns(new TagCollectionStub(new List<Tag> { tag }));

            return repository;
        }

        private IRepository GetRepositoryWhereFileNotFound() {
            IRepository repository = Substitute.For<IRepository>();
            repository.Tags.Throws(
                new FileNotFoundException("'bash' was not found on this system. Please install it and try again."));

            return repository;
        }

        private IRepository GetRepositoryWhereOperationFailed() {
            IRepository repository = Substitute.For<IRepository>();
            repository.Tags.Throws(new OperationFailedException(
                "Subcommand failed with error\n\t$ git tag (exit code: 1)\n\nPermission denied"));

            return repository;
        }

        private IRepository GetRepositoryWhereException() {
            IRepository repository = Substitute.For<IRepository>();
            repository.Tags.Throws<InvalidOperationException>();

            return repository;
        }

        #endregion
    }
}
