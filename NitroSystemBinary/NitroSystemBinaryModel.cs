namespace NitroSystemBinary;

public class NitroSystemBinaryModel : NitroSystemBinaryFileSection
{
    public NitroSystemBinaryModel(byte[] data)
    {
        Type = NitroSystemBinaryFileSectionType.Model;
    }
}