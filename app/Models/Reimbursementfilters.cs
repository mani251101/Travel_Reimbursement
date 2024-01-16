using Microsoft.AspNetCore.Mvc.Filters;
namespace filters
{
    public class Reimbursementfilters : ActionFilterAttribute
    {
        private static int _counter = 0;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _counter++;
            base.OnActionExecuted(filterContext);
        }

        public static int GetCounter()
        {
            return _counter;
        }
    }
}
