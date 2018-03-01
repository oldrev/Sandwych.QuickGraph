//#addin nuget:?package=Cake.DocFx&version=0.5.0
//#tool "docfx.console"

var solutionFile = "Sandwych.QuickGraph.sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


Task("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreRestore(solutionFile, new DotNetCoreRestoreSettings
    {
        Verbosity = DotNetCoreVerbosity.Minimal,
    });
});


Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    // Build the solution.
    var path = MakeAbsolute(new DirectoryPath(solutionFile));
    DotNetCoreBuild(path.FullPath, new DotNetCoreBuildSettings()
    {
        NoRestore = true,
        Configuration = configuration,
    });
});


Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles("./test/**/*.Tests.csproj");
    foreach(var project in projects)
    {
        // .NET Core
        DotNetCoreTest(project.ToString(), new DotNetCoreTestSettings
        {
            Framework = "netcoreapp2.0",
            NoBuild = true,
            NoRestore = true,
            Configuration = configuration,
        });
    }
});


Task("Create-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    // Build libraries
    var projects = GetFiles("./src/**/*.csproj");
    foreach(var project in projects)
    {
        var name = project.GetDirectory().FullPath;
        if(name.EndsWith("Tests") || name.EndsWith("Xunit"))
        {
            continue;
        }

        DotNetCorePack(project.FullPath, new DotNetCorePackSettings {
            NoBuild = true,
            NoRestore = true,
            IncludeSymbols = false,
            Configuration = configuration,
        });
    }
});


/*
Task("Generate-Docs").Does(() => {
    DocFxBuild("./docs/docfx.json");
});


Task("View-Docs").Does(() => {
    DocFxBuild("./docs/docfx.json", new DocFxBuildSettings {
        Serve = true,
    });
});
*/


Task("Travis")
    .IsDependentOn("Run-Unit-Tests");


Task("Appveyor-Build")
    .IsDependentOn("Create-NuGet-Packages");


Task("Appveyor-Test")
    .IsDependentOn("Run-Unit-Tests");


Task("Default")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() => {
    Information("Executing the default task...");
});


RunTarget(target);
