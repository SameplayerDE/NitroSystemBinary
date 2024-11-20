using System.Drawing;
using System.Text;

namespace NitroSystemBinary;

public enum NitroSystemBinaryFileSectionType
{
    Model,
    Texture,
    JointAnimation,
    TexturePattern,
    TextureAnimation,
    MaterialAnimation,
    VisualType,
    Unknown
}

public abstract class NitroSystemBinaryFileSection //(byte[] data)
{
    public NitroSystemBinaryFileSectionType Type;
    public uint Size;
    //private byte[] _data;
    
    public static NitroSystemBinaryFileSection? Read(BinaryReader reader)
    {
        var sectionType = ReadSectionType(reader);
        var sectionSize = reader.ReadUInt32();
        var data = reader.ReadBytes((int)sectionSize - 8);

        var result = ReadSection(sectionType, data);
        if (result != null)
        {
            result.Size = sectionSize;
        }
        return result;
    }

    private static NitroSystemBinaryFileSection? ReadSection(NitroSystemBinaryFileSectionType sectionType, byte[] data)
    {
        return sectionType switch
        {
            NitroSystemBinaryFileSectionType.Model => new NitroSystemBinaryModel(data),
            NitroSystemBinaryFileSectionType.Texture => new NitroSystemBinaryTexture(data),
            NitroSystemBinaryFileSectionType.JointAnimation => new NitroSystemBinaryJointAnimation(data),
            NitroSystemBinaryFileSectionType.TexturePattern => new NitroSystemBinaryTexturePattern(data),
            NitroSystemBinaryFileSectionType.TextureAnimation => new NitroSystemBinaryTextureAnimation(data),
            NitroSystemBinaryFileSectionType.MaterialAnimation => new NitroSystemBinaryMaterialAnimation(data),
            NitroSystemBinaryFileSectionType.VisualType => null, // new NitroSystemBinaryVisual(data),
            NitroSystemBinaryFileSectionType.Unknown => null,
            _ => null
        };
    }

    private static NitroSystemBinaryFileSectionType ReadSectionType(BinaryReader reader)
    {
        var magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
        return magic switch
        {
            "MDL0" => NitroSystemBinaryFileSectionType.Model,
            "TEX0" => NitroSystemBinaryFileSectionType.Texture,
            "JNT0" => NitroSystemBinaryFileSectionType.JointAnimation,
            "PAT0" => NitroSystemBinaryFileSectionType.TexturePattern,
            "SRT0" => NitroSystemBinaryFileSectionType.TextureAnimation,
            "MAT0" => NitroSystemBinaryFileSectionType.MaterialAnimation,
            "VIS0" => NitroSystemBinaryFileSectionType.VisualType,
            _ => NitroSystemBinaryFileSectionType.Unknown
        };
    }
    
}

