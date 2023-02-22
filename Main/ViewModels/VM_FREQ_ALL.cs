using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_FREQ_ALL : BaseViewModel
    {
        public VM_FREQ_ALL(Controller controller) : base(controller)
        {
        }

        public VM_FREQ_ALL() { }

        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string FREQ_ID { get; set; }
        [Display(Name = "疾病")]
        public string DISEASE { get; set; }
        [Display(Name = "疾病ID")]
        public string DISEASEID { get; set; }
        [Display(Name = "关联表ID")]
        public string RELATIONID { get; set; }
        [Display(Name = "突变的基因ID")]
        public string TARGETID { get; set; }
        [Display(Name = "药物ID")]
        public string DRUGID { get; set; }
        [Display(Name = "靶点基因")]
        public string TARGET { get; set; }
        [Display(Name = "突变")]
        public string ALTERATION { get; set; }
        [Display(Name = "突变数目")]
        public int ALTERATION_NUM { get; set; }
        [Display(Name = "疾病样本总数")]
        public int DISEASE_SAMPLE_NUM { get; set; }
        [Display(Name = "突变频率")]
        public float FREQUENCY { get; set; }
        [Display(Name = "研究名称")]
        public string STUDY { get; set; }
        [Display(Name = "突变为空该基因ID")]
        public string EMPTY_TARGETID { get; set; }
        [Display(Name = "医学置信度")]
        public string CLINVAR_SIGNIFICANCE { get; set; }
        [Display(Name = "Entrez基因ID")]
        public string ENTREZ_GENEID { get; set; }
        [Display(Name = "HGNC基因ID")]
        public string HGNC_ID { get; set; }
        [Display(Name = "ENSEMBL基因ID")]
        public string ENSEMBL_ID { get; set; }
        [Display(Name = "基因别名")]
        public string GENE_ALIAS { get; set; }
        [Display(Name = "参考序列")]
        public string REFSEQ_TRANSCRIPT { get; set; }
        [Display(Name = "染色体")]
        public string CHROMOSOME { get; set; }
        [Display(Name = "位置")]
        public string POSITION { get; set; }
        [Display(Name = "COSMIC编号")]
        public string COSMIC { get; set; }
        [Display(Name = "dbSNP的alleleid")]
        public string DBSNP { get; set; }
        [Display(Name = "ClinVar编号")]
        public string CLINVAR { get; set; }
        [Display(Name = "OMIM编号")]
        public string OMIMID { get; set; }
        [Display(Name = "Mutation Data")]
        public string MUTATIONTEXT { get; set; }
        [Display(Name = "Pfam Data")]
        public string PFAM { get; set; }
        [Display(Name = "突变类型")]
        public string VARIANT_CLASSIFICATION { get; set; }
        [Display(Name = "蛋白质位置")]
        public string PROTEIN_POSITION { get; set; }
        [Display(Name = "Uniprot编号")]
        public string SWISSPORT { get; set; }
        [Display(Name = "药物名称")]
        public string DRUG_NAME { get; set; }
        [Display(Name = "ACMG置信度")]
        public string ACMG_SIGNIFICANCE { get; set; }
        [Display(Name = "基因功能和突变分析")]
        public string FUNCTION_AND_CLINICAL_IMPLICATIONS { get; set; }
        [Display(Name = "潜在治疗方案")]
        public string THERAPY_INTERPRETATION { get; set; }

        [Display(Name = "疾病代码")]
        public string DISEASECODE { get; set; }
        [Display(Name = "疾病别名")]
        public string DISEASE_ALIAS { get; set; }
        [Display(Name = "NCI编码")]
        public string NCI_CODE { get; set; }
        [Display(Name = "Oncotree编码")]
        public string ONCOTREE_CODE { get; set; }
        [Display(Name = "疾病等级路径")]
        public string DISEASE_PATH { get; set; }
        [Display(Name = "NCCN疾病指南")]
        public string NCCN_LINK { get; set; }
        [Display(Name = "NCI疾病定义")]
        public string NCI_DISEASE_DEFINITION { get; set; }
        [Display(Name = "疾病分类")]
        public string CLASSIFICATION { get; set; }
        [Display(Name = "疾病途径示意图")]
        public string DISEASE_PATHWAY { get; set; }
        [Display(Name = "疾病突变频率")]
        public string MUTATION_RATE { get; set; }

        [Display(Name = "药物代码")]
        public string DRUGCODE { get; set; }
        [Display(Name = "品牌名")]
        public string BRAND_NAME { get; set; }
        [Display(Name = "公司")]
        public string COMPANY { get; set; }
        [Display(Name = "药物靶点")]
        public string DRUG_TARGET { get; set; }
        [Display(Name = "药物类型")]
        public string DRUG_TYPE { get; set; }
        [Display(Name = "证据等级")]
        public string EVIDENCE_LEVEL { get; set; }
        [Display(Name = "临床试验号")]
        public string CLINICAL_TRIAL { get; set; }
        [Display(Name = "批准机构")]
        public string APPROVED { get; set; }
        [Display(Name = "批准时间")]
        public string APPROVAL_TIME { get; set; }
        [Display(Name = "负基因型")]
        public string NEGATIVE_GENOTYPES { get; set; }
        [Display(Name = "适应症")]
        public string INDICATIONS { get; set; }
        [Display(Name = "作用机制")]
        public string MECHANISM_OF_ACTION { get; set; }
        [Display(Name = "剂量")]
        public string DOSAGE { get; set; }
        [Display(Name = "结构")]
        public string STRUCTURE { get; set; }
        [Display(Name = "结构其他信息")]
        public string STRUCTURE_INFO { get; set; }
        [Display(Name = "参考地址")]
        public string REFERENCE_LINK { get; set; }


        [Display(Name = "创建时间")]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间")]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "操作人员")]
        public string OPERATOR { get; set; }
        [Display(Name = "是否公开")]
        public bool IS_PUB { get; set; }
        [Display(Name = "是否删除")]
        public bool IS_DELETE { get; set; }
        [Display(Name = "版本")]
        public string VERSION { get; set; }
        public int Count { get; set; }

    }
}

