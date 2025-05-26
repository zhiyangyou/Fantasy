using System.Reflection;

class Program {
    static void Main(string[] args) {
        var targetDir = new DirectoryInfo(Assembly.GetExecutingAssembly().FullName)
            .Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FullName;

        var sourceDir = new DirectoryInfo(Assembly.GetExecutingAssembly().FullName)
            .Parent.Parent.Parent.Parent.Parent.Parent.FullName;
        targetDir = Path.Combine(targetDir, "Unity/Client/Assets/GameScripts/GlobalConfig");
        sourceDir = Path.Combine(sourceDir, "Hotfix/ShareToClient");
        Console.WriteLine(targetDir);
        Console.WriteLine(sourceDir);

        foreach (var file in Directory.GetFiles(sourceDir)) {
            if (file.EndsWith(".cs")) {
                var fileInfo = new FileInfo(file);
                var targetFile = Path.Combine(targetDir, fileInfo.Name);
                File.Copy(file, targetFile, true);
                Console.WriteLine($"{file} => {targetFile}");
            }
        }
    }
}