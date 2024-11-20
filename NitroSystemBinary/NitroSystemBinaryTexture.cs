namespace NitroSystemBinary;

public class NitroSystemBinaryTexture : NitroSystemBinaryFileSection
{
    public NitroSystemBinaryTexture(byte[] data)
    {
        Type = NitroSystemBinaryFileSectionType.Texture;
    }
}