using System;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Julesabr.GitBump.Tests.GitTagFactoryModel {
    internal static class Scenarios {
        [Given]
        public static GivenBuilder With(this Given<IGitTag.Factory> @this) {
            return @this.With<GivenBuilder>();
        }

        [Given]
        public static GivenBuilder AVersionFactory(this GivenBuilder @this) {
            IVersion.Factory versionFactory = Substitute.For<IVersion.Factory>();

            versionFactory.Create(Arg.Any<string>())
                .Throws(info =>
                    new ArgumentException(
                        $"Arguments in versionFactory.Create({string.Join(',', info.Args())}) are not stubbed."));
            versionFactory.Configure().Create(Given.AValueWithNoPrefixOrSuffix).Returns(Given.AVersion);
            versionFactory.Configure().CreateEmpty().Returns(Given.AnEmptyVersion);

            @this.VersionFactory = versionFactory;
            return @this;
        }

        [When]
        public static When<IGitTag.Factory, IGitTag> Create(
            this When<IGitTag.Factory> @this,
            string value,
            Options options
        ) {
            return @this.AddResult(factory => factory.Create(value, options));
        }

        [When]
        public static When<IGitTag.Factory, IGitTag> CreateEmpty(this When<IGitTag.Factory> @this, Options options) {
            return @this.AddResult(factory => factory.CreateEmpty(options));
        }

        [Then]
        public static IGitTag? And(this IGitTag? @this) {
            return @this;
        }

        [Then]
        public static IGitTag ShouldHaveAVersion(this IGitTag? @this) {
            @this.Should().NotBeNull();
            @this!.Version.Should().Be(Given.AVersion);
            return @this;
        }

        [Then]
        public static IGitTag ShouldHaveAnEmptyVersion(this IGitTag? @this) {
            @this.Should().NotBeNull();
            @this!.Version.Should().Be(Given.AnEmptyVersion);
            return @this;
        }

        [Then]
        public static IGitTag ShouldHaveNoPrefix(this IGitTag? @this) {
            @this.Should().NotBeNull();
            @this!.Prefix.Should().BeNullOrWhiteSpace();
            return @this;
        }

        [Then]
        public static IGitTag ShouldHaveAPrefix(this IGitTag? @this) {
            @this.Should().NotBeNull();
            @this!.Prefix.Should().Be(Given.ADefaultPrefix);
            return @this;
        }

        [Then]
        public static void ShouldHaveNoSuffix(this IGitTag? @this) {
            @this.Should().NotBeNull();
            @this!.Suffix.Should().BeNullOrWhiteSpace();
        }

        [Then]
        public static void ShouldHaveASuffix(this IGitTag? @this) {
            @this.Should().NotBeNull();
            @this!.Suffix.Should().Be(Given.ADefaultSuffix);
        }
    }
}
