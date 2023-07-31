using System.Threading.Tasks;
using NUnit.Framework;
using ApiBlueprint.IntegrationTests.Testing;

namespace ApiBlueprint.IntegrationTests;

[SetUpFixture]
public class Setup
{
    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        await SystemUnderTest.StartAsync();
    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await SystemUnderTest.ShutdownAsync();
    }
}