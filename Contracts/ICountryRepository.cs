﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface ICountryRepository 
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync(bool trackChanges);
        Country GetCountryById(int countryId, bool trackChanges);
		int CreateCountry(Country Country);

	}
}
