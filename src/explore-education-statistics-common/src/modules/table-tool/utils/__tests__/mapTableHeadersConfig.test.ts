import { UnmappedTableHeadersConfig } from '@common/services/permalinkService';
import {
  CategoryFilter,
  Indicator,
  LocationFilter,
  TimePeriodFilter,
} from '@common/modules/table-tool/types/filters';
import { FullTable } from '@common/modules/table-tool/types/fullTable';
import mapFullTable from '@common/modules/table-tool/utils/mapFullTable';
import mapTableHeadersConfig from '@common/modules/table-tool/utils/mapTableHeadersConfig';
import { TableDataResponse } from '@common/services/tableBuilderService';

describe('mapTableHeadersConfig', () => {
  const testHeaderConfig: UnmappedTableHeadersConfig = {
    columnGroups: [
      [
        {
          value: '598ed9fd-b37e-4e08-baec-08d78f6f2c4d',
          type: 'Filter',
        },
        {
          value: '067de12b-014b-4bbd-baf1-08d78f6f2c4d',
          type: 'Filter',
        },
      ],
    ],
    rowGroups: [
      [
        {
          value: 'd7e7e412-f462-444f-84ac-3454fa471cb8',
          type: 'Filter',
        },
        {
          value: 'a9fe9fa6-e91f-460b-a0b1-66877b97c581',
          type: 'Filter',
        },
      ],
      [
        {
          value: 'E09000003',
          type: 'Location',
          level: 'localAuthority',
        },
        {
          value: 'E08000016',
          type: 'Location',
          level: 'localAuthority',
        },
      ],
    ],
    columns: [
      { value: '2014_AY', type: 'TimePeriod' },
      { value: '2015_AY', type: 'TimePeriod' },
    ],
    rows: [
      {
        value: '9cf0dcf1-367e-4207-2b50-08d78f6f2b08',
        type: 'Indicator',
      },
      {
        value: 'd1c4a0be-8756-470d-2b51-08d78f6f2b08',
        type: 'Indicator',
      },
      {
        value: '63043c27-f055-472f-2b56-08d78f6f2b08',
        type: 'Indicator',
      },
    ],
  };

  const testTableData: TableDataResponse = {
    subjectMeta: {
      filters: {
        Characteristic: {
          totalValue: '',
          hint: 'Filter by pupil characteristic',
          legend: 'Characteristic',
          name: 'characteristic',
          options: {
            EthnicGroupMajor: {
              label: 'Ethnic group major',
              options: [
                {
                  label: 'Ethnicity Major Asian Total',
                  value: '598ed9fd-b37e-4e08-baec-08d78f6f2c4d',
                },
                {
                  label: 'Ethnicity Major Black Total',
                  value: '067de12b-014b-4bbd-baf1-08d78f6f2c4d',
                },
              ],
            },
          },
        },
        SchoolType: {
          totalValue: '',
          hint: 'Filter by school type',
          legend: 'School type',
          name: 'school_type',
          options: {
            Default: {
              label: 'Default',
              options: [
                {
                  label: 'State-funded primary',
                  value: 'd7e7e412-f462-444f-84ac-3454fa471cb8',
                },
                {
                  label: 'State-funded secondary',
                  value: 'a9fe9fa6-e91f-460b-a0b1-66877b97c581',
                },
              ],
            },
          },
        },
      },
      footnotes: [],
      geoJsonAvailable: false,
      indicators: [
        {
          value: '9cf0dcf1-367e-4207-2b50-08d78f6f2b08',
          label: 'Number of overall absence sessions',
          unit: '',
          name: 'sess_overall,Indicator',
          decimalPlaces: 2,
        },
        {
          value: 'd1c4a0be-8756-470d-2b51-08d78f6f2b08',
          label: 'Number of authorised absence sessions',
          unit: '',
          name: 'sess_overall,Indicator',
          decimalPlaces: 2,
        },
        {
          value: '63043c27-f055-472f-2b56-08d78f6f2b08',
          label: 'Number of persistent absentees',
          unit: '',
          name: 'enrolments_pa_10_exact,Indicator',
          decimalPlaces: 2,
        },
      ],
      locations: [
        { value: 'E09000003', label: 'Barnet', level: 'localAuthority' },
        { value: 'E08000016', label: 'Barnsley', level: 'localAuthority' },
      ],
      boundaryLevels: [],
      publicationName: 'Pupil absence in schools in England',
      subjectName: 'Absence by characteristic',
      timePeriodRange: [
        { label: '2014/15', code: 'AY', year: 2014 },
        { label: '2015/16', code: 'AY', year: 2015 },
      ],
    },
    results: [],
  };

  test('maps headers to their classes', () => {
    const table: FullTable = mapFullTable(testTableData);
    const headers = mapTableHeadersConfig(testHeaderConfig, table.subjectMeta);

    expect(headers.columnGroups).toEqual([
      [
        new CategoryFilter({
          value: '598ed9fd-b37e-4e08-baec-08d78f6f2c4d',
          label: 'Ethnicity Major Asian Total',
          group: 'Ethnic group major',
          category: 'Characteristic',
        }),
        new CategoryFilter({
          value: '067de12b-014b-4bbd-baf1-08d78f6f2c4d',
          label: 'Ethnicity Major Black Total',
          group: 'Ethnic group major',
          category: 'Characteristic',
        }),
      ],
    ]);
    expect(headers.rowGroups).toEqual([
      [
        new CategoryFilter({
          value: 'd7e7e412-f462-444f-84ac-3454fa471cb8',
          label: 'State-funded primary',
          group: 'Default',
          category: 'School type',
        }),
        new CategoryFilter({
          value: 'a9fe9fa6-e91f-460b-a0b1-66877b97c581',
          label: 'State-funded secondary',
          group: 'Default',
          category: 'School type',
        }),
      ],
      [
        new LocationFilter({
          value: 'E09000003',
          label: 'Barnet',
          level: 'localAuthority',
        }),
        new LocationFilter({
          value: 'E08000016',
          label: 'Barnsley',
          level: 'localAuthority',
        }),
      ],
    ]);
    expect(headers.columns).toEqual([
      new TimePeriodFilter({
        year: 2014,
        code: 'AY',
        label: '2014/15',
        order: 0,
      }),
      new TimePeriodFilter({
        year: 2015,
        code: 'AY',
        label: '2015/16',
        order: 1,
      }),
    ]);
    expect(headers.rows).toEqual([
      new Indicator({
        value: '9cf0dcf1-367e-4207-2b50-08d78f6f2b08',
        label: 'Number of overall absence sessions',
        unit: '',
        name: 'sess_overall,Indicator',
        decimalPlaces: 2,
      }),
      new Indicator({
        value: 'd1c4a0be-8756-470d-2b51-08d78f6f2b08',
        label: 'Number of authorised absence sessions',
        unit: '',
        name: 'sess_overall,Indicator',
        decimalPlaces: 2,
      }),
      new Indicator({
        value: '63043c27-f055-472f-2b56-08d78f6f2b08',
        label: 'Number of persistent absentees',
        unit: '',
        name: 'enrolments_pa_10_exact,Indicator',
        decimalPlaces: 2,
      }),
    ]);
  });

  test('correctly maps locations with same `value` but different `level`', () => {
    const table: FullTable = mapFullTable({
      results: [],
      subjectMeta: {
        publicationName: '',
        subjectName: '',
        filters: {},
        footnotes: [],
        locations: [
          {
            level: 'localAuthority',
            label: 'Barnet (local authority)',
            value: 'barnet',
          },
          {
            level: 'localAuthorityDistrict',
            label: 'Barnet (local authority district)',
            value: 'barnet',
          },
        ],
        indicators: [],
        boundaryLevels: [],
        timePeriodRange: [],
        geoJsonAvailable: false,
      },
    });

    const headers = mapTableHeadersConfig(
      {
        columnGroups: [],
        columns: [],
        rowGroups: [
          [
            {
              type: 'Location',
              value: 'barnet',
              level: 'localAuthorityDistrict',
            },
            {
              type: 'Location',
              value: 'barnet',
              level: 'localAuthority',
            },
          ],
        ],
        rows: [
          {
            type: 'Location',
            value: 'barnet',
            level: 'localAuthority',
          },
          {
            type: 'Location',
            value: 'barnet',
            level: 'localAuthorityDistrict',
          },
        ],
      },
      table.subjectMeta,
    );

    expect(headers.rowGroups).toEqual([
      [
        new LocationFilter({
          value: 'barnet',
          label: 'Barnet (local authority district)',
          level: 'localAuthorityDistrict',
        }),
        new LocationFilter({
          value: 'barnet',
          label: 'Barnet (local authority)',
          level: 'localAuthority',
        }),
      ],
    ]);

    expect(headers.rows).toEqual([
      new LocationFilter({
        value: 'barnet',
        label: 'Barnet (local authority)',
        level: 'localAuthority',
      }),
      new LocationFilter({
        value: 'barnet',
        label: 'Barnet (local authority district)',
        level: 'localAuthorityDistrict',
      }),
    ]);
  });

  test('throws error if there is an invalid header type', () => {
    const fullTable = mapFullTable(testTableData);

    const headers: UnmappedTableHeadersConfig = {
      columnGroups: [],
      columns: [
        {
          value: 'abcd',
          type: 'NotValid' as 'Indicator',
        },
      ],
      rowGroups: [],
      rows: [],
    };

    expect(() => {
      mapTableHeadersConfig(headers, fullTable.subjectMeta);
    }).toThrowError(
      `Invalid header type: ${JSON.stringify(headers.columns[0])}`,
    );
  });

  test('throws error if could not find header filter', () => {
    const fullTable = mapFullTable(testTableData);

    const headers: UnmappedTableHeadersConfig = {
      columnGroups: [],
      columns: [
        {
          value: 'will not find me',
          type: 'Indicator',
        },
      ],
      rowGroups: [],
      rows: [],
    };

    expect(() => {
      mapTableHeadersConfig(headers, fullTable.subjectMeta);
    }).toThrowError(
      `Could not find matching filter for header: ${JSON.stringify(
        headers.columns[0],
      )}`,
    );
  });

  test('does not map any headers', () => {
    const fullTable = mapFullTable(testTableData);
    const headers = mapTableHeadersConfig(
      {
        columnGroups: [],
        columns: [],
        rowGroups: [],
        rows: [],
      },
      fullTable.subjectMeta,
    );

    expect(headers.columnGroups).toEqual([]);
    expect(headers.rowGroups).toEqual([]);
    expect(headers.columns).toEqual([]);
    expect(headers.rows).toEqual([]);
  });
});
