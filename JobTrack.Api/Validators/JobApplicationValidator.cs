using JobTrack.Api.DTOs;
using JobTrack.Api.Enums;

namespace JobTrack.Api.Validators;

public class JobApplicationValidator
{
    public string? Validate(CreateJobApplicationDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CompanyName))
            return "Şirket adı zorunludur.";

        if (string.IsNullOrWhiteSpace(dto.Position))
            return "Pozisyon zorunludur.";

        if (string.IsNullOrWhiteSpace(dto.Location))
            return "Konum zorunludur.";

        if (!Enum.IsDefined(typeof(ApplicationStatus), dto.Status))
            return "Geçersiz başvuru durumu.";
        return null;
    }
}