using Microsoft.AspNetCore.Mvc;

namespace Main.ViewModels
{
    public class VM_LMDistribute:BaseViewModel
    {
        public VM_LMDistribute(Controller controller) : base(controller)
        {
        }
        public VM_LMDistribute() { }

        public string CODE { get; set; }

        public string NAME { get; set; }

        public bool CHECK { get; set; }




    }
}
