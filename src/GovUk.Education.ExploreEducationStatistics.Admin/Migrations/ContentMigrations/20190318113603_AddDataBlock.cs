﻿using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GovUk.Education.ExploreEducationStatistics.Admin.Migrations.ContentMigrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddDataBlock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Releases",
                keyColumn: "Id",
                keyValue: new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5"),
                column: "Content",
                value: "[{\"Order\":1,\"Heading\":\"About this release\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"This statistical first release (SFR) reports on absence of pupils of compulsory school age in state-funded primary, secondary and special schools during the 2016/17 academic year. Information on absence in pupil referral units, and for pupils aged four, is also included. The Department uses two key measures to monitor pupil absence – overall and persistent absence. Absence by reason and pupils characteristics is also included in this release. Figures are available at national, regional, local authority and school level. Figures held in this release are used for policy development as key indicators in behaviour and school attendance policy. Schools and local authorities also use the statistics to compare their local absence rates to regional and national averages for different pupil groups.\"}]},{\"Order\":2,\"Heading\":\"Absence rates\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The overall absence rate across state-funded primary, secondary and special schools increased from 4.6 per cent in 2015/16 to 4.7 per cent in 2016/17. In primary schools the overall absence rate stayed the same at 4 per cent and the rate in secondary schools increased from 5.2 per cent to 5.4 per cent. Absence in special schools is much higher at 9.7 per cent in 2016/17\\n\\nThe increase in overall absence rate has been driven by an increase in the unauthorised absence rate across state-funded primary, secondary and special schools - which increased from 1.1 per cent to 1.3 per cent between 2015/16 and 2016/17.\\n\\nLooking at longer-term trends, overall and authorised absence rates have been fairly stable over recent years after decreasing gradually between 2006/07 and 2013/14. Unauthorised absence rates have not varied much since 2006/07, however the unauthorised absence rate is now at its highest since records began, at 1.3 per cent.\\n\\nThis increase in unauthorised absence is due to an increase in absence due to family holidays that were not agreed by the school. The authorised absence rate has not changed since last year, at 3.4 per cent. Though in primary schools authorised absence rates have been decreasing across recent years.\\n\\nThe total number of days missed due to overall absence across state-funded primary, secondary and special schools has increased since last year, from 54.8 million in 2015/16 to 56.7 million in 2016/17. This partly reflects the rise in the total number of pupil enrolments, the average number of days missed per enrolment has increased very slightly from 8.1 days in 2015/16 to 8.2 days in 2016/17.\\n\\nIn 2016/17, 91.8 per cent of pupils in state-funded primary, state-funded secondary and special schools missed at least one session during the school year, this is similar to the previous year (91.7 per cent in 2015/16).\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataQuery\":{\"path\":\"/api/tablebuilder/characteristics/national\",\"method\":\"POST\",\"body\":\"{ \\\"attributes\\\": [ \\\"num_schools\\\", \\\"enrolments\\\", \\\"sess_overall_percent\\\", \\\"sess_unauthorised_percent\\\", \\\"sess_authorised_percent\\\" ], \\\"characteristics\\\": [ \\\"Total\\\" ], \\\"endYear\\\": 201617, \\\"publicationId\\\": \\\"cbbd299f-8297-44bc-92ac-558bcf51f8ad\\\", \\\"schoolTypes\\\": [ \\\"Total\\\" ], \\\"startYear\\\": 201213}\"},\"Charts\":[{\"Type\":\"line\",\"Attributes\":[\"sess_overall_percent\",\"sess_unauthorised_percent\",\"sess_authorised_percent\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"}}]}]},{\"Order\":3,\"Heading\":\"Persistent absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The percentage of enrolments in state-funded primary and state-funded secondary schools that were classified as persistent absentees in 2016/17 was 10.8 per cent. This is up from the equivalent figure of 10.5 per cent in 2015/16 (see Figure 2).\\n\\nIn 2016/17, persistent absentees accounted for 37.6 per cent of all absence compared to 36.6 per cent in 2015/16. Longer term, there has been a decrease in the proportion of absence that persistent absentees account for – down from 43.3 per cent in 2011/12.\\n\\nThe overall absence rate for persistent absentees across all schools was 18.1 per cent, nearly four times higher than the rate for all pupils. This is a slight increase from 2015/16, when the overall absence rate for persistent absentees was 17.6 per cent.\\n\\nPersistent absentees account for almost a third, 31.6 per cent, of all authorised absence and more than half, 53.8 per cent of all unauthorised absence. The rate of illness absences is almost four times higher for persistent absentees compared to other pupils, at 7.6 per cent and 2.0 per cent respectively.\"}]},{\"Order\":4,\"Heading\":\"Reasons for absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"InsetTextBlock\",\"Heading\":null,\"Body\":\"Within this release absence by reason is broken down in three different ways:\\n\\nDistribution of absence by reason: The proportion of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of absences.\\n\\nRate of absence by reason: The rate of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of possible sessions.\\n\\nOne or more sessions missed due to each reason: The number of pupil enrolments missing at least one session due to each reason.\"}]},{\"Order\":5,\"Heading\":\"Distribution of absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"Nearly half of all pupils (48.9 per cent) were absent for five days or fewer across state-funded primary, secondary and special schools in 2016/17, down from 49.1 per cent in 2015/16.\\n\\n4.3 per cent of pupil enrolments had more than 25 days of absence in 2016/17 (the same as in 2015/16). These pupil enrolments accounted for 23.5 per cent of days missed. 8.2 per cent of pupil enrolments had no absence during 2016/17.\\n\\nPer pupil enrolment, the average total absence in primary schools was 7.2 days, compared to 16.9 days in special schools and 9.3 days in secondary schools.\\n\\nWhen looking at absence rates across terms for primary, secondary and special schools, the overall absence rate is lowest in the autumn term and highest in the summer term. The authorised rate is highest in the spring term and lowest in the summer term, and the unauthorised rate is highest in the summer term.\"}]},{\"Order\":6,\"Heading\":\"Absence by pupil characteristics\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The patterns of absence rates for pupils with different characteristics have been consistent across recent years.\\n\\n### Gender\\n\\nThe overall absence rates across state-funded primary, secondary and special schools were very similar for boys and girls, at 4.7 per cent and 4.6 per cent respectively. The persistent absence rates were also similar, at 10.9 per cent for boys and 10.6 per cent for girls.\\n\\n### Free school meals (FSM) eligibility\\n\\nAbsence rates are higher for pupils who are known to be eligible for and claiming free school meals. The overall absence rate for these pupils was 7.3 per cent, compared to 4.2 per cent for non FSM pupils. The persistent absence rate for pupils who were eligible for FSM was more than twice the rate for those pupils not eligible for FSM.\\n\\n### National curriculum year group\\n\\nPupils in national curriculum year groups 3 and 4 had the lowest overall absence rates at 3.9 and 4 per cent respectively. Pupils in national curriculum year groups 10 and 11 had the highest overall absence rate at 6.1 per cent and 6.2 per cent respectively. This trend is repeated for persistent absence.\\n\\n### Special educational need (SEN)\\n\\nPupils with a statement of special educational needs (SEN) or education healthcare plan (EHC) had an overall absence rate of 8.2 per cent compared to 4.3 per cent for those with no identified SEN. The percentage of pupils with a statement of SEN or an EHC plan that are persistent absentees was more than two times higher than the percentage for pupils with no identified SEN.\\n\\n### Ethnic group\\n\\nThe highest overall absence rates were for Traveller of Irish Heritage and Gypsy/ Roma pupils at 18.1 per cent and 12.9 per cent respectively. Overall absence rates for pupils of a Chinese and Black African ethnicity were substantially lower than the national average of 4.7 per cent at 2.4 per cent and 2.9 per cent respectively. A similar pattern is seen in persistent absence rates; Traveller of Irish heritage pupils had the highest rate at 64 per cent and Chinese pupils had the lowest rate at 3.1 per cent.\"}]},{\"Order\":7,\"Heading\":\"Absence for four year olds\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The overall absence rate for four year olds in 2016/17 was 5.1 per cent which is lower than the rate of 5.2 per cent which it has been for the last two years.\\n\\nAbsence recorded for four year olds is not treated as 'authorised' or 'unauthorised' and is therefore reported as overall absence only.\"}]},{\"Order\":8,\"Heading\":\"Pupil referral unit absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The overall absence rate for pupil referral units in 2016/17 was 33.9 per cent, compared to 32.6 per cent in 2015/16. The percentage of enrolments in pupil referral units who were persistent absentees was 73.9 per cent in 2016/17, compared to 72.5 per cent in 2015/16.\"}]},{\"Order\":9,\"Heading\":\"Pupil absence by local authority\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"There is variation in overall and persistent absence rates across state-funded primary, secondary and special schools by region and local authority. Similarly to last year, the three regions with the highest overall absence rate across all state-funded primary, secondary and special schools are the North East (4.9 per cent), Yorkshire and the Humber (4.9 per cent) and the South West (4.8 per cent), with Inner and Outer London having the lowest overall absence rate (4.4 per cent). The region with the highest persistent absence rate is Yorkshire and the Humber, where 11.9 per cent of pupil enrolments are persistent absentees, with Outer London having the lowest rate of persistent absence (at 10.0 per cent).\\n\\nAbsence information at local authority district level is also published within this release, in the accompanying underlying data files.\"}]}]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Releases",
                keyColumn: "Id",
                keyValue: new Guid("4fa4fe8e-9a15-46bb-823f-49bf8e0cdec5"),
                column: "Content",
                value: "[{\"Order\":1,\"Heading\":\"About this release\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"This statistical first release (SFR) reports on absence of pupils of compulsory school age in state-funded primary, secondary and special schools during the 2016/17 academic year. Information on absence in pupil referral units, and for pupils aged four, is also included. The Department uses two key measures to monitor pupil absence – overall and persistent absence. Absence by reason and pupils characteristics is also included in this release. Figures are available at national, regional, local authority and school level. Figures held in this release are used for policy development as key indicators in behaviour and school attendance policy. Schools and local authorities also use the statistics to compare their local absence rates to regional and national averages for different pupil groups.\"}]},{\"Order\":2,\"Heading\":\"Absence rates\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The overall absence rate across state-funded primary, secondary and special schools increased from 4.6 per cent in 2015/16 to 4.7 per cent in 2016/17. In primary schools the overall absence rate stayed the same at 4 per cent and the rate in secondary schools increased from 5.2 per cent to 5.4 per cent. Absence in special schools is much higher at 9.7 per cent in 2016/17\\n\\nThe increase in overall absence rate has been driven by an increase in the unauthorised absence rate across state-funded primary, secondary and special schools - which increased from 1.1 per cent to 1.3 per cent between 2015/16 and 2016/17.\\n\\nLooking at longer-term trends, overall and authorised absence rates have been fairly stable over recent years after decreasing gradually between 2006/07 and 2013/14. Unauthorised absence rates have not varied much since 2006/07, however the unauthorised absence rate is now at its highest since records began, at 1.3 per cent.\\n\\nThis increase in unauthorised absence is due to an increase in absence due to family holidays that were not agreed by the school. The authorised absence rate has not changed since last year, at 3.4 per cent. Though in primary schools authorised absence rates have been decreasing across recent years.\\n\\nThe total number of days missed due to overall absence across state-funded primary, secondary and special schools has increased since last year, from 54.8 million in 2015/16 to 56.7 million in 2016/17. This partly reflects the rise in the total number of pupil enrolments, the average number of days missed per enrolment has increased very slightly from 8.1 days in 2015/16 to 8.2 days in 2016/17.\\n\\nIn 2016/17, 91.8 per cent of pupils in state-funded primary, state-funded secondary and special schools missed at least one session during the school year, this is similar to the previous year (91.7 per cent in 2015/16).\"},{\"Type\":\"DataBlock\",\"Heading\":null,\"DataQuery\":{\"path\":\"/api/tablebuilder/characteristics/national\",\"method\":\"POST\",\"body\":\"{ \\\"attributes\\\": [ \\\"num_schools\\\", \\\"enrolments\\\", \\\"sess_overall_percent\\\", \\\"sess_unauthorised_percent\\\", \\\"sess_authorised_percent\\\" ], \\\"characteristics\\\": [ \\\"Total\\\" ], \\\"endYear\\\": 201617, \\\"publicationId\\\": \\\"cbbd299f-8297-44bc-92ac-558bcf51f8ad\\\", \\\"schoolTypes\\\": [ \\\"Total\\\" ], \\\"startYear\\\": 201213}\"},\"Charts\":[{\"Type\":\"Line\",\"Attributes\":[\"sess_overall_percent\",\"sess_unauthorised_percent\",\"sess_authorised_percent\"],\"XAxis\":{\"title\":\"School Year\"},\"YAxis\":{\"title\":\"Absence Rate\"}}]}]},{\"Order\":3,\"Heading\":\"Persistent absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The percentage of enrolments in state-funded primary and state-funded secondary schools that were classified as persistent absentees in 2016/17 was 10.8 per cent. This is up from the equivalent figure of 10.5 per cent in 2015/16 (see Figure 2).\\n\\nIn 2016/17, persistent absentees accounted for 37.6 per cent of all absence compared to 36.6 per cent in 2015/16. Longer term, there has been a decrease in the proportion of absence that persistent absentees account for – down from 43.3 per cent in 2011/12.\\n\\nThe overall absence rate for persistent absentees across all schools was 18.1 per cent, nearly four times higher than the rate for all pupils. This is a slight increase from 2015/16, when the overall absence rate for persistent absentees was 17.6 per cent.\\n\\nPersistent absentees account for almost a third, 31.6 per cent, of all authorised absence and more than half, 53.8 per cent of all unauthorised absence. The rate of illness absences is almost four times higher for persistent absentees compared to other pupils, at 7.6 per cent and 2.0 per cent respectively.\"}]},{\"Order\":4,\"Heading\":\"Reasons for absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"InsetTextBlock\",\"Heading\":null,\"Body\":\"Within this release absence by reason is broken down in three different ways:\\n\\nDistribution of absence by reason: The proportion of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of absences.\\n\\nRate of absence by reason: The rate of absence for each reason, calculated by taking the number of absences for a specific reason as a percentage of the total number of possible sessions.\\n\\nOne or more sessions missed due to each reason: The number of pupil enrolments missing at least one session due to each reason.\"}]},{\"Order\":5,\"Heading\":\"Distribution of absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"Nearly half of all pupils (48.9 per cent) were absent for five days or fewer across state-funded primary, secondary and special schools in 2016/17, down from 49.1 per cent in 2015/16.\\n\\n4.3 per cent of pupil enrolments had more than 25 days of absence in 2016/17 (the same as in 2015/16). These pupil enrolments accounted for 23.5 per cent of days missed. 8.2 per cent of pupil enrolments had no absence during 2016/17.\\n\\nPer pupil enrolment, the average total absence in primary schools was 7.2 days, compared to 16.9 days in special schools and 9.3 days in secondary schools.\\n\\nWhen looking at absence rates across terms for primary, secondary and special schools, the overall absence rate is lowest in the autumn term and highest in the summer term. The authorised rate is highest in the spring term and lowest in the summer term, and the unauthorised rate is highest in the summer term.\"}]},{\"Order\":6,\"Heading\":\"Absence by pupil characteristics\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The patterns of absence rates for pupils with different characteristics have been consistent across recent years.\\n\\n### Gender\\n\\nThe overall absence rates across state-funded primary, secondary and special schools were very similar for boys and girls, at 4.7 per cent and 4.6 per cent respectively. The persistent absence rates were also similar, at 10.9 per cent for boys and 10.6 per cent for girls.\\n\\n### Free school meals (FSM) eligibility\\n\\nAbsence rates are higher for pupils who are known to be eligible for and claiming free school meals. The overall absence rate for these pupils was 7.3 per cent, compared to 4.2 per cent for non FSM pupils. The persistent absence rate for pupils who were eligible for FSM was more than twice the rate for those pupils not eligible for FSM.\\n\\n### National curriculum year group\\n\\nPupils in national curriculum year groups 3 and 4 had the lowest overall absence rates at 3.9 and 4 per cent respectively. Pupils in national curriculum year groups 10 and 11 had the highest overall absence rate at 6.1 per cent and 6.2 per cent respectively. This trend is repeated for persistent absence.\\n\\n### Special educational need (SEN)\\n\\nPupils with a statement of special educational needs (SEN) or education healthcare plan (EHC) had an overall absence rate of 8.2 per cent compared to 4.3 per cent for those with no identified SEN. The percentage of pupils with a statement of SEN or an EHC plan that are persistent absentees was more than two times higher than the percentage for pupils with no identified SEN.\\n\\n### Ethnic group\\n\\nThe highest overall absence rates were for Traveller of Irish Heritage and Gypsy/ Roma pupils at 18.1 per cent and 12.9 per cent respectively. Overall absence rates for pupils of a Chinese and Black African ethnicity were substantially lower than the national average of 4.7 per cent at 2.4 per cent and 2.9 per cent respectively. A similar pattern is seen in persistent absence rates; Traveller of Irish heritage pupils had the highest rate at 64 per cent and Chinese pupils had the lowest rate at 3.1 per cent.\"}]},{\"Order\":7,\"Heading\":\"Absence for four year olds\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The overall absence rate for four year olds in 2016/17 was 5.1 per cent which is lower than the rate of 5.2 per cent which it has been for the last two years.\\n\\nAbsence recorded for four year olds is not treated as 'authorised' or 'unauthorised' and is therefore reported as overall absence only.\"}]},{\"Order\":8,\"Heading\":\"Pupil referral unit absence\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"The overall absence rate for pupil referral units in 2016/17 was 33.9 per cent, compared to 32.6 per cent in 2015/16. The percentage of enrolments in pupil referral units who were persistent absentees was 73.9 per cent in 2016/17, compared to 72.5 per cent in 2015/16.\"}]},{\"Order\":9,\"Heading\":\"Pupil absence by local authority\",\"Caption\":\"\",\"Content\":[{\"Type\":\"MarkDownBlock\",\"Body\":\"There is variation in overall and persistent absence rates across state-funded primary, secondary and special schools by region and local authority. Similarly to last year, the three regions with the highest overall absence rate across all state-funded primary, secondary and special schools are the North East (4.9 per cent), Yorkshire and the Humber (4.9 per cent) and the South West (4.8 per cent), with Inner and Outer London having the lowest overall absence rate (4.4 per cent). The region with the highest persistent absence rate is Yorkshire and the Humber, where 11.9 per cent of pupil enrolments are persistent absentees, with Outer London having the lowest rate of persistent absence (at 10.0 per cent).\\n\\nAbsence information at local authority district level is also published within this release, in the accompanying underlying data files.\"}]}]");
        }
    }
}