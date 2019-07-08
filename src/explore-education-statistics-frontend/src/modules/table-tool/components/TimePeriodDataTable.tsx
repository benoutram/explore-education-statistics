import cartesian from '@common/lib/utils/cartesian';
import formatPretty from '@common/lib/utils/number/formatPretty';
import { TableData } from '@common/services/tableBuilderService';
import TimePeriod from '@common/services/types/TimePeriod';
import { Dictionary } from '@common/types/util';
import {
  CategoryFilter,
  Indicator,
  LocationFilter,
} from '@frontend/modules/table-tool/components/types/filters';
import last from 'lodash/last';
import sortBy from 'lodash/sortBy';
import React, { memo, useEffect, useRef, useState } from 'react';
import DataTableCaption from './DataTableCaption';
import FixedMultiHeaderDataTable from './FixedMultiHeaderDataTable';
import TableHeadersForm, { TableHeadersFormValues } from './TableHeadersForm';

interface Props {
  indicators: Indicator[];
  filters: Dictionary<CategoryFilter[]>;
  timePeriods: TimePeriod[];
  publicationName: string;
  subjectName: string;
  locations: LocationFilter[];
  results: TableData['result'];
}

const TimePeriodDataTable = (props: Props) => {
  const { filters, timePeriods, locations, indicators, results } = props;

  const dataTableRef = useRef<HTMLTableElement>(null);

  const [tableHeaders, setTableHeaders] = useState<TableHeadersFormValues>({
    columnGroups: [],
    columns: [],
    rowGroups: [],
    rows: [],
  });

  useEffect(() => {
    const sortedFilters = sortBy(
      Object.values({
        ...filters,
        locations,
      }),
      [options => options.length],
    );

    const halfwayIndex = Math.floor(sortedFilters.length / 2);
    const columnGroups = sortedFilters.slice(0, halfwayIndex);
    const rowGroups = sortedFilters.slice(halfwayIndex);

    setTableHeaders({
      columnGroups,
      rowGroups,
      columns: timePeriods,
      rows: indicators,
    });
  }, [filters, timePeriods, locations, indicators]);

  const columnHeaders: string[][] = [
    ...tableHeaders.columnGroups.map(colGroup =>
      colGroup.map(group => group.label),
    ),
    tableHeaders.columns.map(column => column.label),
  ];

  const rowHeaders: string[][] = [
    ...tableHeaders.rowGroups.map(rowGroup =>
      rowGroup.map(group => group.label),
    ),
    tableHeaders.rows.map(row => row.label),
  ];

  const rowHeadersCartesian = cartesian(
    ...tableHeaders.rowGroups,
    tableHeaders.rows,
  );

  const columnHeadersCartesian = cartesian(
    ...tableHeaders.columnGroups,
    tableHeaders.columns,
  );

  const rows = rowHeadersCartesian.map(rowFilterCombination => {
    const indicator = last(rowFilterCombination) as Indicator;

    return columnHeadersCartesian.map(columnFilterCombination => {
      const timePeriod = last(columnFilterCombination) as TimePeriod;

      const combinationFilters = [
        ...rowFilterCombination.slice(0, -1),
        ...columnFilterCombination.slice(0, -1),
      ];

      const categoryFilters = combinationFilters.filter(
        filter => filter instanceof CategoryFilter,
      );

      const locationFilters = combinationFilters.filter(
        filter => filter instanceof LocationFilter,
      ) as LocationFilter[];

      const matchingResult = results.find(result => {
        return (
          categoryFilters.every(filter =>
            result.filters.includes(filter.value),
          ) &&
          result.timeIdentifier === timePeriod.code &&
          result.year === timePeriod.year &&
          locationFilters.every(
            filter => result.location[filter.level].code === filter.value,
          )
        );
      });

      if (!matchingResult) {
        return 'n/a';
      }

      const value = matchingResult.measures[indicator.value];

      if (Number.isNaN(Number(value))) {
        return value;
      }

      return `${formatPretty(value)}${indicator.unit}`;
    });
  });

  return (
    <div>
      <TableHeadersForm
        initialValues={tableHeaders}
        onSubmit={value => {
          setTableHeaders(value);

          if (dataTableRef.current) {
            dataTableRef.current.scrollIntoView({
              behavior: 'smooth',
              block: 'start',
            });
          }
        }}
      />

      <FixedMultiHeaderDataTable
        caption={<DataTableCaption {...props} id="dataTableCaption" />}
        columnHeaders={columnHeaders}
        rowHeaders={rowHeaders}
        rows={rows}
        ref={dataTableRef}
      />
    </div>
  );
};

export default memo(TimePeriodDataTable);
