using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Self.Plugin.Payments.PayBright.Services
{
    public static class SignatureHelper<TModel>
        where TModel : PayBrightModelBase
    {
        public static string CalculateSignature(TModel modelObj, string token)
        {
            Type modelType = modelObj.GetType();

            //Define a PropertyInfo Object which contains details about the class property 
            PropertyInfo[] propertyInfos = modelType.GetProperties();

            StringBuilder concatenatedValues = new StringBuilder();
            //Loop in all properties one by one to get name/value
            foreach (PropertyInfo property in propertyInfos.OrderBy(o => o.Name))
            {
                object value = property.GetValue(modelObj);
                if (value != null)
                {
                    concatenatedValues.Append(property.Name);
                    concatenatedValues.Append(property.GetValue(modelObj));
                }
            }

            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(token));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(concatenatedValues.ToString()));

            string result = BitConverter.ToString(signature).Replace("-", string.Empty);
            return result;
        }
    }
}
