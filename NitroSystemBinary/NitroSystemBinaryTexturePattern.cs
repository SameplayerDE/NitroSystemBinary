using System.Text;

namespace NitroSystemBinary;

public class MaterialAnimation
{
    public string ApplyOn;
    public uint KeyFrameCount;
    public KeyFrame[] KeyFrames = [];
}

public class KeyFrame
{
    public ushort FrameIndex;
    public byte TextureIndex;
    public byte PaletteIndex;
}

public class NitroSystemBinaryTexturePattern : NitroSystemBinaryFileSection
{
    public string Name;
    public ushort FrameCount;
    public byte TextureCount;
    public byte PaletteCount;
    public ushort TextureOffset;
    public ushort PaletteOffset;
    public byte AnimationCount;
    public string[] Textures;
    public string[] Palettes;
    
    // TODO: FIX
    public MaterialAnimation[] MaterialAnimation = [new()];
    
    public NitroSystemBinaryTexturePattern(byte[] data)
    {
        Type = NitroSystemBinaryFileSectionType.TexturePattern;
     
        using var memoryStream = new MemoryStream(data);
        using var reader = new BinaryReader(memoryStream);

        // Pattern Name (18 bytes after start, null-terminated, 16 bytes name + padding)
        memoryStream.Seek(0x18, SeekOrigin.Begin);
        Name = ReadNullTerminatedString(reader, 16);

        // Skip 4 bytes (unknown)
        reader.BaseStream.Seek(0x04, SeekOrigin.Current);
        
        // Frame steps (uint16), number of textures (uint8), number of palettes (uint8)
        FrameCount = reader.ReadUInt16();
        TextureCount = reader.ReadByte();
        PaletteCount = reader.ReadByte();
        
        // Texture and palette offsets (uint16 each)
        TextureOffset = reader.ReadUInt16();
        PaletteOffset = reader.ReadUInt16();
        
        // Skip byte (unknown)
        reader.BaseStream.Seek(0x01, SeekOrigin.Current);
        AnimationCount = reader.ReadByte();

        if (AnimationCount > 1)
        {
            return;
        }
        
        // Skip 0x12 bytes (unknown)
        reader.BaseStream.Seek(0x12, SeekOrigin.Current);
        
        // Keyframe count (possibly uint32)
        MaterialAnimation[0].KeyFrameCount = reader.ReadUInt16();

        // Skip 4 bytes (unknown)
        reader.BaseStream.Seek(0x06, SeekOrigin.Current);
        
        // Material Name (50 bytes after start, null-terminated, 16 bytes name + padding)
        MaterialAnimation[0].ApplyOn = ReadNullTerminatedString(reader, 0x10);
        
        // Read keyframes
        MaterialAnimation[0].KeyFrames = new KeyFrame[MaterialAnimation[0].KeyFrameCount];
        for (var i = 0; i < MaterialAnimation[0].KeyFrameCount; i++)
        {
            var frameIndex = reader.ReadUInt16();
            var textureIndex = reader.ReadByte();
            var paletteIndex = reader.ReadByte();
            MaterialAnimation[0].KeyFrames[i] = new KeyFrame
            {
                FrameIndex = frameIndex,
                TextureIndex = textureIndex,
                PaletteIndex = paletteIndex,
            };
        }

        reader.BaseStream.Seek(0x28 + TextureOffset, SeekOrigin.Begin);
        Textures = new string[TextureCount];
        for (var i = 0; i < TextureCount; i++)
        {
            Textures[i] = ReadNullTerminatedString(reader, 0x10);
        }
        
        reader.BaseStream.Seek(0x28 + PaletteOffset, SeekOrigin.Begin);
        Palettes = new string[PaletteCount];
        for (var i = 0; i < PaletteCount; i++)
        {
            Palettes[i] = ReadNullTerminatedString(reader, 0x10);
        }
    }
    
    private static string ReadNullTerminatedString(BinaryReader reader, int fixedLength)
    {
        var bytes = reader.ReadBytes(fixedLength);
        
        var str = Encoding.ASCII.GetString(bytes);
        
        var nullIndex = str.IndexOf('\0');
        if (nullIndex >= 0)
        {
            str = str[..nullIndex];
        }

        return str;
    }
    
}