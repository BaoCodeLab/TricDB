using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_FREQ_FROM_DATASET : BaseViewModel
    {
        public VM_FREQ_FROM_DATASET(Controller controller) : base(controller)
        {
        }

        public VM_FREQ_FROM_DATASET() { }

        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string FREQ_ID { get; set; }

        [Display(Name = "药物ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string DRUGID { get; set; }

        [Display(Name = "药物名称"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string DRUG_NAME { get; set; }

        [Display(Name = "总表关联ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string RELATIONID { get; set; }

        [Display(Name = "疾病"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string DISEASE { get; set; }

        [Display(Name = "疾病ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string DISEASEID { get; set; }

        [Display(Name = "突变的基因ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string TARGETID { get; set; }

        [Display(Name = "靶点基因"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string TARGET { get; set; }

        [Display(Name = "突变"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string ALTERATION { get; set; }

        [Display(Name = "突变数目"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public int ALTERATION_NUM { get; set; }

        [Display(Name = "疾病样本总数"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public int DISEASE_SAMPLE_NUM { get; set; }

        [Display(Name = "突变频率"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public float FREQUENCY { get; set; }

        [Display(Name = "研究名称"), AllowModify, enableSort, enableShow("false"), enableSearch, enableExport, thMinWidth("150")]
        public string STUDY { get; set; }

        [Display(Name = "突变为空该基因ID"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string EMPTY_TARGETID { get; set; }

        [Display(Name = "医学置信度"), AllowModify, enableShow("false"), enableSort, enableSearch, enableExport, thMinWidth("120")]
        public string CLINVAR_SIGNIFICANCE { get; set; }
        [Display(Name = "Entrez基因ID"), AllowModify, enableShow("false"), enableSearch, enableExport, thMinWidth("150")]
        public string ENTREZ_GENEID { get; set; }
        [Display(Name = "HGNC基因ID"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string HGNC_ID { get; set; }
        [Display(Name = "ENSEMBL基因ID"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string ENSEMBL_ID { get; set; }
        [Display(Name = "基因别名"), AllowModify, enableSort, enableShow("false"), enableSearch, enableExport, thMinWidth("120")]
        public string GENE_ALIAS { get; set; }
        [Display(Name = "参考序列"), AllowModify, enableSort, enableShow("false"), enableSearch, enableExport, thMinWidth("120")]
        public string REFSEQ_TRANSCRIPT { get; set; }
        [Display(Name = "染色体"), AllowModify, enableSort, enableShow("false"), enableSearch, enableExport, thMinWidth("150")]
        public string CHROMOSOME { get; set; }
        [Display(Name = "位置"), AllowModify, enableShow("false"), enableSearch, enableExport, thMinWidth("150")]
        public string POSITION { get; set; }
        [Display(Name = "COSMIC编号"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string COSMIC { get; set; }
        [Display(Name = "dbSNP的alleleid"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string DBSNP { get; set; }
        [Display(Name = "ClinVar编号"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string CLINVAR { get; set; }
        [Display(Name = "OMIM编号"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string OMIMID { get; set; }
        [Display(Name = "Mutation Data"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string MUTATIONTEXT { get; set; }
        [Display(Name = "Pfam Data"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string PFAM { get; set; }
        [Display(Name = "突变类型"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string VARIANT_CLASSIFICATION { get; set; }
        [Display(Name = "蛋白质位置"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string PROTEIN_POSITION { get; set; }
        [Display(Name = "Uniprot编号"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string SWISSPORT { get; set; }
        [Display(Name = "ACMG置信度"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string ACMG_SIGNIFICANCE { get; set; }



        [Display(Name = "创建时间"), enableShow("false")]
        public DateTime CREATE_DATE { get; set; }

        [Display(Name = "修改时间"), enableShow("false")]
        public DateTime MODIFY_DATE { get; set; }

        [Display(Name = "操作人员"), enableShow("false")]
        public string OPERATOR { get; set; }

        [Display(Name = "是否公开"), AllowModify, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public bool IS_PUB { get; set; }

        [Display(Name = "是否删除"), enableShow("false")]
        public bool IS_DELETE { get; set; }

        [Display(Name = "版本"), AllowModify, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public string VERSION { get; set; }

    }
}
