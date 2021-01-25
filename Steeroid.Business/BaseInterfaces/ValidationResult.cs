using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steeroid.Business.Validation
{
    public class MyValidationResult
    {
        IList<string> messages;

        public MyValidationResult()
        {
            messages = new List<string>();
        }

        public IList<string> ErrorMessages
        {
            get
            {
                if (messages == null)
                    messages = new List<string>();

                return messages;
            }
        }

        public void AddValidationMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                this.messages.Add(message);
            }
        }

        public void AddValidationMessage(MyValidationResult newValidationResult)
        {
            foreach (string error in newValidationResult.ErrorMessages)
            {
                AddValidationMessage(error);
            }
        }

        public void AddValidationMessage(IList<string> ErrorMessages)
        {
            foreach (string error in ErrorMessages)
            {
                AddValidationMessage(error);
            }
        }

        public bool HasError
        {
            get
            {
                return this.ErrorMessages.Count > 0;
            }
        }
        
        public bool IsValid
        {
            get
            {
                return !HasError;
            }
        }

        public bool IsPropertyValidated { get; set; }
    }
}
