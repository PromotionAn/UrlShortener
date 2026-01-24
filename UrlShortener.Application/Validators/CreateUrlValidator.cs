using FluentValidation;
using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Validators;

public class CreateShortenedUrlValidator : AbstractValidator<CreateShortenedUrlDto>
{
    public CreateShortenedUrlValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty().WithMessage("URL is required")
            .MaximumLength(2048).WithMessage("URL must not exceed 2048 characters")
            .Must(BeAValidUrl).WithMessage("Invalid URL format");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}