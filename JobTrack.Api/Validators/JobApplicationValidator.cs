using JobTrack.Api.Models;
using JobTrack.Api.Enums;

namespace JobTrack.Api.Validators;

public class JobApplicationValidator
{
    public string? Validate(JobApplication application)
    {
        if (string.IsNullOrWhiteSpace(application.CompanyName))
            return "Şirket adı zorunludur.";

        if (string.IsNullOrWhiteSpace(application.Position))
            return "Pozisyon zorunludur.";

        if (string.IsNullOrWhiteSpace(application.Location))
            return "Konum zorunludur.";

        if (!Enum.IsDefined(typeof(ApplicationStatus), application.Status))
            return "Geçersiz başvuru durumu.";
        return null;
    }
}