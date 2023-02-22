using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using Main.ViewModels;
using AutoMapper;
using TDSCoreLib;
using Main.platform;
using Main.Extensions;
using Main.Utils;
using System.Data;
using System.Data.OleDb;
using System.IO;
using ExcelDataReader;
using System.Web;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using X.PagedList;
using Main.ViewModels.Annotation;

namespace Main.Controllers
{
    [Route("Patient")]
    public class ANNO_PATIENTController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public ANNO_PATIENTController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }


        /// <summary>
        /// 绑定页面xx视图
        /// </summary>
        /// <returns>xx视图模型</returns>


        public IActionResult Index(string PatientId)
        {
            VM_ANNO_PATIENT viewModel = new VM_ANNO_PATIENT();
            viewModel.PATIENT_ID = PatientId;
            return View("Index", viewModel);
        }





        // 保存上传的样本数据
        [HttpPost, Route("SavePatient")]
        public IActionResult SavePatient([FromForm] ANNO_PATIENT postPatientData)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWBJ"))
            {
                return Forbid();
            }

            ANNO_PATIENT anno_patient = new ANNO_PATIENT();
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            anno_patient.PATIENT_ID = postPatientData.PATIENT_ID;
            anno_patient.PATIENT_NAME = postPatientData.PATIENT_NAME;
            anno_patient.PATIENT_GENDER = postPatientData.PATIENT_GENDER;
            anno_patient.PATIENT_AGE = postPatientData.PATIENT_AGE;
            anno_patient.PATIENT_STAGE = postPatientData.PATIENT_STAGE;
            anno_patient.PATIENT_DIAG = postPatientData.PATIENT_DIAG;
            anno_patient.PRIOR_TREAT_HIST = postPatientData.PRIOR_TREAT_HIST;
            anno_patient.ETHNICITY = postPatientData.ETHNICITY;
            anno_patient.FAMILY_HISTORY = postPatientData.FAMILY_HISTORY;
            anno_patient.CREATE_DATE = DateTime.Now;
            anno_patient.MODIFY_DATE = DateTime.Now;
            anno_patient.IS_PUB = true;
            anno_patient.IS_DELETE = false;
            anno_patient.OPERATOR = Permission.getCurrentUser();
            anno_patient.VERSION = "en-us";
            _context.ANNO_PATIENT.Add(anno_patient);
            this._context.SaveChanges();
            Log.Write(this.GetType(), "anno_patient", "Summit the patient of code:" + anno_patient.PATIENT_ID + ", Operater is" + t.USERNAME + ",ID is" + t.GID);
            return Ok(new { result = true, msg = "The patient information has been saved, please check in the sample information page." });

        }


    }
}
