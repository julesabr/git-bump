using System;
using FluentAssertions;
using NUnit.Framework;

namespace Julesabr.GitBump.Tests.GitTagFactoryModel {
    public class Tests {
        [Test]
        public void
            Create_ForAValueWithNoPrefixOrSuffix_AndOptionsWithNoPrefixOrSuffix_GivenAVersionFactory_Then_TheResult_Should_HaveVersion_HaveNoPrefix_AndHaveNoSuffix() {
            Given.AGitTagFactory
                .With()
                .AVersionFactory()
                .When()
                .Create(Given.AValueWithNoPrefixOrSuffix, Given.OptionsWithNoPrefixOrSuffix)
                .Then()
                .TheResult
                .ShouldHaveAVersion()
                .And()
                .ShouldHaveNoPrefix()
                .And()
                .ShouldHaveNoSuffix();
        }

        [Test]
        public void
            Create_ForAValueWithTheDefaultPrefixAndSuffix_AndOptionsWithDefaultPrefixAndSuffix_GivenAVersionFactory_Then_TheResult_Should_HaveVersion_HaveAPrefix_AndHaveASuffix() {
            Given.AGitTagFactory
                .With()
                .AVersionFactory()
                .When()
                .Create(Given.AValueWithDefaultPrefixAndSuffix, Given.OptionsWithDefaultPrefixAndSuffix)
                .Then()
                .TheResult
                .ShouldHaveAVersion()
                .And()
                .ShouldHaveAPrefix()
                .And()
                .ShouldHaveASuffix();
        }

        [Test]
        public void
            Create_ForAValueWithTheDefaultPrefixAndWrongSuffix_AndOptionsWithDefaultPrefixAndSuffix_GivenAVersionFactory_Then_Should_Throw_ArgumentException() {
            Given.AGitTagFactory
                .With()
                .AVersionFactory()
                .When()
                .Create(Given.AValueWithDefaultPrefixAndWrongSuffix, Given.OptionsWithDefaultPrefixAndSuffix)
                .ThenAction()
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IGitTag.MissingSuffixError, Given.AValueWithDefaultPrefixAndWrongSuffix,
                    Given.ADefaultSuffix));
        }

        [Test]
        public void
            Create_ForAValueWithTheWrongPrefixAndDefaultSuffix_AndOptionsWithDefaultPrefixAndSuffix_GivenAVersionFactory_Then_Should_Throw_ArgumentException() {
            Given.AGitTagFactory
                .With()
                .AVersionFactory()
                .When()
                .Create(Given.AValueWithWrongPrefixAndDefaultSuffix, Given.OptionsWithDefaultPrefixAndSuffix)
                .ThenAction()
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(string.Format(IGitTag.MissingPrefixError, Given.AValueWithWrongPrefixAndDefaultSuffix,
                    Given.ADefaultPrefix));
        }

        [Test]
        public void Create_ForAnEmptyValue_GivenAVersionFactory_Then_Should_Throw_ArgumentNullException() {
            Given.AGitTagFactory
                .With()
                .AVersionFactory()
                .When()
                .Create("", Given.OptionsWithDefaultPrefixAndSuffix)
                .ThenAction()
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage(IGitTag.BlankStringError + " (Parameter 'value')");
        }

        [Test]
        public void
            CreateEmpty_ForOptionsWithDefaultPrefixAndSuffix_GivenAVersionFactory_Then_TheResult_Should_HaveAnEmptyVersion_HaveAPrefix_AndHaveASuffix() {
            Given.AGitTagFactory
                .With()
                .AVersionFactory()
                .When()
                .CreateEmpty(Given.OptionsWithDefaultPrefixAndSuffix)
                .Then()
                .TheResult
                .ShouldHaveAnEmptyVersion()
                .And()
                .ShouldHaveAPrefix()
                .And()
                .ShouldHaveASuffix();
        }
    }
}
