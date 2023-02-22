using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model
{
    public partial class FREQ_FROM_DATASET
    {

        public string FREQ_ID { get; set; }
        public string RELATIONID { get; set; }
        public string DISEASEID { get; set; }
        public string DRUGID { get; set; }
        public string TARGETID { get; set; }
        public string EMPTY_TARGETID { get; set; }
        public string DISEASE { get; set; }
        public string TARGET { get; set; }
        public string ALTERATION { get; set; }
        public int ALTERATION_NUM { get; set; }
        public int DISEASE_SAMPLE_NUM { get; set; }
        public float FREQUENCY { get; set; }
        public string STUDY { get; set; }

        public string ACMG_SIGNIFICANCE { get; set; }
        public string DRUG_NAME { get; set; }

        public string VARIANT_CLASSIFICATION { get; set; }
        public string CLINVAR_SIGNIFICANCE { get; set; }
        public string ENTREZ_GENEID { get; set; }
        public string HGNC_ID { get; set; }
        public string ENSEMBL_ID { get; set; }
        public string GENE_ALIAS { get; set; }
        public string REFSEQ_TRANSCRIPT { get; set; }
        public string CHROMOSOME { get; set; }
        public string POSITION { get; set; }
        public string COSMIC { get; set; }
        public string DBSNP { get; set; }
        public string CLINVAR { get; set; }
        public string OMIMID { get; set; }
        public string MUTATIONTEXT { get; set; }
        public string PFAM { get; set; }
        public string PROTEIN_POSITION { get; set; }
        public string SWISSPORT { get; set; }

        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public string VERSION { get; set; }

    }
}
