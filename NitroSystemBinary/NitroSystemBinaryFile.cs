using System.Text;

namespace NitroSystemBinary;

public enum NitroSystemBinaryFileType {
    BinaryModelData, // BMD
    BinaryTexture, // BTX
    BinaryCharacterAnimation, // BCA
    BinaryTexturePattern, // BTP
    BinaryTextureAnimation, // BTA
    Unknown // others
}

public class NitroSystemBinaryFile
{
    public NitroSystemBinaryFileType Type;
    public uint Size;
    public ushort HeaderSize;
    public ushort SectionCount;
    public required uint[] SectionOffsets;
    
    public NitroSystemBinaryFileSection?[] Sections; 

    public static NitroSystemBinaryFile? FromFile(string path)
    {
        using var fileStream = new FileStream(path, FileMode.Open);
        using var binaryReader = new BinaryReader(fileStream);
        
        var headerType = ReadHeaderType(binaryReader);
        if (headerType == NitroSystemBinaryFileType.Unknown)
        {
            Console.WriteLine("Unknown file type.");
            return null;
        }

        // skip 4 bytes
        binaryReader.ReadBytes(2); // idk
        binaryReader.ReadBytes(2); // version, maybe
        
        var fileSize = binaryReader.ReadUInt32();
        var headerSize = binaryReader.ReadUInt16();
        var sectionCount = binaryReader.ReadUInt16();
        var sectionOffsets = new uint[sectionCount];
        for (var i = 0; i < sectionCount; i++)
        {
            sectionOffsets[i] = binaryReader.ReadUInt32();
        }

        var sections = ReadSections(binaryReader, sectionOffsets);
        
        return new NitroSystemBinaryFile()
        {
            Type = headerType,
            Size = fileSize,
            HeaderSize = headerSize,
            SectionCount = sectionCount,
            SectionOffsets = sectionOffsets,
            Sections = sections
        };
    }

    private static NitroSystemBinaryFileType ReadHeaderType(BinaryReader reader)
    {
        var magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
        return magic switch
        {
            "BMD0" => NitroSystemBinaryFileType.BinaryModelData,
            "BTX0" => NitroSystemBinaryFileType.BinaryTexture,
            "BCA0" => NitroSystemBinaryFileType.BinaryCharacterAnimation,
            "BTP0" => NitroSystemBinaryFileType.BinaryTexturePattern,
            "BTA0" => NitroSystemBinaryFileType.BinaryTextureAnimation,
            _ => NitroSystemBinaryFileType.Unknown
        };
    }
    
    private static NitroSystemBinaryFileSection?[] ReadSections(BinaryReader reader, uint[] sectionOffsets)
    {
        var sections = new NitroSystemBinaryFileSection?[sectionOffsets.Length];

        for (var i = 0; i < sectionOffsets.Length; i++)
        {
            reader.BaseStream.Seek(sectionOffsets[i], SeekOrigin.Begin);
            sections[i] = NitroSystemBinaryFileSection.Read(reader);
        }

        return sections;
    }
    
}