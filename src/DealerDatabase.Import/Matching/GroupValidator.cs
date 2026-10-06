using DealerDatabase.Import.Matching.Models;
using DealerDatabase.Import.Normalization.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace DealerDatabase.Import.Matching;

public class GroupValidator
{
    public GroupValidationResult Validate(MatchedGroup group)
    {
        var conflicts = new List<string>();
        var warnings = new List<GroupValidationWarning>();

        ValidateCompanyNumbers(group, conflicts);
        ValidateVatNumbers(group, conflicts);
        ValidateFcaFrns(group, conflicts);

        ValidateCompanyNumberWarnings(group, warnings);
        ValidateVatNumberWarnings(group, warnings);
        ValidateFcaFrnWarnings(group, warnings);

        return new GroupValidationResult
        {
            Group = group,
            Conflicts = conflicts,
            Warnings = warnings
        };
    }

    private static bool HasAuthoritativeCompanyNumber(
    NormalizedRecord record)
    {
        return record.CompanyNumber is not null &&
            record.Source is
                SourceType.CompaniesHouse or
                SourceType.Fca or
                SourceType.Ico;
    }

    private static void ValidateCompanyNumbers(
    MatchedGroup group,
    List<string> reasons)
    {
        var companyNumbers = group.Records
            .Where(HasAuthoritativeCompanyNumber)
            .Select(record => record.CompanyNumber!)
            .Distinct()
            .ToList();

        if (companyNumbers.Count > 1)
        {
            reasons.Add(
                $"Conflicting authoritative company numbers: " +
                $"{string.Join(", ", companyNumbers)}");
        }
    }

    private static void ValidateCompanyNumberWarnings(
    MatchedGroup group,
    List<GroupValidationWarning> warnings)
    {
        var authoritativeNumbers = group.Records
            .Where(HasAuthoritativeCompanyNumber)
            .Select(record => record.CompanyNumber!)
            .Distinct()
            .ToList();

        if (authoritativeNumbers.Count != 1)
        {
            return;
        }

        var authoritativeNumber = authoritativeNumbers[0];

        var disagreements = group.Records
            .Where(record =>
                record.CompanyNumber is not null &&
                !HasAuthoritativeCompanyNumber(record) &&
                record.CompanyNumber != authoritativeNumber)
            .ToList();

        foreach (var record in disagreements)
        {
            warnings.Add(new GroupValidationWarning
            {
                Source = record.Source,
                SourceRecordId = record.SourceRecordId,
                Field = "CompanyNumber",
                ObservedValue = record.CompanyNumber!,
                AuthoritativeValue = authoritativeNumber
            });
        }
    }

    private static bool HasAuthoritativeFcaFrn(
    NormalizedRecord record)
    {
        return record.Source == SourceType.Fca &&
            record.FcaFrn is not null;
    }

    private static void ValidateFcaFrns(
    MatchedGroup group,
    List<string> reasons)
    {
        var fcaFrns = group.Records
            .Where(HasAuthoritativeFcaFrn)
            .Select(record => record.FcaFrn!)
            .Distinct()
            .ToList();

        if (fcaFrns.Count > 1)
        {
            reasons.Add(
                $"Conflicting authoritative FCA FRNs: " +
                $"{string.Join(", ", fcaFrns)}");
        }
    }

    private static void ValidateFcaFrnWarnings(
    MatchedGroup group,
    List<GroupValidationWarning> warnings)
    {
        var authoritativeFrns = group.Records
            .Where(HasAuthoritativeFcaFrn)
            .Select(record => record.FcaFrn!)
            .Distinct()
            .ToList();

        if (authoritativeFrns.Count != 1)
        {
            return;
        }

        var authoritativeFrn = authoritativeFrns[0];

        var disagreements = group.Records
            .Where(record =>
                record.FcaFrn is not null &&
                !HasAuthoritativeFcaFrn(record) &&
                record.FcaFrn != authoritativeFrn)
            .ToList();

        foreach (var record in disagreements)
        {
            warnings.Add(new GroupValidationWarning
            {
                Source = record.Source,
                SourceRecordId = record.SourceRecordId,
                Field = "FcaFrn",
                ObservedValue = record.FcaFrn!,
                AuthoritativeValue = authoritativeFrn
            });
        }
    }

    private static bool HasAuthoritativeVatNumber(
    NormalizedRecord record)
    {
        return record.Source == SourceType.Vat &&
            record.VatNumber is not null;
    }

    private static void ValidateVatNumbers(
    MatchedGroup group,
    List<string> reasons)
    {
        var vatNumbers = group.Records
            .Where(HasAuthoritativeVatNumber)
            .Select(record => record.VatNumber!)
            .Distinct()
            .ToList();

        if (vatNumbers.Count > 1)
        {
            reasons.Add(
                $"Conflicting authoritative VAT numbers: " +
                $"{string.Join(", ", vatNumbers)}");
        }
    }

    private static void ValidateVatNumberWarnings(
    MatchedGroup group,
    List<GroupValidationWarning> warnings)
    {
        var authoritativeNumbers = group.Records
            .Where(HasAuthoritativeVatNumber)
            .Select(record => record.VatNumber!)
            .Distinct()
            .ToList();

        if (authoritativeNumbers.Count != 1)
        {
            return;
        }

        var authoritativeNumber = authoritativeNumbers[0];

        var disagreements = group.Records
            .Where(record =>
                record.VatNumber is not null &&
                !HasAuthoritativeVatNumber(record) &&
                record.VatNumber != authoritativeNumber)
            .ToList();

        foreach (var record in disagreements)
        {
            warnings.Add(new GroupValidationWarning
            {
                Source = record.Source,
                SourceRecordId = record.SourceRecordId,
                Field = "VatNumber",
                ObservedValue = record.VatNumber!,
                AuthoritativeValue = authoritativeNumber
            });
        }
    }
}