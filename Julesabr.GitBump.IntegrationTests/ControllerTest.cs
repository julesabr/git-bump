using System;
using System.IO;
using FluentAssertions;
using Julesabr.LibGit;
using NUnit.Framework;

namespace Julesabr.GitBump.IntegrationTests {
    public class ControllerTest {
        [Test]
        public void GitBump_GivenRepositoryWithNoSignificantChange_AndDefaultOptions_ThenDontBumpTag_AndOutputVersion() {
            Options options = new();
            IRepository repository = RepositoryStubWithNoSignificantChangeOnMain.Create();
            StringWriter stdOut = new();
            Console.SetOut(stdOut);
            Controller controller = new(repository);

            ExitCode exitCode = controller.GitBump(options);

            stdOut.ToString().Trim().Should().Be("1.2.3");
            exitCode.Should().Be(ExitCode.Success);
        }
    }
}
