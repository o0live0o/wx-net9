namespace wx.Shared.Tools;

public class VersionUpdater
{
    private const string SECTION_START = "<Package>";
    private const string SECTION_END = "</Package>";
    
    private const string VERSION_START = "<Version>";
    private const string VERSION_END = "</Version>";
    
    public static string UpdateVersion(string content)
    {
        int sectionStartIdx = content.IndexOf(SECTION_START);
        int sectionEndIdx = content.IndexOf(SECTION_END);
        if (sectionEndIdx == -1 ||  sectionStartIdx == -1 || sectionStartIdx > sectionEndIdx)
            throw new InvalidDataException("Invalid package file.");
        
        int versionStartIdx = content.IndexOf(VERSION_START);
        int versionEndIdx = content.IndexOf(VERSION_END);
        if (versionEndIdx == -1 || versionStartIdx == -1 || versionStartIdx > versionEndIdx)
            throw new InvalidDataException("Invalid package file. No version found.");
        
        int versionValueStartIdx = versionStartIdx + VERSION_START.Length;
        string currentVersion = content.Substring(versionValueStartIdx, versionEndIdx - versionValueStartIdx);
        
        string[] parts = currentVersion.Split('.');
        if (parts.Length != 4 || !int.TryParse(parts[3], out int revision))
        {
            throw new InvalidDataException("Invalid version format. Expected format: x.x.x.x");
        }
        
        parts[3] = (revision + 1).ToString();
        string newVersion = string.Join(".", parts);
        
        string newContent = content.Substring(0, versionValueStartIdx) 
                            + newVersion 
                            + content.Substring(versionEndIdx);
        
        return newContent;
    }
}