import {
  testChartConfiguration,
  testChartTableData,
  testDeprecatedChartConfiguration,
} from '@common/modules/charts/components/__tests__/__data__/testChartData';
import {
  testMapConfiguration,
  testMapTableData,
} from '@common/modules/charts/components/__tests__/__data__/testMapBlockData';
import getDefaultTableHeaderConfig from '@common/modules/table-tool/utils/getDefaultTableHeadersConfig';
import mapFullTable from '@common/modules/table-tool/utils/mapFullTable';
import mapUnmappedTableHeaders from '@common/modules/table-tool/utils/mapUnmappedTableHeaders';
import _tableBuilderService, {
  TableDataQuery,
} from '@common/services/tableBuilderService';
import {
  Chart,
  DataBlock,
  DataBlockRequest,
} from '@common/services/types/blocks';
import { waitFor, screen } from '@testing-library/dom';
import { render } from '@testing-library/react';
import React from 'react';
import DataBlockRenderer from '../DataBlockRenderer';

jest.mock('@common/services/tableBuilderService');

jest.mock('recharts/lib/util/LogUtils');

const tableBuilderService = _tableBuilderService as jest.Mocked<
  typeof _tableBuilderService
>;

describe('DataBlockRenderer', () => {
  const testDataBlock: DataBlock = {
    id: 'test-id',
    type: 'DataBlock',
    heading: '',
    order: 0,
    name: 'Test data block',
    source: '',
    dataBlockRequest: {
      subjectId: '1',
      geographicLevel: 'country',
      timePeriod: {
        startYear: 2012,
        startCode: 'AY',
        endYear: 2016,
        endCode: 'AY',
      },
      filters: ['characteristic-total', 'school-type-total'],
      indicators: [
        'authorised-absence-rate',
        'unauthorised-absence-rate',
        'overall-absence-rate',
      ],
      locations: {},
    },
    charts: [],
    tables: [],
  };

  test('renders line chart', async () => {
    tableBuilderService.getTableData.mockImplementation(() =>
      Promise.resolve(testChartTableData),
    );

    const { container } = render(
      <DataBlockRenderer
        id="test-datablock"
        dataBlock={{
          ...testDataBlock,
          charts: [testChartConfiguration],
        }}
      />,
    );

    await waitFor(() => {
      expect(tableBuilderService.getTableData).toBeCalledWith({
        ...testDataBlock.dataBlockRequest,
        includeGeoJson: false,
      } as TableDataQuery);

      expect(
        container.querySelectorAll('section.govuk-tabs__panel'),
      ).toHaveLength(1);

      expect(container.querySelectorAll('.recharts-line')).toHaveLength(3);
    });
  });

  test('renders horizontal chart', async () => {
    tableBuilderService.getTableData.mockImplementation(() =>
      Promise.resolve(testChartTableData),
    );

    const { container } = render(
      <DataBlockRenderer
        id="test-block"
        dataBlock={{
          ...testDataBlock,
          charts: [
            {
              ...testChartConfiguration,
              type: 'horizontalbar',
            } as Chart,
          ],
        }}
      />,
    );

    await waitFor(() => {
      expect(tableBuilderService.getTableData).toBeCalledWith({
        ...testDataBlock.dataBlockRequest,
        includeGeoJson: false,
      } as TableDataQuery);

      expect(
        container.querySelectorAll('section.govuk-tabs__panel'),
      ).toHaveLength(1);

      expect(container.querySelectorAll('.recharts-bar')).toHaveLength(3);
    });
  });

  test('renders vertical chart', async () => {
    tableBuilderService.getTableData.mockImplementation(
      (_: DataBlockRequest) => {
        return Promise.resolve(testChartTableData);
      },
    );

    const { container } = render(
      <DataBlockRenderer
        id="test-block"
        dataBlock={{
          ...testDataBlock,
          charts: [
            {
              ...testChartConfiguration,
              type: 'verticalbar',
            } as Chart,
          ],
        }}
      />,
    );

    await waitFor(() => {
      expect(tableBuilderService.getTableData).toBeCalledWith({
        ...testDataBlock.dataBlockRequest,
        includeGeoJson: false,
      } as TableDataQuery);

      expect(
        container.querySelectorAll('section.govuk-tabs__panel'),
      ).toHaveLength(1);

      expect(container.querySelectorAll('.recharts-bar')).toHaveLength(3);
    });
  });

  test('renders table', async () => {
    tableBuilderService.getTableData.mockImplementation(
      (_: DataBlockRequest) => {
        return Promise.resolve(testChartTableData);
      },
    );

    const fullTable = mapFullTable(testChartTableData);

    render(
      <DataBlockRenderer
        id="test-block"
        dataBlock={{
          ...testDataBlock,
          tables: [
            {
              tableHeaders: mapUnmappedTableHeaders(
                getDefaultTableHeaderConfig(fullTable.subjectMeta),
              ),
              indicators: [
                'authorised-absence-rate',
                'unauthorised-absence-rate',
                'overall-absence-rate',
              ],
            },
          ],
        }}
      />,
    );

    await waitFor(() => {
      expect(tableBuilderService.getTableData).toBeCalledWith({
        ...testDataBlock.dataBlockRequest,
        includeGeoJson: false,
      } as TableDataQuery);

      expect(screen.getByRole('table')).toBeInTheDocument();
      expect(screen.getAllByRole('row')).toHaveLength(4);
      expect(screen.getAllByRole('cell')).toHaveLength(16);
    });
  });

  test('renders map', async () => {
    const getDataBlockForSubject = tableBuilderService.getTableData.mockImplementation(
      (_: DataBlockRequest) => {
        return Promise.resolve(testMapTableData);
      },
    );

    const { container } = render(
      <DataBlockRenderer
        id="test-block"
        dataBlock={{
          ...testDataBlock,
          charts: [testMapConfiguration],
        }}
      />,
    );

    await waitFor(() => {
      expect(getDataBlockForSubject).toBeCalledWith({
        ...testDataBlock.dataBlockRequest,
        includeGeoJson: true,
      } as TableDataQuery);

      expect(container.querySelector('.leaflet-container')).toBeInTheDocument();
    });
  });

  test('can render line chart with deprecated `labels` for data sets', async () => {
    tableBuilderService.getTableData.mockImplementation(() =>
      Promise.resolve(testChartTableData),
    );

    const { container } = render(
      <DataBlockRenderer
        id="test-datablock"
        dataBlock={{
          ...testDataBlock,
          charts: [testDeprecatedChartConfiguration],
        }}
      />,
    );

    await waitFor(() => {
      expect(tableBuilderService.getTableData).toBeCalledWith({
        ...testDataBlock.dataBlockRequest,
        includeGeoJson: false,
      } as TableDataQuery);

      expect(
        container.querySelectorAll('section.govuk-tabs__panel'),
      ).toHaveLength(1);

      expect(container.querySelectorAll('.recharts-line')).toHaveLength(3);

      const legendItems = container.querySelectorAll('.recharts-legend-item');
      expect(legendItems[0]).toHaveTextContent(
        'Unauthorised absence rate (England)',
      );
      expect(legendItems[1]).toHaveTextContent(
        'Authorised absence rate (England)',
      );
      expect(legendItems[2]).toHaveTextContent(
        'Overall absence rate (England)',
      );
    });
  });

  test('re-rendering with new data block does not throw error', async () => {
    tableBuilderService.getTableData.mockImplementation(() =>
      Promise.resolve(testChartTableData),
    );

    const fullTable = mapFullTable(testChartTableData);

    const { rerender } = render(
      <DataBlockRenderer
        id="test-block"
        dataBlock={{
          ...testDataBlock,
          tables: [
            {
              tableHeaders: mapUnmappedTableHeaders(
                getDefaultTableHeaderConfig(fullTable.subjectMeta),
              ),
              indicators: [
                'authorised-absence-rate',
                'unauthorised-absence-rate',
                'overall-absence-rate',
              ],
            },
          ],
        }}
      />,
    );

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
      expect(screen.getAllByRole('row')).toHaveLength(4);
      expect(screen.getAllByRole('cell')).toHaveLength(16);
    });

    tableBuilderService.getTableData.mockImplementation(() =>
      Promise.resolve({
        subjectMeta: {
          ...testChartTableData.subjectMeta,
          indicators: [
            {
              label: 'Number of authorised absence sessions',
              unit: '',
              value: 'authorised-absence-sessions',
              name: 'absence_sess',
            },
          ],
          timePeriodRange: [{ code: 'AY', label: '2018/19', year: 2018 }],
        },
        results: [
          {
            filters: ['characteristic-total', 'school-type-total'],
            geographicLevel: 'Country',
            location: { country: { code: 'E92000001', name: 'England' } },
            measures: {
              'authorised-absence-sessions': '500000',
            },
            timePeriod: '2018_AY',
          },
        ],
      }),
    );

    expect(() => {
      rerender(
        <DataBlockRenderer
          id="test-block"
          dataBlock={{
            ...testDataBlock,
            dataBlockRequest: {
              subjectId: '1',
              geographicLevel: 'country',
              timePeriod: {
                startYear: 2018,
                startCode: 'AY',
                endYear: 2018,
                endCode: 'AY',
              },
              filters: ['characteristic-total', 'school-type-total'],
              indicators: ['authorised-absence-sessions'],
              locations: {},
            },
            tables: [
              {
                tableHeaders: {
                  columnGroups: [],
                  rowGroups: [],
                  columns: [
                    {
                      type: 'TimePeriod',
                      value: '2018_AY',
                    },
                  ],
                  rows: [
                    {
                      type: 'Indicator',
                      value: 'authorised-absence-sessions',
                    },
                  ],
                },
                indicators: ['authorised-absence-sessions'],
              },
            ],
          }}
        />,
      );
    }).not.toThrowError();

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
      expect(screen.getAllByRole('row')).toHaveLength(2);
      expect(screen.getAllByRole('cell')).toHaveLength(2);
    });
  });
});
