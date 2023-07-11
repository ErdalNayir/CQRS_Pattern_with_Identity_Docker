using Domain.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Validations
{
	public class ActivityValidator:AbstractValidator<Activity>
	{
		public ActivityValidator() 
		{
			RuleFor(x=>x.Title).NotEmpty();
			RuleFor(x => x.Description).NotEmpty();
			RuleFor(x => x.City).NotEmpty();
			RuleFor(x => x.Venue).NotEmpty();
			RuleFor(x => x.Category).NotEmpty();
			RuleFor(x => x.Date).NotEmpty();
			
		}
	}
}
