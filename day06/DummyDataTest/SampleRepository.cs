using Bogus;
using System;
using System.Collections.Generic;

namespace DummyDataTest
{
    class SampleRepository
    {
        public IEnumerable<Customer> GetCustomers()
        {
            Randomizer.Seed = new Random(123456);
            var genCostomer = new Faker<Customer>()
                .RuleFor(r => r.Id, Guid.NewGuid)
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Address, f => f.Address.FullAddress())
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("010-####-####"))
                .RuleFor(r => r.ContactName, f => f.Name.FullName());

            return genCostomer.Generate(100000);
        }
    }
}
