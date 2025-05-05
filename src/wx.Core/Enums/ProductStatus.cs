namespace wx.Core.Enums;

[Flags]
public enum ProductStatus
{
    None = 0,
    UploadImage = 1,
    IsEmbedding = 1 << 1,
    Disabled = 1 << 2,
}
