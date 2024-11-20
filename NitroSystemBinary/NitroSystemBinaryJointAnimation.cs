namespace NitroSystemBinary;

public class NitroSystemBinaryJointAnimation : NitroSystemBinaryFileSection
{
    public NitroSystemBinaryJointAnimation(byte[] data)
    {
        Type = NitroSystemBinaryFileSectionType.JointAnimation;
    }
}