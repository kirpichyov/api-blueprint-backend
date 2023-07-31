using Flurl.Http;
using ApiBlueprint.Application.Models.Auth;

namespace ApiBlueprint.IntegrationTests.Testing;

public sealed record AuthorizedClient(IFlurlClient FlurlClient, UserRegisterRequest UserInfo, AuthResponse AuthInfo);