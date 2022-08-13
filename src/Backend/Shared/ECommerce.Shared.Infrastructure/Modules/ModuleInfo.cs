namespace ECommerce.Shared.Infrastructure.Modules
{
    internal record class ModuleInfo(string Name, string Path, IEnumerable<string> Policies);
}
