using NitroSystemBinary;

const string path = @"A:\ModelExporter\Platin\output_nds\";

void PrintSections(NitroSystemBinaryFile file)
{
    foreach (var section in file.Sections)
    {
        if (section == null)
        {
            continue;
        }
        
        //Console.WriteLine("\t" + section.Type);
        
        if (section.Type == NitroSystemBinaryFileSectionType.TexturePattern)
        {
            var pattern = (NitroSystemBinaryTexturePattern)section;
            Console.WriteLine("\t\tName: " + pattern.Name);
            Console.WriteLine("\t\tAnimationCount: " + pattern.AnimationCount);
            Console.WriteLine("\t\tFrameCount: " + pattern.FrameCount);
            if (pattern.AnimationCount == 1)
            {
                foreach (var texture in pattern.Textures)
                {
                    Console.WriteLine("\t\t\t" + texture);
                }
            }
        }
    }
}

foreach (var file in Directory.EnumerateFiles(path))
{
    Console.WriteLine(file);
    var nitroFile = NitroSystemBinaryFile.FromFile(file);
    if (nitroFile == null)
    {
        continue;
    }
    Console.WriteLine(nitroFile.Type);
    Console.WriteLine(nitroFile.SectionCount);
    
    PrintSections(nitroFile);
}

    






