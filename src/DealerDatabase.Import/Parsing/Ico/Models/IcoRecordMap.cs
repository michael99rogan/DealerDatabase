using CsvHelper.Configuration;

namespace DealerDatabase.Import.Parsing.Ico.Models;

public sealed class IcoRecordMap : ClassMap<IcoRecord>
{
    public IcoRecordMap()
    {
        Map(m => m.RegistrationNumber)
            .Name("Registration_number");

        Map(m => m.OrganisationName)
            .Name("Organisation_name");

        Map(m => m.CompanyRegistrationNumber)
            .Name("Company_registration_number");

        Map(m => m.AddressLine1)
            .Name("Organisation_address_line_1");

        Map(m => m.AddressLine2)
            .Name("Organisation_address_line_2");

        Map(m => m.AddressLine3)
            .Name("Organisation_address_line_3");

        Map(m => m.Postcode)
            .Name("Organisation_postcode");

        Map(m => m.PublicAuthority)
            .Name("Public_authority");

        Map(m => m.StartDateOfRegistration)
            .Name("Start_date_of_registration");

        Map(m => m.EndDateOfRegistration)
            .Name("End_date_of_registration");

        Map(m => m.PaymentTier)
            .Name("Payment_tier");

        Map(m => m.TradingNames)
            .Name("Trading_names");
    }
}