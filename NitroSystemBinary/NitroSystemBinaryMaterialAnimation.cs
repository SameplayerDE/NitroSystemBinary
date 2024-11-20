namespace NitroSystemBinary;

public class NitroSystemBinaryMaterialAnimation : NitroSystemBinaryFileSection
{
    public NitroSystemBinaryMaterialAnimation(byte[] data)
    {
        Type = NitroSystemBinaryFileSectionType.MaterialAnimation;
    }
}