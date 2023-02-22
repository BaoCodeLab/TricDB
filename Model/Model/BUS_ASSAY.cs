using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_ASSAY
    {
        public string SEQ_ASSAY_ID { get; set; }
        public string ASSAYCODE { get; set; }
        public bool? IS_PAIRED_END { get; set; }
        public string LIBRARY_SELECTION { get; set; }
        public string LIBRARY_STRATEGY { get; set; }
        public string PLATFORM { get; set; }
        public int? READ_LENGTH { get; set; }
        public string TARGET_CAPTURE_KIT { get; set; }
        public string INSTRUMENT_MODEL { get; set; }
        public int? GENE_PADDING { get; set; }
        public int? NUMBER_OF_GENES { get; set; }
        public string VARIANT_CLASSIFICATIONS { get; set; }
        public string CENTER { get; set; }
        public string ALTERATION_TYPES { get; set; }
        public string PRESERVATION_TECHNIQUE { get; set; }
        public string SPECIMEN_TUMOR_CELLULARITY { get; set; }
        public string CALLING_STRATEGY { get; set; }
        public string COVERAGE { get; set; }
        public string SEQ_PIPELINE_ID { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string VERSION { get; set; }
    }
}
