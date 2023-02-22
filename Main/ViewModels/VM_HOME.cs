using Microsoft.AspNetCore.Mvc;

namespace Main.ViewModels
{
    public partial class VM_HOME : BaseViewModel
    {
        public VM_HOME(Controller controller) : base(controller) {

        }

        public VM_HOME() { }
        public string userinfo { get; set; }
    }
}
