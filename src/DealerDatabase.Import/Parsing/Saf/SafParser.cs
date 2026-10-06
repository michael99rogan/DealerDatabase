using System.Xml.Linq;
using DealerDatabase.Import.Parsing.Saf.Models;

namespace DealerDatabase.Import.Parsing.Saf;

public class SafParser
{
    public SafExport Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "SAF data file was not found.",
                filePath);
        }

        var document = XDocument.Load(filePath);

        var root = document.Root;

        if (root is null || root.Name != "SafRegister")
        {
            throw new InvalidDataException(
                $"Expected SAF document root 'SafRegister' in '{filePath}'.");
        }

        return new SafExport
        {
            Exported = (string?)root.Attribute("exported"),

            Members = root
                .Elements("Member")
                .Select(ParseMember)
                .ToList()
        };
    }

    private static SafMember ParseMember(XElement element)
    {
        return new SafMember
        {
            Id = (string?)element.Attribute("id"),

            Name = (string?)element.Element("Name"),
            LegalName = (string?)element.Element("LegalName"),
            TradingAs = (string?)element.Element("TradingAs"),

            Town = (string?)element.Element("Town"),
            Postcode = (string?)element.Element("Postcode"),

            Telephone = (string?)element.Element("Telephone"),
            Website = (string?)element.Element("Website"),

            Status = (string?)element.Element("Status"),
            Expiry = (string?)element.Element("Expiry")
        };
    }
}