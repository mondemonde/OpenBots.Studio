using Steeroid.Business.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models.BaseInterfaces
{
    public class ServiceResponse<T>
    {
       public T model;

        MyValidationResult validationMessage;

        public MyValidationResult Validation
        {
            get
            {
                return validationMessage;
            }

            //set
            //{
            //    validationMessage = value;
            //}
        }

        public void AddValidation(MyValidationResult validator)
        {
            if(validator!=null)
              this.validationMessage.AddValidationMessage(validator);
        }


        public ServiceResponse()
        {

            this.validationMessage = new MyValidationResult();
        }

        public void AddModel(T model)
        {
            //if (model != default(T))
            //{
            //    this.model = model;
            //}
            this.model = model;
            this.HasData = true;
        }

        public T Model
        {
            get
            {
                return model;
            }
        }

        public void AddValidationMessage(string message)
        {
            this.validationMessage.AddValidationMessage(message);
        }

        public void AddValidationMessage(IList<string> messages)
        {
            if (messages == null)
                return;

            foreach (string message in messages)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    this.validationMessage.AddValidationMessage(message);
                }
            }
        }


        public void AddValidationMessage(MyValidationResult newValidation)
        {
            foreach (string message in newValidation.ErrorMessages)
            {
                validationMessage.AddValidationMessage(message);
            }

        }

        public IList<string> ErrorMessages
        {
            get
            {
                return validationMessage.ErrorMessages;
            }
        }

        public bool HasError
        {
            get
            {
                return validationMessage.HasError;
            }
        }

        public bool IsValid
        {
            get
            {
                if (HasData && ErrorMessages.Count == 0)
                {
                    return true;
                }
                else
                    return false;


            }
        }



        public bool HasData
        {
            //get
            //{
            //    return model != default(T);
            //}

            get;
            private set;

        }



        public bool IsValidForNewResponse<U>(ServiceResponse<U> newResponse, bool addErrorMessage = true)
        {
            if (this.HasError)
            {
                if (addErrorMessage)
                    newResponse.AddValidationMessage(this.ErrorMessages);
                return false;
            }

            if (!this.HasData)
            {
                if (addErrorMessage)
                    newResponse.AddValidationMessage("Has no data.");
                return false;
            }

            else
                return true;



        }


    }

}
