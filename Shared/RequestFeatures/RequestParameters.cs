﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
	public class RequestParameters
	{
		const int maxPageSize = 50;
		public int PageNumber { get; set; } = 1;       
        private int _pageSize = 10;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
                                           
		//public string? ordereby { get; set; }

        public override string ToString()
		{
			return maxPageSize.ToString() + PageNumber.ToString() + _pageSize.ToString();
		}
	}
}
