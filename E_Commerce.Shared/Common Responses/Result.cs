using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.Common_Responses
{
    public class Result
    {
        //this Class Has Status[IsSuccess => return Value +IsFaluire => return error and Excplain this Error] + List Of Errors
        //Result Pattern internally use Compoite Design Pattern => because this Class has List of Errors + Status
        //as Result Pattern retrun List Of Error and each  Error has anthor Complex Object
        private readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0;//دى ترجع true if Countof Errors=0 يعنى مفيش اى errors
        //this Automatic Property 
        public bool isFailuire => !IsSuccess;
        public IReadOnlyList<Error> Errors => _errors;//يقدر يوصل للerrors دى من الداخل فقط مش من الخارج عشان نوعها Readonlylist=> يعنى لو عايز اوصلها هنا فى الclass دا عادى هقدر اوصلها واعملها set كمان 
                                                      //انما لو  حد من برة عايز يوصل للerrors دى مش هيعرف غير عن طريق الThis Property => بس مش هيقدر يعملها set لانه هيقدر يوصلها فقط ومش هيقدر يعدل عليها 

        //برضو هنا عايز اللى يعمل object from this Class يعمله مش بالطريقة العادية انا بقا هجبره على طريقة معينة 
        #region الطريقة العادية لعمل Object from this Class 
        protected Result() { } //in Success- Ok

        //Failure - only one Error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        //Validation Error من امثلة ان Fauilure Has Multi Error Not Single Error 
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }
        #endregion

        #region  طريقة Static Factory Method
        public static Result Ok() => new Result();//دا فى حالة الOk تنادى على Constructor الفاضى عادى جدا
        public static Result Fail(Error error) => new Result(error);//this Failure with Only one error
        public static Result Fail(List<Error> errors) => new Result(errors);//this Failure With Multi Errors
        //دا كله بيرجع فى حالة الSuccess بيرجع بobject from Result والمفروض انا بيرجع ومعاه data => Make Result Generic

        #endregion
    }

    //To return Object From Result with Data
    public class Result<TValue> : Result
    {
        //المفروض الParent Class Constructor Will Chain For Empty Parameter less Constructor in Child Class 
        //المشكلة هنا الConstructor الParent is Private وانا مش عايزه يتشاف برة بس يتورث عادى يبقى نعمل =>Make Empty Parameter Less Constrcutor in Parent as Protected
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("You CanNot Access The Value incase of Failure Scenario");//دى بعملها عشان اقدر اوصل للProperty اللى هى readonly Private لللى بر ة بس مش هيعرف يعمل عليها اى حاجة

        //Factory Method عشان اصنع object بطريقة عشان ميعمهوش بطريقة العادية 
        private Result(TValue value) : base()//this Success with Data 
        {
            _value = value;
        }


        private Result(Error error)
            : base(error)//this Fauilre with only one Error
        {
            _value = default!;
        }

        private Result(List<Error> errors) : base(errors)//Failure With Multi Errors
        {
            _value = default!;
        }

        //هوصل للConstructors دول ازاى عن طريق الStatic Factory Method
        public static Result<TValue> Ok(TValue value) => new(value);
        public new static Result<TValue> Fail(Error error) => new(error);
        public new static Result<TValue> Fail(List<Error> errors) => new(errors);
        //============================================================================
        #region Operator Overloading
        //this Operator Overloading using this => لما ابعت  return Data Direct => كدة معناها ان بعمل Object fRom Result<ProductDTO> by using Ok Methid That Take Value
        //لما اعمل return Error=> كدة انا عملت Object from Result<ProductDTO> by using Fail Method That Take Error
        //لما اعمل return List<Error> => كدة انا عملت Object from Result<ProductDTO> by using Fail Method That Take List<Error>
        public static implicit operator Result<TValue>(TValue value) => Ok(value);//دى بعملها عشان اقدر اعمل Implicit Conversion from TValue to Result<TValue> يعنى لو انا عندى object من النوع TValue اقدر اعمله Assign to Object from Result<TValue> مباشرة بدون اى مشاكل
        public static implicit operator Result<TValue>(Error error) => Fail(error);//دى بعملها عشان اقدر اعمل Implicit Conversion from Error to Result<TValue> يعنى لو انا عندى object من النوع Error اقدر اعمله Assign to Object from Result<TValue> مباشرة بدون اى مشاكل
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);//دى بعملها عشان اقدر اعمل Implicit Conversion from List<Error> to Result<TValue> يعنى لو انا عندى object من النوع List<Error> اقدر اعمله Assign to Object from Result<TValue> مباشرة بدون اى مشاكل
        #endregion
    }
}
