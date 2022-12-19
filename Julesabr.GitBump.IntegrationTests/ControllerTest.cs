using System;
using System.IO;
using FluentAssertions;
using Julesabr.LibGit;
using NSubstitute;
using NUnit.Framework;

namespace Julesabr.GitBump.IntegrationTests {
    public class ControllerTest {
        #region Default Options

        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndDefaultOptions_ThenDontBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABugFix_AndDefaultOptions_ThenBumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.2.4");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithAFeature_AndDefaultOptions_ThenBumpTagAsMinor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABreakingChange_AndDefaultOptions_ThenBumpTagAsMajor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("2.0.0");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithNoTag_AndDefaultOptions_ThenBumpFirstTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            Options options = new Options().Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("0.1.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion

        #region Prerelease Enabled
        
        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndIsPrerelease_ThenDontBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnDev.Create();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);
        
            ExitCode exitCode = controller.GitBump(options);
        
            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABugFix_AndIsPrerelease_ThenBumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnDev.Create();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.2.4.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithAFeature_AndIsPrerelease_ThenBumpTagAsMinor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnDev.Create();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.3.0.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABreakingChange_AndIsPrerelease_ThenBumpTagAsMajor_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnDev.Create();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("2.0.0.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithAPrereleaseChange_AndIsPrerelease_ThenBumpTagAsPrerelease_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithPrereleaseChangeOnDev.Create();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.2.4.dev.2");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABugFix_IsPrerelease_AndChannelIsStaging_ThenBumpTagAsPatch_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnDev.Create();
            Options options = new Options {
                Prerelease = true,
                Channel = "staging"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.2.4.staging.1");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithNoTag_AndIsPrerelease_ThenBumpFirstTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnDev.Create();
            Options options = new Options {
                Prerelease = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("0.1.0.dev.1");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        #endregion

        #region Tagging Enabled

        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndTaggingIsEnabled_ThenDontBumpTag_AndOutputNone() {
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be(Controller.ReturnNone);
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABugFix_AndTaggingIsEnabled_ThenBumpTagAsPatch_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.2.4", "");
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.2.4");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithAFeature_AndTaggingIsEnabled_ThenBumpTagAsMinor_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.3.0", "");
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABreakingChange_AndTaggingIsEnabled_ThenBumpTagAsMajor_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v2.0.0", "");
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("2.0.0");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithNoTag_AndTaggingIsEnabled_ThenBumpFirstTag_CreateAGitTag_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithNoTagOnMain.Create();
            Options options = new Options {
                Tag = true
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v0.1.0", "");
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("0.1.0");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithAFeature_TaggingIsEnabled_AndPrefixIsSet_ThenBumpTagAsMinor_CreateAGitTagWithNewPrefix_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("ver");
            Options options = new Options {
                Tag = true,
                Prefix = "ver"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("ver1.3.0", "");
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithAFeature_TaggingIsEnabled_AndSuffixIsSet_ThenBumpTagAsMinor_CreateAGitTagWithNewSuffix_AndOutputNewVersion() {
            IRepository repository = RepositoryStubWithFeatureOnMain.Create("v", "-preview");
            Options options = new Options {
                Tag = true,
                Suffix = "-preview"
            }.Default(repository);
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.3.0-preview", "");
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion
    }
}
