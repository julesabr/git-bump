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

        #endregion

        #region Prereleases
        
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
        
        #endregion
    }
}
