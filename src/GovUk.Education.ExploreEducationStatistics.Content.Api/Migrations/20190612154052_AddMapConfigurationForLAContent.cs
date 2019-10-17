﻿using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GovUk.Education.ExploreEducationStatistics.Content.Api.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddMapConfigurationForLAContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Releases",
                keyColumn: "Id",
                keyValue: new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5"),
                column: "Content",
                value: "[{\"Order\":1,\"Heading\":\"About these statistics\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The statistics and data cover the absence of pupils of compulsory school age during the 2016/17 academic year in the following state-funded school types:\\n\\n- primary schools\\n- secondary schools\\n- special schools\\n\\nThey also includes information fo [pupil referral units](../glossary#pupil-referral-unit) and pupils aged 4 years.\\n\\nWe use the key measures of [overall absence](../glossary#overall-absence) and [persistent absence](../glossary#persistent-absence) to monitor pupil absence and also include [absence by reason](#contents-section-heading-4) and [pupil characteristics](#contents-section-heading-6).\\n\\nThe statistics and data are available at national, regional, local authority (LA) and school level and are used by LAs and schools to compare their local absence rates to regional and national averages for different pupil groups.\\n\\nThey're also used for policy development as key indicators in behaviour and school attendance policy.\\n\"}]},{\"Order\":2,\"Heading\":\"Pupil absence rates\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"**Overall absence**\\n\\nThe [overall absence](../glossary#overall-absence) rate has increased across state-funded primary, secondary and special schools between 2015/16 and 2016/17 driven by an increase in the unauthorised absence rate.\\n\\nIt increased from 4.6% to 4.7% over this period while the [unauthorised absence](../glossary#unauthorised-absence) rate increased from 1.1% to 1.3%.\\n\\nThe rate stayed the same at 4% in primary schools but increased from 5.2% to 5.4% for secondary schools. However, in special schools it was much higher and rose to 9.7%.\\n\\nThe overall and [authorised absence](../glossary#authorised-absence) rates have been fairly stable over recent years after gradually decreasing between 2006/07 and 2013/14.\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataBlockRequest\":{\"subjectId\":1,\"geographicLevel\":\"National\",\"countries\":null,\"localAuthorities\":null,\"regions\":null,\"startYear\":\"2012\",\"endYear\":\"2016\",\"filters\":[\"1\",\"2\"],\"indicators\":[\"23\",\"26\",\"28\"]},\"Charts\":[{\"Indicators\":[\"23\",\"26\",\"28\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"},\"Type\":\"line\"}],\"Summary\":null,\"Tables\":[{\"indicators\":[\"23\",\"26\",\"28\"]}]},{\"Type\":\"MarkDownBlock\",\"Body\":\"**Unauthorised absence**\\n\\nThe [unauthorised absence](../glossary#unauthorised-absence) rate has not varied much since 2006/07 but is at its highest since records began - 1.3%.\\n\\nThis is due to an increase in absence due to family holidays not agreed by schools.\\n\\n**Authorised absence**\\n\\nThe [authorised absence](../glossary#authorised-absence) rate has stayed at 3.4% since 2015/16 but has been decreasing in recent years within primary schools.\\n\\n**Total number of days missed**\\n\\nThe total number of days missed for [overall absence](../glossary#overall-absence) across state-funded primary, secondary and special schools has increased to 56.7 million from 54.8 million in 2015/16.\\n\\nThis partly reflects a rise in the total number of pupils with the average number of days missed per pupil slightly increased to 8.2 days from 8.1 days in 2015/16.\\n\\nIn 2016/17, 91.8% of primary, secondary and special school pupils missed at least 1 session during the school year - similar to the 91.7% figure from 2015/16.\"}]},{\"Order\":3,\"Heading\":\"Persistent absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [persistent absence](../glossary#persistent-absence) rate increased to and accounted for 37.6% of all absence - up from 36.6% in 2015 to 16 but still down from 43.3% in 2011 to 12.\\n\\nIt also accounted for almost a third (31.6%) of all [authorised absence](../glossary#authorised-absence) and more than half (53.8%) of all [unauthorised absence](../glossary#unauthorised-absence).\\n\\nOverall, it's increased across primary and secondary schools to 10.8% - up from 10.5% in 2015 to 16.\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataBlockRequest\":{\"subjectId\":1,\"geographicLevel\":\"National\",\"countries\":null,\"localAuthorities\":null,\"regions\":null,\"startYear\":\"2012\",\"endYear\":\"2016\",\"filters\":[\"1\",\"2\"],\"indicators\":[\"23\",\"26\",\"28\"]},\"Charts\":[{\"Indicators\":[\"23\",\"26\",\"28\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"},\"Type\":\"line\"}],\"Summary\":null,\"Tables\":[{\"indicators\":[\"23\",\"26\",\"28\"]}]},{\"Type\":\"MarkDownBlock\",\"Body\":\"**Persistent absentees**\\n\\nThe [overall absence](../glossary#overall-absence) rate for persistent absentees across all schools increased to 18.1% - nearly 4 times higher than the rate for all pupils. This is slightly up from 17.6% in 2015/16.\\n\\n**Illness absence rate**\\n\\nThe illness absence rate is almost 4 times higher for persistent absentees at 7.6% compared to 2% for other pupils.\"}]},{\"Order\":4,\"Heading\":\"Reasons for absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"InsetTextBlock\",\"Heading\":null,\"Body\":\"These have been broken down into the following:\\n\\n* distribution of absence by reason - the proportion of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of absences\\n\\n* rate of absence by reason - the rate of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of possible sessions\\n\\n* one or more sessions missed due to each reason - the number of pupils missing at least 1 session due to each reason\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataBlockRequest\":{\"subjectId\":1,\"geographicLevel\":\"National\",\"countries\":null,\"localAuthorities\":null,\"regions\":null,\"startYear\":\"2012\",\"endYear\":\"2016\",\"filters\":[\"1\",\"2\"],\"indicators\":[\"23\",\"26\",\"28\"]},\"Charts\":[{\"Indicators\":[\"23\",\"26\",\"28\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"},\"Type\":\"line\"}],\"Summary\":null,\"Tables\":[{\"indicators\":[\"23\",\"26\",\"28\"]}]},{\"Type\":\"MarkDownBlock\",\"Body\":\"**Illness**\\n\\nThis is the main driver behind [overall absence](../glossary#overall-absence) and accounted for 55.3% of all absence - down from 57.3% in 2015/16 and 60.1% in 2014/15.\\n\\nWhile the overall absence rate has slightly increased since 2015/16 the illness rate has stayed the same at 2.6%.\\n\\nThe absence rate due to other unauthorised circumstances has also stayed the same since 2015/16 at 0.7%.\\n\\n**Absence due to family holiday**\\n\\nThe unauthorised holiday absence rate has increased gradually since 2006/07 while authorised holiday absence rates are much lower than in 2006/07 and remained steady over recent years.\\n\\nThe percentage of pupils who missed at least 1 session due to family holiday increased to 16.9% - up from 14.7% in 2015/16.\\n\\nThe absence rate due to family holidays agreed by the school stayed at 0.1%.\\n\\nMeanwhile, the percentage of all possible sessions missed due to unauthorised family holidays increased to 0.4% - up from 0.3% in 2015/16.\\n\\n**Regulation amendment**\\n\\nA regulation amendment in September 2013 stated that term-time leave could only be granted in exceptional circumstances which explains the sharp fall in authorised holiday absence between 2012/13 and 2013/14.\\n\\nThese statistics and data relate to the period after the [Isle of Wight Council v Jon Platt High Court judgment (May 2016)](https://commonslibrary.parliament.uk/insights/term-time-holidays-supreme-court-judgment/) where the High Court supported a local magistrates’ ruling that there was no case to answer.\\n\\nThey also partially relate to the period after the April 2017 Supreme Court judgment where it unanimously agreed that no children should be taken out of school without good reason and clarified that 'regularly' means 'in accordance with the rules prescribed by the school'.\"}]},{\"Order\":5,\"Heading\":\"Distribution of absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"Nearly half of all pupils (48.9%) were absent for 5 days or less across primary, secondary and special schools - down from 49.1% in 2015/16.\\n\\nThe average total absence for primary school pupils was 7.2 days compared to 16.9 days for special school and 9.3 day for secondary school pupils.\\n\\nThe rate of pupils who had more than 25 days of absence stayed the same as in 2015/16 at 4.3%.\\n\\nThese pupils accounted for 23.5% of days missed while 8.2% of pupils had no absence.\\n\\n**Absence by term**\\n\\nAcross all schools:\\n\\n* [overall absence](../glossary#overall-absence) - highest in summer and lowest in autumn\\n\\n* [authorised absence](../glossary#authorised-absence) - highest in spring and lowest in summer\\n\\n* [unauthorised absence](../glossary#unauthorised-absence) - highest in summer\"}]},{\"Order\":6,\"Heading\":\"Absence by pupil characteristics\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [overall absence](../glossary#overall-absence) and [persistent absence](..glossary#persistent-absence) patterns for pupils with different characteristics have been consistent over recent years.\\n\\n**Ethnic groups**\\n\\nOverall absence rate:\\n\\n* Travellers of Irish heritage and Gypsy / Roma pupils - highest at 18.1% and 12.9% respectively\\n\\n* Chinese and Black African ethnicity pupils - substantially lower than the national average of 4.7% at 2.4% and 2.9% respectively\\n\\nPersistent absence rate:\\n\\n* Travellers of Irish heritage pupils - highest at 64%\\n\\n* Chinese pupils - lowest at 3.1%\\n\\n**Free school meals (FSM) eligibility**\\n\\nOverall absence rate:\\n\\n* pupils known to be eligible for and claiming FSM - higher at 7.3% compared to 4.2% for non-FSM pupils\\n\\nPersistent absence rate:\\n\\n* pupils known to be eligible for and claiming FSM - more than double the rate of non-FSM pupils\\n\\n**Gender**\\n\\nOverall absence rate:\\n\\n* boys and girls - very similar at 4.7% and 4.6% respectively\\n\\nPersistent absence rate:\\n\\n* boys and girls - similar at 10.9% and 10.6% respectively\\n\\n**National curriculum year group**\\n\\nOverall absence rate:\\n\\n* pupils in national curriculum year groups 3 and 4 - lowest at 3.9% and 4% respectively\\n\\n* pupils in national curriculum year groups 10 and 11 - highest at 6.1% and 6.2% respectively\\n\\nThis trend is repeated for the persistent absence rate.\\n\\n**Special educational need (SEN)**\\n\\nOverall absence rate:\\n\\n* pupils with a SEN statement or education healthcare (EHC) plan - 8.2% compared to 4.3% for those with no identified SEN\\n\\nPersistent absence rate:\\n\\n* pupils with a SEN statement or education healthcare (EHC) plan - more than 2 times higher than pupils with no identified SEN\"}]},{\"Order\":7,\"Heading\":\"Absence for 4-year-olds\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [overall absence](../glossary#overall-absence) rate decreased to 5.1% - down from 5.2% for the previous 2 years.\\n\\nAbsence recorded for 4-year-olds is not treated as authorised or unauthorised and only reported as overall absence.\"}]},{\"Order\":8,\"Heading\":\"Pupil referral unit absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [overall absence](../glossary#overall-absence) rate increased to 33.9% - up from 32.6% in 2015/16.\\n\\nThe [persistent absence](../glossary#persistent-absence) rate increased to 73.9% - up from 72.5% in 2015/16.\"}]},{\"Order\":9,\"Heading\":\"Regional and local authority (LA) breakdown\",\"Caption\":\"\",\"Content\":[{\"Type\":\"DataBlock\",\"Heading\":null,\"DataBlockRequest\":{\"subjectId\":1,\"geographicLevel\":\"Local_Authority\",\"countries\":null,\"localAuthorities\":null,\"regions\":null,\"startYear\":\"2016\",\"endYear\":\"2017\",\"filters\":[\"1\",\"2\"],\"indicators\":[\"23\",\"26\",\"28\"]},\"Charts\":[{\"Indicators\":[\"23\",\"26\",\"28\"],\"Type\":\"map\",\"XAxis\":{\"title\":\"map\"},\"YAxis\":{\"title\":\"map\"}}],\"Summary\":null,\"Tables\":null},{\"Type\":\"MarkDownBlock\",\"Body\":\"[Overall absence](../glossary#overall-absence) and [persistent absence](../glossary#persistent-absence) rates vary across primary, secondary and special schools by region and local authority (LA).\\n\\n**Overall absence**\\n\\nSimilar to 2015/16, the 3 regions with the highest rates across all school types were:\\n\\n* North East - 4.9%\\n\\n* Yorkshire and the Humber - 4.9%\\n\\n* South West - 4.8%\\n\\nMeanwhile, Inner and Outer London had the lowest rates at 4.4%.\\n\\n**Persistent absence**\\n\\nThe region with the highest persistent absence rate was Yorkshire and the Humber with 11.9% while Outer London had the lowest rate at 10%.\\n\\n**Local authority (LA) level data**\\n\\nDownload data in the following formats or access our data via our API:\\n\\n[Download .csv files]('#')\\n\\n[Download Excel files]('#')\\n\\n[Download pdf files]('#')\\n\\n[Access API]('#') - [What is an API?]('../glossary#what-is-an-api')\"}]}]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Releases",
                keyColumn: "Id",
                keyValue: new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5"),
                column: "Content",
                value: "[{\"Order\":1,\"Heading\":\"About these statistics\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The statistics and data cover the absence of pupils of compulsory school age during the 2016/17 academic year in the following state-funded school types:\\n\\n- primary schools\\n- secondary schools\\n- special schools\\n\\nThey also includes information fo [pupil referral units](../glossary#pupil-referral-unit) and pupils aged 4 years.\\n\\nWe use the key measures of [overall absence](../glossary#overall-absence) and [persistent absence](../glossary#persistent-absence) to monitor pupil absence and also include [absence by reason](#contents-section-heading-4) and [pupil characteristics](#contents-section-heading-6).\\n\\nThe statistics and data are available at national, regional, local authority (LA) and school level and are used by LAs and schools to compare their local absence rates to regional and national averages for different pupil groups.\\n\\nThey're also used for policy development as key indicators in behaviour and school attendance policy.\\n\"}]},{\"Order\":2,\"Heading\":\"Pupil absence rates\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"**Overall absence**\\n\\nThe [overall absence](../glossary#overall-absence) rate has increased across state-funded primary, secondary and special schools between 2015/16 and 2016/17 driven by an increase in the unauthorised absence rate.\\n\\nIt increased from 4.6% to 4.7% over this period while the [unauthorised absence](../glossary#unauthorised-absence) rate increased from 1.1% to 1.3%.\\n\\nThe rate stayed the same at 4% in primary schools but increased from 5.2% to 5.4% for secondary schools. However, in special schools it was much higher and rose to 9.7%.\\n\\nThe overall and [authorised absence](../glossary#authorised-absence) rates have been fairly stable over recent years after gradually decreasing between 2006/07 and 2013/14.\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataBlockRequest\":{\"subjectId\":1,\"geographicLevel\":\"National\",\"countries\":null,\"localAuthorities\":null,\"regions\":null,\"startYear\":\"2012\",\"endYear\":\"2016\",\"filters\":[\"1\",\"2\"],\"indicators\":[\"23\",\"26\",\"28\"]},\"Charts\":[{\"Indicators\":[\"23\",\"26\",\"28\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"},\"Type\":\"line\"}],\"Summary\":null,\"Tables\":[{\"indicators\":[\"23\",\"26\",\"28\"]}]},{\"Type\":\"MarkDownBlock\",\"Body\":\"**Unauthorised absence**\\n\\nThe [unauthorised absence](../glossary#unauthorised-absence) rate has not varied much since 2006/07 but is at its highest since records began - 1.3%.\\n\\nThis is due to an increase in absence due to family holidays not agreed by schools.\\n\\n**Authorised absence**\\n\\nThe [authorised absence](../glossary#authorised-absence) rate has stayed at 3.4% since 2015/16 but has been decreasing in recent years within primary schools.\\n\\n**Total number of days missed**\\n\\nThe total number of days missed for [overall absence](../glossary#overall-absence) across state-funded primary, secondary and special schools has increased to 56.7 million from 54.8 million in 2015/16.\\n\\nThis partly reflects a rise in the total number of pupils with the average number of days missed per pupil slightly increased to 8.2 days from 8.1 days in 2015/16.\\n\\nIn 2016/17, 91.8% of primary, secondary and special school pupils missed at least 1 session during the school year - similar to the 91.7% figure from 2015/16.\"}]},{\"Order\":3,\"Heading\":\"Persistent absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [persistent absence](../glossary#persistent-absence) rate increased to and accounted for 37.6% of all absence - up from 36.6% in 2015 to 16 but still down from 43.3% in 2011 to 12.\\n\\nIt also accounted for almost a third (31.6%) of all [authorised absence](../glossary#authorised-absence) and more than half (53.8%) of all [unauthorised absence](../glossary#unauthorised-absence).\\n\\nOverall, it's increased across primary and secondary schools to 10.8% - up from 10.5% in 2015 to 16.\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataBlockRequest\":{\"subjectId\":1,\"geographicLevel\":\"National\",\"countries\":null,\"localAuthorities\":null,\"regions\":null,\"startYear\":\"2012\",\"endYear\":\"2016\",\"filters\":[\"1\",\"2\"],\"indicators\":[\"23\",\"26\",\"28\"]},\"Charts\":[{\"Indicators\":[\"23\",\"26\",\"28\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"},\"Type\":\"line\"}],\"Summary\":null,\"Tables\":[{\"indicators\":[\"23\",\"26\",\"28\"]}]},{\"Type\":\"MarkDownBlock\",\"Body\":\"**Persistent absentees**\\n\\nThe [overall absence](../glossary#overall-absence) rate for persistent absentees across all schools increased to 18.1% - nearly 4 times higher than the rate for all pupils. This is slightly up from 17.6% in 2015/16.\\n\\n**Illness absence rate**\\n\\nThe illness absence rate is almost 4 times higher for persistent absentees at 7.6% compared to 2% for other pupils.\"}]},{\"Order\":4,\"Heading\":\"Reasons for absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"InsetTextBlock\",\"Heading\":null,\"Body\":\"These have been broken down into the following:\\n\\n* distribution of absence by reason - the proportion of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of absences\\n\\n* rate of absence by reason - the rate of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of possible sessions\\n\\n* one or more sessions missed due to each reason - the number of pupils missing at least 1 session due to each reason\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataBlockRequest\":{\"subjectId\":1,\"geographicLevel\":\"National\",\"countries\":null,\"localAuthorities\":null,\"regions\":null,\"startYear\":\"2012\",\"endYear\":\"2016\",\"filters\":[\"1\",\"2\"],\"indicators\":[\"23\",\"26\",\"28\"]},\"Charts\":[{\"Indicators\":[\"23\",\"26\",\"28\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"},\"Type\":\"line\"}],\"Summary\":null,\"Tables\":[{\"indicators\":[\"23\",\"26\",\"28\"]}]},{\"Type\":\"MarkDownBlock\",\"Body\":\"**Illness**\\n\\nThis is the main driver behind [overall absence](../glossary#overall-absence) and accounted for 55.3% of all absence - down from 57.3% in 2015/16 and 60.1% in 2014/15.\\n\\nWhile the overall absence rate has slightly increased since 2015/16 the illness rate has stayed the same at 2.6%.\\n\\nThe absence rate due to other unauthorised circumstances has also stayed the same since 2015/16 at 0.7%.\\n\\n**Absence due to family holiday**\\n\\nThe unauthorised holiday absence rate has increased gradually since 2006/07 while authorised holiday absence rates are much lower than in 2006/07 and remained steady over recent years.\\n\\nThe percentage of pupils who missed at least 1 session due to family holiday increased to 16.9% - up from 14.7% in 2015/16.\\n\\nThe absence rate due to family holidays agreed by the school stayed at 0.1%.\\n\\nMeanwhile, the percentage of all possible sessions missed due to unauthorised family holidays increased to 0.4% - up from 0.3% in 2015/16.\\n\\n**Regulation amendment**\\n\\nA regulation amendment in September 2013 stated that term-time leave could only be granted in exceptional circumstances which explains the sharp fall in authorised holiday absence between 2012/13 and 2013/14.\\n\\nThese statistics and data relate to the period after the [Isle of Wight Council v Jon Platt High Court judgment (May 2016)](https://commonslibrary.parliament.uk/insights/term-time-holidays-supreme-court-judgment/) where the High Court supported a local magistrates’ ruling that there was no case to answer.\\n\\nThey also partially relate to the period after the April 2017 Supreme Court judgment where it unanimously agreed that no children should be taken out of school without good reason and clarified that 'regularly' means 'in accordance with the rules prescribed by the school'.\"}]},{\"Order\":5,\"Heading\":\"Distribution of absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"Nearly half of all pupils (48.9%) were absent for 5 days or less across primary, secondary and special schools - down from 49.1% in 2015/16.\\n\\nThe average total absence for primary school pupils was 7.2 days compared to 16.9 days for special school and 9.3 day for secondary school pupils.\\n\\nThe rate of pupils who had more than 25 days of absence stayed the same as in 2015/16 at 4.3%.\\n\\nThese pupils accounted for 23.5% of days missed while 8.2% of pupils had no absence.\\n\\n**Absence by term**\\n\\nAcross all schools:\\n\\n* [overall absence](../glossary#overall-absence) - highest in summer and lowest in autumn\\n\\n* [authorised absence](../glossary#authorised-absence) - highest in spring and lowest in summer\\n\\n* [unauthorised absence](../glossary#unauthorised-absence) - highest in summer\"}]},{\"Order\":6,\"Heading\":\"Absence by pupil characteristics\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [overall absence](../glossary#overall-absence) and [persistent absence](..glossary#persistent-absence) patterns for pupils with different characteristics have been consistent over recent years.\\n\\n**Ethnic groups**\\n\\nOverall absence rate:\\n\\n* Travellers of Irish heritage and Gypsy / Roma pupils - highest at 18.1% and 12.9% respectively\\n\\n* Chinese and Black African ethnicity pupils - substantially lower than the national average of 4.7% at 2.4% and 2.9% respectively\\n\\nPersistent absence rate:\\n\\n* Travellers of Irish heritage pupils - highest at 64%\\n\\n* Chinese pupils - lowest at 3.1%\\n\\n**Free school meals (FSM) eligibility**\\n\\nOverall absence rate:\\n\\n* pupils known to be eligible for and claiming FSM - higher at 7.3% compared to 4.2% for non-FSM pupils\\n\\nPersistent absence rate:\\n\\n* pupils known to be eligible for and claiming FSM - more than double the rate of non-FSM pupils\\n\\n**Gender**\\n\\nOverall absence rate:\\n\\n* boys and girls - very similar at 4.7% and 4.6% respectively\\n\\nPersistent absence rate:\\n\\n* boys and girls - similar at 10.9% and 10.6% respectively\\n\\n**National curriculum year group**\\n\\nOverall absence rate:\\n\\n* pupils in national curriculum year groups 3 and 4 - lowest at 3.9% and 4% respectively\\n\\n* pupils in national curriculum year groups 10 and 11 - highest at 6.1% and 6.2% respectively\\n\\nThis trend is repeated for the persistent absence rate.\\n\\n**Special educational need (SEN)**\\n\\nOverall absence rate:\\n\\n* pupils with a SEN statement or education healthcare (EHC) plan - 8.2% compared to 4.3% for those with no identified SEN\\n\\nPersistent absence rate:\\n\\n* pupils with a SEN statement or education healthcare (EHC) plan - more than 2 times higher than pupils with no identified SEN\"}]},{\"Order\":7,\"Heading\":\"Absence for 4-year-olds\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [overall absence](../glossary#overall-absence) rate decreased to 5.1% - down from 5.2% for the previous 2 years.\\n\\nAbsence recorded for 4-year-olds is not treated as authorised or unauthorised and only reported as overall absence.\"}]},{\"Order\":8,\"Heading\":\"Pupil referral unit absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The [overall absence](../glossary#overall-absence) rate increased to 33.9% - up from 32.6% in 2015/16.\\n\\nThe [persistent absence](../glossary#persistent-absence) rate increased to 73.9% - up from 72.5% in 2015/16.\"}]},{\"Order\":9,\"Heading\":\"Regional and local authority (LA) breakdown\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"MAP GOES HERE\"},{\"Type\":\"MarkDownBlock\",\"Body\":\"[Overall absence](../glossary#overall-absence) and [persistent absence](../glossary#persistent-absence) rates vary across primary, secondary and special schools by region and local authority (LA).\\n\\n**Overall absence**\\n\\nSimilar to 2015/16, the 3 regions with the highest rates across all school types were:\\n\\n* North East - 4.9%\\n\\n* Yorkshire and the Humber - 4.9%\\n\\n* South West - 4.8%\\n\\nMeanwhile, Inner and Outer London had the lowest rates at 4.4%.\\n\\n**Persistent absence**\\n\\nThe region with the highest persistent absence rate was Yorkshire and the Humber with 11.9% while Outer London had the lowest rate at 10%.\\n\\n**Local authority (LA) level data**\\n\\nDownload data in the following formats or access our data via our API:\\n\\n[Download .csv files]('#')\\n\\n[Download Excel files]('#')\\n\\n[Download pdf files]('#')\\n\\n[Access API]('#') - [What is an API?]('../glossary#what-is-an-api')\"}]}]");
        }
    }
}
