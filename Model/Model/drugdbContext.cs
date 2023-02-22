using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model.Model
{
    public partial class drugdbContext : DbContext
    {
        public drugdbContext()
        {
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
 public drugdbContext(DbContextOptions<drugdbContext> options, IHttpContextAccessor httpContextAccessor=null): base(options){
_httpContextAccessor = httpContextAccessor;
}
public virtual DbSet<AGGREGATEDCOUNTER> AGGREGATEDCOUNTER { get; set; }
        public virtual DbSet<ANNO_SAMPLE> ANNO_SAMPLE { get; set; }
        public virtual DbSet<ANNO_PROJECT> ANNO_PROJECT { get; set; }
        public virtual DbSet<ANNO_PROSAMP> ANNO_PROSAMP { get; set; }
        public virtual DbSet<ANNO_PATIENT> ANNO_PATIENT { get; set; }
        public virtual DbSet<ANNO_FILE> ANNO_FILE { get; set; }
        public virtual DbSet<ANNO_REPORT> ANNO_REPORT { get; set; }
        public virtual DbSet<BUS_ASSAY> BUS_ASSAY { get; set; }
        public virtual DbSet<BUS_CLINICAL_SAMPLE> BUS_CLINICAL_SAMPLE { get; set; }
        public virtual DbSet<BUS_CNA_HG> BUS_CNA_HG { get; set; }
        public virtual DbSet<BUS_DISEASE> BUS_DISEASE { get; set; }
        public virtual DbSet<BUS_DRUG> BUS_DRUG { get; set; }
        public virtual DbSet<BUS_FUSION> BUS_FUSION { get; set; }
        public virtual DbSet<BUS_CNA> BUS_CNA { get; set; }
        public virtual DbSet<BUS_PATIENT> BUS_PATIENT { get; set; }
        public virtual DbSet<BUS_RELATION> BUS_RELATION { get; set; }
        public virtual DbSet<BUS_TARGET> BUS_TARGET { get; set; }
        public virtual DbSet<BUS_TARGET_SAMPLE_RELATION> BUS_TARGET_SAMPLE_RELATION { get; set; }
        public virtual DbSet<CMS_AD> CMS_AD { get; set; }
        public virtual DbSet<CMS_ARTICLE> CMS_ARTICLE { get; set; }
        public virtual DbSet<CMS_CHANNEL> CMS_CHANNEL { get; set; }
        public virtual DbSet<CMS_FILELIST> CMS_FILELIST { get; set; }
        public virtual DbSet<CMS_LINK> CMS_LINK { get; set; }
        public virtual DbSet<COUNTER> COUNTER { get; set; }
        public virtual DbSet<FD_BO> FD_BO { get; set; }
        public virtual DbSet<FD_BO_FIELD> FD_BO_FIELD { get; set; }
        public virtual DbSet<FD_FORM> FD_FORM { get; set; }
        public virtual DbSet<FD_FORM_CASE> FD_FORM_CASE { get; set; }
        public virtual DbSet<FD_FORM_DATA> FD_FORM_DATA { get; set; }
        public virtual DbSet<FD_FORM_FIELD> FD_FORM_FIELD { get; set; }
        public virtual DbSet<FD_FORM_PERMISSION> FD_FORM_PERMISSION { get; set; }
        public virtual DbSet<FD_FORM_PRINT> FD_FORM_PRINT { get; set; }
        public virtual DbSet<FREQ_FROM_DATASET> FREQ_FROM_DATASET { get; set; }
        public virtual DbSet<FREQ_RELAT_ALL> FREQ_RELAT_ALL { get; set; }
        public virtual DbSet<HASH> HASH { get; set; }
        public virtual DbSet<JOB> JOB { get; set; }
        public virtual DbSet<JOBPARAMETER> JOBPARAMETER { get; set; }
        public virtual DbSet<JOBQUEUE> JOBQUEUE { get; set; }
        public virtual DbSet<JOBSTATE> JOBSTATE { get; set; }
        public virtual DbSet<LIST> LIST { get; set; }
        public virtual DbSet<PF_FILE> PF_FILE { get; set; }
        public virtual DbSet<PF_LM> PF_LM { get; set; }
        public virtual DbSet<PF_LOG> PF_LOG { get; set; }
        public virtual DbSet<PF_MENU> PF_MENU { get; set; }
        public virtual DbSet<PF_MSG> PF_MSG { get; set; }
        public virtual DbSet<PF_MSG_REPLY> PF_MSG_REPLY { get; set; }
        public virtual DbSet<PF_MSG_STATE> PF_MSG_STATE { get; set; }
        public virtual DbSet<PF_NEW> PF_NEW { get; set; }
        public virtual DbSet<PF_NEW_ROLE> PF_NEW_ROLE { get; set; }
        public virtual DbSet<PF_ORG> PF_ORG { get; set; }
        public virtual DbSet<PF_PERMISSION> PF_PERMISSION { get; set; }
        public virtual DbSet<PF_PRINT_TMPL> PF_PRINT_TMPL { get; set; }
        public virtual DbSet<PF_PROFILE> PF_PROFILE { get; set; }
        public virtual DbSet<PF_REMINDER> PF_REMINDER { get; set; }
        public virtual DbSet<PF_ROLE> PF_ROLE { get; set; }
        public virtual DbSet<PF_ROLE_PERMISSION> PF_ROLE_PERMISSION { get; set; }
        public virtual DbSet<PF_SMSCODE> PF_SMSCODE { get; set; }
        public virtual DbSet<PF_STATE> PF_STATE { get; set; }
        public virtual DbSet<PF_USER> PF_USER { get; set; }
        public virtual DbSet<PF_USER_ORG> PF_USER_ORG { get; set; }
        public virtual DbSet<PF_USER_ROLE> PF_USER_ROLE { get; set; }
        public virtual DbSet<PF_ZDYDA> PF_ZDYDA { get; set; }
        public virtual DbSet<SERVER> SERVER { get; set; }
        public virtual DbSet<SET> SET { get; set; }
        public virtual DbSet<STATE> STATE { get; set; }
        public virtual DbSet<WEIXIN_USER> WEIXIN_USER { get; set; }

        // Unable to generate entity type for table 'distributedlock'. Please see the warning messages.
        // Unable to generate entity type for table 'genie_data_cna_hg19'. Please see the warning messages.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AGGREGATEDCOUNTER>(entity =>
            {
                entity.ToTable("aggregatedcounter");

                entity.HasIndex(e => e.KEY)
                    .HasName("IX_CounterAggregated_Key")
                    .IsUnique();

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EXPIREAT)
                    .HasColumnName("ExpireAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.KEY)
                    .IsRequired()
                    .HasColumnName("Key")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.VALUE)
                    .HasColumnName("Value")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<ANNO_SAMPLE>(entity =>
            {
                entity.HasKey(e => e.ANNO_SAMPLEID);

                entity.ToTable("anno_sample");

                entity.Property(e => e.ANNO_SAMPLEID)
                    .HasColumnName("anno_sampleid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.USER_ID)
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PATIENT_ID)
                    .HasColumnName("patient_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SAMPLE_NAME)
                    .HasColumnName("sample_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.SAMPLE_SOURCE)
                    .HasColumnName("sample_source")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.SAMPLE_TYPE)
                    .HasColumnName("sample_type")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.SAMPLE_POSI)
                    .HasColumnName("sample_posi")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.SAMPLE_METHOD)
                    .HasColumnName("sample_method")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.MUTATION_TYPE)
                    .HasColumnName("mutation_type")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SEQUENCE_TYPE)
                    .HasColumnName("sequence_type")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.MSI)
                    .HasColumnName("msi")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CAPTURE_SIZE)
                    .HasColumnName("capture_size")
                    .HasColumnType("double(9)");

                entity.Property(e => e.SAMPLE_DATE)
                    .HasColumnName("sample_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ACCESSION_DATE)
                    .HasColumnName("accession_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SAMPLE_DIAG)
                    .HasColumnName("sample_diag")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.OPERATOR)
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");


            });


            modelBuilder.Entity<ANNO_PATIENT>(entity =>
            {
                entity.HasKey(e => e.PATIENT_ID);

                entity.ToTable("anno_patient");

                entity.Property(e => e.PATIENT_ID)
                    .HasColumnName("patient_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PATIENT_NAME)
                    .HasColumnName("patient_name")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.PATIENT_GENDER)
                    .HasColumnName("patient_gender")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PATIENT_AGE)
                    .HasColumnName("patient_age")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PATIENT_DIAG)
                    .HasColumnName("patient_diag")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.PATIENT_STAGE)
                    .HasColumnName("patient_stage")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.PRIOR_TREAT_HIST)
                    .HasColumnName("prior_treat_hist")
                    .HasColumnType("text");

                entity.Property(e => e.ETHNICITY)
                    .HasColumnName("ethnicity")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.FAMILY_HISTORY)
                    .HasColumnName("family_history")
                    .HasColumnType("text");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.OPERATOR)
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");


            });


            modelBuilder.Entity<ANNO_FILE>(entity =>
            {
                entity.HasKey(e => e.FILE_ID);

                entity.ToTable("anno_file");

                entity.Property(e => e.FILE_ID)
                    .HasColumnName("file_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ANNO_SAMPLEID)
                    .HasColumnName("ann_sampleid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.FILE_TYPE)
                    .HasColumnName("file_type")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.ANALYSIS_TYPE)
                    .HasColumnName("patient_gender")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.OPERATOR)
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");


            });


            modelBuilder.Entity<ANNO_PROJECT>(entity =>
            {
                entity.HasKey(e => e.PROJECT_ID);

                entity.ToTable("anno_project");

                entity.Property(e => e.PROJECT_ID)
                    .HasColumnName("project_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.USER_ID)
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ORDER)
                    .HasColumnName("order")
                    .HasColumnType("int");

                entity.Property(e => e.PROJECT_NAME)
                    .HasColumnName("project_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PROJECT_DESCRIP)
                    .HasColumnName("project_descrip")
                    .HasColumnType("text");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.OPERATOR)
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");


            });


            modelBuilder.Entity<ANNO_PROSAMP>(entity =>
            {
                entity.HasKey(e => e.PROSAMP_ID);

                entity.ToTable("anno_prosamp");

                entity.Property(e => e.PROSAMP_ID)
                    .HasColumnName("prosamp_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ANNO_SAMPLEID)
                    .HasColumnName("anno_sampleid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PROJECT_ID)
                    .HasColumnName("project_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.OPERATOR)
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");


            });



            modelBuilder.Entity<ANNO_REPORT>(entity =>
            {
                entity.HasKey(e => e.REPORT_ID);

                entity.ToTable("anno_report");

                entity.Property(e => e.REPORT_ID)
                    .HasColumnName("report_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ANNO_SAMPLEID)
                    .HasColumnName("anno_sampleid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.REPORT_NAME)
                    .HasColumnName("report_name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.REPORT_DESCRIPTION)
                    .HasColumnName("report_description")
                    .HasColumnType("text");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.OPERATOR)
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");


            });




            modelBuilder.Entity<BUS_ASSAY>(entity =>
            {
                entity.HasKey(e => e.SEQ_ASSAY_ID);

                entity.ToTable("bus_assay");

                entity.Property(e => e.SEQ_ASSAY_ID)
                    .HasColumnName("seq_assay_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ALTERATION_TYPES)
                    .HasColumnName("alteration_types")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.ASSAYCODE)
                    .IsRequired()
                    .HasColumnName("assaycode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CALLING_STRATEGY)
                    .HasColumnName("calling_strategy")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CENTER)
                    .HasColumnName("center")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.COVERAGE)
                    .HasColumnName("coverage")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.GENE_PADDING)
                    .HasColumnName("gene_padding")
                    .HasColumnType("int(11)");

                entity.Property(e => e.INSTRUMENT_MODEL)
                    .HasColumnName("instrument_model")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PAIRED_END)
                    .HasColumnName("is_paired_end")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.LIBRARY_SELECTION)
                    .HasColumnName("library_selection")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.LIBRARY_STRATEGY)
                    .HasColumnName("library_strategy")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NUMBER_OF_GENES)
                    .HasColumnName("number_of_genes")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PLATFORM)
                    .HasColumnName("platform")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PRESERVATION_TECHNIQUE)
                    .HasColumnName("preservation_technique")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.READ_LENGTH)
                    .HasColumnName("read_length")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SEQ_PIPELINE_ID)
                    .HasColumnName("seq_pipeline_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SPECIMEN_TUMOR_CELLULARITY)
                    .HasColumnName("specimen_tumor_cellularity")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.TARGET_CAPTURE_KIT)
                    .HasColumnName("target_capture_kit")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.VARIANT_CLASSIFICATIONS)
                    .HasColumnName("variant_classifications")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<BUS_CLINICAL_SAMPLE>(entity =>
            {
                entity.HasKey(e => e.SAMPLE_ID);

                entity.ToTable("bus_clinical_sample");

                entity.HasIndex(e => e.PATIENT_ID)
                    .HasName("FK_PID_idx");

                entity.HasIndex(e => e.SAMPLE_ID)
                    .HasName("IndexSampleID");

                entity.Property(e => e.SAMPLE_ID)
                    .HasColumnName("sample_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.AGE_AT_SEQ_REPORT)
                    .HasColumnName("age_at_seq_report")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CANCER_TYPE)
                    .HasColumnName("cancer_type")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.CANCER_TYPE_DETAILED)
                    .HasColumnName("cancer_type_detailed")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ONCOTREE_CODE)
                    .HasColumnName("oncotree_code")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PATIENT_ID)
                    .HasColumnName("patient_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SAMPLECODE)
                    .IsRequired()
                    .HasColumnName("samplecode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SAMPLE_TYPE)
                    .HasColumnName("sample_type")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SAMPLE_TYPE_DETAILED)
                    .HasColumnName("sample_type_detailed")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.SEQ_ASSAY_ID)
                    .HasColumnName("seq_assay_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");

                entity.HasOne(d => d.BUS_PATIENT)
                    .WithMany(p => p.BUS_PATIENTNavigation)
                    .HasForeignKey(d => d.PATIENT_ID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PID");
            });

            modelBuilder.Entity<BUS_CNA_HG>(entity =>
            {
                entity.HasKey(e => e.CNA_ID);

                entity.ToTable("bus_cna_hg");

                entity.Property(e => e.CNA_ID)
                    .HasColumnName("cna_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CHROM)
                    .HasColumnName("chrom")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CNACODE)
                    .IsRequired()
                    .HasColumnName("cnacode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.LOCASTART)
                    .HasColumnName("locastart")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.LOCEND)
                    .HasColumnName("locend")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NUMMARK)
                    .HasColumnName("nummark")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SAMPLE_ID)
                    .IsRequired()
                    .HasColumnName("sample_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SEGMEAN).HasColumnName("segmean");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<BUS_DISEASE>(entity =>
            {
                entity.HasKey(e => e.DISEASEID);

                entity.ToTable("bus_disease");

                entity.Property(e => e.DISEASEID)
                    .HasColumnName("diseaseid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CLASSIFICATION)
                    .HasColumnName("classification")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DISEASE)
                    .HasColumnName("disease")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.DISEASECODE)
                    .HasColumnName("diseasecode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DISEASE_ALIAS)
                    .HasColumnName("disease_alias")
                    .HasColumnType("text");

                entity.Property(e => e.NCI_CODE)
                    .HasColumnName("NCI_code")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ONCOTREE_CODE)
                    .HasColumnName("ONCOTREE_code")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DISEASE_PATH)
                    .HasColumnName("disease_path")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.NCCN_LINK)
                    .HasColumnName("NCCN_link")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.NCCN_REF)
                    .HasColumnName("NCCN_ref")
                    .HasColumnType("text");

                entity.Property(e => e.NCI_DISEASE_DEFINITION)
                    .HasColumnName("NCI_Disease_Definition")
                    .HasColumnType("text");

                entity.Property(e => e.DISEASE_PATHWAY)
                    .HasColumnName("disease_pathway")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.HIT)
                    .HasColumnName("hit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PATHWAY_REFER)
                    .HasColumnName("pathway_refer")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<FREQ_FROM_DATASET>(entity =>
            {
                entity.HasKey(e => e.FREQ_ID);

                entity.ToTable("frequency_from_dataset");

                entity.Property(e => e.FREQ_ID)
                    .HasColumnName("freq_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.RELATIONID)
                    .HasColumnName("relationid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DRUGID)
                    .HasColumnName("drugid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DISEASEID)
                    .HasColumnName("diseaseid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.TARGETID)
                    .HasColumnName("targetid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DISEASE)
                    .HasColumnName("disease")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.TARGET)
                    .HasColumnName("target")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.ALTERATION)
                    .HasColumnName("alteration")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.ALTERATION_NUM)
                    .HasColumnName("alteration_num")
                    .HasColumnType("int(9)");

                entity.Property(e => e.DISEASE_SAMPLE_NUM)
                    .HasColumnName("disease_sample_num")
                    .HasColumnType("int(9)");

                entity.Property(e => e.FREQUENCY)
                    .HasColumnName("frequency")
                    .HasColumnType("float(5,5)");

                entity.Property(e => e.STUDY)
                    .HasColumnName("study")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.EMPTY_TARGETID)
                    .HasColumnName("empty_targetid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CHROMOSOME)
                    .HasColumnName("chromosome")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.CLINVAR_SIGNIFICANCE)
                    .HasColumnName("clinical_significance")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.ENSEMBL_ID)
                    .HasColumnName("ensembl_id")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ENTREZ_GENEID)
                    .HasColumnName("entrez_geneid")
                    .HasColumnType("varchar(45)");



                entity.Property(e => e.DRUG_NAME)
                    .HasColumnName("drug_name")
                    .HasColumnType("varchar(90)");


                entity.Property(e => e.ACMG_SIGNIFICANCE)
                    .HasColumnName("ACMG_significance")
                    .HasColumnType("varchar(128)");


                entity.Property(e => e.GENE_ALIAS)
                    .HasColumnName("gene_alias")
                    .HasColumnType("longtext");

                entity.Property(e => e.HGNC_ID)
                    .HasColumnName("hgnc_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.COSMIC)
                    .HasColumnName("cosmic")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.DBSNP)
                    .HasColumnName("dbsnp")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.CLINVAR)
                    .HasColumnName("clinvar")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.OMIMID)
                    .HasColumnName("omimid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.MUTATIONTEXT)
                    .IsRequired()
                    .HasColumnName("mutationtext")
                    .HasColumnType("char(0)")
                    .HasDefaultValueSql("''");
;
                entity.Property(e => e.PFAM)
                    .HasColumnName("pfam")
                    .HasColumnType("longtext");

                entity.Property(e => e.POSITION)
                    .HasColumnName("position")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.PROTEIN_POSITION)
                    .HasColumnName("protein_position")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.REFSEQ_TRANSCRIPT)
                    .HasColumnName("refseq_transcript")
                    .HasColumnType("longtext");

                entity.Property(e => e.SWISSPORT)
                    .HasColumnName("swissport")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.VARIANT_CLASSIFICATION)
                    .HasColumnName("variant_classification")
                    .HasColumnType("varchar(128)");



                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });



            modelBuilder.Entity<FREQ_RELAT_ALL>(entity =>
            {
                entity.HasKey(e => e.FREQ_RELAT_ID);

                entity.ToTable("freq_relat_all");

                entity.Property(e => e.FREQ_RELAT_ID)
                    .HasColumnName("freq_relat_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.FREQ_ID)
                    .HasColumnName("freq_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.RELATIONID)
                    .HasColumnName("relationid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DRUGID)
                    .HasColumnName("drugid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.TARGETID)
                    .HasColumnName("targetid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DISEASEID)
                    .HasColumnName("diseaseid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");
            });



                modelBuilder.Entity<BUS_DRUG>(entity =>
            {
                entity.HasKey(e => e.DRUGID);

                entity.ToTable("bus_drug");

                entity.Property(e => e.DRUGID)
                    .HasColumnName("drugid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.BRAND_NAME)
                    .HasColumnName("brand_name")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.COMPANY)
                    .HasColumnName("company")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.COMPANY_ALIAS)
                    .HasColumnName("company_alias")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DRUGCODE)
                    .IsRequired()
                    .HasColumnName("drugcode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DRUG_NAME)
                    .HasColumnName("drug_name")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.DRUG_TARGET)
                    .HasColumnName("drug_target")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.DRUG_TYPE)
                    .HasColumnName("drug_type")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.HIT)
                    .HasColumnName("hit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MECHANISM_OF_ACTION)
                    .HasColumnName("mechanism_of_action")
                    .HasColumnType("longtext");

                entity.Property(e => e.STRUCTURE)
                    .HasColumnName("structure")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.STRUCTURE_INFO)
                    .HasColumnName("structure_info")
                    .HasColumnType("text");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.OPERATOR)
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<BUS_FUSION>(entity =>
            {
                entity.HasKey(e => e.FUSION_ID);

                entity.ToTable("bus_fusion");

                entity.Property(e => e.FUSION_ID)
                    .HasColumnName("fusion_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FRAME)
                    .HasColumnName("frame")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.FUSION)
                    .HasColumnName("fusion")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.FUSIONCODE)
                    .IsRequired()
                    .HasColumnName("fusioncode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SAMPLE_ID)
                    .HasColumnName("sample_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.TARGET)
                    .HasColumnName("target")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<BUS_CNA>(entity =>
            {
                entity.HasKey(e => new { e.SAMPLE_ID,e.TARGET});

                entity.ToTable("bus_cna");

                entity.Property(e => e.SAMPLE_ID)
                    .HasColumnName("sample_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.TARGET)
                   .HasColumnName("target")
                   .HasColumnType("varchar(90)");

                entity.Property(e => e.GASTIC_SCORE)
                   .HasColumnName("gastic_score")
                   .HasColumnType("float(5,1)");

                entity.Property(e => e.CNA_TYPE)
                   .HasColumnName("cna_type")
                   .HasColumnType("varchar(45)");

            });

            modelBuilder.Entity<BUS_PATIENT>(entity =>
            {
                entity.HasKey(e => e.PATIENT_ID);

                entity.ToTable("bus_patient");

                entity.Property(e => e.PATIENT_ID)
                    .HasColumnName("patient_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CENTER)
                    .HasColumnName("center")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ETHNICITY)
                    .HasColumnName("ethnicity")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PATIENTCODE)
                    .IsRequired()
                    .HasColumnName("patientcode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PRIMARY_RACE)
                    .HasColumnName("primary_race")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.SEX)
                    .HasColumnName("sex")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<BUS_RELATION>(entity =>
            {
                entity.HasKey(e => e.RELATIONID);

                entity.ToTable("bus_relation");

                entity.HasIndex(e => new { e.TARGETID, e.DISEASEID, e.DRUGID })
                    .HasName("Index_UNI");

                entity.Property(e => e.RELATIONID)
                    .HasColumnName("relationid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ALTERATION_RATE)
                    .HasColumnName("alteration_rate")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.APPROVAL_TIME)
                    .HasColumnName("approval_time")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.NEGATIVE_GENOTYPES)
                    .HasColumnName("negative_genotypes")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.APPROVED)
                    .HasColumnName("approved")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.CLINICAL_TRIAL)
                    .HasColumnName("clinical_trial")
                    .HasColumnType("longtext");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DISEASEID)
                    .HasColumnName("diseaseid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DOSAGE)
                    .HasColumnName("dosage")
                    .HasColumnType("longtext");

                entity.Property(e => e.DRUGID)
                    .HasColumnName("drugid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.EVIDENCE_LEVEL)
                    .HasColumnName("evidence_level")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.FUNCTION_AND_CLINICAL_IMPLICATIONS)
                    .HasColumnName("function_and_clinical_implications")
                    .HasColumnType("longtext");

                entity.Property(e => e.INDICATIONS)
                    .HasColumnName("indications")
                    .HasColumnType("longtext");

                entity.Property(e => e.DATA_TYPE)
                    .HasColumnName("data_type")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MUTATION_RATE)
                    .HasColumnName("mutation_rate")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.GERMLINE_10389_RATE)
                    .HasColumnName("germline_10389_rate")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CHINESE_10000_RATE)
                    .HasColumnName("chinese_10000_rate")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.REFERENCE_LINK)
                    .HasColumnName("reference_link")
                    .HasColumnType("text");

                entity.Property(e => e.SPECIFICITY)
                    .HasColumnName("specificity")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.TARGETID)
                    .HasColumnName("targetid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.THERAPY_INTERPRETATION)
                    .HasColumnName("therapy_interpretation")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<BUS_TARGET>(entity =>
            {
                entity.HasKey(e => e.TARGETID);

                entity.ToTable("bus_target");

                entity.HasIndex(e => e.SWISSPORT)
                    .HasName("IndexProtein");

                entity.HasIndex(e => e.TARGET)
                    .HasName("IndexTarget");

                entity.HasIndex(e => e.TARGETID)
                    .HasName("IndexTargetid");

                entity.Property(e => e.TARGETID)
                    .HasColumnName("targetid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ALTERATION)
                    .HasColumnName("alteration")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.CHROMOSOME)
                    .HasColumnName("chromosome")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.COSMIC_EVIDENCE)
                    .HasColumnName("cosmic_evidence")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.CLINICAL_SIGNIFICANCE)
                    .HasColumnName("clinical_significance")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.VIC_EVIDENCE)
                    .HasColumnName("VIC_evidence")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ENSEMBL_ID)
                    .HasColumnName("ensembl_id")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ENTREZ_GENEID)
                    .HasColumnName("entrez_geneid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.GENE_ALIAS)
                    .HasColumnName("gene_alias")
                    .HasColumnType("longtext");

                entity.Property(e => e.HGNC_ID)
                    .HasColumnName("hgnc_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.HIT)
                    .HasColumnName("hit")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.COSMIC)
                    .HasColumnName("cosmic")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.DBSNP)
                    .HasColumnName("dbsnp")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.CLINVAR)
                    .HasColumnName("clinvar")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.OMIMID)
                    .HasColumnName("omimid")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.MUTATIONTEXT)
                    .IsRequired()
                    .HasColumnName("mutationtext")
                    .HasColumnType("char(0)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PATHWAY_FIGURE)
                    .HasColumnName("pathway_figure")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.PATHWAY_LINKS_KEGG)
                    .HasColumnName("pathway_links_kegg")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.PFAM)
                    .HasColumnName("pfam")
                    .HasColumnType("longtext");

                entity.Property(e => e.POSITION)
                    .HasColumnName("position")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.PROTEIN_POSITION)
                    .HasColumnName("protein_position")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.REFSEQ_TRANSCRIPT)
                    .HasColumnName("refseq_transcript")
                    .HasColumnType("longtext");

                entity.Property(e => e.RESISTANCE)
                    .HasColumnName("resistance")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.STRAND)
                    .HasColumnName("strand")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.SWISSPORT)
                    .HasColumnName("swissport")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.TARGET)
                    .HasColumnName("target")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.TARGETCODE)
                    .HasColumnName("targetcode")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.VARIANT_CLASSIFICATION)
                    .HasColumnName("variant_classification")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.VARIANT_TYPE)
                    .HasColumnName("variant_type")
                    .HasColumnType("varchar(90)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<BUS_TARGET_SAMPLE_RELATION>(entity =>
            {
                entity.HasKey(e => e.TARGET_SAMPLE_RELATE_ID);

                entity.ToTable("bus_target_sample_relation");

                entity.HasIndex(e => e.SAMPLE_ID)
                    .HasName("IndexSampleId");

                entity.HasIndex(e => e.TARGETID)
                    .HasName("IndexTargetId");

                entity.Property(e => e.TARGET_SAMPLE_RELATE_ID)
                    .HasColumnName("target_sample_relate_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.IS_PUB)
                    .HasColumnName("is_pub")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SAMPLE_ID)
                    .HasColumnName("sample_id")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.TARGETID)
                    .HasColumnName("targetid")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<CMS_AD>(entity =>
            {
                entity.HasKey(e => e.ADID);

                entity.ToTable("cms_ad");

                entity.Property(e => e.ADID).HasColumnType("varchar(50)");

                entity.Property(e => e.ADNAME)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ADTEXT)
                    .IsRequired()
                    .HasColumnType("longtext");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<CMS_ARTICLE>(entity =>
            {
                entity.HasKey(e => e.ARTICLEID);

                entity.ToTable("cms_article");

                entity.HasIndex(e => e.CHANNELID)
                    .HasName("FK_cms_article_CHANNELID");

                entity.Property(e => e.ARTICLEID).HasColumnType("varchar(50)");

                entity.Property(e => e.ARTICLECONTENT)
                    .IsRequired()
                    .HasColumnType("longtext");

                entity.Property(e => e.ARTICLEEDITOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ARTICLEHIT)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.ARTICLEINDEXPIC)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ARTICLEISTITLE).HasColumnType("bit(1)");

                entity.Property(e => e.ARTICLEKEYWORDS)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ARTICLEREDIRECT)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ARTICLETIME).HasColumnType("datetime");

                entity.Property(e => e.ARTICLETITLE)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ARTICLETOPNUM).HasColumnType("int(11)");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.CHANNELID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.ISPUB).HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.CMS_CHANNEL)
                    .WithMany(p => p.CMS_CHANNELNavigation)
                    .HasForeignKey(d => d.CHANNELID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_cms_article_CHANNELID");
            });

            modelBuilder.Entity<CMS_CHANNEL>(entity =>
            {
                entity.HasKey(e => e.CHANNELID);

                entity.ToTable("cms_channel");

                entity.Property(e => e.CHANNELID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.CHANNELNAME)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.CHANNELREDIRECT)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.CHANNELXH).HasColumnType("int(11)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.ISMENU).HasColumnType("bit(1)");

                entity.Property(e => e.ISPUB).HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(1)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PARENTCHANNELID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PRECHANNELNAME)
                    .IsRequired()
                    .HasColumnType("varchar(200)");
            });

            modelBuilder.Entity<CMS_FILELIST>(entity =>
            {
                entity.HasKey(e => e.FID);

                entity.ToTable("cms_filelist");

                entity.Property(e => e.FID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.FCONTENTTYPE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FNAME)
                    .IsRequired()
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.FPATH)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.FSIZE).HasColumnType("int(11)");

                entity.Property(e => e.OPERATER)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<CMS_LINK>(entity =>
            {
                entity.HasKey(e => e.LINKID);

                entity.ToTable("cms_link");

                entity.Property(e => e.LINKID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.INDEXPIC)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ISPUB).HasColumnType("bit(1)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(1)");

                entity.Property(e => e.LINKTEXT)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.LINKTYPE)
                    .IsRequired()
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.LINKURL)
                    .IsRequired()
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.LINKXH).HasColumnType("int(11)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PRECSS)
                    .IsRequired()
                    .HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<COUNTER>(entity =>
            {
                entity.ToTable("counter");

                entity.HasIndex(e => e.KEY)
                    .HasName("IX_Counter_Key");

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EXPIREAT)
                    .HasColumnName("ExpireAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.KEY)
                    .IsRequired()
                    .HasColumnName("Key")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.VALUE)
                    .HasColumnName("Value")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<FD_BO>(entity =>
            {
                entity.ToTable("fd_bo");

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BOTYPE)
                    .IsRequired()
                    .HasColumnName("boType")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DATAFORMAT)
                    .IsRequired()
                    .HasColumnName("dataFormat")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPTIONS)
                    .IsRequired()
                    .HasColumnName("options")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PARENTID)
                    .IsRequired()
                    .HasColumnName("parentId")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PK)
                    .IsRequired()
                    .HasColumnName("pk")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.STATUS)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TYPEID)
                    .IsRequired()
                    .HasColumnName("typeId")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<FD_BO_FIELD>(entity =>
            {
                entity.ToTable("fd_bo_field");

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ATTRLENGTH)
                    .HasColumnName("attrLength")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BO_ID)
                    .IsRequired()
                    .HasColumnName("bo_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnName("bz")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DATATYPE)
                    .IsRequired()
                    .HasColumnName("dataType")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.DEFVALUE)
                    .IsRequired()
                    .HasColumnName("defValue")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.DESC)
                    .IsRequired()
                    .HasColumnName("desc")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.FIELDNAME)
                    .IsRequired()
                    .HasColumnName("fieldName")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FORMAT)
                    .IsRequired()
                    .HasColumnName("format")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ISNULL)
                    .IsRequired()
                    .HasColumnName("isNull")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PRECISION)
                    .HasColumnName("precision")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<FD_FORM>(entity =>
            {
                entity.ToTable("fd_form");

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ATTRS)
                    .IsRequired()
                    .HasColumnName("attrs")
                    .HasColumnType("longtext");

                entity.Property(e => e.BUSID)
                    .IsRequired()
                    .HasColumnName("busId")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DESC)
                    .IsRequired()
                    .HasColumnName("desc")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(4)");

                entity.Property(e => e.KEY)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODE)
                    .IsRequired()
                    .HasColumnName("mode")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PERMISSIONS)
                    .IsRequired()
                    .HasColumnName("permissions")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.TYPEID)
                    .IsRequired()
                    .HasColumnName("typeId")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TYPENAME)
                    .IsRequired()
                    .HasColumnName("typeName")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<FD_FORM_CASE>(entity =>
            {
                entity.ToTable("fd_form_case");

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BODATA)
                    .IsRequired()
                    .HasColumnName("boData")
                    .HasColumnType("longtext");

                entity.Property(e => e.BO_ID)
                    .IsRequired()
                    .HasColumnName("bo_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FORM_DATA)
                    .IsRequired()
                    .HasColumnName("form_data")
                    .HasColumnType("longtext");

                entity.Property(e => e.FORM_ID)
                    .IsRequired()
                    .HasColumnName("form_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FORM_KEY)
                    .IsRequired()
                    .HasColumnName("form_key")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FORM_NAME)
                    .IsRequired()
                    .HasColumnName("form_name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORGID)
                    .IsRequired()
                    .HasColumnName("orgId")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORGNAME)
                    .IsRequired()
                    .HasColumnName("orgName")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORGPATH)
                    .IsRequired()
                    .HasColumnName("orgPath")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PERMISSIONS)
                    .IsRequired()
                    .HasColumnName("permissions")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.STATE)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<FD_FORM_DATA>(entity =>
            {
                entity.ToTable("fd_form_data");

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnName("bz")
                    .HasColumnType("longtext");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FORM_CASE_ID)
                    .IsRequired()
                    .HasColumnName("form_case_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FORM_ID)
                    .IsRequired()
                    .HasColumnName("form_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(4)");

                entity.Property(e => e.KEY)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.VALUE)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<FD_FORM_FIELD>(entity =>
            {
                entity.ToTable("fd_form_field");

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DATATYPE)
                    .IsRequired()
                    .HasColumnName("dataType")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.DESC)
                    .IsRequired()
                    .HasColumnName("desc")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.FIELD_NAME)
                    .IsRequired()
                    .HasColumnName("field_name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FIELD_OPTIONS)
                    .IsRequired()
                    .HasColumnName("field_options")
                    .HasColumnType("longtext");

                entity.Property(e => e.FIELD_TYPE)
                    .IsRequired()
                    .HasColumnName("field_type")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FORM_ID)
                    .IsRequired()
                    .HasColumnName("form_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE)
                    .HasColumnName("is_delete")
                    .HasColumnType("bit(4)");

                entity.Property(e => e.LABEL)
                    .IsRequired()
                    .HasColumnName("label")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODIFY_DATE)
                    .HasColumnName("modify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnName("operator")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORDER)
                    .HasColumnName("order")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SHOWNAME)
                    .IsRequired()
                    .HasColumnName("showName")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.VERSION)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<FD_FORM_PERMISSION>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("fd_form_permission");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ1)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BZ2)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.FORM_KEY)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FORM_KEY_FIELD)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FORM_NAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORGNAME)
                    .IsRequired()
                    .HasColumnType("longtext");

                entity.Property(e => e.ORGPATH)
                    .IsRequired()
                    .HasColumnType("longtext");

                entity.Property(e => e.ROLE_CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ROLE_NAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TYPE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<FD_FORM_PRINT>(entity =>
            {
                entity.ToTable("fd_form_print");

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CONTENT)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("longtext");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.DESC)
                    .IsRequired()
                    .HasColumnName("desc")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.FORMKEY)
                    .IsRequired()
                    .HasColumnName("formKey")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.HTML)
                    .IsRequired()
                    .HasColumnName("html")
                    .HasColumnType("longtext");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<HASH>(entity =>
            {
                entity.ToTable("hash");

                entity.HasIndex(e => new { e.KEY, e.FIELD })
                    .HasName("IX_Hash_Key_Field")
                    .IsUnique();

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EXPIREAT).HasColumnName("ExpireAt");

                entity.Property(e => e.FIELD)
                    .IsRequired()
                    .HasColumnName("Field")
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.KEY)
                    .IsRequired()
                    .HasColumnName("Key")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.VALUE)
                    .HasColumnName("Value")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<JOB>(entity =>
            {
                entity.ToTable("job");

                entity.HasIndex(e => e.STATENAME)
                    .HasName("IX_Job_StateName");

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ARGUMENTS)
                    .IsRequired()
                    .HasColumnName("Arguments")
                    .HasColumnType("longtext");

                entity.Property(e => e.CREATEDAT).HasColumnName("CreatedAt");

                entity.Property(e => e.EXPIREAT).HasColumnName("ExpireAt");

                entity.Property(e => e.INVOCATIONDATA)
                    .IsRequired()
                    .HasColumnName("InvocationData")
                    .HasColumnType("longtext");

                entity.Property(e => e.STATEID)
                    .HasColumnName("StateId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.STATENAME)
                    .HasColumnName("StateName")
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<JOBPARAMETER>(entity =>
            {
                entity.ToTable("jobparameter");

                entity.HasIndex(e => e.JOBID)
                    .HasName("FK_JobParameter_Job");

                entity.HasIndex(e => new { e.JOBID, e.NAME })
                    .HasName("IX_JobParameter_JobId_Name")
                    .IsUnique();

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.JOBID)
                    .HasColumnName("JobId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("Name")
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.VALUE)
                    .HasColumnName("Value")
                    .HasColumnType("longtext");

                entity.HasOne(d => d.JOB)
                    .WithMany(p => p.JOBNavigation)
                    .HasForeignKey(d => d.JOBID)
                    .HasConstraintName("FK_JobParameter_Job");
            });

            modelBuilder.Entity<JOBQUEUE>(entity =>
            {
                entity.ToTable("jobqueue");

                entity.HasIndex(e => new { e.QUEUE, e.FETCHEDAT })
                    .HasName("IX_JobQueue_QueueAndFetchedAt");

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FETCHEDAT).HasColumnName("FetchedAt");

                entity.Property(e => e.FETCHTOKEN)
                    .HasColumnName("FetchToken")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.JOBID)
                    .HasColumnName("JobId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QUEUE)
                    .IsRequired()
                    .HasColumnName("Queue")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<JOBSTATE>(entity =>
            {
                entity.ToTable("jobstate");

                entity.HasIndex(e => e.JOBID)
                    .HasName("FK_JobState_Job");

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CREATEDAT).HasColumnName("CreatedAt");

                entity.Property(e => e.DATA)
                    .HasColumnName("Data")
                    .HasColumnType("longtext");

                entity.Property(e => e.JOBID)
                    .HasColumnName("JobId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("Name")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.REASON)
                    .HasColumnName("Reason")
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.JOB)
                    .WithMany(p => p.JOB1)
                    .HasForeignKey(d => d.JOBID)
                    .HasConstraintName("FK_JobState_Job");
            });

            modelBuilder.Entity<LIST>(entity =>
            {
                entity.ToTable("list");

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EXPIREAT).HasColumnName("ExpireAt");

                entity.Property(e => e.KEY)
                    .IsRequired()
                    .HasColumnName("Key")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.VALUE)
                    .HasColumnName("Value")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<PF_FILE>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_file");

                entity.HasIndex(e => e.WGID)
                    .HasName("WGID_INX");

                entity.HasIndex(e => new { e.WGID, e.LX })
                    .HasName("WGID_LX_INX");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.FILENAME)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.FILEURI)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.IP)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.LX)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MD5)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PX).HasColumnType("int(11)");

                entity.Property(e => e.TYPE)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.WGID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_LM>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_lm");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_LOG>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_log");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CZDX)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.LXM)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.RZLX)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.RZNR)
                    .IsRequired()
                    .HasColumnType("longtext");

                entity.Property(e => e.RZSJ).HasColumnType("datetime");
            });

            modelBuilder.Entity<PF_MENU>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_menu");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.ICON).HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PERMISSION_CODE).HasColumnType("varchar(50)");

                entity.Property(e => e.SUPER)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TYPE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.URL).HasColumnType("varchar(200)");
            });

            modelBuilder.Entity<PF_MSG>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_msg");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CONTENT)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ROLE)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TYPE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.URL)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.USERNAME)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<PF_MSG_REPLY>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_msg_reply");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CONTENT)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.IS_READ).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.MSG_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.URL)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.USERNAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_MSG_STATE>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_msg_state");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CONTENT)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.IS_NOTIFY).HasColumnType("bit(4)");

                entity.Property(e => e.IS_READ).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.MSG_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.URL)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.USERNAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_NEW>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_new");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.AUTHOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CONTENT)
                    .IsRequired()
                    .HasColumnType("longtext");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.LM_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PUBLISH_TIME).HasColumnType("datetime");

                entity.Property(e => e.STATE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.VI_TIME).HasColumnType("int(11)");
            });

            modelBuilder.Entity<PF_NEW_ROLE>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_new_role");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NEW_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ROLE_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_ORG>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_org");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ1).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ2).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ3).HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.DEPTH).HasColumnType("int(11)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PATH)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.SUPER)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TYPE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_PERMISSION>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_permission");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_PRINT_TMPL>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_print_tmpl");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ1).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ2).HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.DEPTH).HasColumnType("int(11)");

                entity.Property(e => e.FGID).HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SUPER)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_PROFILE>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_profile");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.AGE).HasColumnType("int(11)");

                entity.Property(e => e.BZ)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.DLZH)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.GRAH)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MAIL)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PHONE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SEX)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SR).HasColumnType("date");

                entity.Property(e => e.TXDZ)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.ZW)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_REMINDER>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_reminder");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.API)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.CONTENT)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.EDATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PARAM1)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PARAM2)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PARAM3)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PARAM4)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ROLES)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RULE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SDATE).HasColumnType("datetime");

                entity.Property(e => e.STATUS)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.USERS)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<PF_ROLE>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_role");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_ROLE_PERMISSION>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_role_permission");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PER_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ROLE_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_SMSCODE>(entity =>
            {
                entity.HasKey(e => e.CID);

                entity.ToTable("pf_smscode");

                entity.Property(e => e.CID).HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(1)");

                entity.Property(e => e.MOBILE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SENDRET)
                    .IsRequired()
                    .HasColumnType("varchar(512)");
            });

            modelBuilder.Entity<PF_STATE>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_state");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CODE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORDERS).HasColumnType("int(11)");

                entity.Property(e => e.TYPE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_USER>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_user");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.FISRTNAME)
                    .HasColumnName("FisrtName")
                    .HasColumnType("varchar(80)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.LASTNAME)
                    .HasColumnName("LastName")
                    .HasColumnType("varchar(80)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.PASSWORD)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.RYBM)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SJHM)
                    .IsRequired()
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.USERNAME)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.XMBM)
                    .IsRequired()
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.YHZT)
                    .IsRequired()
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<PF_USER_ORG>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_user_org");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ1)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BZ2)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORG_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORG_NAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ORG_PATH)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.USER_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.USER_NAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_USER_ROLE>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_user_role");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ROLE_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.USER_GID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<PF_ZDYDA>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("pf_zdyda");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ1)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BZ2)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BZ3)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.DAY)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.DKEY)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPERATOR)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TYPE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.VALUE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.XGDA1)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.XGDA2)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.XGDA3)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<SERVER>(entity =>
            {
                entity.ToTable("server");

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.DATA)
                    .IsRequired()
                    .HasColumnName("Data")
                    .HasColumnType("longtext");

                entity.Property(e => e.LASTHEARTBEAT).HasColumnName("LastHeartbeat");
            });

            modelBuilder.Entity<SET>(entity =>
            {
                entity.ToTable("set");

                entity.HasIndex(e => new { e.KEY, e.VALUE })
                    .HasName("IX_Set_Key_Value")
                    .IsUnique();

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EXPIREAT)
                    .HasColumnName("ExpireAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.KEY)
                    .IsRequired()
                    .HasColumnName("Key")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.SCORE).HasColumnName("Score");

                entity.Property(e => e.VALUE)
                    .IsRequired()
                    .HasColumnName("Value")
                    .HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<STATE>(entity =>
            {
                entity.ToTable("state");

                entity.HasIndex(e => e.JOBID)
                    .HasName("FK_HangFire_State_Job");

                entity.Property(e => e.ID)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CREATEDAT).HasColumnName("CreatedAt");

                entity.Property(e => e.DATA)
                    .HasColumnName("Data")
                    .HasColumnType("longtext");

                entity.Property(e => e.JOBID)
                    .HasColumnName("JobId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasColumnName("Name")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.REASON)
                    .HasColumnName("Reason")
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.JOB)
                    .WithMany(p => p.JOB2)
                    .HasForeignKey(d => d.JOBID)
                    .HasConstraintName("FK_HangFire_State_Job");
            });

            modelBuilder.Entity<WEIXIN_USER>(entity =>
            {
                entity.HasKey(e => e.GID);

                entity.ToTable("weixin_user");

                entity.Property(e => e.GID).HasColumnType("varchar(50)");

                entity.Property(e => e.BZ1)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.BZ2)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CITY)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.COUNTRY)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");

                entity.Property(e => e.HEADIMGURL)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.IS_DELETE).HasColumnType("bit(4)");

                entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");

                entity.Property(e => e.NICKNAME)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.OPENID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PROVINCE)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SEX)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.UNIONID)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });
        }
    }
}
