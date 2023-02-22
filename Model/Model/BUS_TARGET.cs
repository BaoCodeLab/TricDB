using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_TARGET
    {
        public string TARGETID { get; set; }
        public string TARGETCODE { get; set; }
        public string TARGET { get; set; }
        public string ALTERATION { get; set; }
        public string RESISTANCE { get; set; }
        public string COSMIC_EVIDENCE { get; set; }
        public string CLINICAL_SIGNIFICANCE { get; set; }
        public string VIC_EVIDENCE { get; set; }
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
        public string PATHWAY_LINKS_KEGG { get; set; }
        public string PATHWAY_FIGURE { get; set; }
        public int? HIT { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string VERSION { get; set; }
        public string MUTATIONTEXT { get; set; }
        public string PFAM { get; set; }
        public string STRAND { get; set; }
        public string VARIANT_CLASSIFICATION { get; set; }
        public string VARIANT_TYPE { get; set; }
        public string PROTEIN_POSITION { get; set; }
        public string SWISSPORT { get; set; }
    }
}
