using NitroSystemBinary;

const string path = @"A:\ModelExporter\Platin\output_nds\ele_door1_op.nsbtp";

Console.WriteLine(path);
var nitroFile = NitroSystemBinaryFile.FromFile(path);
if (nitroFile == null)
{
    return;
}
Console.WriteLine(nitroFile.Type);
Console.WriteLine(nitroFile.SectionCount);
    
foreach (var section in nitroFile.Sections)
{
    if (section == null)
    {
        continue;
    }
    Console.WriteLine("\t" + section.Type);
}

/**
foreach (var file in Directory.EnumerateFiles(path))
{
    Console.WriteLine(path);
    var nitroFile = NitroSystemBinaryFile.FromFile(path);
    if (nitroFile == null)
    {
        continue;
    }
    Console.WriteLine(nitroFile.Type);
    Console.WriteLine(nitroFile.SectionCount);
    
    foreach (var section in nitroFile.Sections)
    {
        if (section == null)
        {
            continue;
        }
        Console.WriteLine("\t" + section.Type);
    }
}
**/