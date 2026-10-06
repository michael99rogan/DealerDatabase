// Matching strategy ref: JF-DDB-07
using DealerDatabase.Import.Matching.Models;
using DealerDatabase.Import.Normalization.Models;

namespace DealerDatabase.Import.Matching;

public class MatchScorer
{
    private const int DedupeConfidenceFloor = 100;

    private const int CompanyNumberScore = 100;
    private const int VatNumberScore = 100;
    private const int FcaFrnScoreAuthoritative = 100;
    private const int FcaFrnScoreNonAuthoritative = 20;

    private const int DomainScore = 70;
    private const int EmailScore = 60;
    private const int PhoneScore = 50;

    private const int NameScore = 40;
    private const int PostcodeScore = 30;

    public MatchResult Compare(NormalizedRecord left,
        NormalizedRecord right)
    {
        var score = 0;
        var reasons = new List<string>();
        var hasConflict = false;

        if (ValuesConflict(
            left.CompanyNumber,
            right.CompanyNumber))
        {
            hasConflict = true;

            reasons.Add(
                $"CONFLICT Company Numbers:" +
                $"{left.CompanyNumber} != {right.CompanyNumber}"
            );
        }

        if (ValuesConflict(
            left.VatNumber,
            right.VatNumber))
        {
            hasConflict = true;

            reasons.Add(
                $"CONFLICT VAT numbers: " +
                $"{left.VatNumber} != {right.VatNumber}");
        }

        if (HasFcaConflict(
            left,
            right))
        {
            hasConflict = true;

            reasons.Add(
                $"CONFLICT FCA FRNs: " +
                $"{left.FcaFrn} != {right.FcaFrn}");
        }

        if (ValuesMatch(
            left.CompanyNumber,
            right.CompanyNumber))
        {
            score += CompanyNumberScore;

            reasons.Add(
                $"+{CompanyNumberScore} company number match: " +
                left.CompanyNumber);
        }

        if (ValuesMatch(
            left.VatNumber,
            right.VatNumber))
        {
            score += VatNumberScore;

            reasons.Add(
                $"+{VatNumberScore} VAT number match: " +
                left.VatNumber);
        }

        if (HasOverlap(left.Names, right.Names))
        {
            score += NameScore;
            reasons.Add($"+{NameScore} name match");
        }

        // Additional logic to lower the match rating of scraped crawler data
        if (ValuesMatch(
            left.FcaFrn,
            right.FcaFrn))
        {
            if (left.Source == SourceType.Crawler ||
                right.Source == SourceType.Crawler)
            {
                score += FcaFrnScoreNonAuthoritative;
                reasons.Add(
                    $"+20 scraped FCA FRN match: {left.FcaFrn}");
            }
            else
            {
                score += FcaFrnScoreAuthoritative;
                reasons.Add(
                    $"+100 FCA FRN match: {left.FcaFrn}");
            }
        }

        if (ValuesMatch(
            left.Domain,
            right.Domain))
        {
            score += DomainScore;

            reasons.Add(
                $"+{DomainScore} domain match: " +
                left.Domain);
        }

        if (HasOverlap(left.Emails, right.Emails))
        {
            score += EmailScore;
            reasons.Add($"+{EmailScore} email match");
        }

        if (HasOverlap(left.Phones, right.Phones))
        {
            score += PhoneScore;
            reasons.Add($"+{PhoneScore} phone match");
        }

        if (ValuesMatch(
            left.Address?.Postcode,
            right.Address?.Postcode))
        {
            score += PostcodeScore;

            reasons.Add(
                $"+{PostcodeScore} postcode match: " +
                left.Address!.Postcode);
        }

        return new MatchResult
        {
            Left = left,
            Right = right,
            Score = score,
            IsMatch = score >= DedupeConfidenceFloor && !hasConflict,
            HasConflict = hasConflict,
            Reasons = reasons
        };
    }

    private static bool ValuesMatch(
        string? left,
        string? right)
    {
        return left is not null &&
               right is not null &&
               left == right;
    }

    private static bool HasOverlap(
        IEnumerable<string> left,
        IEnumerable<string> right)
    {
        return left.Intersect(right).Any();
    }

    private static bool ValuesConflict(
        string? left,
        string? right)
    {
        return left is not null &&
            right is not null &&
            left != right;
    }

    // Additional logic to determine if FCA source is authoritative
    // Treating only SourceType FCA as authoritative source
    private static bool HasFcaConflict(
    NormalizedRecord left,
    NormalizedRecord right)
    {
        if (left.FcaFrn is null ||
            right.FcaFrn is null)
        {
            return false;
        }

        if (left.FcaFrn == right.FcaFrn)
        {
            return false;
        }

        return IsAuthoritativeFcaSource(left) &&
            IsAuthoritativeFcaSource(right);
    }

    // Added in response to crawler FCA data being potentially unreliable
    private static bool IsAuthoritativeFcaSource(
    NormalizedRecord record)
    {
        return record.Source == SourceType.Fca;
    }
}