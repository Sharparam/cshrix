// <copyright file="build.cake">
//   Copyright (c) 2019 by Adam Hellberg.
//
//   This Source Code Form is subject to the terms of the Mozilla Public
//   License, v. 2.0. If a copy of the MPL was not distributed with this
//   file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>

#addin nuget:?package=Cake.DocFx&version=0.12.0
#addin nuget:?package=Cake.Coveralls&version=0.9.0
#addin nuget:?package=Cake.Codecov&version=0.5.0
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"
#tool "nuget:?package=OpenCover&version=4.7.922"
#tool "nuget:?package=ReportGenerator&version=4.1.4"
#tool nuget:?package=coveralls.io&version=1.4.2
#tool nuget:?package=Codecov&version=1.4.0

var isAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isTravis = TravisCI.IsRunningOnTravisCI;
var isCi = isAppVeyor || isTravis;
var cover = isAppVeyor || HasArgument("cover");

var target = Argument("target", "Default");

var configuration = HasArgument("configuration")
    ? Argument<string>("Configuration")
    : EnvironmentVariable("CONFIGURATION") != null
        ? EnvironmentVariable("CONFIGURATION")
        : "Release";

var buildNumber = HasArgument("build-number")
    ? Argument<int>("BuildNumber")
    : isAppVeyor
        ? AppVeyor.Environment.Build.Number
        : isTravis
            ? TravisCI.Environment.Build.BuildNumber
            : EnvironmentVariable("BUILD_NUMBER") != null
                ? int.Parse(EnvironmentVariable("BUILD_NUMBER"))
                : 0;

var coverallsRepoToken = HasArgument("coveralls-token")
    ? Argument<string>("CoverallsToken")
    : EnvironmentVariable("COVERALLS_REPO_TOKEN");

var isTag = IsTag();
var isPr = isAppVeyor
    ? AppVeyor.Environment.PullRequest?.IsPullRequest == true
    : isTravis
        ? TravisCI.Environment.PullRequest?.IsPullRequest == true
        : false;

var testFailed = false;
var solutionDir = System.IO.Directory.GetCurrentDirectory();

var testResultDir = Argument("testResultDir", System.IO.Path.Combine(solutionDir, "test-results"));
var artifactDir = Argument("artifactDir", "./artifacts");
var coverageResultsFile = System.IO.Path.Combine(artifactDir, "opencover-results.xml");
var slnName = Argument("slnName", "Cshrix");

var solutionFile = System.IO.Path.Combine(solutionDir, $"{slnName}.sln");
var isDevelop = false;

GitVersion version = null;

bool IsTag()
{
    if (HasArgument("isTag"))
    {
        return Argument<string>("isTag") == "true";
    }

    return IsAppVeyorTag() || IsTravisTag();
}

bool IsTravisTag()
{
    return isTravis && !string.IsNullOrWhiteSpace(TravisCI.Environment.Build.Tag);
}

bool IsAppVeyorTag()
{
    return isAppVeyor && AppVeyor.Environment.Repository.Tag?.IsTag == true;
}

GitVersion GetGitVersion(bool buildServerOutput = false)
{
    var settings = new GitVersionSettings
    {
        RepositoryPath = ".",
        OutputType = buildServerOutput ? GitVersionOutput.BuildServer : GitVersionOutput.Json
    };

    return GitVersion(settings);
}

DotNetCoreMSBuildSettings GetMSBuildSettings(GitVersion version)
{
    return new DotNetCoreMSBuildSettings()
        .SetVersion(version.NuGetVersion)
        .SetFileVersion(version.AssemblySemFileVer)
        .SetInformationalVersion(version.InformationalVersion)
        .WithProperty("AssemblyVersion", version.AssemblySemVer);
}

DotNetCoreBuildSettings GetBuildSettings()
{
    return new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        MSBuildSettings = GetMSBuildSettings(version)
    };
}

IEnumerable<string> ReadCoverageFilters(string path)
{
    return System.IO.File.ReadLines(path).Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("#"));
}

if (isTravis && cover)
{
    Information("OpenCover does not work on Travis CI, disabling coverage generation");
    cover = false;
}

Setup(ctx =>
{
    Information("PATH is {0}", EnvironmentVariable("PATH"));

    var docFxBranch = EnvironmentVariable("DOCFX_SOURCE_BRANCH_NAME");
    if (docFxBranch != null)
    {
        Information("DocFx branch is {0}", docFxBranch);
    }

    if (isCi)
    {
        GetGitVersion(true);
    }

    version = GetGitVersion();
    isDevelop = isAppVeyor
        ? AppVeyor.Environment.Repository.Branch == "develop"
        : isTravis
            ? TravisCI.Environment.Build.Branch == "develop"
            : version.BranchName == "develop";

    Information("Version: {0} on {1}", version.InformationalVersion, version.CommitDate);
});

Task("Clean")
    .Does(() =>
    {
        var settings = new DeleteDirectorySettings {
            Recursive = true,
            Force = true
        };

        if (DirectoryExists(testResultDir))
        {
            CleanDirectory(testResultDir);
            DeleteDirectory(testResultDir, settings);
        }

        if (DirectoryExists(artifactDir))
        {
            CleanDirectory(artifactDir);
            DeleteDirectory(artifactDir, settings);
        }

        var binDirs = GetDirectories("./src/*/bin");
        var objDirs = GetDirectories("./src/*/obj");
        var testResDirs = GetDirectories("./**/TestResults");

        CleanDirectories(binDirs);
        CleanDirectories(objDirs);
        CleanDirectories(testResDirs);

        DeleteDirectories(binDirs, settings);
        DeleteDirectories(objDirs, settings);
        DeleteDirectories(testResDirs, settings);
    });

Task("PrepareDirectories")
    .Does(() =>
    {
        EnsureDirectoryExists(testResultDir);
        EnsureDirectoryExists(artifactDir);
    });

Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("PrepareDirectories")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        Information("Build solution: {0}", solutionFile);
        var settings = GetBuildSettings();
        DotNetCoreBuild(solutionFile, settings);
    });

void Test(ICakeContext context = null)
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true
    };

    if (isAppVeyor)
    {
        settings.ArgumentCustomization = args => args.Append("--logger:AppVeyor");
    }
    else
    {
        settings.ArgumentCustomization = args => args.Append("--logger:trx");
    }

    if (context == null)
    {
        DotNetCoreTest(solutionFile, settings);
    }
    else
    {
        context.DotNetCoreTest(solutionFile, settings);
    }

    if (isAppVeyor)
    {
        return;
    }

    var tmpTestResultFiles = GetFiles("./**/TestResults/*.*");
    CopyFiles(tmpTestResultFiles, testResultDir);
}

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        if (cover)
        {
            Information("Running tests with coverage, using OpenCover");

            var filters = ReadCoverageFilters("./src/coverage-filters.txt");
            var settings = filters.Aggregate(new OpenCoverSettings
            {
                OldStyle = true,
                MergeOutput = true
            }, (a, e) => a.WithFilter(e));

            OpenCover(c => Test(c), new FilePath(coverageResultsFile), settings);

            ReportGenerator(coverageResultsFile, "./artifacts/coverage-report");
        }
        else
        {
            Information("Running tests without coverage");
            Test();
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        if (testFailed)
        {
            Information("Do not pack because tests failed");
            return;
        }

        var projects = GetSrcProjectFiles();
        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = artifactDir,
            MSBuildSettings = GetMSBuildSettings(version)
        };

        foreach (var project in projects)
        {
            Information("Pack {0}", project.FullPath);
            DotNetCorePack(project.FullPath, settings);
        }
    });

Task("Publish")
    .IsDependentOn("Test")
    .DoesForEach(GetSrcProjectFiles(), project =>
    {
        var projectDir = System.IO.Path.GetDirectoryName(project.FullPath);
        var projectName = new System.IO.DirectoryInfo(projectDir).Name;
        var outputDir = System.IO.Path.Combine(artifactDir, projectName);
        EnsureDirectoryExists(outputDir);

        Information("Publish {0} to {1}", projectName, outputDir);

        var settings = new DotNetCorePublishSettings
        {
            OutputDirectory = outputDir,
            Configuration = configuration,
            MSBuildSettings = GetMSBuildSettings(version)
        };

        DotNetCorePublish(project.FullPath, settings);
    });

Task("Docs")
    .IsDependentOn("Build")
    .WithCriteria(!isTravis)
    .Does(() =>
    {
        DocFxMetadata("./docs/docfx.json");
        DocFxBuild("./docs/docfx.json");
        Zip("./docs/_site", $"./artifacts/cshrix_{version.SemVer}_docs.zip");
    });

Task("Coveralls")
    .WithCriteria(cover)
    .WithCriteria(isAppVeyor)
    .WithCriteria(coverallsRepoToken != null)
    .IsDependentOn("Test")
    .Does(() =>
    {
        Information("Running Coveralls tool on OpenCover result");

        CoverallsIo(coverageResultsFile, new CoverallsIoSettings
        {
            FullSources = true,
            RepoToken = coverallsRepoToken
        });
    });

Task("Codecov")
    .WithCriteria(cover)
    .WithCriteria(isAppVeyor)
    .IsDependentOn("Test")
    .Does(() =>
    {
        var ccVersion = $"{version.FullSemVer}.build.{BuildSystem.AppVeyor.Environment.Build.Version}";

        Information($"Running Codecov tool with version {ccVersion} on OpenCover result");

        Codecov(new CodecovSettings
        {
            Files = new[] { coverageResultsFile },
            Required = true,
            Branch = Uri.EscapeDataString(version.BranchName),
            EnvironmentVariables = new Dictionary<string, string>
            {
                ["APPVEYOR_BUILD_VERSION"] = Uri.EscapeDataString(ccVersion)
            }
        });
    });

Task("Default")
    .IsDependentOn("Test")
    .Does(() =>
    {
        Information("Build and test the whole solution.");
        Information("To pack (nuget) the application use the cake build argument: --target Pack");
        Information("To publish (to run it somewhere else) the application use the cake build argument: --target Publish");
    });

Task("CI")
    .IsDependentOn("Test")
    .IsDependentOn("Docs");

Task("AppVeyor")
    .IsDependentOn("CI")
    .IsDependentOn("Coveralls")
    .IsDependentOn("Codecov");

Task("Travis").IsDependentOn("CI");

FilePathCollection GetSrcProjectFiles()
{
    return GetFiles("./src/**/*.csproj");
}

RunTarget(target);
