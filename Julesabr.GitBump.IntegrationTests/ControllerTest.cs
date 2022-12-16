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
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndDefaultOptions_ThenDontBumpTag_AndOutputVersion() {
            Options options = new();
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.DidNotReceive().ApplyTag(Arg.Any<string>(), Arg.Any<string>());
            repository.Network.DidNotReceive().PushTags();
            stdOut.ToString().Trim().Should().Be("1.2.3");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABugFix_AndDefaultOptions_ThenBumpTagAsPatch_AndOutputNewVersion() {
            Options options = new();
            IRepository repository = RepositoryStubWithBugFixOnMain.Create();
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.2.4", "");
            repository.Network.Received().PushTags();
            stdOut.ToString().Trim().Should().Be("1.2.4");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithAFeature_AndDefaultOptions_ThenBumpTagAsMinor_AndOutputNewVersion() {
            Options options = new();
            IRepository repository = RepositoryStubWithFeatureOnMain.Create();
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v1.3.0", "");
            repository.Network.Received().PushTags();
            stdOut.ToString().Trim().Should().Be("1.3.0");
            exitCode.Should().Be(ExitCode.Success);
        }
        
        [Test]
        public void GitBump_GivenRepositoryWithABreakingChange_AndDefaultOptions_ThenBumpTagAsMajor_AndOutputNewVersion() {
            Options options = new();
            IRepository repository = RepositoryStubWithBreakingChangeOnMain.Create();
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            repository.Received().ApplyTag("v2.0.0", "");
            repository.Network.Received().PushTags();
            stdOut.ToString().Trim().Should().Be("2.0.0");
            exitCode.Should().Be(ExitCode.Success);
        }

        #endregion
    }
}
