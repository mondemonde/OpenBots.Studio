using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Steeroid.Business.Validation
{  
    public class EntityValidator<T> where T : class
    {
        public static MyValidationResult ValidatePropertyAttribute(T entity)
        {
            if (entity == null)
            {
                return null;
            }
            MyValidationResult thisValidation = new MyValidationResult();
            MyValidationResult validation = new MyValidationResult();

            if (thisValidation.IsPropertyValidated == false)
            {
                var validationResults = new List<ValidationResult>();
                var vc = new ValidationContext(entity, null, null);
                var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);
               // var result = new MyValidationResult(validationResults);
                foreach (ValidationResult propertyValidator in validationResults)
                {
                    validation.AddValidationMessage(propertyValidator.ErrorMessage);
                }
                validation.IsPropertyValidated = true;

                return validation;
            }
            else
            {
                //validation.Errors.Clear();//= new List<ValidationResult>();
                return validation;// thisValidation;

            }   
        }
    }

    //public class ValidationHelper
    //{
    //    public static MyValidationResult ValidateEntity<T>(T entity, ref MyValidationResult validation)
    //        where T : class
    //    {
           
    //        return new EntityValidator<T>().ValidatePropertyAttribute(entity,ref validation);
    //    }

    //}
}
