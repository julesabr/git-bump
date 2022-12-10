# git-bump
A simple CLI command to bump the version on a git project.

## Usage
git-bump will use the latest commits between now and the last annotated tag on the 
current branch, excluding the commit with the tag. It is assumed that your annotated
tags represent your project's releases and that they have the version in them
somewhere. If there are no annotated tags, the default version of 0.1.0 is used as 
the first version. Lightweight tags are assumed to not represent releases and are 
therefore ignored.

### Semantic Versioning
git-bump uses the semantic versioning specification when bumping versions. That means
your projects must use the format x.y.z[.\<branch>.n] where x is the major version,
y is the minor version, and z is the patch version. Optionally, there may also be a
prerelease branch where the branch is your git branch and n is the prerelease version.
For more information, please view the [semantic versioning](https://semver.org) 
specification for yourself. See below for the types of version bumps and the types of 
commits that will trigger them:
- **Major**: Triggered when the latest commits contain at least one **BREAKING CHANGE**. When there is a major bump, the major version increases and the minor and patch are reset to 0 (1.2.3 -> 2.0.0). If it is a prerelease, the prerelease version is reset to 1 (1.2.3.dev.5 -> 2.0.0.dev.1). 
  - A **BREAKING CHANGE** is any change that is NOT backwards compatible with prior versions and upgrading to this version may break existing code. This generally includes any change that updates existing features, the way those features are used, or removing features altogether. For example, refactoring a feature or changing background behavior, like changing the database provider, won't be breaking. Changing user input or changing user output or user-facing behavior, however, is.
- **Minor**: Triggered when the latest commits contain only additive changes like new **Features** and no breaking changes. When there is a minor bump, the major version stays the same, the minor version increases and the patch is reset to 0 (1.2.3 -> 1.3.0). If it is a prerelease, the prerelease version is reset to 1 (1.2.3.dev.5 -> 1.3.0.dev.1).
- **Patch**: Triggered when the latest commits contain only bug fixes or other smaller changes. When there is a patch bump, the major and minor versions stay the same and the patch version increases (1.2.3 -> 1.2.4). If it is a prerelease, the prerelease version is reset to 1 (1.2.3.dev.5 -> 1.2.4.dev.1). Below are the types of commits that will trigger a patch:
  - **Bug Fixes**
  - **Refactoring**
  - **Performance Enhancements**
  - **Documentation**
  - **Build Components / Dependencies**
  - **Continuous Integration**
  - **Reverts**
- **Prerelease**: Triggered during a prerelease when there are no changes of higher precedence. For example, if the last release version is 1.0.1, there are new **Bug Fixes**, and the current branch is dev, then the next prerelease version will be 1.0.2.dev.1. If more changes were made and none are greater than a patch (aka no features or breaking changes), then the next prerelease version will be 1.0.2.dev.2. Then, if a new **Feature** was added, the prerelease version will be bumped to 1.1.0.dev.1. This cycle will continue like that as new changes are added, except the only kind of change that is a higher precedent now is a **BREAKING CHANGE**. This allows you to make as many individual changes as you need in prerelease without drastically increasing the release version until a normal release is made. As long as a higher precedent change isn't made, the current version will be maintained by increasing the prerelease version.

### Conventional Commits
In order to bump the semantic version of your project, a standard for determining the 
type of each commit is needed. For this purpose, git-bump uses 
[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/#specification). 
The conventional commit specification provides a standard for formatting your commit 
messages so that any developers or automated tools could instantly know what kind of 
change each commit made just by looking at the message. For git-bump to work properly,
all git commit messages on the branch must start with the commit type, followed by an
optional scope in parenthesis, and a colon and then the rest of the message.
```
feat: <message>
```
or
```
feat(calc): <message>
```
You can also add a breaking change to any commit type with either an '!', or a 
BREAKING CHANGE description in the commit message footer, or both. A breaking change 
will ALWAYS trigger a major bump, regardless of the commit type it is added to.
```
feat!: <message>

BREAKING CHANGE: <explanation>
```
or
```
feat(calc)!: <message>

BREAKING CHANGE: <explanation>
```
The example above shows feature changes but this will work for any commit type 
(ex. fix, chore, etc). For more information on the conventional commit specification,
please see the online documentation linked above. Commits that don't follow
the specification are allowed but they will be ignored by git-bump and will cause 
no change to the version unless of course a greater change is present. Some commit types
will trigger no version bump and will return the last created version. Below are 
the commit types that are supported by git-bump:
- `feat` Adds a new feature or updates an existing feature. Will trigger a minor bump unless there is a BREAKING CHANGE. This should be made into a BREAKING CHANGE if you updated or removed an existing feature.
- `fix` Fixes a bug. Will trigger a patch bump unless there is a BREAKING CHANGE. Generally, bug fixes are not breaking as they don't change how a feature is used, only how a feature works in the background.
- `docs` Documentation only changes. Will trigger a patch bump.
- `refactor` Changes that rewrite/restructure your code, however does not change any behaviour. Will trigger a patch bump.
- `perf` Special `refactor` commits, that improve performance. Will trigger a patch bump.
- `build` Affects build components like build tool, ci pipeline, dependencies, etc. Will trigger a patch bump.
- `ci` Changes to CI configuration files and scripts (examples: CircleCi, TravisCI, Jenkins, etc). Will trigger a patch bump.
- `revert` Reverts a previous change. Will trigger a patch bump, regardless of the type of change being reverted.
- `style` Does not affect the meaning of the code (white-space, formatting, missing semi-colons, etc). Will trigger NO bump.
- `test` Add missing tests or correcting existing tests. Will trigger NO bump.
- `chore` Miscellaneous commits (e.g. modifying `.gitignore`). Will trigger NO bump.
