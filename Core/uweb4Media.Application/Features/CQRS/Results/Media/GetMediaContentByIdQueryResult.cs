using System; // DateTime için

namespace uweb4Media.Application.Features.CQRS.Queries.Media; // Queries namespace'ine ait bir Result

public class GetMediaContentByIdQueryResult
{
    public int Id { get; set; }
    // User bilgileri ayrı çekilebilir veya doğrudan DTO'ya eklenebilir
    public int UserId { get; set; } // İçeriği oluşturan kullanıcının ID'si
    public string Author { get; set; } // AppUser.Name veya AppUser.Username
    public string AuthorAvatar { get; set; } // AppUser.AvatarUrl

    public string Url { get; set; }
    public string Title { get; set; }
    public string Sector { get; set; }
    public string Channel { get; set; }
    public string ContentType { get; set; }
    public string Thumbnail { get; set; } // Thumbnail URL'si

    // Frontend'in beklediği diğer alanlar
    public int Likes { get; set; } // MediaContent.LikesCount'tan gelecek
    public int CommentsCount { get; set; } // MediaContent.CommentsCount'tan gelecek
    public DateTime Timestamp { get; set; } // MediaContent.CreatedDate'ten gelecek
    public int ViewCount { get; set; } // MediaContent.ViewCount'tan gelecek
    public bool IsPremium { get; set; } // MediaContent.IsPremium'dan gelecek

    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Duration { get; set; }
    public string? Excerpt { get; set; }
    public string? YoutubeVideoId { get; set; }
    public string? Tags { get; set; }
}