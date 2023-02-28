using FluentValidation;

namespace Catalog.Core.Data.Models;

public class DatabaseSettingsModel
{
    public const string SectionName = "DatabaseSettings";

    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
    public required string CollectionName { get; set; }
}

public class DatabaseSettingsModelValidator : AbstractValidator<DatabaseSettingsModel>
{
    public DatabaseSettingsModelValidator()
    {
        RuleFor(x => x.ConnectionString).NotEmpty();
        RuleFor(x => x.DatabaseName).NotEmpty();
        RuleFor(x=> x.CollectionName).NotEmpty();
    }
}