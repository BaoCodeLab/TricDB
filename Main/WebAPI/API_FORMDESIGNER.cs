using AutoMapper;
using Main.platform;
using Main.Utils;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using TDSCoreLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/formdesigner")]
    public class API_FORMDESIGNER : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly drugdbContext _context;

        public API_FORMDESIGNER(drugdbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取地区选择框数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ActionResult getPickerData()
        {
            FileStream fileStream = new FileStream(_hostingEnvironment.WebRootPath + "/static/area.json", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string data = reader.ReadToEnd();
                return Ok(data);
            }
        }

        /// <summary>
        /// ueditor配置
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult ueditorConfig()
        {

            var config = new
            {

                videoMaxSize = 102400000,
                videoActionName = "uploadvideo",
                fileActionName = "uploadfile",
                fileManagerListPath = "/ueditor/jsp/upload/file/",
                imageCompressBorder = 1600,
                imageManagerAllowFiles = new string[] {
        ".png",
        ".jpg",
        ".jpeg",
        ".gif",
        ".bmp"
    },
                imageManagerListPath = "/ueditor/jsp/upload/image/",
                fileMaxSize = 51200000,
                fileManagerAllowFiles = new string[] {
        ".png",".jpg",".jpeg",".gif",".bmp",".flv",".swf",".mkv",".avi",".rm",".rmvb",".mpeg",".mpg",".ogg", ".ogv",".mov",".wmv",".mp4",".webm",".mp3",".wav",".mid",".rar",".zip",".tar",".gz",".7z",".bz2",".cab",".iso",".doc",".docx",".xls",".xlsx",".ppt",".pptx",".pdf",".txt",".md",".xml"
    },
                fileManagerActionName = "listfile",
                snapscreenInsertAlign = "none",
                scrawlActionName = "uploadscrawl",
                videoFieldName = "upfile",
                imageCompressEnable = true,
                videoUrlPrefix = "/ibps/components/upload/ueditor/download.htm?filePath=",
                fileManagerUrlPrefix = "",
                catcherAllowFiles = new string[] {
        ".png",".jpg",".jpeg",".gif",".bmp"
    },
                imageManagerActionName = "listimage",
                snapscreenPathFormat = "/ueditor/jsp/upload/image/{yyyy}{mm}{dd}/{time}{rand:6}",
                scrawlPathFormat = "/ueditor/jsp/upload/image/{yyyy}{mm}{dd}/{time}{rand:6}",
                scrawlMaxSize = 2048000,
                imageInsertAlign = "none",
                catcherPathFormat = "/ueditor/jsp/upload/image/{yyyy}{mm}{dd}/{time}{rand:6}",
                catcherMaxSize = 2048000,
                snapscreenUrlPrefix = "",
                imagePathFormat = "/ueditor/jsp/upload/image/{yyyy}{mm}{dd}/{time}{rand:6}",
                imageManagerUrlPrefix = "/ibps/components/upload/ueditor/preview.htm?imagePath=",
                scrawlUrlPrefix = "/ibps/components/upload/ueditor/preview.htm?imagePath=",
                scrawlFieldName = "upfile",
                imageMaxSize = 2048000,
                imageAllowFiles = new string[] {
        ".png",".jpg",".jpeg",".gif",".bmp"
    },
                snapscreenActionName = "uploadimage",
                catcherActionName = "catchimage",
                fileFieldName = "upfile",
                fileUrlPrefix = "/ibps/components/upload/ueditor/download.htm?filePath=",
                imageManagerInsertAlign = "none",
                catcherLocalDomain = new string[] {
        "202.114.144.115",
        "localhost",
        "img.baidu.com"
    },
                filePathFormat = "/ueditor/jsp/upload/file/{yyyy}{mm}{dd}/{time}{rand:6}",
                videoPathFormat = "/ueditor/jsp/upload/video/{yyyy}{mm}{dd}/{time}{rand:6}",
                fileManagerListSize = 20,
                imageActionName = "uploadimage",
                imageFieldName = "upfile",
                imageUrlPrefix = "/ibps/components/upload/ueditor/preview.htm?imagePath=",
                scrawlInsertAlign = "none",
                fileAllowFiles = new string[] {
        ".png",".jpg",".jpeg",".gif",".bmp",".flv",".swf",".mkv",".avi",".rm",".rmvb",".mpeg",".mpg",".ogg",".ogv",
        ".mov",".wmv",".mp4",".webm",".mp3",".wav",".mid",".rar",".zip",".tar",".gz",".7z",".bz2",".cab",".iso",
        ".doc",".docx",".xls",".xlsx",".ppt",".pptx",".pdf",".txt",".md",".xml"
    },
                catcherUrlPrefix = "",
                imageManagerListSize = 20,
                catcherFieldName = "source",
                videoAllowFiles = new string[] {
        ".flv",".swf",".mkv",".avi",".rm",".rmvb",".mpeg",".mpg",".ogg",".ogv",".mov",".wmv",".mp4",".webm",".mp3",".wav",".mid"
    }

            };
            return Ok(config);

        }

        /// <summary>
        /// 读取已配置的自动编号规则
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getSelectorData([FromQuery]string type)
        {
            var item1 = new
            {
                pk = "",
                name = "项目编号",
                ip = "",
                createBy = "",
                createTime = "",
                updateBy = "",
                updateTime = "",
                createOrgId = "",
                tenantI = "",
                dbtype = "",
                id = "478526191620325376",
                alias = "xmbh",
                regulation = "ITEM-{yyyy}{MM}{dd}{NO}",
                genType = 3,
                noLength = 5,
                curData = "",
                initValue = 1,
                curValue = "",
                step = 1
            };
            var item2 = new
            {
                pk = "",
                name = "内贸合同编号",
                ip = "",
                createBy = "",
                createTime = "",
                updateBy = "",
                updateTime = "",
                createOrgId = "",
                tenantI = "",
                dbtype = "",
                id = "475954820772003840",
                alias = "nmhtbh",
                regulation = "ZXY-GM-{yyyy}-{NO}",
                genType = 3,
                noLength = 4,
                curData = "",
                initValue = 1,
                curValue = "",
                step = 1
            };
            var result = new
            {
                result = true,
                data = new ArrayList() { item1, item2 }
            };
            return Ok(result);
        }

        /// <summary>
        /// 读取关联数据源配置列表
        /// </summary>
        /// <param name="cascade"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getRelatedDataList([FromQuery]string cascade = "true", string type = "valueSource")
        {

            var result = new
            {

                result = true,
                data = new ArrayList() {
                    new {
                        pk="",
                        name="差旅报销单数据源",
                        ip=string.Empty,
                        createBy=string.Empty,
                        createTime=string.Empty,
                        updateBy=string.Empty,
                        updateTime=string.Empty,
                        createOrgId=string.Empty,
                        tenantId=string.Empty,
                        dbtype=string.Empty,
                        id="492015792762650624",
                        datasetKey=string.Empty,
                        key="clbxdsjy",
                        desc=string.Empty,
                        typeId=string.Empty,
                        type=string.Empty,
                        showType=string.Empty,
                        composeType=string.Empty,
                        unique=string.Empty,
                        dialogs=string.Empty,
                        attrs=string.Empty,
                        fieldList=string.Empty,
                        tplList=string.Empty,
                        queryColumns="",
                        filterConditions="",
                        resultColumns=new ArrayList() {new { label="姓名",name="xing_ming_",same="Y",field_type=string.Empty,index=1} }
                    },
                    new {
                        pk="",
                        name="学校组织机构值来源",
                       ip=string.Empty,
                        createBy=string.Empty,
                        createTime=string.Empty,
                        updateBy=string.Empty,
                        updateTime=string.Empty,
                        createOrgId=string.Empty,
                        tenantId=string.Empty,
                        dbtype=string.Empty,
                        id="476073791089278976",
                        datasetKey=string.Empty,
                        key="xxzzjgzly",
                        desc=string.Empty,
                        typeId=string.Empty,
                        type=string.Empty,
                        showType=string.Empty,
                        composeType=string.Empty,
                        unique=string.Empty,
                        dialogs=string.Empty,
                        attrs=string.Empty,
                        fieldList=string.Empty,
                        tplList=string.Empty,
                        queryColumns=new ArrayList(){new {label="主键",name="id_",index=1},new {label="行政岗位",name="gang_wei_",index=2 },new {label="管辖事宜",name="guan_xia_shi_yi_",index=3},new {label="组织父ID",name="zu_zhi_fu_i_d_",index=4} },
                        filterConditions="",
                        resultColumns=new ArrayList(){new { label="行政岗位",name="gang_wei_",index=1},new{label="主键",name="id_",index=2},new {label="管辖事宜",name="guan_xia_shi_yi_",index=3},new {label="组织父ID",name="zu_zhi_fu_i_d_",index=4} }
                    }
                }
            };
            return Ok(result);
        }

        /// <summary>
        /// 根据关联数据源配置获取数据字段列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getRelatedDataByKey([FromQuery]string key)
        {
            List<dynamic> test = new List<dynamic>() {
                    new {
                        pk="",
                        name="差旅报销单数据源",
                        ip=string.Empty,
                        createBy=string.Empty,
                        createTime=string.Empty,
                        updateBy=string.Empty,
                        updateTime=string.Empty,
                        createOrgId=string.Empty,
                        tenantId=string.Empty,
                        dbtype=string.Empty,
                        id="492015792762650624",
                        datasetKey=string.Empty,
                        key="clbxdsjy",
                        desc=string.Empty,
                        typeId=string.Empty,
                        type=string.Empty,
                        showType=string.Empty,
                        composeType=string.Empty,
                        unique=string.Empty,
                        dialogs=string.Empty,
                        attrs=string.Empty,
                        fieldList=string.Empty,
                        tplList=string.Empty,
                        queryColumns="",
                        filterConditions="",
                        resultColumns=new ArrayList() {new { label="姓名",name="xing_ming_",same="Y",field_type=string.Empty,index=1} }
                    },
                    new {
                        pk="",
                        name="学校组织机构值来源",
                       ip=string.Empty,
                        createBy=string.Empty,
                        createTime=string.Empty,
                        updateBy=string.Empty,
                        updateTime=string.Empty,
                        createOrgId=string.Empty,
                        tenantId=string.Empty,
                        dbtype=string.Empty,
                        id="476073791089278976",
                        datasetKey=string.Empty,
                        key="xxzzjgzly",
                        desc=string.Empty,
                        typeId=string.Empty,
                        type=string.Empty,
                        showType=string.Empty,
                        composeType=string.Empty,
                        unique=string.Empty,
                        dialogs=string.Empty,
                        attrs=string.Empty,
                        fieldList=string.Empty,
                        tplList=string.Empty,
                        queryColumns=new ArrayList(){new {label="主键",name="id_",index=1},new {label="行政岗位",name="gang_wei_",index=2 },new {label="管辖事宜",name="guan_xia_shi_yi_",index=3},new {label="组织父ID",name="zu_zhi_fu_i_d_",index=4} },
                        filterConditions="",
                        resultColumns=new ArrayList(){new { label="行政岗位",name="gang_wei_",index=1},new{label="主键",name="id_",index=2},new {label="管辖事宜",name="guan_xia_shi_yi_",index=3},new {label="组织父ID",name="zu_zhi_fu_i_d_",index=4} }
                    }
                };
            var data = test.Where(w => w.key == key).First();
            var result = new
            {

                result = true,
                data
            };
            return Ok(result);
        }

        /// <summary>
        /// 根据字段获取关联数据源数据（Post方式）
        /// </summary>
        /// <param name="queryKey"></param>
        /// <param name="dynamicParams"></param>
        /// <param name="key"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getRelatedData([FromQuery]string queryKey, string dynamicParams, string key, int page = 1, int rows = 20)
        {
            ArrayList test = new ArrayList() {
                    new {
                        id_ ="476069305293733888",
                        guan_xia_shi_yi_="",
                        zu_zhi_fu_i_d_="0",
                        gang_wei_="校长"
                    },
                    new {
                        id_ ="476069866084761600",
                        guan_xia_shi_yi_="",
                        zu_zhi_fu_i_d_="0",
                        gang_wei_="党总支副书记"
                    }
                };
            var result = new
            {

                result = true,
                data = test
            };
            return Ok(result);
        }

        /// <summary>
        /// 获取状态类，combobox下拉框组件
        /// </summary>
        /// <param name="typekey"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult getByStateTypeForComBo([FromQuery]string typekey)
        {
            var result = _context.PF_STATE.Where(w => w.IS_DELETE == false && w.TYPE == typekey).OrderBy(o => o.CODE).Select(s => new { id = s.GID, parentId = s.TYPE, name = s.NAME, key = s.CODE }).ToList();
            return Ok(result);
        }

        /// <summary>
        /// 根据字段获取关联数据源数据（Get方式）
        /// </summary>
        /// <param name="typekey"></param>
        /// <param name="displayMode"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getByStateTypeForComBo([FromQuery]string typekey, string displayMode = "path", string split = "/")
        {
            var result = _context.PF_STATE.Where(w => w.IS_DELETE == false && w.TYPE == typekey).OrderBy(o => o.CODE).Select(s => new { id = s.GID, parentId = s.TYPE, name = s.NAME, key = s.CODE }).ToList();
            return Ok(result);
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getCurrentUserInfo()
        {
            var queryResult = from s in _context.PF_USER
                              join c in _context.PF_PROFILE on s.USERNAME equals c.DLZH into grp
                              from g in grp.DefaultIfEmpty()
                              where s.IS_DELETE == false && s.USERNAME == Permission.getCurrentUser()
                              select new
                              {
                                  userID = s.USERNAME,
                                  userName = g == null ? s.USERNAME : g.NAME

                              };
            return Ok(queryResult.SingleOrDefault());

        }
        /// <summary>
        /// 根据流水配置编号
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getNextIdByAlias([FromQuery] string alias)
        {

            var result = new
            {
                result = 1,
                message = "ZXY-GM-2018-0001",
                cause = ""
            };
            return Ok(result);
        }

        /// <summary>
        /// 获取PF_State中Type列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult getStateTypes()
        {
            var typeList = _context.PF_STATE.Where(w => w.IS_DELETE == false).Select(s => new { name = s.TYPE, typeKey = s.TYPE }).Distinct().ToList();
            return Ok(typeList);
        }

        /// <summary>
        /// 根据PF_State的Type字段获取值列表
        /// </summary>
        /// <param name="typeKey"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult getNameByStateType([FromQuery]string typeKey)
        {
            var nameList = _context.PF_STATE.Where(w => w.TYPE == typeKey).Select(s => new { id = s.GID, parentId = s.TYPE, name = s.NAME, key = s.CODE }).ToList();
            return Ok(nameList);
        }

        /// <summary>
        /// 获取当前用户上传文件的列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult fileListJsonByUser([FromQuery]int page = 1, int rows = 10, string field = "CREATE_DATE", string order = "desc")
        {
            var filelist = _context.PF_FILE
           .Where(("OPERATOR==@0 and is_delete == false"), Permission.getCurrentUser()) //like查找
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * rows) //跳过前x项
           .Take(rows)//从当前位置开始取前x项
           .Select(s => new { fileName = s.FILENAME, ext = s.TYPE.Replace(".", ""), creator = s.OPERATOR, createTime = s.CREATE_DATE.ToString("yyyy-MM-dd HH:mm:ss"), id = s.GID, filePath = s.FILEURI })
           .ToList();//将结果转为List类型
            var result = new
            {
                records = filelist.Count(),
                page,
                total = (filelist.Count() / rows) + 1,
                rows = filelist
            };
            return Ok(result);
        }

        /// <summary>
        /// 新建或保存表单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [HttpPost("save")]
        public ActionResult Save([FromForm]string data)
        {
            dynamic obj = JsonConvert.DeserializeObject(data);
            string attrs = JsonConvert.SerializeObject(obj.attrs);
            string name = obj.name;
            string key = obj.key;
            string typeName = obj.typeName;
            string busId = obj.busId;//业务对象编号
            string code = obj.code;//业务对象编码
            typeName = string.IsNullOrEmpty(typeName) ? "" : typeName;
            string typeId = obj.typeId;
            typeId = string.IsNullOrEmpty(typeId) ? "" : typeId;
            string desc = obj.desc;
            desc = string.IsNullOrEmpty(desc) ? "" : desc;
            string id = obj.id;
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                _context.FD_FORM.Add(new FD_FORM() { ID = id, NAME = name, DESC = desc, KEY = key, TYPEID = typeId, MODE = "bo", TYPENAME = typeName, BUSID = busId, CODE = code, ATTRS = attrs, PERMISSIONS = "", VERSION = DateTime.Now.ToString("yyyyMMddHHmmss"), CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser(), IS_DELETE = false });
            }
            else
            {
                _context.FD_FORM.Update(new FD_FORM() { ID = id, NAME = name, DESC = desc, KEY = key, TYPEID = typeId, MODE = "bo", TYPENAME = typeName, BUSID = busId, CODE = code, ATTRS = attrs, PERMISSIONS = "", VERSION = DateTime.Now.ToString("yyyyMMddHHmmss"), CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser(), IS_DELETE = false });

            }
            List<FD_FORM_FIELD> fieldlist = _context.FD_FORM_FIELD.Where(w => w.FORM_ID == id && w.IS_DELETE == false).ToList();
            int paixu = 0;
            foreach (dynamic field in obj.fields)
            {
                paixu++;
                string f_name = field.name;
                string f_showName = field.showName;
                string f_label = field.label;
                string f_field_type = field.field_type;
                string f_desc = field.desc;
                string f_field_name = field.field_name;
                string f_dataType = field.dataType;
                string f_options = JsonConvert.SerializeObject(field.field_options);
                f_desc = string.IsNullOrEmpty(f_desc) ? "" : f_desc;
                f_field_name = string.IsNullOrEmpty(f_field_name) ? "" : f_field_name;
                f_label = string.IsNullOrEmpty(f_label) ? "" : f_label;
                f_dataType = string.IsNullOrEmpty(f_dataType) ? "" : f_dataType;
                int hasField = fieldlist.Where(w => w.FORM_ID == id && w.NAME == f_name).Count();
                if (hasField > 0)
                {
                    var currentField = fieldlist.Where(w => w.NAME == f_name && w.FORM_ID == id).First();
                    currentField.NAME = f_name;
                    currentField.LABEL = f_label;
                    currentField.SHOWNAME = f_showName;
                    currentField.DESC = f_desc;
                    currentField.FIELD_TYPE = f_field_type;
                    currentField.FIELD_OPTIONS = f_options;
                    currentField.FIELD_NAME = f_field_name;
                    currentField.DATATYPE = f_dataType;
                    currentField.VERSION = "0";
                    currentField.ORDER = paixu;
                    currentField.MODIFY_DATE = DateTime.Now;
                    currentField.OPERATOR = Permission.getCurrentUser();

                    _context.FD_FORM_FIELD.Update(currentField);

                }
                else
                {
                    _context.FD_FORM_FIELD.Add(new FD_FORM_FIELD() { ID = Guid.NewGuid().ToString(), FORM_ID = id, NAME = f_name, LABEL = f_label, SHOWNAME = f_showName, DESC = f_desc, FIELD_TYPE = f_field_type, FIELD_OPTIONS = f_options, FIELD_NAME = f_field_name, DATATYPE = f_dataType, VERSION = "0", CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser(), IS_DELETE = false });

                }
            }
            _context.SaveChanges(true);

            var result = new { result = 1, message = "保存表单模版成功", cause = "", id = "" };
            return Ok(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 执行表单拷贝
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult saveCopy([FromForm]string id, string key, string name)
        {
            if (!string.IsNullOrEmpty(id))
            {
                FD_FORM o = _context.FD_FORM.Where(w => w.ID == id).First();
                string newID = Guid.NewGuid().ToString();
                o.ID = newID;
                o.KEY = key;
                o.NAME = name;
                _context.FD_FORM.Add(o);
                List<FD_FORM_FIELD> o_f = _context.FD_FORM_FIELD.Where(w => w.FORM_ID == id).ToList();
                foreach (FD_FORM_FIELD o_ff in o_f)
                {
                    o_ff.ID = Guid.NewGuid().ToString();
                    o_ff.FORM_ID = newID;
                    _context.FD_FORM_FIELD.Add(o_ff);
                }
                int result = _context.SaveChanges(true);
                if (result > 0)
                {
                    return Ok("{\"result\":1,\"message\":\"复制表单成功\",\"cause\":\"\"}");

                }
                else
                {
                    return Ok("{\"result\":0,\"message\":\"复制表单失败，请检查原因\",\"cause\":\"\"}");

                }

            }
            else
            {
                return Ok("{\"result\":0,\"message\":\"未指定待复制的表单\",\"cause\":\"\"}");
            }
        }

        /// <summary>
        /// 删除自定义表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult removeForm(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Ok("{\"result\":0,\"message\":\"表单编号未指定，删除失败\",\"cause\":\"\"}");
            }
            else
            {
                var targetForm = _context.FD_FORM.Where(w => w.ID == id).First();
                var targetFormFields = _context.FD_FORM_FIELD.Where(w => w.FORM_ID == id).ToList();
                targetForm.IS_DELETE = true;
                foreach (FD_FORM_FIELD ff in targetFormFields)
                {
                    ff.IS_DELETE = true;
                    _context.FD_FORM_FIELD.Update(ff);
                }
                _context.FD_FORM.Update(targetForm);
                int result = _context.SaveChanges(true);
                if (result > 0)
                {
                    return Ok("{\"result\":1,\"message\":\"删除表单成功！\",\"cause\":\"\"}");

                }
                else
                {
                    return Ok("{\"result\":0,\"message\":\"未删除表单，请检查原因\",\"cause\":\"\"}");

                }
            }
        }

        /// <summary>
        /// 获取业务对象分类
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult getBOTypeTreeData([FromForm]string categoryKey)
        {
            var result = new ArrayList() {
                new {
                        pk="",
                       name="正式表单",
                       ip="",
                       createBy="1",
                       createTime="2018-08-09 13:56:30",
                       updateBy="",
                       updateTime="",
                       createOrgId="",
                       tenantId="",
                       dbtype="",
                       subs=new ArrayList() { },
                       id="477112943952003072",
                       categoryKey="BO_TYPE",
                       typeKey="prd",
                       struType="1",
                       parentId="286814138233389362",
                       depth=1,
                       path="286814138233389362.477112943952003072.",
                       isLeaf="Y",
                       ownerId="0",
                       sn=1
                },new {
                    pk ="",
                   name="测试表单",
                   ip="",
                   createBy="1",
                   createTime="2018-08-09 08:46:15",
                   updateBy="",
                   updateTime="",
                   createOrgId="",
                   tenantId="",
                   dbtype="",
                   subs=new ArrayList() { },
                   id="477034867381501952",
                   categoryKey="BO_TYPE",
                   typeKey="dev",
                   struType="1",
                   parentId="286814138233389362",
                   depth=1,
                   path="286814138233389362.477034867381501952.",
                   isLeaf="Y",
                   ownerId="0",
                   sn=2 },
                new {
                       pk="",
                       name="业务对象分类",
                       ip="",
                       createBy="",
                       createTime="",
                       updateBy="",
                       updateTime="",
                       createOrgId="",
                       tenantId="",
                       dbtype="",
                       subs=new ArrayList() { },
                       id="286814138233389362",
                       categoryKey="BO_TYPE",
                       typeKey="BO_TYPE",
                       struType="",
                       parentId="-1",
                       depth=0,
                       path="",
                       isLeaf="Y",
                       ownerId="",
                       sn=0
                }
            };
            return Ok(result);
        }

        /// <summary>
        /// 中文转拼音首字母缩写
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="chinese"></param>
        /// <param name="mode"></param>
        /// <param name="type"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult pinyinServlet([FromForm]string rand, string chinese, string mode, string type)
        {
            if (string.IsNullOrEmpty(chinese))
            {
                return Ok();
            }
            else
            {
                return Ok(pinyinHelper.IndexCode(chinese));
            }
        }

        /// <summary>
        /// 检查业务对象code是否重复
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <param name="isMain"></param>
        /// <param name="isMaster"></param>
        /// <param name="pid"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult BOcheckCode([FromForm]string code, string id, string isMain, string isMaster, string pid)
        {
            int c = _context.FD_BO.Where(w => w.CODE == code).Count();
            var result = new { result = 0, message = "", cause = "" };
            if (c == 0 || !string.IsNullOrEmpty(id))
            {
                result = new { result = 1, message = "", cause = "" };
            }
            return Ok(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 保存业务对象修改、新建结果
        /// </summary>
        /// <param name="boDefs"></param>
        /// <param name="saveType"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult BOsave([FromForm]string boDefs, string saveType)
        {
            dynamic obj = JsonConvert.DeserializeObject(boDefs);
            string name = obj[0].name;
            string code = obj[0].code;
            string boType = "object";
            string state = obj[0].state;
            //string typeId = obj[0].typeId;
            string typeId = "477034867381501952";
            string status = "actived";//obj[0].status;
            string pk = obj[0].pk;
            string options = JsonConvert.SerializeObject(obj[0].options);
            string relation = obj[0].relation;//one2many
            string desc = obj[0].desc;
            string isMaster = obj[0].isMaster;
            string id = obj[0].id;
            string dataFormat = obj[0].dataFormat;
            string newid = Guid.NewGuid().ToString();
            if (state == "new")
            {
                _context.FD_BO.Add(new FD_BO() { ID = newid, PK = pk, TYPEID = typeId, OPTIONS = options, NAME = name, CODE = code, BOTYPE = boType, STATUS = status, DATAFORMAT = dataFormat, PARENTID = "0", CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser(), IS_DELETE = false });
            }
            else
            {
                _context.FD_BO.Update(new FD_BO() { ID = id, PK = pk, TYPEID = typeId, OPTIONS = options, NAME = name, CODE = code, BOTYPE = boType, STATUS = status, DATAFORMAT = dataFormat, PARENTID = "0", CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser(), IS_DELETE = false });
            }

            foreach (dynamic attr in obj[0].attrs)
            {
                string attr_id = attr.id;
                string attr_name = attr.name;
                string attr_code = attr.code;
                string attr_fieldName = attr.fieldName;
                string attr_dataType = attr.dataType;
                int attr_attrLength = 50;
                int.TryParse(attr.attrLength.ToString(), out attr_attrLength);
                int attr_precision = 0;
                int.TryParse(attr.precision.ToString(), out attr_precision);
                string attr_format = attr.format == null ? "" : attr.format;
                string attr_isNull = attr.isNull == null ? "" : attr.isNull;
                string attr_defValue = attr.defValue == null ? "" : attr.defValue;
                string attr_desc = attr.desc == null ? "" : attr.desc;
                if (state == "new")
                {
                    _context.FD_BO_FIELD.Add(new FD_BO_FIELD() { ID = Guid.NewGuid().ToString(), BO_ID = newid, NAME = attr_name, CODE = attr_code, FIELDNAME = attr_fieldName, DATATYPE = attr_dataType, ATTRLENGTH = attr_attrLength, PRECISION = attr_precision, FORMAT = attr_format, DESC = attr_desc, DEFVALUE = attr_defValue, ISNULL = attr_isNull, BZ = "", CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser(), IS_DELETE = false });
                }
                else
                {
                    int count = _context.FD_BO_FIELD.Where(w => w.ID == attr_id).Count();
                    if (count == 1)
                    {
                        _context.FD_BO_FIELD.Update(new FD_BO_FIELD() { ID = attr_id, BO_ID = id, NAME = attr_name, CODE = attr_code, FIELDNAME = attr_fieldName, DATATYPE = attr_dataType, ATTRLENGTH = attr_attrLength, PRECISION = attr_precision, FORMAT = attr_format, DESC = attr_desc, DEFVALUE = attr_defValue, ISNULL = attr_isNull, BZ = "", CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser() });

                    }
                    else
                    {
                        //修改时新增了字段
                        _context.FD_BO_FIELD.Add(new FD_BO_FIELD() { ID = attr_id, BO_ID = id, NAME = attr_name, CODE = attr_code, FIELDNAME = attr_fieldName, DATATYPE = attr_dataType, ATTRLENGTH = attr_attrLength, PRECISION = attr_precision, FORMAT = attr_format, DESC = attr_desc, DEFVALUE = attr_defValue, ISNULL = attr_isNull, BZ = "", CREATE_DATE = DateTime.Now, MODIFY_DATE = DateTime.Now, OPERATOR = Permission.getCurrentUser(), IS_DELETE = false });

                    }
                }
            }
            int excuResult = _context.SaveChanges(true);

            var result = new { result = 1, message = "保存业务对象定义成功", cause = "", id = newid };
            return Ok(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// 获取业务对象列表（含搜索）
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="nd"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord"></param>
        /// <param name="Q_NAME">搜索名称</param>
        /// <param name="Q_CODE">搜索CODE</param>
        /// <param name="Q_STATUS">搜索状态</param>
        /// <param name="Q_TYPE_ID">搜索业务对象分类</param>
        /// <param name="Q_IS_CREATE_TABLE_S">搜索是否发布成表</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult BOlistjson([FromForm]string _search, string nd, int page, int rows, string sidx, string sord, string Q_NAME = "", string Q_CODE = "", string Q_STATUS = "", string Q_TYPE_ID = "", string Q_IS_CREATE_TABLE_S = "")
        {
            Q_NAME = string.IsNullOrEmpty(Q_NAME) ? "" : Q_NAME;
            Q_CODE = string.IsNullOrEmpty(Q_CODE) ? "" : Q_CODE;
            Q_STATUS = string.IsNullOrEmpty(Q_STATUS) ? "" : Q_STATUS;
            Q_TYPE_ID = string.IsNullOrEmpty(Q_TYPE_ID) ? "" : Q_TYPE_ID;
            if (string.IsNullOrEmpty(sidx))
            {
                sidx = "create_date";
            }
            if (string.IsNullOrEmpty(sord))
            {
                sord = "desc";
            }
            var filelist = _context.FD_BO
          .Where("code.contains(@0) and name.contains(@1) and status.contains(@2) and typeId.contains(@3) and is_delete==false", Q_CODE, Q_NAME, Q_STATUS, Q_TYPE_ID)
          .OrderBy(sidx + " " + sord)//按条件排序
          .Skip((page - 1) * rows) //跳过前x项
          .Take(rows)//从当前位置开始取前x项
          .Select(s => new { name = s.NAME, id = s.ID, code = s.CODE, pk = s.PK, boType = s.BOTYPE, status = s.STATUS, dataFormat = s.DATAFORMAT, parentId = s.PARENTID, options = s.OPTIONS })
          .ToList();//将结果转为List类型
            int records = _context.FD_BO.Where(w => w.IS_DELETE == false)
                .Where("code.contains(@0) and name.contains(@1) and status.contains(@2) and typeId.contains(@3) and is_delete==false", Q_CODE, Q_NAME, Q_STATUS, Q_TYPE_ID)
                .Count();
            var result = new
            {
                records,
                page,
                total = (records / rows) + 1,
                rows = filelist
            };
            return Ok(result);
        }

        /// <summary>
        /// 执行业务对象复制
        /// </summary>
        /// <param name="cascade"></param>
        /// <param name="defCode"></param>
        /// <param name="defId"></param>
        /// <param name="defName"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult BOsavecopy([FromForm]string cascade, string defCode, string defId, string defName)
        {
            var o_bo = _context.FD_BO.Where(w => w.ID == defId).First();
            var o_bo_attrs = _context.FD_BO_FIELD.Where(w => w.BO_ID == defId).ToList();
            string newID = Guid.NewGuid().ToString();
            o_bo.ID = newID;
            o_bo.NAME = defName;
            o_bo.CODE = defCode;
            _context.FD_BO.Add(o_bo);
            foreach (FD_BO_FIELD attr in o_bo_attrs)
            {
                attr.ID = Guid.NewGuid().ToString();
                attr.BO_ID = newID;
                _context.FD_BO_FIELD.Add(attr);
            }
            _context.SaveChanges(true);
            return Ok("{\"result\":1,\"message\":\"复制业务对象成功!\",\"cause\":\"\"}");
        }

        /// <summary>
        /// 删除业务对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rmType"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult BOremove(string id, string rmType)
        {
            var o_bo = _context.FD_BO.Where(w => w.ID == id).First();
            var o_bo_attrs = _context.FD_BO_FIELD.Where(w => w.BO_ID == id).ToList();
            o_bo.IS_DELETE = true;
            _context.Update(o_bo);
            foreach (FD_BO_FIELD attr in o_bo_attrs)
            {
                attr.IS_DELETE = true;
                _context.Update(attr);
            }
            _context.SaveChanges(true);
            return Ok("{\"result\":1,\"message\":\"删除业务对象定义成功!\",\"cause\":\"\"}");
        }

        /// <summary>
        /// 读取表单模板列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">每页行数</param>
        /// <param name="Q_NAME">表单名称</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult FormList([FromForm]int page, int rows, string Q_NAME)
        {
            Q_NAME = string.IsNullOrEmpty(Q_NAME) ? "" : Q_NAME;
            var formList = _context.FD_FORM
                .Where(w => w.IS_DELETE == false && w.NAME.Contains(Q_NAME))
                .Skip((page - 1) * rows) //跳过前x项
                .Take(rows)//从当前位置开始取前x项
                .Select(s => new { pk = "", name = s.NAME, createBy = s.OPERATOR, createTime = s.CREATE_DATE, updateBy = s.OPERATOR, updateTime = s.MODIFY_DATE, id = s.ID, key = s.KEY, mode = s.MODE, desc = s.DESC, status = "deploy", typeId = s.TYPEID, isMain = "Y", version = "1", count = _context.FD_FORM_CASE.Where(c => c.FORM_ID == s.ID).Count() });
            int records = _context.FD_FORM.Where(w => w.IS_DELETE == false).Count();
            var result = new
            {
                records,
                page,
                total = (records / rows) + 1,
                rows = formList
            };
            return Ok(result);
        }

        /// <summary>
        /// 判断是否存在重复的表单Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult isFormKeyExists([FromQuery] string key)
        {
            int count = _context.FD_FORM.Where(w => w.KEY == key).Count();
            if (count == 0)
            {
                return Ok("{\"result\":1,\"message\":\"\",\"cause\":\"\"}");
            }
            else
            {
                return Ok("{\"result\":0,\"message\":\"\",\"cause\":\"\"}");

            }
        }

        /// <summary>
        /// 读取业务对象字段列表
        /// </summary>
        /// <param name="busId"></param>
        /// <param name="code"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult buildBOTree([FromForm] string busId, string code, string mode)
        {
            var bo = _context.FD_BO.Where(w => w.ID == busId && w.CODE == code).Select(s => new { id = s.ID, name = s.NAME, key = s.CODE, parentId = "0", type = "table", attrType = "table", tableName = s.CODE, icon = "fa fa-table", isPk = false }).First();
            var bo_fields = _context.FD_BO_FIELD.Where(w => w.BO_ID == busId).Select(s => new { id = s.ID, name = s.NAME, key = s.CODE, parentId = s.BO_ID, type = s.DATATYPE, attrType = "field", bo.tableName, icon = "fa fa-" + s.DATATYPE, isPk = false }).ToList();
            bo_fields.Add(bo);
            return Ok(bo_fields);
        }
        /// <summary>
        /// 读取组织树
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult buildORGTree([FromForm] string type)
        {
            var org = _context.PF_ORG.Where(w => w.IS_DELETE == false).Select(s => new { pk = "", name = s.TITLE, createBy = s.OPERATOR, createTime = s.CREATE_DATE, updateBy = s.OPERATOR, updateTime = s.MODIFY_DATE, id = s.GID, alias = s.CODE, parentId = s.SUPER, path = s.PATH, depth = s.DEPTH, type = "org", nocheck = false, chkDisabled = false, click = true, open = true, title = s.TITLE }).ToList();
            return Ok(org);
        }

        /// <summary>
        /// 检索用户数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="_search"></param>
        /// <param name="inclueChild"></param>
        /// <param name="nd"></param>
        /// <param name="orgId">组织GID</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序值</param>
        /// <param name="Q_ACCOUNT">检索账号</param>
        /// <param name="Q_FULLNAME">检索姓名</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult dialogUserJson([FromQuery]string type, [FromForm] string _search, string inclueChild, string nd, string orgId, int page, int rows, string Q_ACCOUNT, string Q_FULLNAME, string sidx, string sord)
        {
            orgId = string.IsNullOrEmpty(orgId) ? "" : orgId;
            Q_ACCOUNT = string.IsNullOrEmpty(Q_ACCOUNT) ? "" : Q_ACCOUNT;
            Q_FULLNAME = string.IsNullOrEmpty(Q_FULLNAME) ? "" : Q_FULLNAME;
            sidx = string.IsNullOrEmpty(sidx) ? "USER_GID" : sidx;
            sord = string.IsNullOrEmpty(sord) ? "ASC" : sord;
            var userlist = _context.PF_USER_ORG
                .Where("ORG_GID.contains(@0) and USER_NAME.contains(@1) and BZ1.contains(@2) and IS_DELETE==false", orgId, Q_ACCOUNT, Q_FULLNAME)
                .OrderBy(sidx + " " + sord)
                .Skip((page - 1) * rows)
                .Take(rows)
                .Select(s => new { id = s.USER_GID, account = s.USER_NAME, isSuper = "N", fullname = s.BZ1, userId = s.USER_NAME, super = false });
            int records = _context.PF_USER_ORG
                 .Where("ORG_GID.contains(@0) and USER_NAME.contains(@1) and BZ1.contains(@2) and IS_DELETE==false", orgId, Q_ACCOUNT, Q_FULLNAME).Count();

            var result = new
            {
                records,
                page,
                total = (records / rows) + 1,
                rows = userlist
            };
            return Ok(result);
        }

        /// <summary>
        /// 获取表单打印模板列表
        /// </summary>
        /// <param name="Q_FORM_KEY">表单key值</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult formPrintList([FromQuery]string Q_FORM_KEY, [FromForm]int page, int rows)
        {
            Q_FORM_KEY = string.IsNullOrEmpty(Q_FORM_KEY) ? "" : Q_FORM_KEY;
            var filelist = _context.FD_FORM_PRINT
          .Where("formKey.contains(@0) and is_delete==false", Q_FORM_KEY)
          .OrderBy("create_date desc")
          .Skip((page - 1) * rows)
          .Take(rows)
          .Select(s => new { name = s.NAME, id = s.ID, formKey = s.FORMKEY, content = s.CONTENT, createTime = s.CREATE_DATE, updateBy = s.OPERATOR, updateTime = s.MODIFY_DATE, createBy = s.OPERATOR })
          .ToList();//将结果转为List类型
            int records = _context.FD_FORM_PRINT.Where(w => w.IS_DELETE == false)
                .Where("formKey.contains(@0) and is_delete==false", Q_FORM_KEY)
                .Count();
            var result = new
            {
                records,
                page,
                total = (records / rows) + 1,
                rows = filelist
            };
            return Ok(result);
        }

        /// <summary>
        /// 删除表单打印模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult formPrintRemove([FromQuery]string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var target = _context.FD_FORM_PRINT.Where(w => w.ID == id).First();
                target.IS_DELETE = true;
                _context.FD_FORM_PRINT.Update(target);
                int result = _context.SaveChanges(true);
                if (result > 0)
                {
                    return Ok("{\"result\":1,\"message\":\"删除表单打印模板成功!\",\"cause\":\"\"}");

                }
                else
                {
                    return Ok("{\"result\":0,\"message\":\"删除表单打印模板失败!\",\"cause\":\"\"}");

                }
            }
            else
            {

                return Ok("{\"result\":0,\"message\":\"参数定义失败!\",\"cause\":\"\"}");
            }
        }


        /// <summary>
        /// 根据formKey值获取表单字段列表
        /// </summary>
        /// <param name="formKey">表单key</param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getFormFields([FromQuery] string formKey)
        {
            string form_id = _context.FD_FORM.Where(w => w.KEY == formKey).First().ID;
            var list = _context.FD_FORM_FIELD.Where(w => w.FORM_ID == form_id).Select(s => new { name = s.NAME, label = s.LABEL, desc = s.DESC, field_type = s.FIELD_TYPE, field_options = JsonConvert.DeserializeObject(s.FIELD_OPTIONS), field_name = s.SHOWNAME }).ToList();
            return Ok(new { result = true, fields = JsonConvert.SerializeObject(list) });
        }

        /// <summary>
        /// 初始化表单打印模板设计器数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getFormPrintInitData([FromQuery]string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var result = new
                {
                    result = true,
                    template = new
                    {
                        name = "未命名模版",
                        content = JsonConvert.DeserializeObject("{\"type\":\"template\",\"name\":\"未命名模版\",\"page\":{\"size\":\"A4\",\"layout\":\"portrait\",\"margin\":[72.188934,67.2756]},\"header\":{\"columns\":[],\"margin\":18.8976},\"footer\":{\"columns\":[],\"margin\":18.8976},\"background\":{\"printable\":false},\"default\":{\"col_width\":100,\"row_height\":24},\"global\":{\"auto_cell_height\":false},\"cols\":[],\"rows\":[],\"range\":{\"s\":\"0:0\",\"e\":\"0:0\"},\"styles\":{\"default\":{\"font-family\":\"SimSun\",\"font-size\":12,\"font-style\":\"normal\",\"font-weight\":\"normal\",\"color\":\"#000\",\"line-height\":14.4,\"text-align\":\"left\",\"vertical-align\":\"middle\",\"text-decoration\":[],\"opacity\":1,\"padding\":[4],\"border-style\":\"none\",\"border-width\":1,\"border-color\":\"#000\"}},\"dpi\":{\"design\":96,\"print\":72},\"watermark\":{},\"cells\":{},\"merge\":{},\"images\":{}}")
                    }
                };
                return Ok(result);
            }
            else
            {
                var target = _context.FD_FORM_PRINT.Where(w => w.ID == id).First();
                var result = new
                {
                    result = true,
                    template = new
                    {
                        name = target.NAME,
                        content = JsonConvert.DeserializeObject(target.CONTENT)
                    }
                };
                return Ok(result);
            }
        }


        /// <summary>
        /// 保存表单打印模板
        /// </summary>
        /// <param name="data"></param>
        /// <param name="formKey"></param>
        /// <param name="id"></param>
        /// <param name="html">模板html，用于转pdf</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult formPrintSave([FromForm]string data, string formKey, string id, string html)
        {
            //将html中的${label}修改为${code}，便于前端打印

            string pattern = @"(?<=\${)[^{]*(?=})";
            string formID = _context.FD_FORM.Where(w => w.KEY == formKey).First().ID;
            var form_Fields = _context.FD_FORM_FIELD.Where(w => w.FORM_ID == formID).ToList();
            Regex regex = new Regex(pattern);
            foreach (Match match in regex.Matches(html))
            {
                string o_key = match.Value;
                string n_key = form_Fields.Where(w => w.LABEL == o_key).First().NAME;
                html = html.Replace("${" + o_key + "}", "${" + n_key + "}");
            }
            var Json = (JObject)JsonConvert.DeserializeObject(data);
            if (!string.IsNullOrEmpty(id))
            {
                var target = _context.FD_FORM_PRINT.Where(w => w.ID == id).First();
                target.CONTENT = Json["content"].ToString();
                target.NAME = Json["name"].ToString();
                target.HTML = html;
                _context.FD_FORM_PRINT.Update(target);
            }
            else
            {
                FD_FORM_PRINT newFD = new FD_FORM_PRINT();
                newFD.ID = Guid.NewGuid().ToString();
                newFD.NAME = Json["name"].ToString();
                newFD.CONTENT = Json["content"].ToString();
                newFD.HTML = html;
                newFD.DESC = "";
                newFD.CREATE_DATE = DateTime.Now;
                newFD.MODIFY_DATE = DateTime.Now;
                newFD.OPERATOR = Permission.getCurrentUser();
                newFD.IS_DELETE = false;
                newFD.FORMKEY = formKey;
                _context.FD_FORM_PRINT.Add(newFD);
            }
            int result = _context.SaveChanges(true);
            if (result > 0)
            {
                return Ok("{\"result\":1,\"message\":\"保存表单打印模板成功!\",\"cause\":\"\"}");

            }
            else
            {
                return Ok("{\"result\":0,\"message\":\"保存表单打印模板失败!\",\"cause\":\"\"}");

            }
        }

        /// <summary>
        /// 检查上传文件MD5，避免重复文件上传
        /// </summary>
        /// <param name="fileMD5"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult checkFileMD5([FromForm]string fileMD5)
        {
            int hasFile = _context.PF_FILE.Where(w => w.MD5 == fileMD5).Count();
            if (hasFile == 0)
            {
                return Ok("{\"result\":0,\"message\":\"\",\"cause\":\"\"}");
            }
            else
            {
                return Ok("{\"result\":1,\"message\":\"\",\"cause\":\"\"}");
            }
        }

        /// <summary>
        /// 检查大文件（暂未实现）
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="chunkSize"></param>
        /// <param name="fileExists"></param>
        /// <param name="fileMd5"></param>
        /// <param name="filename"></param>
        /// <param name="fileSize"></param>
        /// <param name="isChunk"></param>
        /// <param name="paramJson"></param>
        /// <param name="uploadType"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult checkFileChunk([FromForm]int chunk, int chunkSize, int fileExists, string fileMd5, string filename, int fileSize, string isChunk, string paramJson, string uploadType)
        {
            return Ok("{\"result\":1,\"message\":\"\",\"cause\":\"\"}");
        }
        /// <summary>
        /// 合并大文件（暂未实现）
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="chunkSize"></param>
        /// <param name="fileExists"></param>
        /// <param name="fileMd5"></param>
        /// <param name="filename"></param>
        /// <param name="fileSize"></param>
        /// <param name="isChunk"></param>
        /// <param name="paramJson"></param>
        /// <param name="uploadType"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult mergeFileChunks([FromForm]int chunk, int chunkSize, int fileExists, string fileMd5, string filename, int fileSize, string isChunk, string paramJson, string uploadType)
        {
            int fileCount = _context.PF_FILE.Where(w => w.MD5 == fileMd5).Count();
            if (fileCount > 0)
            {

                PF_FILE file = _context.PF_FILE.Where(w => w.MD5 == fileMd5).First();
                return Ok(new { result = 1, message = "", cause = "", success = true, vars = new { }, fileInfo = new { id = file.GID, fileName = file.FILENAME, filePath = file.FILEURI + file.TYPE, totalBytes = fileSize, ext = file.TYPE.Replace(".", ""), md5 = file.MD5 } });
            }
            else
            {
                return Ok(new { result = 0, message = "未找到文件", cause = "", success = false });
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="deleteIds"></param>
        /// <returns></returns>

        [HttpPost, Route("[action]")]
        public ActionResult deleteFile([FromQuery]string deleteIds)
        {
            int fileCount = _context.PF_FILE.Where(w => w.GID == deleteIds).Count();
            if (fileCount > 0)
            {
                PF_FILE file = _context.PF_FILE.Where(w => w.GID == deleteIds).First();
                file.IS_DELETE = true;
                _context.PF_FILE.Update(file);
                _context.SaveChanges(true);
                return Ok("{\"result\":1,\"message\":\"删除文件成功\",\"cause\":\"\"}");
            }
            else
            {
                return Ok("{\"result\":0,\"message\":\"未找到文件\",\"cause\":\"\"}");
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult uploadFile()
        {
            string tempGID = "";
            string tempTitle = "";
            if (!Directory.Exists(WebPath.FILE_ABSOLUTE + WebPath.FILE_REALTE))//判断文件夹是否存在 
            {
                Directory.CreateDirectory(WebPath.FILE_ABSOLUTE + WebPath.FILE_REALTE);//不存在则创建文件夹 
            }
            List<IFormFile> formFiles = Request.Form.Files.ToList();
            if (formFiles.Count == 0)
            {
                return Ok(new { result = 0, message = "未找到文件", cause = "", success = false });
            }
            foreach (IFormFile formFile in formFiles)
            {
                PF_FILE pf_file = new PF_FILE();
                if (Path.HasExtension(formFile.FileName))
                {
                    pf_file.FILENAME = Path.GetFileNameWithoutExtension(formFile.FileName);
                    pf_file.TYPE = Path.GetExtension(formFile.FileName);
                }
                else
                {
                    pf_file.FILENAME = formFile.FileName;

                }
                //读取文件MD5码
                string MD5 = Helper.checkMD5(formFile.OpenReadStream());
                pf_file.MD5 = MD5;
                tempTitle = pf_file.FILENAME;
                //文件路径
                string GID = Guid.NewGuid().ToString();
                pf_file.GID = GID;
                string xdlj = WebPath.FILE_REALTE + GID;//相对路径
                tempGID = GID;
                string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
                string path = jdlj + xdlj;
                pf_file.FILEURI = xdlj;
                pf_file.PX = 0;
                pf_file.IP = HttpContext.Connection.RemoteIpAddress.ToString();
                pf_file.OPERATOR = Permission.getCurrentUser();
                //保存文件
                using (FileStream fs = System.IO.File.Create(path))
                {
                    // 复制文件  
                    formFile.CopyTo(fs);
                    // 清空缓冲区数据  
                    fs.Flush();
                }

                _context.Add(pf_file);
            }
            _context.SaveChanges();
            return Ok(new { result = 1, message = "文件上传成功", cause = "", success = false });

        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getFile([FromQuery]string id)
        {
            int fileCount = _context.PF_FILE.Where(w => w.GID == id).Count();
            if (fileCount > 0)
            {
                PF_FILE file = _context.PF_FILE.Where(w => w.GID == id).First();
                string xdlj = file.FILEURI;
                string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
                var stream = System.IO.File.OpenRead(jdlj + xdlj);
                if (stream == null)
                {
                    return NotFound(new { msg = "文件不存在" });
                }
                return File(stream, Helper.GetContentType(file.TYPE.Replace(".", "")), file.FILENAME + file.TYPE);
            }
            else
            {
                return Ok("{\"result\":0,\"message\":\"未找到文件\",\"cause\":\"\"}");
            }
        }

        /// <summary>
        /// 新建表单实例时加载的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public ActionResult getFormInitData([FromQuery]string defId, string taskId, string showType = "edit")
        {
            bool isStart = true;
            string _boData = "";
            string _formData = "";
            var form = _context.FD_FORM.Where(w => w.ID == defId).First();
            if (!string.IsNullOrEmpty(taskId))
            {
                FD_FORM_CASE fc = _context.FD_FORM_CASE.Where(w => w.ID == taskId).First();
                isStart = false;
                _boData = fc.BODATA;
                _formData = fc.FORM_DATA;
            }
            else
            {
                var form_fields = _context.FD_FORM_FIELD.Where(w => w.FORM_ID == defId).OrderBy(o => o.ORDER).Select(s => new { name = s.NAME, label = s.LABEL, desc = s.DESC, field_type = s.FIELD_TYPE, field_options = JsonConvert.DeserializeObject(s.FIELD_OPTIONS), field_name = s.SHOWNAME }).ToList();
                var formData = new
                {
                    id = form.ID,
                    name = form.NAME,
                    desc = form.DESC,
                    key = form.KEY,
                    typeId = form.TYPEID,
                    mode = form.MODE,
                    attrs = JsonConvert.DeserializeObject(form.ATTRS),
                    busId = form.BUSID,
                    code = form.KEY,
                    fields = form_fields
                };
                _boData = "{}";
                _formData = JsonConvert.SerializeObject(formData);
            }
            //检查是否有打印模板
            var printTmpl = _context.FD_FORM_PRINT.Where(w => w.IS_DELETE == false && w.FORMKEY == form.KEY);
            var result = new
            {
                result = true,
                boData = _boData,
                buttons = new ArrayList() {
                    new {name="保存",alias="startFlow",supportScript=true },
                    new {name="打印页面",alias="print",supportScript=false }
                    //new { name = "自定义", alias = "custom", supportScript = true }
                },
                permissions = showType == "view" ? "{\"tables\":{},\"opinions\":{},\"fields\":{}}" : string.Empty,
                attributes = new { },
                isStart,
                formModel = new
                {
                    name = form.NAME,
                    parentFlowKey = "local_",
                    type = "INNER",
                    templateId = printTmpl.Count() > 0 ? printTmpl.First().FORMKEY : "",//打印模板编号
                    formValue = form.KEY,
                    id = form.ID,
                    key = form.KEY,
                    formData = _formData
                }
            };
            return Ok(result);
        }

        /// <summary>
        /// 保存表单数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defId">表单模板ID</param>
        /// <param name="taskId">实例id，判断是新增还是保存</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult saveFormData([FromForm]string data, string defId, string taskId)
        {
            JObject json = (JObject)JsonConvert.DeserializeObject(data);
            UserORG orgInfo = Permission.getCurrentUserOrg();
            string id = json["id"] == null ? "" : json["id"].ToString();
            var form = _context.FD_FORM.Where(w => w.ID == defId).First();
            var form_fields = _context.FD_FORM_FIELD.Where(w => w.FORM_ID == defId).OrderBy(o => o.ORDER).Select(s => new { name = s.NAME, label = s.LABEL, desc = s.DESC, field_type = s.FIELD_TYPE, field_options = JsonConvert.DeserializeObject(s.FIELD_OPTIONS), field_name = s.SHOWNAME }).ToList();
            var form_data = new
            {
                id = form.ID,
                name = form.NAME,
                desc = form.DESC,
                key = form.KEY,
                typeId = form.TYPEID,
                mode = form.MODE,
                attrs = JsonConvert.DeserializeObject(form.ATTRS),
                busId = form.BUSID,
                code = form.KEY,
                fields = form_fields
            };
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(taskId))
            {
                id = Guid.NewGuid().ToString();
                //存储表单数据
                object o = json;
                foreach (JProperty fr in json.Properties())
                {
                    string key = fr.Name;
                    string value = fr.Value.ToString();
                    FD_FORM_DATA fd = new FD_FORM_DATA();
                    fd.ID = Guid.NewGuid().ToString();
                    fd.FORM_ID = defId;
                    fd.FORM_CASE_ID = id;
                    fd.KEY = key;
                    fd.VALUE = string.IsNullOrEmpty(value) ? "" : value;
                    fd.BZ = convertJSONValueToValue(value);
                    fd.CREATE_DATE = DateTime.Now;
                    fd.MODIFY_DATE = DateTime.Now;
                    fd.OPERATOR = Permission.getCurrentUser();
                    fd.IS_DELETE = false;
                    _context.FD_FORM_DATA.Add(fd);
                }
                //新建表单实例
                FD_FORM_CASE fc = new FD_FORM_CASE();

                fc.ID = id;
                fc.FORM_ID = defId;
                fc.FORM_NAME = form.NAME;
                fc.FORM_KEY = form.KEY;
                fc.FORM_DATA = JsonConvert.SerializeObject(form_data);
                fc.BO_ID = form.BUSID;
                fc.BODATA = data;
                fc.ORGID = orgInfo.ORG_GID;
                fc.ORGNAME = orgInfo.ORG_NAME;
                fc.ORGPATH = orgInfo.ORG_PATH;
                fc.STATE = "已启动";
                fc.PERMISSIONS = form.PERMISSIONS;
                fc.CREATE_DATE = DateTime.Now;
                fc.MODIFY_DATE = DateTime.Now;
                fc.OPERATOR = Permission.getCurrentUser();
                fc.IS_DELETE = false;
                fc.VERSION = "0";
                _context.FD_FORM_CASE.Add(fc);
            }
            else
            {
                object o = json;
                var form_datas = _context.FD_FORM_DATA.Where(w => w.FORM_CASE_ID == taskId).ToList();
                var form_case = _context.FD_FORM_CASE.Where(w => w.ID == taskId).First();

                form_case.BO_ID = form.BUSID;
                form_case.FORM_DATA = JsonConvert.SerializeObject(form_data);
                form_case.BODATA = data;
                form_case.MODIFY_DATE = DateTime.Now;
                form_case.PERMISSIONS = form.PERMISSIONS;
                form_case.VERSION = (int.Parse(form_case.VERSION) + 1).ToString();
                _context.FD_FORM_CASE.Update(form_case);
                foreach (JProperty fr in json.Properties())
                {
                    string key = fr.Name;
                    string value = fr.Value.ToString();
                    var current = form_datas.Where(w => w.KEY == key).First();
                    current.VALUE = value;
                    current.MODIFY_DATE = DateTime.Now;
                }
            }
            _context.SaveChanges(true);
            return Ok("{\"result\":1,\"message\":\"保存成功\",\"cause\":\"\"}");
        }

        /// <summary>
        /// 根据表单模板ID获取表单实例列表
        /// </summary>
        /// <param name="formKey"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchfield"></param>
        /// <param name="searchword"></param>
        /// <param name="gangwei">岗位</param>
        /// <param name="daterange">日期范围</param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]/{formKey}")]
        public ResultList<VM_FD_FORM_CASE> getFormCaseList([FromRoute]string formKey, [FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string gangwei = "", string daterange = "", string field = "CREATE_DATE", string order = "DESC")
        {
            //0、预先读取出档案列表
            var RYDA = _context.PF_PROFILE.Where(w => w.IS_DELETE == false).Select(s => new { s.CODE, s.NAME }).ToList();
            //1、设定检索默认值
            searchfield = string.IsNullOrEmpty(searchfield) ? "id" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;

            DateTime start = Convert.ToDateTime("1900-01-01");
            DateTime end = Convert.ToDateTime("2999-01-01");
            if (!string.IsNullOrEmpty(daterange))
            {
                string[] timeRange = daterange.Replace(" - ", "|").Split('|');
                start = Convert.ToDateTime(timeRange[0]);
                end = Convert.ToDateTime(timeRange[1]).AddDays(1);
            }
            //---授权：组织角色授权来源于FD_FORM_PERMISSION表，以及自己创建的数据
            string permissionStr = " and 1>2";
            ArrayList queryParams = new ArrayList() { searchword, formKey, start, end };
            var formPermission = _context.FD_FORM_PERMISSION.Where(w => w.IS_DELETE == false && w.FORM_KEY == formKey).ToList();
            var userRoles = Permission.getCurrentUserRoles();
            var permissionRoles = formPermission.Select(s => s.ROLE_CODE).Intersect(userRoles).ToList();
            if (permissionRoles.Count > 0)
            {
                string permissionType = "仅个人";
                List<string> Types = formPermission.Where(w => permissionRoles.Contains(w.ROLE_CODE)).Select(s => s.TYPE).ToList();
                //一个用户可能有多个角色，会对应多个表单权限，以最高权限为准：全部>自定义>下级>仅个人
                List<string> typeQueue = new List<string>() { "全部", "自定义", "下级", "仅个人" };
                foreach (string type in Types)
                {
                    if (typeQueue.FindIndex(s => s == type) < typeQueue.FindIndex(s => s == permissionType))
                    {
                        permissionType = type;
                    }
                }
                switch (permissionType)
                {
                    case "全部": permissionStr = ""; break;
                    case "下级":
                        {
                            permissionStr = " and (orgPath.Contains(@" + queryParams.Count + ") or operator.Contains(@" + (queryParams.Count + 1) + "))";
                            queryParams.Add(Permission.getCurrentUserOrg().ORG_PATH);
                            queryParams.Add(Permission.getCurrentUser());//包括自己
                        }; break;
                    case "仅个人":
                        {
                            permissionStr = " and operator.Contains(@" + queryParams.Count + ")";
                            queryParams.Add(Permission.getCurrentUser());
                        }; break;
                    case "自定义":
                        {
                            List<string> allowORGs = new List<string>();
                            foreach (string role in permissionRoles)
                            {
                                List<string> tempORGs = formPermission.Where(w => w.ROLE_CODE == role).Select(s => s.ORGPATH).First().Split(',').ToList();
                                allowORGs.Union(tempORGs).ToList();
                            }
                            permissionStr = " and @" + queryParams.Count + ".Contains(orgPath)";
                            queryParams.Add(allowORGs);
                        }; break;
                }
            }

            //--------------------------------------
            var queryResult = _context.FD_FORM_CASE
            .Where((searchfield + ".Contains(@0) and form_key=@1 and is_delete == false and CREATE_DATE>@2 and CREATE_DATE<@3" + permissionStr), queryParams.ToArray()) //like查找
            .OrderBy(field + " " + order);
            var a = queryResult.ToList();
            //转换规则Start
            var config = new MapperConfiguration(
                 cfg =>
                 {
                     cfg.CreateMap<FD_FORM_CASE, VM_FD_FORM_CASE>()
                     .ForMember(dest => dest.OPERATOR, opts => opts.MapFrom(src => RYDA.Where(w => w.CODE == src.OPERATOR).Select(s => s.NAME).FirstOrDefault()));
                 });
            //转换规则End
            IMapper mapper = config.CreateMapper();
            var data = mapper.Map<List<FD_FORM_CASE>, List<VM_FD_FORM_CASE>>(queryResult.Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList());
            return new ResultList<VM_FD_FORM_CASE>

            {
                TotalCount = queryResult.Count(),
                Results = data
            };
        }

        /// <summary>
        /// 删除表单实例
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public ActionResult deleteFormCase([FromForm]string id)
        {
            var currCase = _context.FD_FORM_CASE.Where(w => w.ID == id).First();
            currCase.IS_DELETE = true;
            _context.FD_FORM_CASE.Update(currCase);
            var currCaseData = _context.FD_FORM_DATA.Where(w => w.FORM_CASE_ID == id).ToList();
            foreach (var d in currCaseData)
            {
                d.IS_DELETE = true;
                _context.FD_FORM_DATA.Update(d);
            }
            _context.SaveChanges(true);
            return Ok(new { success = "true" });
        }

        /// <summary>
        /// 批量删除表单实例
        /// </summary>
        /// <param name="ids">表单实例id，中间用;分割</param>
        /// <returns></returns>
        [HttpDelete, Route("[action]")]
        public ActionResult bulkDeleteFormCase([FromForm]string ids)
        {
            string[] IDs = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var x = _context.FD_FORM_CASE.Where(w => IDs.Contains(w.ID) && w.IS_DELETE == false).ToList();
            var y = _context.FD_FORM_DATA.Where(w => IDs.Contains(w.FORM_CASE_ID) && w.IS_DELETE == false).ToList();
            foreach (var d in x)
            {
                d.IS_DELETE = true;
                d.MODIFY_DATE = DateTime.Now;
                d.OPERATOR = User.Identity.Name;
            }
            foreach (var d in y)
            {
                d.IS_DELETE = true;
                d.MODIFY_DATE = DateTime.Now;
                d.OPERATOR = User.Identity.Name;
            }
            int result = _context.SaveChanges(true);
            if (result > 0)
            {
                return Ok(new { success = "true", msg = "成功删除" + result + "条数据" });
            }
            else
            {
                return Ok(new { success = "true", msg = "未删除数据" });

            }
        }

        private static string convertJSONValueToValue(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            else
            {
                try
                {
                    JObject j = (JObject)JsonConvert.DeserializeObject(input);
                    return j[0]["name"].ToString();
                }
                catch
                {
                    return input;
                }
            }
        }
    }
}
