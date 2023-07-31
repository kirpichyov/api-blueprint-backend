using System.Threading.Tasks;
using ApiBlueprint.Application.Models.Profile;

namespace ApiBlueprint.Application.Contracts;

public interface IProfileService
{
    Task<CurrentUserProfileResponse> GetCurrentProfile();
}