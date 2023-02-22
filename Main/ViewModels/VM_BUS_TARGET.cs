using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_BUS_TARGET : BaseViewModel
    {
        public VM_BUS_TARGET(Controller controller) : base(controller)
        {
        }
        public VM_BUS_TARGET() { }
        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string TARGETID { get; set; }
        [Display(Name = "靶点代码"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string TARGETCODE { get; set; }
        [Display(Name = "靶点"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string TARGET { get; set; }
        [Display(Name = "基因突变"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string ALTERATION { get; set; }
        [Display(Name = "抗药性"), AllowModify, enableSort, enableShow("false"), enableSearch, enableExport, thMinWidth("120")]
        public string RESISTANCE { get; set; }
        [Display(Name = "医学置信度"), AllowModify, enableShow("false"), enableSort, enableSearch, enableExport, thMinWidth("120")]
        public string CLINICAL_SIGNIFICANCE { get; set; }
        [Display(Name = "COSMIC置信度"), AllowModify, enableShow("false"), enableSort, enableSearch, enableExport, thMinWidth("120")]
        public string COSMIC_EVIDENCE { get; set; }
        [Display(Name = "VIC置信度"), AllowModify, enableShow("false"), enableSort, enableSearch, enableExport, thMinWidth("120")]
        public string VIC_EVIDENCE { get; set; }
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
        [Display(Name = "KEGG外部链接"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string PATHWAY_LINKS_KEGG { get; set; }
        [Display(Name = "途径展示图"), AllowModify, enableShow("false"), enableSearch, enableExport]
        public string PATHWAY_FIGURE { get; set; }
        [Display(Name = "点击率"), enableSort, enableShow("false"), enableExport]
        public int HIT { get; set; }
        [Display(Name = "创建时间"), enableShow("false")]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间"), enableShow("false")]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "操作人员"), enableShow("false")]
        public string OPERATOR { get; set; }
        [Display(Name = "Mutation Data"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string MUTATIONTEXT { get; set; }
        [Display(Name = "Pfam Data"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string PFAM { get; set; }
        [Display(Name = "编码链"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string STRAND { get; set; }
        [Display(Name = "突变类型"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string VARIANT_CLASSIFICATION { get; set; }
        [Display(Name = "变异类型"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string VARIANT_TYPE { get; set; }
        [Display(Name = "蛋白质位置"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string PROTEIN_POSITION { get; set; }
        [Display(Name = "Uniprot编号"), AllowModify, enableSort, enableShow("false"), enableExport, thMinWidth("120")]
        public string SWISSPORT { get; set; }
        [Display(Name = "是否删除"), enableShow("false")]
        public bool IS_DELETE { get; set; }
        [Display(Name = "是否公开"), AllowModify, enableShow("false"), Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public bool IS_PUB { get; set; }
        [Display(Name = "版本"), AllowModify, enableShow("false"), Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public string VERSION { get; set; }
    }
}
