using System;
using System.Collections.Generic;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Common.Database;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Importer.Exceptions;
using GovUk.Education.ExploreEducationStatistics.Data.Importer.Services;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Extensions;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Model;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Models;
using GovUk.Education.ExploreEducationStatistics.Data.Processor.Services.Interfaces;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Services
{
    public enum ValidationErrorMessages
    {
        [EnumLabelValue("Meta header contains quotes")]
        MetaHeaderContainsQuotes,

        [EnumLabelValue("Metafile is missing expected column")]
        MetaFileMissingExpectedColumn,

        [EnumLabelValue("Metafile contains quotes")]
        MetaFileContainsQuotes,

        [EnumLabelValue("Metafile has invalid number of columns")]
        MetaFileHasInvalidNumberOfColumns,

        [EnumLabelValue("Metafile has invalid values")]
        MetaFileHasInvalidValues,

        [EnumLabelValue("Datafile contains quotes")]
        DataFileContainsQuotes,

        [EnumLabelValue("Datafile has invalid number of columns")]
        DataFileHasInvalidNumberOfColumns,

        [EnumLabelValue("Datafile has invalid geographic level")]
        DataFileHasInvalidGeographicLevel,

        [EnumLabelValue("Datafile has invalid time identifier")]
        DataFileHasInvalidTimeIdentifier,

        [EnumLabelValue("Datafile has invalid time period")]
        DataFileHasInvalidTimePeriod,
        
        [EnumLabelValue("Datafile is missing expected column")]
        DataFileMissingExpectedColumn
    }

    public class ValidatorService : IValidatorService
    {
        private static readonly List<string> MandatoryObservationColumns = new List<string>
        {
            "time_identifier,TIME_IDENTIFIER",
            "time_period",
            "geographic_level"
        };
        
        public List<string> Validate(ImportMessage message, SubjectData subjectData)
        {
            var errors = new List<string>();

            var metaHeaders = subjectData.GetMetaLines().First();
            ValidateMetaHeader(metaHeaders, errors);

            // If the meta header not ok then stop error checks
            if (errors.Count != 0)
            {
                return errors;
            }

            ValidateMetaRows(subjectData.GetMetaLines(), errors);
            var headers = subjectData.GetCsvLines().First().Split(',').ToList();
            ValidateObservationHeaders(headers, errors);
            ValidateObservations(headers, subjectData.GetCsvLines(), errors);

            return errors;
        }

        private static void ValidateMetaHeader(string header, List<string> errors)
        {
            if (RowContainsQuotes(header))
                // No further checks if quotes exist
            {
                errors.Add(ValidationErrorMessages.MetaHeaderContainsQuotes.GetEnumLabel());
            }
            else
                // Check for unexpected column names
            {
                Array.ForEach(Enum.GetNames(typeof(MetaColumns)), col =>
                {
                    if (!header.Contains(col))
                    {
                        errors.Add(ValidationErrorMessages.MetaFileMissingExpectedColumn.GetEnumLabel() + " : " + col);
                    }
                });
            }
        }

        private static void ValidateMetaRows(IEnumerable<string> lines, List<string> errors)
        {
            var idx = 2;
            var headers = lines.First().Split(',').ToList();

            foreach (var line in lines.Skip(1))
            {
                ValidateMetaRow(line, idx++, headers, errors);
            }
        }

        private static void ValidateObservations(List<string> headers, IEnumerable<string> lines, List<string> errors)
        {
            var idx = 2;
            foreach (var line in lines.Skip(1))
            {
                if (errors.Count == 100)
                {
                    errors.Add("Only first 100 errors are returned");
                    break;
                }
                ValidateObservationRow(line, idx++, headers, errors);
            }
        }
        
        private static void ValidateObservationHeaders(List<string> headers, List<string> errors)
        {
            foreach (var mandatoryCol in MandatoryObservationColumns)
            {
                if (!headers.Contains(mandatoryCol))
                {
                    errors.Add(ValidationErrorMessages.DataFileMissingExpectedColumn.GetEnumLabel() + " : " + mandatoryCol.Split(",")[0]);
                }
                if (errors.Count == 100)
                {
                    errors.Add("Only first 100 errors are returned");
                    break;
                }
            }
        }

        private static void ValidateMetaRow(string row, int rowNumber, List<string> headers, List<string> errors)
        {
            var numExpectedColumns = headers.Count;

            if (RowContainsQuotes(row))
            {
                // No further checks if quotes exist
                errors.Add(
                    $"error at row {rowNumber}: " + ValidationErrorMessages.MetaFileContainsQuotes.GetEnumLabel());
            }
            else
            {
                if (HasUnexpectedNumberOfColumns(row, numExpectedColumns))
                {
                    errors.Add($"error at row {rowNumber}: " +
                               ValidationErrorMessages.MetaFileHasInvalidNumberOfColumns.GetEnumLabel());
                }

                try
                {
                    ImporterMetaService.GetMetaRow(row, headers);
                }
                catch (Exception e)
                {
                    errors.Add($"error at row {rowNumber}: " +
                               ValidationErrorMessages.MetaFileHasInvalidValues.GetEnumLabel() + " : " + e.Message);
                }
            }
        }

        private static void ValidateObservationRow(string row, int rowNumber, List<string> headers, List<string> errors)
        {
            if (RowContainsQuotes(row))
            {
                // No further checks if quotes exist
                errors.Add(
                    $"error at row {rowNumber}: " + ValidationErrorMessages.DataFileContainsQuotes.GetEnumLabel());
            }
            else
            {
                if (HasUnexpectedNumberOfColumns(row, headers.Count))
                {
                    errors.Add($"error at row {rowNumber}: " +
                               ValidationErrorMessages.DataFileHasInvalidNumberOfColumns.GetEnumLabel());
                }

                try
                {
                    var line = row.Split(',');
                    ImporterService.GetGeographicLevel(line, headers);
                    ImporterService.GetTimeIdentifier(line, headers);
                    ImporterService.GetYear(line, headers);
                }
                catch (InvalidGeographicLevelException)
                {
                    errors.Add($"error at row {rowNumber}: " +
                               ValidationErrorMessages.DataFileHasInvalidGeographicLevel.GetEnumLabel());
                }
                catch (InvalidTimeIdentifierException)
                {
                    errors.Add($"error at row {rowNumber}: " +
                               ValidationErrorMessages.DataFileHasInvalidTimeIdentifier.GetEnumLabel());
                }
                catch (InvalidTimePeriod)
                {
                    errors.Add($"error at row {rowNumber}: " +
                               ValidationErrorMessages.DataFileHasInvalidTimePeriod.GetEnumLabel());
                }
            }
        }

        private static bool RowContainsQuotes(string row)
        {
            return row.Contains("\"");
        }

        private static bool HasUnexpectedNumberOfColumns(string row, int numExpectedColumns)
        {
            return row.Split(',').Length != numExpectedColumns;
        }
    }
}