namespace uweb4Media.Application.Features.CQRS.Commands.Media;

public class CreateMediaContentCommand
{
    public int UserId { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Sector { get; set; }
    public string Channel { get; set; }
    public string ContentType { get; set; }
    public string Thumbnail { get; set; }

    // --- MediaContent entity'nize eklenen yeni alanlar buraya da eklenmeli ---
    public bool IsPremium { get; set; } = false; // Command'da da varsayılan değer belirtebilirsiniz
    public string? MetaTitle { get; set; } // Nullable olarak belirtilebilir
    public string? MetaDescription { get; set; } // Nullable olarak belirtilebilir
    public string? Duration { get; set; } // Nullable
    public string? Excerpt { get; set; } // Nullable
    public string? YoutubeVideoId { get; set; } // Nullable
    public string? Tags { get; set; } // Nullable
}