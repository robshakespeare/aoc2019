#addin "Cake.FileHelpers&version=3.2.1"

var target = Argument<string>("target", "CreateDay");
var workingDir = Argument<string>("testResultsDir", "./.cake-working/");

Task("CreateDay")
    .Does(() =>
    {
        string day = null;
        while (string.IsNullOrWhiteSpace(day))
        {
            Information($"Enter number of the day to create:");
            day = System.Console.ReadLine();
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

        foreach(var dir in GetDirectories("./.cake-working/*"))
        {
            var newDirPath = $"./{dir.GetDirectoryName()}".Replace("XXX", day);
            MoveDirectory(dir, newDirPath);
        }

        DeleteDirectory(workingDir, new DeleteDirectorySettings { Recursive = true, Force = true });

        Information("Created files for day " + day);
    });

RunTarget(target);
