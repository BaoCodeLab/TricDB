using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_DISEASE
    {
        public string DISEASEID { get; set; }
        public string DISEASECODE { get; set; }
        public string DISEASE { get; set; }
        public string DISEASE_ALIAS { get; set; }
        public string CLASSIFICATION { get; set; }
        public string DISEASE_PATHWAY { get; set; }
        public string PATHWAY_REFER { get; set; }
        public string NCI_CODE { get; set; }
        public string ONCOTREE_CODE { get; set; }
        public string DISEASE_PATH { get; set; }
        public string NCI_DISEASE_DEFINITION { get; set; }
        public string NCCN_LINK { get; set; }

        public string NCCN_REF { get; set; }
        public int? HIT { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string VERSION { get; set; }
    }
}
