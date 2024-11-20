using System.Text;

namespace NitroSystemBinary;

public class NitroSystemBinaryTexturePattern : NitroSystemBinaryFileSection
{
    public uint Unknown;
    public ushort FrameRelated;
    public byte NumTextureNames;
    public byte NumPaletteNames;
    
    public NitroSystemBinaryTexturePattern(byte[] data)
    {
        Type = NitroSystemBinaryFileSectionType.TexturePattern;
     
        using var memoryStream = new MemoryStream(data);
        using var reader = new BinaryReader(memoryStream);
        
        Unknown = reader.ReadUInt32();
        FrameRelated = reader.ReadUInt16();
        reader.ReadBytes(0x12);
        var patternName = ReadNullTerminatedString(reader);
        reader.ReadBytes(0x2B);
        var materialName = ReadNullTerminatedString(reader);
        //NumTextureNames = reader.ReadByte();
        //NumPaletteNames = reader.ReadByte();
        //var textureNamesOffset = reader.ReadUInt16();
        //var paletteNamesOffset = reader.ReadUInt16();
        
        //var textureNames = ReadNames(reader, textureNamesOffset, NumTextureNames);

    }
    
    private static List<string> ReadNames(BinaryReader reader, long offset, int count)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);

        var names = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var name = ReadNullTerminatedString(reader);
            names.Add(name);
        }
        return names;
    }
    
    private static string ReadNullTerminatedString(BinaryReader reader)
    {
        var sb = new StringBuilder();
        byte b;
        while ((b = reader.ReadByte()) != 0)
        {
            sb.Append((char)b);
        }
        return sb.ToString();
    }
    
}