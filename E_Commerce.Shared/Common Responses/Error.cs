using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.Common_Responses
{
    // دلؤقتى الطريقة العادية للhandle Exception => throw Exception يعنى بيخلى الApplication Stop his Stack Trace and CLR Create New Stack Trace  ودا مش احسن فى الPerformance لان الطريقة دى بعد كدة هستخدمها عشان handle unexpected Error 
    //انما الExcepeted Error اعملها بالطريقة Result Pattern => Create Object From this class and this Object has Error+ Status 
    //كل دا عشان اعلى فى الPerformnace عشان كدة الResult Pattern دا مستخدم عشان Handle Response + Error فى حالة الExcpedted Error  عشان الClr ميعملشى new Stack For this App لاء هيخليه يمشى فى نفس الStack
    public class Error
    {
        //انا عايز اللى يعمل object من الclass دا يعمله بطريقة غير عادية يعنى مش يعمله وبعدين يبعت الParameter وينادى على الConstructor لاء انا عايزه يتعمل بطريقة Customized اكون انا اللى عاملها وهو يمشى عليها 
        //عشان كدة عملت Private Constructor + Factory Method=> كل دا using Factory Design Pattern
        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }
        
        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }
        //بعمل لكل Type of Error => one Static Factory Method 
        //يعنى لو عندى Five Types of error => Make Five Static Factory Method
        public static Error Failure(string code="Genral Failure", string description="A General Failure has Occured")
        {
            return new Error(code, description, ErrorType.Faluire);
        }
        //this Static Factory Method For Validation Error
        public static Error Validation(string code = "Validation Failure", string description = "A Validation Error has Occured")
        {
            return new Error(code, description, ErrorType.Validation);
        }
        //كدة انا خليت اى حد عايز يعمل object from this Class مش هيعمله بالطريقة العادية لاء هيعمله بالطريقة اللى انا هخليه يعملها using Static Factory Method انما مش هيعمل Object using Parameterized Constructor
        //this Static Factory Method For Not Found Error
        public static Error NotFound(string code = "NotFound Failure", string description = "the Request Resource Was Not Found")
        {
            return new Error(code, description, ErrorType.NotFound);
        }
        //this Static Factory Method For UnAuthorized Error
        public static Error UnAuthorized(string code = "UnAuthorized Failure", string description = "You Are Not Authorized To Perform this Action")
        {
            return new Error(code, description, ErrorType.UnAuthorized);
        }
        //this Static Factory Method For Forbidden Error
        public static Error Forbidden(string code = "Forbidden Failure", string description = "You donot have the Access to this resource, access denied")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        //this Static Factory Method For Invalid Credentials Error
        public static Error InvalidCredentials(string code = "Invalid Credentials Failure", string description = "Your Credentials  is  Invalid to reach this resource")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }


    }
}
