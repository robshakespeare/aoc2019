#addin "Cake.FileHelpers&version=3.2.1"

var target = Argument<string>("target", "CreateDay");
var day = Argument<string>("day", "");
var workingDir = Argument<string>("workingDir", "./.cake-working/");

Task("CreateDay")
    .Does(() =>
    {
        if (string.IsNullOrWhiteSpace(day))
        {
            Error($"Usage: dotnet cake --day=X");
            return;
        }

        Information($"Creating files for day {day}...");

        CreateDirectory(workingDir);
        CleanDirectory(workingDir);

        CopyFiles(GetFiles("./Template/**/*.*"), workingDir, true);
        ReplaceTextInFiles("./.cake-working/**/*.*", "XXX", day);

        foreach(var file in GetFiles("./.cake-working/**/*.*"))
        {
            var newFilePath = file.GetDirectory().CombineWithFilePath(file.GetFilename().FullPath.Replace("XXX", day));
            MoveFile(file, newFilePath);
        }

        foreach(var dir in GetDirectories("./.cake-working/*/*"))
        {
            var newDirPath = $"./{dir.Segments[dir.Segments.Length-2]}/{dir.GetDirectoryName()}".Replace("XXX", day);
            MoveDirectory(dir, newDirPath);
        }

        DeleteDirectory(workingDir, new DeleteDirectorySettings { Recursive = true, Force = true });

        Information("Created files for day " + day);
    });

RunTarget(target);
