namespace NitroSystemBinary;

public class NitroSystemBinaryTextureAnimation : NitroSystemBinaryFileSection
{
    public NitroSystemBinaryTextureAnimation(byte[] data)
    {
        Type = NitroSystemBinaryFileSectionType.TextureAnimation;
    }
}